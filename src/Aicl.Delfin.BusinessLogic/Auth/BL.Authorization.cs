using System.Linq;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.ServiceHost;

using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;

using Aicl.Delfin.DataAccess;
namespace Aicl.Delfin.BusinessLogic
{
	public static partial class BL
	{
				
		public static AuthorizationResponse GetAuthorizations(this Authorization request, 
		                                           Factory factory, IRequestContext requestContext){
			
			var httpRequest = requestContext.Get<IHttpRequest>();	
			IAuthSession session = httpRequest.GetSession();
						
			if (!session.HasRole(RoleNames.Admin))
			{
				request.UserId= int.Parse(session.UserAuthId);
			}
			
			List<AuthRole> roles = new List<AuthRole>();
			List<AuthPermission> permissions= new List<AuthPermission>();
			
            List<AuthRoleUser> aur= new List<AuthRoleUser>();
            List<AuthRole> rol = new List<AuthRole>();
            List<AuthPermission> per = new List<AuthPermission>();
            List<AuthRolePermission> rol_per = new List<AuthRolePermission>();
            
            factory.Execute(proxy=>
            {
				aur=  proxy.Get<AuthRoleUser>(q=>q.UserId==request.UserId);
				//proxy.GetByUserIdFromCache<AuthRoleUser>(request.UserId); // causa problemas .net !!! no en mono
                rol= proxy.GetFromCache<AuthRole>();
                per= proxy.GetFromCache<AuthPermission>();
                rol_per= proxy.GetFromCache<AuthRolePermission>();
            
                foreach( var r in aur)
                {
                    AuthRole ar= rol.First(x=>x.Id== r.AuthRoleId);
                    roles.Add(ar);
                    rol_per.Where(q=>q.AuthRoleId==ar.Id).ToList().ForEach(y=>{
                        AuthPermission up=  per.First( p=> p.Id== y.AuthPermissionId);
                        if( permissions.FindIndex(f=>f.Name==up.Name)<0) // .IndexOf(up) <0)
                            permissions.Add(up);
                    }) ;
                };    

            });
			
			return new AuthorizationResponse(){
				Permissions= permissions,
				Roles= roles,
			};
		}			
	}
}