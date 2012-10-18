using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.DataAccess;
using System.Collections.Generic;
using Aicl.Delfin.Model.Operations;
using ServiceStack.OrmLite;
using Mono.Linq.Expressions;
using ServiceStack.Common;

namespace Aicl.Delfin.BusinessLogic
{
	public static partial class BL
	{
		#region Get
        public static Response<AuthPermission> Get(this AuthPermission request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
            return factory.Execute(proxy=>{

				long? totalCount=null;

				var paginador= new Paginador(httpRequest);

				var visitor = ReadExtensions.CreateExpression<AuthPermission>();
				var predicate = PredicateBuilder.True<AuthPermission>();
							
				if(!request.Name.IsNullOrEmpty()){
					predicate= q=>q.Name.Contains(request.Name);
				}

								                
				visitor.Where(predicate);
                if(paginador.PageNumber.HasValue)
                {
					visitor.Select(r=> Sql.Count(r.Id));
					totalCount= proxy.Count(visitor);
					visitor.Select();
                    int rows= paginador.PageSize.HasValue? paginador.PageSize.Value:BL.ResponsePageSize;
                    visitor.Limit(paginador.PageNumber.Value*rows, rows);
                }
                                
                
				return new Response<AuthPermission>(){
                	Data=proxy.Get(visitor),
                	TotalCount=totalCount
            	};
            });
  
        }
        #endregion Get

		#region Post
        public static Response<AuthPermission> Post(this AuthPermission request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Create(request);
			});

			List<AuthPermission> data = new List<AuthPermission>();
			data.Add(request);
			
			return new Response<AuthPermission>(){
				Data=data
			};	
		}
		#endregion Post


		#region Put
        public static Response<AuthPermission> Put(this AuthPermission request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Update(request);
			});

			List<AuthPermission> data = new List<AuthPermission>();
			data.Add(request);
			
			return new Response<AuthPermission>(){
				Data=data
			};	
		}
		#endregion Put



		#region Delete
        public static Response<AuthPermission> Delete(this AuthPermission request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Delete<AuthPermission>(q=>q.Id==request.Id);
			});

			List<AuthPermission> data = new List<AuthPermission>();
			data.Add(request);
			
			return new Response<AuthPermission>(){
				Data=data
			};	
		}
		#endregion Delete


	}
}

