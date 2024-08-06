using System.Diagnostics;
using Autofac;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Common.Redis;

public delegate IDatabase RedisResolver(string key);

public static class RedisExtensions
{
    public delegate IDatabase RedisResolver(string key, int database = 0);

    private static readonly Dictionary<string, IConnectionMultiplexer> RedisDictionary = new();

    private static readonly object Locker = new object();

    public static ContainerBuilder AddRedis(this ContainerBuilder container)
    {
        container.Register(provider =>
        {
            return new RedisResolver((key, database) =>
            {
                lock (Locker)
                {
                    if (RedisDictionary.TryGetValue(key, out var multiplexer))
                        return multiplexer.GetDatabase(database);

                    // 后来注册的情况
                    var optionsSnapshot = provider.ResolveOptional<IOptionsSnapshot<RedisOptions>>();
                    var redisOption = optionsSnapshot?.Get(key);

                    if (redisOption == null)
                        throw new KeyNotFoundException($"未添加{key}的Redis连接配置");
                    var connectStr = redisOption.GenConnectStr();
                    var logger = provider.Resolve<ILogger<IConnectionMultiplexer>>();

                    logger.LogInformation($"连接数据库 {connectStr}");

                    var multipartContent = ConnectionMultiplexer.Connect(connectStr);

                    RedisDictionary.Add(key, multipartContent);

                    return multipartContent.GetDatabase(database);
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