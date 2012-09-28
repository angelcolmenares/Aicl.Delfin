using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Funq;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.Redis;
using ServiceStack.Common.Web;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.Logging.Support.Logging;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.ServiceInterface;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.MySql;
using ServiceStack.Common;
using ServiceStack.Text;

using Aicl.Delfin.Model.Types;
using Aicl.Delfin.DataAccess;
using Aicl.Delfin.Interface;


namespace Aicl.Delfin.Setup
{
	public class AppHost:AppHostHttpListenerBase
	{
		static ILog log;
		
		public AppHost (): base("Aicl.Delfin", typeof(AuthenticationService).Assembly)
		{
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
						{ "Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS" },
					},
				  DefaultContentType = ContentType.Json 
			});
						
			var config = new AppConfig(new ConfigurationResourceManager());
			container.Register(config);
						
            OrmLiteConfig.DialectProvider= MySqlDialectProvider.Instance;
			
			IDbConnectionFactory dbFactory = new OrmLiteConnectionFactory(
				ConfigUtils.GetConnectionString("ApplicationDb"));


			var factory = new Factory(){
				DbFactory=dbFactory
			};

			container.Register<Factory>(factory);		

            log.InfoFormat("Configurando sistema de autenticacion");
			var cu = ConfigureAuth(container);

            log.InfoFormat("Configurando role Admin");
            ConfigurePermissions(dbFactory, cu);

			CreateAppTables(dbFactory);
			CreateCiudades(dbFactory);
			CreateFormasPago(dbFactory);
			CreateConsecutivos(dbFactory);

			var user= CrearDemoUser();
			if (user!=default(UserAuth))CreateRoles(dbFactory, user);

			CreateEmpresa(factory);

			log.InfoFormat("AppHost Configured: " + DateTime.Now);
		}
		

		void CreateAppTables(IDbConnectionFactory factory)
        {
			log.InfoFormat("Creando AppTables....");

			var appSettings = new ConfigurationResourceManager();

			var fileName= appSettings.Get<string>("AssemblyWithModels", string.Empty);
			if(string.IsNullOrEmpty( fileName )){
				log.InfoFormat("AssemblyWithModels NO Definido");
				return;
			}

			var nameSpace= appSettings.Get<string>("ModelsNamespace", string.Empty);
			var ignore = appSettings.Get<string>("Ignore", string.Empty);

			string [] ilist= ignore.Split(',');

			Assembly assembly = Assembly.LoadFrom(fileName);

			if (assembly==null){
				log.InfoFormat("AssemblyWithModels NO pudo ser cargado");
				return;
			}

			var recreateAppTables = appSettings.Get<bool>("RecreateAppTables", false);

			factory.Exec(dbCmd=>{
				foreach(Type t in  assembly.GetTypes()){
					if (t.Namespace==nameSpace && !( t.Name.StartsWith("Auth") || t.Name.StartsWith("Userauth"))
					    && ! t.IsInterface
					    )
					{	
						log.InfoFormat( "Creando {0} ", t.Name);
						try{
							dbCmd.CreateTable(recreateAppTables &&  !ilist.Contains(t.Name), t);
							log.InfoFormat( "Tabla {0} creada", t.Name);
						}
						catch(Exception e ){
							log.InfoFormat( "Error al crear  {0} : {1} ", t.Name, e.Message);
							Console.WriteLine("Enter para continuar");
							Console.ReadLine();
						}
					}
				}
			});

			log.InfoFormat("AppTables ok");
		}

		void CreateCiudades(IDbConnectionFactory factory)
        {
			log.InfoFormat("Creando Ciudades....");

			var appSettings = new ConfigurationResourceManager();

			var crear= appSettings.Get<bool>("CreateCiudades", false);
			if(!crear){
				log.InfoFormat("Crear ciudades NO");
				return;
			}

			factory.Exec(dbCmd=>{
				dbCmd.DeleteAll<Ciudad>();
				dbCmd.InsertAll(new Ciudades());
			});

			log.InfoFormat("Crear Ciudades ok");
		}

		void CreateFormasPago(IDbConnectionFactory factory)
        {
			log.InfoFormat("Creando FormasPago....");

			var appSettings = new ConfigurationResourceManager();

			var crear= appSettings.Get<bool>("CreateFormasPago", false);
			if(!crear){
				log.InfoFormat("Crear FormasPago NO");
				return;
			}

			factory.Exec(dbCmd=>{
				dbCmd.DeleteAll<FormaPago>();
				dbCmd.InsertAll(new FormasPago());
			});

			log.InfoFormat("Crear FormasPago ok");
		}

		void CreateConsecutivos(IDbConnectionFactory factory)
        {
			log.InfoFormat("Creando Consecutivos....");

			var appSettings = new ConfigurationResourceManager();

			var crear= appSettings.Get<bool>("CreateConsecutivos", false);
			if(!crear){
				log.InfoFormat("Crear Consecutivos NO");
				return;
			}

			factory.Exec(dbCmd=>{
				dbCmd.DeleteAll<Consecutivo>();
				dbCmd.InsertAll(new Consecutivos());
			});

			log.InfoFormat("Crear Consecutivos ok");
		}



        void ConfigurePermissions(IDbConnectionFactory factory, CreatedUsers users)
        {

            var appSettings = new ConfigurationResourceManager();

			var reCreatePermissionsTables = appSettings.Get<bool>("ReCreatePermissionsTables", false);

			Console.WriteLine("Recrear PermissionsTables : {0}", reCreatePermissionsTables);

            factory.Exec(dbCmd=>{

                log.InfoFormat("Creando Auth tablas");
                dbCmd.CreateTable<AuthPermission>(reCreatePermissionsTables);
                dbCmd.CreateTable<AuthRole>(reCreatePermissionsTables);
                dbCmd.CreateTable<AuthRolePermission>(reCreatePermissionsTables);
                dbCmd.CreateTable<AuthRoleUser>(reCreatePermissionsTables);
                log.InfoFormat("Auth Tablas creadas");

				if (!appSettings.Get("AddUsers", false)) {
					log.InfoFormat("AddUsers NO");
					return ;
				}

                AuthRole aur= dbCmd.FirstOrDefault<AuthRole>(r=> r.Name=="Admin");
                if(aur==default(AuthRole))
                {
                    log.InfoFormat("Creando Admin Role");
                    aur= new AuthRole(){Name="Admin", Title="Admin"};
                    dbCmd.Insert(aur);
                    aur.Id=Convert.ToInt32(dbCmd.GetLastInsertId());
                    log.InfoFormat("Admin Role Creado");
                }

                AuthRoleUser auru= dbCmd.FirstOrDefault<AuthRoleUser>(r=> r.AuthRoleId==aur.Id &&
                                                                      r.UserId==users.Admin.Id);
                if(auru==default(AuthRoleUser))
                {
                    log.InfoFormat("Asignando  Admin Role al usuario Admin");
                    auru=new AuthRoleUser(){UserId=users.Admin.Id, AuthRoleId= aur.Id};
                    dbCmd.Insert(auru);
                    log.InfoFormat("Admin Role asignado al usuario Admin");
                }

             });

        }

		
		CreatedUsers ConfigureAuth(Container container){
			
			var appSettings = new ConfigurationResourceManager();
			double se= appSettings.Get("DefaultSessionExpiry", 480);
			AuthProvider.DefaultSessionExpiry=TimeSpan.FromMinutes(se);			

			if (appSettings.Get("EnableRedisForAuthCache", false)){
				string cacheHost= appSettings.Get("AuthCacheHost", "localhost:6379");			
				int cacheDb= appSettings.Get("AuthCacheDb",8);				
										
				string cachePassword= appSettings.Get("AuthCachePassword",string.Empty);
						
				var p = new PooledRedisClientManager(new string[]{cacheHost},
							new string[]{cacheHost},
							cacheDb); 
				
				if(! string.IsNullOrEmpty(cachePassword))
					p.GetClient().Password= cachePassword;
				
				container.Register<ICacheClient>(p);
			}
			else
			{
				container.Register<ICacheClient>(new MemoryCacheClient());	
			}
			
			Plugins.Add(new AuthFeature(
				 () => new AuthUserSession(), // or Use your own typed Custom AuthUserSession type
				new IAuthProvider[]
        	{
				new AuthenticationProvider(){SessionExpiry=TimeSpan.FromMinutes(se)}
        	})
			{
				IncludeAssignRoleServices=false, 
			});
		    				
			var dbFactory = new OrmLiteConnectionFactory(ConfigUtils.GetConnectionString("UserAuth")) ;
			
			OrmLiteAuthRepository authRepo = new OrmLiteAuthRepository(
				dbFactory
			);
			
			container.Register<IUserAuthRepository>(
				c => authRepo
			); //Use OrmLite DB Connection to persist the UserAuth and AuthProvider info

			
			if (appSettings.Get("EnableRegistrationFeature", false))
				Plugins.Add( new  RegistrationFeature());
			
			
			
			if (!appSettings.Get("AddUsers", false)) return default(CreatedUsers);
			
			
			// addusers
			var oldL =MySqlDialectProvider.Instance.DefaultStringLength;
			
			MySqlDialectProvider.Instance.DefaultStringLength=64;
			if (appSettings.Get("RecreateAuthTables", false))
				authRepo.DropAndReCreateTables(); //Drop and re-create all Auth and registration tables
			else{
				authRepo.CreateMissingTables();   //Create only the missing tables				
			}
			MySqlDialectProvider.Instance.DefaultStringLength=oldL;
						
		    //Add admin user  
			string userName = "admin";
			string password = "aqPxym161t";
		
			List<string> permissions= new List<string>(
			new string[]{	
		
			});
			
            CreatedUsers cu = new CreatedUsers();

            var userAuth=authRepo.GetUserAuthByUserName(userName);

			if ( userAuth== default(UserAuth) ){
                log.InfoFormat("creando usuario:'{0}'", userName);
				List<string> roles= new List<string>();
				roles.Add(RoleNames.Admin);
			    string hash;
			    string salt;
			    new SaltedHash().GetHashAndSaltString(password, out hash, out salt);
			    authRepo.CreateUserAuth(new UserAuth {
				    DisplayName = "Admin Delfin",
			        Email = userName+"@mail.com",
			        UserName = userName,
			        FirstName = "",
			        LastName = "Administrador de la Aplicacion",
			        PasswordHash = hash,
			        Salt = salt,
					Roles =roles,
					Permissions=permissions
			    }, password);
                log.InfoFormat("Usuario:'{0}' creado", userName);
                userAuth= authRepo.GetUserAuthByUserName(userName);
			}
			
            cu.Admin= userAuth;

			userName = "alfredo.ramon";
			password = "74wdln12";
		
			permissions= new List<string>(
			new string[]{	
			
			});
			
            userAuth= authRepo.GetUserAuthByUserName(userName);
			if ( userAuth== default(UserAuth) ){
                log.InfoFormat("creando usuario:'{0}'", userName);
				List<string> roles= new List<string>();
				roles.Add("User");
				string hash;
			    string salt;
			    new SaltedHash().GetHashAndSaltString(password, out hash, out salt);
			    authRepo.CreateUserAuth(new UserAuth {
				    DisplayName = "Alfredo Ramon",
			        Email = "alfredoramon@colmetrik.com",
			        UserName = userName,
			        FirstName = "",
			        LastName = "Director Administrativo y Comercial",
			        PasswordHash = hash,
			        Salt = salt,
					Roles =roles,
					Permissions=permissions
			    }, password);
                userAuth= authRepo.GetUserAuthByUserName(userName);
                log.InfoFormat("Usuario:'{0}' creado", userName);
			}

            cu.User= userAuth;

            return cu;
		}


		UserAuth CrearDemoUser(){

			log.InfoFormat("Creando DemoUser");

			var appSettings = new ConfigurationResourceManager();

			var crear= appSettings.Get<bool>("CreateDemoUser", false);
			if(!crear){
				log.InfoFormat("Crear DemoUser NO");
				return default(UserAuth);
			};


			var dbFactory = new OrmLiteConnectionFactory(ConfigUtils.GetConnectionString("UserAuth")) ;
			
			OrmLiteAuthRepository authRepo = new OrmLiteAuthRepository(
				dbFactory
			);

			string userName = "demo";
			string password = "12345678";
		
			List<string> permissions= new List<string>(
			new string[]{	
		
			});
			            

            var userAuth=authRepo.GetUserAuthByUserName(userName);

			if ( userAuth== default(UserAuth) ){
                log.InfoFormat("creando usuario:'{0}'", userName);
				List<string> roles= new List<string>();
				roles.Add("Demo");
			    string hash;
			    string salt;
			    new SaltedHash().GetHashAndSaltString(password, out hash, out salt);
			    authRepo.CreateUserAuth(new UserAuth {
				    DisplayName ="Demo Delfin" ,
			        Email = "angel.ignacio.colmenares@gmail.com",
			        UserName = userName,
			        FirstName = "",
			        LastName = "Demo Cotizador",
			        PasswordHash = hash,
			        Salt = salt,
					Roles =roles,
					Permissions=permissions
			    }, password);
                log.InfoFormat("Usuario:'{0}' creado", userName);
                userAuth= authRepo.GetUserAuthByUserName(userName);
			}
			           
			return userAuth;

		}


		void CreateRoles(IDbConnectionFactory factory, UserAuth user)
        {
			log.InfoFormat("Creando Roles");

			var appSettings = new ConfigurationResourceManager();

			var crear= appSettings.Get<bool>("CreateRoles", false);
			if(!crear){
				log.InfoFormat("Crear Roles NO");
				return;
			};

			string roleName="Gestion Ofertas";

			factory.Exec(dbCmd=>{
				var role= dbCmd.FirstOrDefault<AuthRole>(q=>q.Name== roleName);
				if(role==default(AuthRole)){
					role = new AuthRole {
						Name=roleName,
						Directory="pedido",
						Title="Ofertas",
						ShowOrder="01"
					};
					dbCmd.Insert(role);
					role.Id=Convert.ToInt32(dbCmd.GetLastInsertId());
				}
				log.InfoFormat("Role {0} id {1}", roleName, role.Id );
				var aur= dbCmd.FirstOrDefault<AuthRoleUser>( q=> q.UserId==user.Id && q.AuthRoleId==role.Id);

				if(aur==default(AuthRoleUser)){
					aur= new AuthRoleUser{
						AuthRoleId=role.Id,
						UserId= user.Id
					};
					dbCmd.Insert(aur);
				};

				roleName="Gestion Clientes";
				role= dbCmd.FirstOrDefault<AuthRole>(q=>q.Name== roleName);
				if(role==default(AuthRole)){
					role = new AuthRole {
						Name=roleName,
						Directory="cliente",
						Title="Clientes",
						ShowOrder="02"
					};
					dbCmd.Insert(role);
					role.Id=Convert.ToInt32(dbCmd.GetLastInsertId());
				}
				log.InfoFormat("Role {0} id {1}", roleName, role.Id );
				aur= dbCmd.FirstOrDefault<AuthRoleUser>( q=> q.UserId==user.Id && q.AuthRoleId==role.Id);
				if(aur==default(AuthRoleUser)){
					aur= new AuthRoleUser{
						AuthRoleId=role.Id,
						UserId= user.Id
					};
					dbCmd.Insert(aur);
				};

				roleName="Gestion Servicios";
				role= dbCmd.FirstOrDefault<AuthRole>(q=>q.Name== roleName);
				if(role==default(AuthRole)){
					role = new AuthRole {
						Name=roleName,
						Directory="servicio",
						Title="Servicios",
						ShowOrder="03"
					};
					dbCmd.Insert(role);
					role.Id=Convert.ToInt32(dbCmd.GetLastInsertId());
				}
				log.InfoFormat("Role {0} id {1}", roleName, role.Id );
				aur= dbCmd.FirstOrDefault<AuthRoleUser>( q=> q.UserId==user.Id && q.AuthRoleId==role.Id);
				if(aur==default(AuthRoleUser)){
					aur= new AuthRoleUser{
						AuthRoleId=role.Id,
						UserId= user.Id
					};
					dbCmd.Insert(aur);
				};


			});


		}

		void CreateEmpresa(Factory factory)
        {
			log.InfoFormat("Creando Empresa....");

			var appSettings = new ConfigurationResourceManager();

			var crear= appSettings.Get<bool>("CreateEmpresa", false);
			if(!crear){
				log.InfoFormat("Crear Empresa NO");
				return;
			}

			factory.Execute(proxy=>{
				var empresa = proxy.GetEmpresa();
				if(empresa==default(Empresa))
				{
					string ser =appSettings.Get<string>("Empresa",string.Empty);
					if( ser.IsNullOrEmpty()){
						empresa = new Empresa{
							Nit="00", 
							Nombre="Demo Empresa",
							Alias="demo delfin", 
							Direccion="SiempreViva",
							MailServerEnableSsl=true,
							MailServerPort=587,
							MailServerUrl="smtp.mailgun.org",
							MailServerUser="demo@aicl.mailgun.org",
							ApplicationMailBox="demo@aicl.mailgun.org",
							ApplicationHost="http://localhost:8080"

						};
					}
					else{
						empresa=JsonSerializer.DeserializeFromString<Empresa>(ser);
					}	

				}
				Console.WriteLine("Digita la clave para el servicio de correo[{0}]",empresa.MailServerPassword);
				var claveCorreo = Console.ReadLine();
				empresa.MailServerPassword=  claveCorreo.IsNullOrEmpty()?empresa.MailServerPassword:claveCorreo;
				if(empresa.Id==default(int))
					proxy.PostEmpresa(empresa);
				else
					proxy.PutEmpresa(empresa);

			});

			log.InfoFormat("Crear Empresa ok");
		}

		
	}

    public class CreatedUsers
    {
        public CreatedUsers(){}

        public UserAuth Admin {get; set;}
        public UserAuth User {get; set;}
    }
}