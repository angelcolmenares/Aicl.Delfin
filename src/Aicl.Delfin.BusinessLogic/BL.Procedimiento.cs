using ServiceStack.Common;
using System.Linq.Expressions;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.DataAccess;
using Mono.Linq.Expressions;
using System.Collections.Generic;
using ServiceStack.Common.Web;

namespace Aicl.Delfin.BusinessLogic
{
	public static partial class BL
	{
		#region Get
        public static Response<Procedimiento> Get(this Procedimiento request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
            return factory.Execute(proxy=>{

				long? totalCount=null;

				var paginador= new Paginador(httpRequest);
            	
				var visitor = ReadExtensions.CreateExpression<Procedimiento>();
                var predicate = PredicateBuilder.True<Procedimiento>();

				if(!request.Nombre.IsNullOrEmpty()){
					predicate= q=>q.Nombre.Contains(request.Nombre);
					visitor.OrderBy(f=>f.Nombre);
				}
                
				var qs= httpRequest.QueryString["Activo"];
				bool activo;
				if(bool.TryParse(qs,out activo)){
					predicate= predicate.AndAlso(q=>q.Activo==activo);
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
                


				return new Response<Procedimiento>(){
                	Data=proxy.Get(visitor),
                	TotalCount=totalCount
            	};
            });
  
        }
        #endregion 


		#region Post
        public static Response<Procedimiento> Post(this Procedimiento request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			if(request.Nombre.IsNullOrEmpty() || request.Nombre.Trim()=="")
				throw HttpError.Unauthorized("Debe Indicar el Nombre del Procedimiento");

			factory.Execute(proxy=>{
				proxy.Create(request);
			});

			List<Procedimiento> data = new List<Procedimiento>();
			data.Add(request);
			
			return new Response<Procedimiento>(){
				Data=data
			};	
		}
		#endregion Post


		#region Put
        public static Response<Procedimiento> Put(this Procedimiento request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			if(request.Nombre.IsNullOrEmpty() || request.Nombre.Trim()=="")
				throw HttpError.Unauthorized("Debe Indicar el Nombre del Procedimiento");

			factory.Execute(proxy=>{
				proxy.Update(request);
			});

			List<Procedimiento> data = new List<Procedimiento>();
			data.Add(request);
			
			return new Response<Procedimiento>(){
				Data=data
			};	
		}
		#endregion Put

		#region Delete
        public static Response<Procedimiento> Delete(this Procedimiento request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Delete<Procedimiento>(q=>q.Id==request.Id);
			});

			List<Procedimiento> data = new List<Procedimiento>();
			data.Add(request);
			
			return new Response<Procedimiento>(){
				Data=data
			};	
		}
		#endregion Delete

	}
}

