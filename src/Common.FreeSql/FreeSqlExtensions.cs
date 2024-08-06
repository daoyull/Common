using System.Diagnostics;
using System.Reflection;
using Autofac;
using Common.FreeSql.Models;
using Common.Lib.Attributes;
using Common.Lib.Helpers;
using Common.Lib.Service;
using FreeSql;
using FreeSql.Aop;
using FreeSql.Internal.Model;
using Mapster;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Common.FreeSql;

public delegate IFreeSql FreeSqlResolver(string key);

/// <summary>
/// https://github.com/dotnetcore/FreeSql/issues/44
/// 文档地址
/// https://github.com/dotnetcore/FreeSql/discussions/1033
/// </summary>
public static class FreeSqlExtensions
{
    static FreeSqlExtensions()
    {
        TypeAdapterConfig.GlobalSettings.NewConfig<IPageQuery, BasePagingInfo>()
            .Map(desc => desc.PageNumber, src => src.PageNum)
            .Map(desc => desc.PageSize, src => src.PageSize);
    }

    private static readonly Dictionary<string, IFreeSql> FreeSqlDictionary = new();

    private static readonly object Locker = new object();


    public static ContainerBuilder AddFreeSql(this ContainerBuilder container)
    {
        container.Register(provider =>
        {
            return new FreeSqlResolver(key =>
            {
                lock (Locker)
                {
                    if (FreeSqlDictionary.TryGetValue(key, out var freeSql)) return freeSql;

                    // 后来注册的情况
                    var optionsSnapshot = provider.ResolveOptional<IOptionsSnapshot<FreeSqlOptions>>();
                    var freeSqlOptions = optionsSnapshot?.Get(key);

                    if (freeSqlOptions == null)
                        throw new KeyNotFoundException($"未添加{key}的FreeSql连接配置");

                    var logger = provider.Resolve<ILogger<IFreeSql>>();

                    logger.LogInformation("连接数据库 {DataType}@{Flag}", freeSqlOptions.DataType, key);

                    var db = new FreeSqlBuilder()
                        .UseConnectionString(freeSqlOptions.DataType, freeSqlOptions.ConnectionString)
                        .UseNoneCommandParameter(freeSqlOptions.EnableNoneCommandParameter)
                        .UseAutoSyncStructure(freeSqlOptions.EnableAutoSyncStructure)
                        .UseSlave(freeSqlOptions.SlaveConnectionStrings)
                        .Build();

                    db.AutoLoadId();
                    // 启用日志记录
                    if (true)
                    {
                        db.Aop.CurdBefore += (_, e) =>
                        {
                            if (e.DbParms != null && e.DbParms.Length > 0)
                            {
                                logger.LogInformation(
                                    "{Flag}数据库执行:\nSql: {Sql}",
                                    key, e.Sql);
                            }
                            else
                            {
                                logger.LogInformation(
                                    "{Flag}数据库执行:\nSql: {Sql}\nDbParms: {@Parameter}",
                                    key, e.Sql, e.DbParms);
                            }

                            Debug.WriteLine(e.Sql);
                        };
                    }

                    FreeSqlDictionary.Add(key, db);

                    return db;
                }
            });
        }).SingleInstance();


        return container;
    }

    public static IFreeSql AutoLoadId(this IFreeSql freeSql)
    {
        freeSql.Aop.AuditValue += (s, e) =>
        {
            // 雪花Id
            if (
                e.AuditValueType == AuditValueType.Insert && e.Column.CsType == typeof(long) &&
                e.Property.GetCustomAttribute<SnowflakeAttribute>(false) != null &&
                e.Value?.ToString() == "0")
                e.Value = IdHelper.SnowId;

            // Guid
            if (e.AuditValueType == AuditValueType.Insert && e.Column.CsType == typeof(string) &&
                e.Property.GetCustomAttribute<GuidAttribute>(false) != null &&
                string.IsNullOrEmpty(e.Value?.ToString()))
                e.Value = IdHelper.Guid;

            // SimpleGuid
            if (e.AuditValueType == AuditValueType.Insert && e.Column.CsType == typeof(string) &&
                e.Property.GetCustomAttribute<SimpleGuidAttribute>(false) != null &&
                string.IsNullOrEmpty(e.Value?.ToString()))
                e.Value = IdHelper.SimpleGuid;

            if (e.Column.CsName == "CreateTime" && e.Column.CsType == typeof(DateTime) &&
                e.AuditValueType == AuditValueType.Insert
                && (e.Value == null || (DateTime)e.Value == DateTime.MinValue))
                e.Value = TimeHelper.NowCst;
            if (e.Column.CsName == "UpdateTime" && e.Column.CsType == typeof(DateTime)
                                                && (e.Value == null || (DateTime)e.Value == DateTime.MinValue))
                e.Value = TimeHelper.NowCst;
        };
        return freeSql;
    }


    public static IFreeSql Get(this FreeSqlResolver resolver, string key)
    {
        return resolver(key);
    }

    public static async Task<TDestination> MapperTo<TSource, TDestination>(this Task<TSource> source)
    {
        var sourceValue = await source;
        return sourceValue.Adapt<TDestination>();
    }
}