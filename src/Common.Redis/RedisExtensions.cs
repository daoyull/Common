using System.Diagnostics;
using Autofac;
using Common.Lib.Exceptions;
using LanguageExt.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Common.Redis;

public delegate Result<IDatabase> RedisResolver(string key, int database = 0);

public static class RedisExtensions
{
    private static readonly Dictionary<string, IConnectionMultiplexer> RedisDictionary = new();

    private static readonly object Locker = new object();

    public static ContainerBuilder AddEmptyRedis(this ContainerBuilder container)
    {
        var redisResolver = new RedisResolver((_, _) => new Result<IDatabase>(new BusinessException("Empty Redis")));
        container.RegisterInstance(redisResolver).SingleInstance();
        return container;
    }

    public static ContainerBuilder AddRedis(this ContainerBuilder container)
    {
        container.Register(provider =>
        {
            return new RedisResolver((key, database) =>
            {
                lock (Locker)
                {
                    if (RedisDictionary.TryGetValue(key, out var multiplexer))
                        return new Result<IDatabase>(multiplexer.GetDatabase(database));

                    // 后来注册的情况
                    var optionsSnapshot = provider.ResolveOptional<IOptionsSnapshot<RedisOptions>>();
                    var redisOption = optionsSnapshot?.Get(key);

                    if (redisOption == null || string.IsNullOrEmpty(redisOption.Ip))
                        return new Result<IDatabase>(new KeyNotFoundException($"未添加{key}的Redis连接配置"));
                    var connectStr = redisOption.GenConnectStr();
                    var logger = provider.Resolve<ILogger<IConnectionMultiplexer>>();

                    logger.LogInformation($"连接数据库 {connectStr}");

                    var multipartContent = ConnectionMultiplexer.Connect(connectStr);

                    RedisDictionary.Add(key, multipartContent);

                    return new Result<IDatabase>(multipartContent.GetDatabase(database));
                }
            });
        }).SingleInstance();


        return container;
    }

    public static string GenConnectStr(this RedisOptions redisOptions)
    {
        if (string.IsNullOrEmpty(redisOptions.Ip) || redisOptions.Port == 0)
        {
            throw new ArgumentException("Redis配置错误");
        }

        return
            $"{redisOptions.Ip}:{redisOptions.Port}{(string.IsNullOrEmpty(redisOptions.Password) ? "" : $",password={redisOptions.Password}")}";
    }
}