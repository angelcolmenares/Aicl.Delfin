using System;
﻿using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using ServiceStack.ServiceInterface;
using ServiceStack.OrmLite;
using Mono.Linq.Expressions;
using ServiceStack.Common.Web;


namespace Aicl.Delfin.Interface
{
	[RequiresAuthenticateAttribute(ApplyTo.Get)]
	//[PermissionAttribute(ApplyTo.All,"Cliente.update")]
	public class OfertaInformeService:AppRestService<OfertaInformeRequest>
	{
		public override object OnGet (OfertaInformeRequest request)
		{
			try{


				var visitor = ReadExtensions.CreateExpression<OfertaInforme>();
				var predicate = PredicateBuilder.True<OfertaInforme>();
	
				if(request.Desde!=default(DateTime)){
						predicate= q=>q.FechaEnvio>=request.Desde;
				}
				else
					throw HttpError.Unauthorized("Debe Indicar la fecha de inicio del informe (Desde)");

				if(request.Hasta!=default(DateTime)){
					predicate= predicate.OrElse(q=>q.FechaEnvio<=request.Hasta);
				}
				else
					throw HttpError.Unauthorized("Debe Indicar la fecha de terminacion del informe (Hasta)");

				predicate= predicate.AndAlso(q=>q.FechaAnulado==null);

				visitor.Where(predicate);

				return Factory.Execute(proxy=>{
					                	
					return new  Response<OfertaInforme>(){
						Data=proxy.Get(visitor)
                	};

				});
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<OfertaInforme>>(e,"GetOfertaInformeError");
			}
		}


	}
}
