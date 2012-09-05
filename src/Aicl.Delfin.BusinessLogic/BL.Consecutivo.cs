using System.Linq.Expressions;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.DataAccess;
using Mono.Linq.Expressions;
using System.Collections.Generic;

namespace Aicl.Delfin.BusinessLogic
{
	public static partial class BL
	{
		#region Get
        public static Response<Consecutivo> Get(this Consecutivo request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
            return factory.Execute(proxy=>{

				long? totalCount=null;

				var paginador= new Paginador(httpRequest);
            	
                var predicate = PredicateBuilder.True<Consecutivo>();

                var visitor = ReadExtensions.CreateExpression<Consecutivo>();
				visitor.Where(predicate);
                if(paginador.PageNumber.HasValue)
                {
					visitor.Select(r=> Sql.Count(r.Id));
					totalCount= proxy.Count(visitor);
					visitor.Select();
                    int rows= paginador.PageSize.HasValue? paginador.PageSize.Value:BL.PageSize;
                    visitor.Limit(paginador.PageNumber.Value*rows, rows);
                }
                
				return new Response<Consecutivo>(){
                	Data=proxy.Get(visitor),
                	TotalCount=totalCount
            	};
            });
  
        }
        #endregion 


		#region Post
        public static Response<Consecutivo> Post(this Consecutivo request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Create(request);
			});

			List<Consecutivo> data = new List<Consecutivo>();
			data.Add(request);
			
			return new Response<Consecutivo>(){
				Data=data
			};	
		}
		#endregion Post


		#region Put
        public static Response<Consecutivo> Put(this Consecutivo request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Update(request);
			});

			List<Consecutivo> data = new List<Consecutivo>();
			data.Add(request);
			
			return new Response<Consecutivo>(){
				Data=data
			};	
		}
		#endregion Put

		#region Delete
        public static Response<Consecutivo> Delete(this Consecutivo request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Delete<Consecutivo>(q=>q.Id==request.Id);
			});

			List<Consecutivo> data = new List<Consecutivo>();
			data.Add(request);
			
			return new Response<Consecutivo>(){
				Data=data
			};	
		}
		#endregion Delete

	}
}

