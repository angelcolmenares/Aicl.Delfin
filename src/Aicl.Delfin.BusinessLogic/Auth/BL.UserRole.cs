using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.DataAccess;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
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
        public static Response<UserRole> Get(this UserRole request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
            return factory.Execute(proxy=>{

				var userSession = httpRequest.GetSession();

				long? totalCount=null;

				var paginador= new Paginador(httpRequest);

				var visitor = ReadExtensions.CreateExpression<UserRole>();
				var predicate = PredicateBuilder.True<UserRole>();


				if(request.UserId==default(int))
					request.UserId= int.Parse(userSession.UserAuthId);
				else if(!(userSession.HasRole(RoleNames.Admin)) &&
				        request.UserId != int.Parse(userSession.UserAuthId)
				        )
				{
					throw HttpError.Unauthorized("Usuario no puede leer listado de roles de otro usuario");
				}

				predicate= q=>q.UserId==request.UserId;

								                
				visitor.Where(predicate);
                if(paginador.PageNumber.HasValue)
                {
					visitor.Select(r=> Sql.Count(r.Id));
					totalCount= proxy.Count(visitor);
					visitor.Select();
                    int rows= paginador.PageSize.HasValue? paginador.PageSize.Value:BL.ResponsePageSize;
                    visitor.Limit(paginador.PageNumber.Value*rows, rows);
                }
                                
                
				return new Response<UserRole>(){
                	Data=proxy.Get(visitor),
                	TotalCount=totalCount
            	};
            });
  
        }
        #endregion Get

		#region Post
        public static Response<UserRole> Post(this UserRole request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Create(request);
			});

			List<UserRole> data = new List<UserRole>();
			data.Add(request);
			
			return new Response<UserRole>(){
				Data=data
			};	
		}
		#endregion Post



		#region Delete
        public static Response<UserRole> Delete(this UserRole request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Delete<UserRole>(q=>q.Id==request.Id);
			});

			List<UserRole> data = new List<UserRole>();
			data.Add(request);
			
			return new Response<UserRole>(){
				Data=data
			};	
		}
		#endregion Delete


	}
}

