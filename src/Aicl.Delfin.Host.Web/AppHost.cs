using System;
using Funq;
using ServiceStack.Redis;
using ServiceStack.Common.Web;
using ServiceStack.Common.Utils;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.Logging.Support.Logging;
using ServiceStack.Logging.Log4Net ;
using ServiceStack.OrmLite;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.ServiceInterface;
using ServiceStack.OrmLite.MySql;

using Aicl.Delfin.DataAccess;
using Aicl.Delfin.Interface;
using ServiceStack.ServiceHost;

namespace Aicl.Delfin.Host.Web
{
	public class AppHost:AppHostBase
	{
		static ILog log;
		
		public AppHost (): base("Aicl.Delfin", typeof(AuthenticationService).Assembly)
		{
			var appSettings = new ConfigurationResourceManager();
			if (appSettings.Get("EnableLog4Net", false))
			{
				var cf="log4net.conf".MapHostAbsolutePath();
				log4net.Config.XmlConfigurator.Configure(
					new System.IO.FileInfo(cf));
				LogManager.LogFactory = new  Log4NetFactory();
			}
			else
				LogManager.LogFactory = new ConsoleLogFactory();
						
			log = LogManager.GetLogger(typeof (AppHost));

		}
		
		public override void Configure(Container container)
		{
			//Permit modern browsers (e.g. Firefox) to allow sending of any REST HTTP Method
			base.SetConfig(new EndpointHostConfig
			{
				GlobalResponseHeaders =
					{
						{ "Access-Control-Allow-Origin", "*" },
						{ "Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS, PATCH" },
					},
				DefaultContentType = ContentType.Json,
				//EnableFeatures =  Feature.All.Remove(Feature.Html),
			});
							
									
			ConfigureApp(container);
			log.InfoFormat("AppHost Configured: " + DateTime.Now);
		}
				
		
		
		void ConfigureApp(Container container){

			var appSettings = new ConfigurationResourceManager();
                    
            double se= appSettings.Get("DefaultSessionExpiry", 480);
            AuthProvider.DefaultSessionExpiry=TimeSpan.FromMinutes(se);         
                                   
            string cacheHost= appSettings.Get("REDISTOGO_URL","localhost:6379").Replace("redis://redistogo-appharbor:","").Replace("/","");

            var redisClientManager = new BasicRedisClientManager(new string[]{cacheHost});
            
			OrmLiteConfig.DialectProvider= MySqlDialectProvider.Instance;
            
            IDbConnectionFactory dbFactory = new OrmLiteConnectionFactory(
                ConfigUtils.GetConnectionString("ApplicationDb"));
            
			var factory = new Factory(){
                DbFactory=dbFactory,
                RedisClientsManager = redisClientManager
			};

			var empresa = factory.Execute(proxy=>{
				return proxy.GetEmpresa();
			});

			var mailer = new Mailer(empresa);

            container.Register(appSettings);
            container.Register<Factory>(factory);
			container.Register(mailer);
            //container.Register<ICacheClient>(new MemoryCacheClient { FlushOnDispose = false });
            container.Register<IRedisClientsManager>(c => redisClientManager);
                        
            Plugins.Add(new AuthFeature(
                 () => new AuthUserSession(), // or Use your own typed Custom AuthUserSession type
                new IAuthProvider[]
            {
                new AuthenticationProvider(){SessionExpiry=TimeSpan.FromMinutes(se)}

            })
            {
                IncludeAssignRoleServices=false, 
            });
                            
            
            OrmLiteAuthRepository authRepo = new OrmLiteAuthRepository(
                dbFactory
            );
            
            container.Register<IUserAuthRepository>(
                c => authRepo
            ); 

            
            if(appSettings.Get("EnableRegistrationFeature", false))
                Plugins.Add( new  RegistrationFeature());
						
		}
		
	}
}