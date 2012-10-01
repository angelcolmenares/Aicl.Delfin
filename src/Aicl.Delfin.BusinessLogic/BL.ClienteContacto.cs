using System;
using System.Linq.Expressions;
using ServiceStack.OrmLite;
using ServiceStack.Common;
using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.DataAccess;
using Mono.Linq.Expressions;

namespace Aicl.Delfin.BusinessLogic
{
	public static partial class BL
	{
		#region Get
        public static Response<ClienteContacto> Get(this ClienteContacto request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
            return factory.Execute(proxy=>{

				long? totalCount=null;

				var paginador= new Paginador(httpRequest);
            	var queryString= httpRequest.QueryString;

                var predicate = PredicateBuilder.True<ClienteContacto>();

				if(!request.Nombre.IsNullOrEmpty()) 
				{
	                predicate= q=>q.Nombre.StartsWith(request.Nombre) ;
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

                var visitor = ReadExtensions.CreateExpression<ClienteContacto>();
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
                
				return new Response<ClienteContacto>(){
                	Data=proxy.Get(visitor),
                	TotalCount=totalCount
            	};
            });
  
        }
        #endregion 

	}
}

