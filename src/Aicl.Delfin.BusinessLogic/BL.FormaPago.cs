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
        public static Response<FormaPago> Get(this FormaPago request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
            return factory.Execute(proxy=>{

				long? totalCount=null;

				var paginador= new Paginador(httpRequest);
            	
                var predicate = PredicateBuilder.True<FormaPago>();

                var visitor = ReadExtensions.CreateExpression<FormaPago>();
				visitor.Where(predicate);
                if(paginador.PageNumber.HasValue)
                {
					visitor.Select(r=> Sql.Count(r.Id));
					totalCount= proxy.Count(visitor);
					visitor.Select();
                    int rows= paginador.PageSize.HasValue? paginador.PageSize.Value:BL.ResponsePageSize;
                    visitor.Limit(paginador.PageNumber.Value*rows, rows);
                }
                
				return new Response<FormaPago>(){
                	Data=proxy.Get(visitor),
                	TotalCount=totalCount
            	};
            });
  
        }
        #endregion 


		#region Post
        public static Response<FormaPago> Post(this FormaPago request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Create(request);
			});

			List<FormaPago> data = new List<FormaPago>();
			data.Add(request);
			
			return new Response<FormaPago>(){
				Data=data
			};	
		}
		#endregion Post


		#region Put
        public static Response<FormaPago> Put(this FormaPago request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Update(request);
			});

			List<FormaPago> data = new List<FormaPago>();
			data.Add(request);
			
			return new Response<FormaPago>(){
				Data=data
			};	
		}
		#endregion Put

		#region Delete
        public static Response<FormaPago> Delete(this FormaPago request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Delete<FormaPago>(q=>q.Id==request.Id);
			});

			List<FormaPago> data = new List<FormaPago>();
			data.Add(request);
			
			return new Response<FormaPago>(){
				Data=data
			};	
		}
		#endregion Delete

	}
}

