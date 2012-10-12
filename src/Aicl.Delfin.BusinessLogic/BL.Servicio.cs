using ServiceStack.Common;
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
        public static Response<Servicio> Get(this Servicio request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
            return factory.Execute(proxy=>{

				long? totalCount=null;

				var paginador= new Paginador(httpRequest);
            	
                var predicate = PredicateBuilder.True<Servicio>();

                var visitor = ReadExtensions.CreateExpression<Servicio>();

				if(!request.Nombre.IsNullOrEmpty()){
					predicate = predicate.AndAlso(q=> q.Nombre.Contains(request.Nombre));
				}

				visitor.Where(predicate).OrderBy(f=>f.Nombre);
                if(paginador.PageNumber.HasValue)
                {
					visitor.Select(r=> Sql.Count(r.Id));
					totalCount= proxy.Count(visitor);
					visitor.Select();
                    int rows= paginador.PageSize.HasValue? paginador.PageSize.Value:BL.ResponsePageSize;
                    visitor.Limit(paginador.PageNumber.Value*rows, rows);
                }
                
				return new Response<Servicio>(){
                	Data=proxy.Get(visitor),
                	TotalCount=totalCount
            	};
            });
  
        }
        #endregion 


		#region Post
        public static Response<Servicio> Post(this Servicio request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Create(request);
			});

			List<Servicio> data = new List<Servicio>();
			data.Add(request);
			
			return new Response<Servicio>(){
				Data=data
			};	
		}
		#endregion Post


		#region Put
        public static Response<Servicio> Put(this Servicio request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Update(request);
			});

			List<Servicio> data = new List<Servicio>();
			data.Add(request);
			
			return new Response<Servicio>(){
				Data=data
			};	
		}
		#endregion Put

		#region Delete
        public static Response<Servicio> Delete(this Servicio request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Delete<Servicio>(q=>q.Id==request.Id);
			});

			List<Servicio> data = new List<Servicio>();
			data.Add(request);
			
			return new Response<Servicio>(){
				Data=data
			};	
		}
		#endregion Delete

	}
}

