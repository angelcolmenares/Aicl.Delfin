using System;
using System.Globalization;
using ServiceStack.Common;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.FluentValidation;
using Aicl.Delfin.Model.Types;
using ServiceStack.Redis;

namespace Aicl.Delfin.Interface
{
	public class AuthenticationProvider:CredentialsAuthProvider
	{
		
		class CredentialsAuthValidator : AbstractValidator<Auth>
		{
			public CredentialsAuthValidator()
			{
				RuleFor(x => x.UserName).NotEmpty();
				RuleFor(x => x.Password).NotEmpty();
			}
		}
		
		public override bool TryAuthenticate(IServiceBase authService, string userName, string password)
		{
			var authRepo = authService.TryResolve<IUserAuthRepository>();
			if (authRepo == null)
			{
				Log.WarnFormat("Tried to authenticate without a registered IUserAuthRepository");
				return false;
			}
			
			var session = authService.GetSession();
			UserAuth userAuth = null;
			if (authRepo.TryAuthenticate(userName, password, out userAuth))
			{
				var usermeta =userAuth.Get<UserMeta>();
				if(usermeta!=default(UserMeta))
				{
					if(!usermeta.Activo)
						throw HttpError.Unauthorized("Usuario se encuentra INACTIVO");

					if(usermeta.ExpiresAt.HasValue && usermeta.ExpiresAt.Value<DateTime.Now)
						throw HttpError.Unauthorized("la cuenta  ha expirado");
				}

				var cache = authService.TryResolve<IRedisClientsManager>();
				if(cache!=null){
					var sessionId = authService.GetSessionId();
				    using (var client = cache.GetClient())
					{
						User user = new User();
						user.PopulateWith(userAuth);
						user.Activo=usermeta.Activo;
						user.Cargo= usermeta.Cargo;
						user.ExpiresAt= usermeta.ExpiresAt;
						client.Set<User>( user.GetUserDataUrn(sessionId,"_user_"), user,SessionExpiry.Value );

					}
				}
			
				session.PopulateWith(userAuth);
				session.IsAuthenticated = true;
				session.UserAuthId =  userAuth.Id.ToString(CultureInfo.InvariantCulture);
				//session.ProviderOAuthAccess = authRepo.GetUserOAuthProviders(session.UserAuthId)
				//								.ConvertAll(x => (IOAuthTokens)x);
                //userAuth.Meta fecha de expiracion , y otros datos!!!
				return true;
			}
			return false;
		}
		
		/*
		public override void OnAuthenticated (ServiceStack.ServiceInterface.IServiceBase authService, IAuthSession session, IOAuthTokens tokens, Dictionary<string, string> authInfo)
		{		
			base.OnAuthenticated (authService, session, tokens, authInfo);
		}
		*/
		
		public override object Authenticate(IServiceBase authService, IAuthSession session, Auth request)
		{
			new CredentialsAuthValidator().ValidateAndThrow(request);
			return CustomAuthenticate(authService, session, request.UserName, request.Password);
		}
		
		protected object CustomAuthenticate(IServiceBase authService, IAuthSession session, string userName, string password)
		{
			if (!LoginMatchesSession(session, userName))
			{
				authService.RemoveSession();
				session = authService.GetSession();
			}

			if (TryAuthenticate(authService, userName, password))
			{
                if (session.UserAuthName == null)
                    session.UserAuthName = userName;
                
                OnAuthenticated(authService, session, null, null);

				return new AuthResponse {
					UserName = userName,
					SessionId = session.Id,
				};
			}

			throw HttpError.Unauthorized("Usuario o Clave invalida");
		}
		/*
		public override bool IsAuthorized(IAuthSession session, IOAuthTokens tokens, Auth request=null)
		{
			return base.IsAuthorized(session, tokens, request);
		}
		*/
	}
}