using System.Linq.Expressions;
using ServiceStack.OrmLite;
using ServiceStack.Common;
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
        public static Response<Ciudad> Get(this Ciudad request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
            return factory.Execute(proxy=>{

				long? totalCount=null;

				var paginador= new Paginador(httpRequest);

				var visitor = ReadExtensions.CreateExpression<Ciudad>();

                var predicate = PredicateBuilder.True<Ciudad>();

				if(request.Id!=default(int))
					predicate= q=>q.Id==request.Id;

				if(!request.Nombre.IsNullOrEmpty()) 
				{
	                predicate= q=>q.Nombre.StartsWith(request.Nombre) ;
					visitor.OrderBy(r=>r.Nombre);
				}

                
				visitor.Where(predicate);
                if(paginador.PageNumber.HasValue)
                {
					visitor.Select(r=> Sql.Count(r.Id));
					totalCount= proxy.Count(visitor);
					visitor.Select();
                    int rows= paginador.PageSize.HasValue? paginador.PageSize.Value:BL.PageSize;
                    visitor.Limit(paginador.PageNumber.Value*rows, rows);
                }
                               
                
                
				return new Response<Ciudad>(){
                	Data=proxy.Get(visitor),
                	TotalCount=totalCount
            	};
            });
  
        }
        #endregion Get

		#region Post
        public static Response<Ciudad> Post(this Ciudad request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Create(request);
			});

			List<Ciudad> data = new List<Ciudad>();
			data.Add(request);
			
			return new Response<Ciudad>(){
				Data=data
			};	
		}
		#endregion Post


		#region Put
        public static Response<Ciudad> Put(this Ciudad request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Update(request);
			});

			List<Ciudad> data = new List<Ciudad>();
			data.Add(request);
			
			return new Response<Ciudad>(){
				Data=data
			};	
		}
		#endregion Put

		#region Delete
        public static Response<Ciudad> Delete(this Ciudad request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Delete<Ciudad>(q=>q.Id==request.Id);
			});

			List<Ciudad> data = new List<Ciudad>();
			data.Add(request);
			
			return new Response<Ciudad>(){
				Data=data
			};	
		}
		#endregion Delete


	}
}

