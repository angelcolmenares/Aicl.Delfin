using System;
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
        public static Response<Cliente> Get(this Cliente request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
            return factory.Execute(proxy=>{

				long? totalCount=null;

				var paginador= new Paginador(httpRequest);
            	var queryString= httpRequest.QueryString;

                var predicate = PredicateBuilder.True<Cliente>();

				if(!request.Nombre.IsNullOrEmpty()) 
				{
	                predicate= q=>q.Nombre.Contains(request.Nombre) ;
				}

				if(!request.Nit.IsNullOrEmpty()) 
				{
	                predicate= predicate.AndAlso( q=>q.Nit.StartsWith(request.Nit));
				}

				var qs= queryString["Activo"];
				bool activo;
				if(bool.TryParse(qs,out activo)){
					predicate= predicate.AndAlso(q=>q.Activo==activo);
				}

                var visitor = ReadExtensions.CreateExpression<Cliente>();
				visitor.Where(predicate);
                if(paginador.PageNumber.HasValue)
                {
					visitor.Select(r=> Sql.Count(r.Id));
					totalCount= proxy.Count(visitor);
					visitor.Select();
                    int rows= paginador.PageSize.HasValue? paginador.PageSize.Value:BL.ResponsePageSize;
                    visitor.Limit(paginador.PageNumber.Value*rows, rows);
                }
                                
                visitor.OrderBy(r=>r.Nombre);
                
				return new Response<Cliente>(){
                	Data=proxy.Get(visitor),
                	TotalCount=totalCount
            	};
            });
  
        }
        #endregion 


		#region Post
        public static Response<Cliente> Post(this Cliente request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Create(request);
			});

			List<Cliente> data = new List<Cliente>();
			data.Add(request);
			
			return new Response<Cliente>(){
				Data=data
			};	
		}
		#endregion Post


		#region Put
        public static Response<Cliente> Put(this Cliente request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Update(request);
			});

			List<Cliente> data = new List<Cliente>();
			data.Add(request);
			
			return new Response<Cliente>(){
				Data=data
			};	
		}
		#endregion Put

		#region Delete
        public static Response<Cliente> Delete(this Cliente request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Delete<Cliente>(q=>q.Id==request.Id);
			});

			List<Cliente> data = new List<Cliente>();
			data.Add(request);
			
			return new Response<Cliente>(){
				Data=data
			};	
		}
		#endregion Delete

	}
}

