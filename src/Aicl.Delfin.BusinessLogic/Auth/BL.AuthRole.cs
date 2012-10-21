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
        public static Response<AuthRole> Get(this AuthRole request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
            return factory.Execute(proxy=>{

				long? totalCount=null;

				var paginador= new Paginador(httpRequest);

				var visitor = ReadExtensions.CreateExpression<AuthRole>();
				var predicate = PredicateBuilder.True<AuthRole>();
							
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
                                
                
				return new Response<AuthRole>(){
                	Data=proxy.Get(visitor),
                	TotalCount=totalCount
            	};
            });
  
        }
        #endregion Get

		#region Post
        public static Response<AuthRole> Post(this AuthRole request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.DeleteFromCache<AuthRole>();
				proxy.Create(request);
			});

			List<AuthRole> data = new List<AuthRole>();
			data.Add(request);
			
			return new Response<AuthRole>(){
				Data=data
			};	
		}
		#endregion Post


		#region Put
        public static Response<AuthRole> Put(this AuthRole request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.DeleteFromCache<AuthRole>();
				proxy.Update(request);
			});

			List<AuthRole> data = new List<AuthRole>();
			data.Add(request);
			
			return new Response<AuthRole>(){
				Data=data
			};	
		}
		#endregion Put



		#region Delete
        public static Response<AuthRole> Delete(this AuthRole request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.DeleteFromCache<AuthRole>();
				proxy.Delete<AuthRole>(q=>q.Id==request.Id);
			});

			List<AuthRole> data = new List<AuthRole>();
			data.Add(request);
			
			return new Response<AuthRole>(){
				Data=data
			};	
		}
		#endregion Delete


	}
}

