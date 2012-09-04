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
        public static Response<PedidoItem> Get(this PedidoItem request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
            return factory.Execute(proxy=>{

				long? totalCount=null;

				var paginador= new Paginador(httpRequest);
            	
                var predicate = PredicateBuilder.True<PedidoItem>();

                var visitor = ReadExtensions.CreateExpression<PedidoItem>();
				visitor.Where(predicate);
                if(paginador.PageNumber.HasValue)
                {
					visitor.Select(r=> Sql.Count(r.Id));
					totalCount= proxy.Count(visitor);
					visitor.Select();
                    int rows= paginador.PageSize.HasValue? paginador.PageSize.Value:BL.PageSize;
                    visitor.Limit(paginador.PageNumber.Value*rows, rows);
                }
                
				return new Response<PedidoItem>(){
                	Data=proxy.Get(visitor),
                	TotalCount=totalCount
            	};
            });
  
        }
        #endregion 


		#region Post
        public static Response<PedidoItem> Post(this PedidoItem request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Create(request);
			});

			List<PedidoItem> data = new List<PedidoItem>();
			data.Add(request);
			
			return new Response<PedidoItem>(){
				Data=data
			};	
		}
		#endregion Post


		#region Put
        public static Response<PedidoItem> Put(this PedidoItem request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Update(request);
			});

			List<PedidoItem> data = new List<PedidoItem>();
			data.Add(request);
			
			return new Response<PedidoItem>(){
				Data=data
			};	
		}
		#endregion Put

		#region Delete
        public static Response<PedidoItem> Delete(this PedidoItem request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Delete<PedidoItem>(q=>q.Id==request.Id);
			});

			List<PedidoItem> data = new List<PedidoItem>();
			data.Add(request);
			
			return new Response<PedidoItem>(){
				Data=data
			};	
		}
		#endregion Delete

	}
}

