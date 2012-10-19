using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.DataAccess;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using Aicl.Delfin.Model.Operations;
using ServiceStack.OrmLite;
using Mono.Linq.Expressions;

namespace Aicl.Delfin.BusinessLogic
{
	public static partial class BL
	{
		#region Get
        public static Response<RolePermission> Get(this RolePermission request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
            return factory.Execute(proxy=>{

				long? totalCount=null;

				var paginador= new Paginador(httpRequest);

				var visitor = ReadExtensions.CreateExpression<RolePermission>();
				var predicate = PredicateBuilder.True<RolePermission>();

				predicate= q=>q.AuthRoleId==request.AuthRoleId;
												                
				visitor.Where(predicate).OrderBy(f=>f.Name) ;
                if(paginador.PageNumber.HasValue)
                {
					visitor.Select(r=> Sql.Count(r.Id));
					totalCount= proxy.Count(visitor);
					visitor.Select();
                    int rows= paginador.PageSize.HasValue? paginador.PageSize.Value:BL.ResponsePageSize;
                    visitor.Limit(paginador.PageNumber.Value*rows, rows);
                }
                                
                
				return new Response<RolePermission>(){
                	Data=proxy.Get(visitor),
                	TotalCount=totalCount
            	};
            });
  
        }
        #endregion Get

		#region Post
        public static Response<RolePermission> Post(this RolePermission request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Create(request);
			});

			List<RolePermission> data = new List<RolePermission>();
			data.Add(request);
			
			return new Response<RolePermission>(){
				Data=data
			};	
		}
		#endregion Post



		#region Delete
        public static Response<RolePermission> Delete(this RolePermission request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Delete<RolePermission>(q=>q.Id==request.Id);
			});

			List<RolePermission> data = new List<RolePermission>();
			data.Add(request);
			
			return new Response<RolePermission>(){
				Data=data
			};	
		}
		#endregion Delete


	}
}

