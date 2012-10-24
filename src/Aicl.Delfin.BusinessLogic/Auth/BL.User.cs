using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.DataAccess;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.Common;
using ServiceStack.ServiceInterface.Auth;
using Aicl.Delfin.Model.Operations;
using ServiceStack.OrmLite;
using Mono.Linq.Expressions;
using ServiceStack.Common.Web;

namespace Aicl.Delfin.BusinessLogic
{
	public static partial class BL
	{
		#region Get
        public static Response<User> Get(this User request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
            return factory.Execute(proxy=>{

				var userSession = httpRequest.GetSession();

				//if(!( userSession.HasRole(RoleNames.Admin) ||
				//   userSession.HasPermission("User.read") ))
				//	throw HttpError.Unauthorized("Usuario no autorizado para leer listado de usuarios");

				long? totalCount=null;

				var paginador= new Paginador(httpRequest);

				var visitor = ReadExtensions.CreateExpression<User>();
				var predicate = PredicateBuilder.True<User>();

				if (userSession.HasRole(RoleNames.Admin))
				{
					if(request.Id!=default(int))
					predicate= q=>q.Id==request.Id;

					if(!request.UserName.IsNullOrEmpty()) 
	                	predicate= q=>q.UserName.StartsWith(request.UserName) ;

					if(userSession.UserName!=BL.AdminUser)
						predicate=predicate.AndAlso(q=>q.UserName!=BL.AdminUser);

				}
				else
				{
					var id = int.Parse(userSession.UserAuthId);
					predicate= q=>q.Id==id;
				}
				                
				visitor.Where(predicate).OrderBy(r=>r.UserName);;
                if(paginador.PageNumber.HasValue)
                {
					visitor.Select(r=> Sql.Count(r.Id));
					totalCount= proxy.Count(visitor);
					visitor.Select();
                    int rows= paginador.PageSize.HasValue? paginador.PageSize.Value:BL.ResponsePageSize;
                    visitor.Limit(paginador.PageNumber.Value*rows, rows);
                }
                                
                
				return new Response<User>(){
                	Data=proxy.Get(visitor),
                	TotalCount=totalCount
            	};
            });
  
        }
        #endregion Get

		#region Post
        public static Response<User> Post(this User request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {

			if(request.IsDummyPassword())
				throw HttpError.Unauthorized("password no válido");

			var authRepo = httpRequest.TryResolve<IUserAuthRepository>();
			if(authRepo==null)
				throw HttpError.NotFound("AuthRepository NO configurado");

			var  user= new UserAuth
			{	
				FirstName= request.FirstName,
				LastName= request.LastName,
				Email= request.Email,
				UserName= request.UserName,
				DisplayName = request.FirstName +" "+ request.LastName
			};
			user.Set<UserMeta>( new UserMeta{
				Cargo= request.Cargo,
				Activo=request.Activo,
				ExpiresAt= request.ExpiresAt
			});

			user = authRepo.CreateUserAuth(user, request.Password);
			request.Id= user.Id;
			request.SetDummyPassword();

			List<User> data = new List<User>();
			data.Add(request);
			
			return new Response<User>(){
				Data=data
			};	
		}
		#endregion Post


		#region Put
        public static Response<User> Put(this User request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			var userSession = httpRequest.GetSession();

			if(!( userSession.HasRole(RoleNames.Admin) ||
				    userSession.HasPermission("User.update") ))
				throw HttpError.Unauthorized("Usuario no autorizado para actualizar");


			var authRepo = httpRequest.TryResolve<IUserAuthRepository>();
			if(authRepo==null)
				throw HttpError.NotFound("AuthRepository NO configurado");

			var  user= authRepo.GetUserAuth(request.Id.ToString());

			if (!(request.Id== int.Parse(userSession.UserAuthId) ||
				userSession.HasRole(RoleNames.Admin)) )
				throw HttpError.Unauthorized("No puede cambiar los datos de otro usuario");

			if(user == default(UserAuth))
				throw HttpError.NotFound(
					string.Format("Usuario con Id:'{0}' NO encontrado",request.Id));


			var  newUser= new UserAuth
			{	
				Id= request.Id,
				FirstName= request.FirstName,
				LastName= request.LastName,
				Email= request.Email,
				UserName= request.UserName,
				DisplayName= request.FirstName+" "+request.LastName,
				ModifiedDate= System.DateTime.Now,
			};
			newUser.Set<UserMeta>( new UserMeta{
				Cargo= request.Cargo,
				Activo=request.Activo,
				ExpiresAt= request.ExpiresAt
			});

			if(request.Password.IsNullOrEmpty() 
			   ||  request.IsDummyPassword()){

				factory.Execute(proxy=>{

					proxy.Update<UserAuth>(
						newUser,
						ev=>ev.Where(q=>q.Id==request.Id).
					Update(f=> new {
						f.UserName, f.FirstName, f.LastName, f.Email, f.Meta,
						f.DisplayName,
						f.ModifiedDate
					}));
				});

			}

			else
				user = authRepo.UpdateUserAuth(user, newUser,request.Password);

			request.SetDummyPassword();

			List<User> data = new List<User>();
			data.Add(request);
			
			return new Response<User>(){
				Data=data
			};	
		}
		#endregion Put

		#region Delete
        public static Response<User> Delete(this User request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Delete<User>(q=>q.Id==request.Id);
			});

			List<User> data = new List<User>();
			data.Add(request);
			
			return new Response<User>(){
				Data=data
			};	
		}
		#endregion Delete


	}
}

