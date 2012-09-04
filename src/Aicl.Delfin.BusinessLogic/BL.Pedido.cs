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
        public static Response<Pedido> Get(this Pedido request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
            return factory.Execute(proxy=>{

				long? totalCount=null;

				var paginador= new Paginador(httpRequest);
            	
                var predicate = PredicateBuilder.True<Pedido>();

                var visitor = ReadExtensions.CreateExpression<Pedido>();
				visitor.Where(predicate);
                if(paginador.PageNumber.HasValue)
                {
					visitor.Select(r=> Sql.Count(r.Id));
					totalCount= proxy.Count(visitor);
					visitor.Select();
                    int rows= paginador.PageSize.HasValue? paginador.PageSize.Value:BL.PageSize;
                    visitor.Limit(paginador.PageNumber.Value*rows, rows);
                }
                
				return new Response<Pedido>(){
                	Data=proxy.Get(visitor),
                	TotalCount=totalCount
            	};
            });
  
        }
        #endregion 


		#region Post
        public static Response<Pedido> Post(this Pedido request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Create(request);
			});

			List<Pedido> data = new List<Pedido>();
			data.Add(request);
			
			return new Response<Pedido>(){
				Data=data
			};	
		}
		#endregion Post


		#region Put
        public static Response<Pedido> Put(this Pedido request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Update(request);
			});

			List<Pedido> data = new List<Pedido>();
			data.Add(request);
			
			return new Response<Pedido>(){
				Data=data
			};	
		}
		#endregion Put

		#region Delete
        public static Response<Pedido> Delete(this Pedido request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Delete<Pedido>(q=>q.Id==request.Id);
			});

			List<Pedido> data = new List<Pedido>();
			data.Add(request);
			
			return new Response<Pedido>(){
				Data=data
			};	
		}
		#endregion Delete

	}
}

