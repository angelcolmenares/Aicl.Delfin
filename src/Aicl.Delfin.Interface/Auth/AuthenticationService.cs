using System;
﻿
using ServiceStack.Common;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;

using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.DataAccess;
using Aicl.Delfin.BusinessLogic;

namespace Aicl.Delfin.Interface
{
	public class AuthenticationService:RestServiceBase<Authentication>
	{
		public Factory Factory{ get; set;}
		
		public override object OnPost (Authentication request)
		{
			return OnGet(request);
		}
		

		public override object OnGet (Authentication request)
		{
			//try{
            AuthService authService = ResolveService<AuthService>();
            
            object fr= authService.Post(new Auth {
                provider = AuthService.CredentialsProvider,
                UserName = request.UserName,
                Password = request.Password
            }) ;
                        
            
            IAuthSession session = this.GetSession();
            
            if(!session.IsAuthenticated)
            {
                HttpError e = fr as HttpError;
                if(e!=null) throw e;
                
                Exception ex = fr as Exception;
                throw ex;
            };
            
            Aicl.Delfin.Model.Types.Authorization auth = new Aicl.Delfin.Model.Types.Authorization(){
                UserId= int.Parse(session.UserAuthId)
            };
            
            AuthorizationResponse aur = auth.GetAuthorizations(Factory,RequestContext);
            
            session.Permissions= aur.Permissions.ConvertAll(f=>f.Name);
			session.Roles= aur.Roles.ConvertAll(f=>f.Name);  //(from r in aur.Roles select r.Name).ToList();
            
            authService.SaveSession(session);
            return new AuthenticationResponse(){
                DisplayName= session.DisplayName.IsNullOrEmpty()? session.UserName: session.DisplayName,
                Roles= aur.Roles,
                Permissions= aur.Permissions
            };

		}
		
		public override object OnDelete (Authentication request)
		{
			AuthService authService = base.ResolveService<AuthService>();
			var response =authService.Delete(new Auth {
					provider = AuthService.LogoutAction
			});
				
			return response;
		}
		
	}
}
