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
using ServiceStack.Common.Web;

namespace Aicl.Delfin.BusinessLogic
{
	public static partial class BL
	{
		#region Get
        public static Response<Contacto> Get(this Contacto request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
            return factory.Execute(proxy=>{

				long? totalCount=null;

				var paginador= new Paginador(httpRequest);
            	var queryString= httpRequest.QueryString;

				var visitor = ReadExtensions.CreateExpression<Contacto>();
                var predicate = PredicateBuilder.True<Contacto>();

				if(request.IdCliente!=default(int))
					predicate= q=>q.IdCliente==request.IdCliente;

				else if(!request.Nombre.IsNullOrEmpty()) 
				{
	                predicate= q=>q.Nombre.StartsWith(request.Nombre) ;
					visitor.OrderBy(r=>r.Nombre);
				}


				var qs= queryString["Activo"];
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
                                
                
                
				return new Response<Contacto>(){
                	Data=proxy.Get(visitor),
                	TotalCount=totalCount
            	};
            });
  
        }
        #endregion 


		#region Post
        public static Response<Contacto> Post(this Contacto request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			if(request.IdCliente==default(int))
				throw HttpError.Unauthorized("Debe Indicar el IdCliente");

			if(request.Nombre.IsNullOrEmpty() || request.Nombre.Trim()=="")
			throw HttpError.Unauthorized("Debe Indicar el Nombre del Contacto");

			factory.Execute(proxy=>{
				proxy.Create(request);
			});

			List<Contacto> data = new List<Contacto>();
			data.Add(request);
			
			return new Response<Contacto>(){
				Data=data
			};	
		}
		#endregion Post


		#region Put
        public static Response<Contacto> Put(this Contacto request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			if(request.IdCliente==default(int))
				throw HttpError.Unauthorized("Debe Indicar el IdCliente");

			if(request.Nombre.IsNullOrEmpty() || request.Nombre.Trim()=="")
			throw HttpError.Unauthorized("Debe Indicar el Nombre del Contacto");

			factory.Execute(proxy=>{
				proxy.Update(request);
			});

			List<Contacto> data = new List<Contacto>();
			data.Add(request);
			
			return new Response<Contacto>(){
				Data=data
			};	
		}
		#endregion Put

		#region Delete
        public static Response<Contacto> Delete(this Contacto request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.Delete<Contacto>(q=>q.Id==request.Id);
			});

			List<Contacto> data = new List<Contacto>();
			data.Add(request);
			
			return new Response<Contacto>(){
				Data=data
			};	
		}
		#endregion Delete

	}
}

