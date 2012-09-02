using System;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using ServiceStack.OrmLite;
using ServiceStack.Redis;
using ServiceStack.Common.Utils;
using ServiceStack.DesignPatterns.Model;

namespace Aicl.Delfin.DataAccess
{
	public static class OrmLiteExtensions
	{		

		internal static double DiasEnCache=7;

		internal static void InsertAndAssertId<T>(this IDbCommand dbCmd,T request, 
		                                     SqlExpressionVisitor<T> visitor=null) 
			where T: IHasId<Int32>, new()
		{
			if(visitor==null) dbCmd.Insert<T>(request);
			else dbCmd.InsertOnly<T>(request,visitor);
		
            dbCmd.AssertId(request);	
		}

        internal static void AssertId<T>(this IDbCommand dbCmd,T request) 
            where T: IHasId<Int32>, new()
        {

            if(request.Id==default(int))
            {
                Type type = typeof(T);
                PropertyInfo pi= ReflectionUtils.GetPropertyInfo(type, OrmLiteConfig.IdField);
                var li = dbCmd.GetLastInsertId();
                ReflectionUtils.SetProperty(request, pi, Convert.ToInt32(li));  
            }
            
        }

		internal static List<T>  Get<T>(this IDbCommand dbCmd, IRedisClient redisClient)
            where T: new()
        {
            return dbCmd.Get<T>(redisClient, string.Format("urn:{0}", typeof(T).Name), null);
        }

        internal static List<T>  Get<T>(this IDbCommand dbCmd, IRedisClient redisClient, string cacheKey, SqlExpressionVisitor<T> visitor)
            where T: new()
        {
            
            return redisClient.Get(cacheKey, () =>
            {
                return visitor==null?  dbCmd.Select<T>():dbCmd.Select<T>(visitor) ;
            },
            TimeSpan.FromDays(DiasEnCache));
        }

       
	}
}