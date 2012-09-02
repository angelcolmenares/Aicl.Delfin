using System;
using ServiceStack.ServiceInterface;
using ServiceStack.Redis;

namespace Aicl.Delfin.DataAccess
{
	public static class RedisExtensions
	{
	
		internal static T Get<T> (this IRedisClient redisClient,string cacheKey,
			Func<T> factoryFn,
		    TimeSpan? expiresIn=null)
		{
			var res = redisClient.Get<T>(cacheKey);
			if (res != null)
			{ 
				redisClient.CacheSet<T>(cacheKey, res, expiresIn);
				return res;
			}
			else
			{
				res= factoryFn();
				if (res != null ) redisClient.CacheSet<T>(cacheKey, res, expiresIn);
				return res;
			}
		}

	}
}

