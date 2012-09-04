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
        public static Response<ServicioProcedimiento> Get(this ServicioProcedimiento request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
            return factory.Execute(proxy=>{

				long? totalCount=null;

				var paginador= new Paginador(httpRequest);
            	
                var predicate = PredicateBuilder.True<ServicioProcedimiento>();

                var visitor = ReadExtensions.CreateExpression<ServicioProcedimiento>();
				visitor.Where(predicate);
                if(paginador.PageNumber.HasValue)
                {
					visitor.Select(r=> Sql.Count(r.Id));
					totalCount= proxy.Count(visitor);
					visitor.Select();
                    int rows= paginador.PageSize.HasValue? paginador.PageSize.Value:BL.PageSize;
                    visitor.Limit(paginador.PageNumber.Value*rows, rows);
                }
                
				return new Response<ServicioProcedimiento>(){
                	Data=proxy.Get(visitor),
                	TotalCount=totalCount
            	};
            });
  
        }
        #endregion 


		#region Post
        public static Response<ServicioProcedimiento> Post(this ServicioProcedimiento request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Create(request);
			});

			List<ServicioProcedimiento> data = new List<ServicioProcedimiento>();
			data.Add(request);
			
			return new Response<ServicioProcedimiento>(){
				Data=data
			};	
		}
		#endregion Post


		#region Put
        public static Response<ServicioProcedimiento> Put(this ServicioProcedimiento request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Update(request);
			});

			List<ServicioProcedimiento> data = new List<ServicioProcedimiento>();
			data.Add(request);
			
			return new Response<ServicioProcedimiento>(){
				Data=data
			};	
		}
		#endregion Put

		#region Delete
        public static Response<ServicioProcedimiento> Delete(this ServicioProcedimiento request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Delete<ServicioProcedimiento>(q=>q.Id==request.Id);
			});

			List<ServicioProcedimiento> data = new List<ServicioProcedimiento>();
			data.Add(request);
			
			return new Response<ServicioProcedimiento>(){
				Data=data
			};	
		}
		#endregion Delete

	}
}

