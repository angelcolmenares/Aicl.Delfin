using System;
using ServiceStack.Common;
﻿using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.BusinessLogic;
using ServiceStack.ServiceInterface;
using ServiceStack.OrmLite;
using Mono.Linq.Expressions;
using System.Collections.Generic;
using ServiceStack.Common.Web;


namespace Aicl.Delfin.Interface
{
	[RequiresAuthenticateAttribute(ApplyTo.Get)]
	[PermissionAttribute(ApplyTo.All,"Cliente.update")]
	public class TareaService:AppRestService<Tarea>
	{
		public override object OnGet (Tarea request)
		{
			try{
				var httpRequest= RequestContext.Get<IHttpRequest>();

				return Factory.Execute(proxy=>{

					long? totalCount=null;
					var paginador= new Paginador(httpRequest);       

					var visitor = ReadExtensions.CreateExpression<Tarea>();
					var predicate = PredicateBuilder.True<Tarea>();
					var userId = int.Parse(httpRequest.GetSession().UserAuthId);
					predicate= q=>q.UserId==userId;

					if(request.IdCliente.HasValue && request.IdCliente.Value!=default(int)){
						predicate=q=>q.IdCliente== request.IdCliente.Value;
					}

					var qs= httpRequest.QueryString["Cumplida"];
					bool cumplida;
					if(bool.TryParse(qs,out cumplida)){
						predicate= predicate.AndAlso(q=>q.Cumplida==cumplida);
					}


					visitor.Where(predicate).OrderByDescending(f=>f.Fecha);

	                if(paginador.PageNumber.HasValue)
	                {
						visitor.Select(r=> Sql.Count(r.Id));
						totalCount= proxy.Count(visitor);
						visitor.Select();
	                    int rows= paginador.PageSize.HasValue? paginador.PageSize.Value:BL.ResponsePageSize;
	                    visitor.Limit(paginador.PageNumber.Value*rows, rows);
	                }
                	
					return new  Response<Tarea>(){
						TotalCount=totalCount,
						Data=proxy.Get(visitor)
                	};

				});
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Tarea>>(e,"GetTareaError");
			}
		}

		public override object OnPost (Tarea request)
		{
			try{
				if( request.Tema.IsNullOrEmpty())
					throw HttpError.NotFound(string.Format("Debe indicar el tema de la tarea"));

				var httpRequest= RequestContext.Get<IHttpRequest>();

				request.UserId = int.Parse(httpRequest.GetSession().UserAuthId);

				Factory.Execute(proxy=>{
					proxy.Create(request);
				});

				List<Tarea> data = new List<Tarea>();
				data.Add(request);
			
				return new Response<Tarea>(){
					Data=data
				};

			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Tarea>>(e,"PostTareaError");
			}
		}


		public override object OnPut (Tarea request)
		{
			try{
				if( request.Tema.IsNullOrEmpty())
					throw HttpError.NotFound("Debe indicar el tema de la tarea");

				Factory.Execute(proxy=>{

					var oldData= proxy.FirstOrDefaultById<Tarea>(request.Id);

					if(oldData==default(Tarea))
						throw HttpError.NotFound(string.Format("No existe tarea con Id:'{0}'", request.Id));

					var httpRequest= RequestContext.Get<IHttpRequest>();

					var userId = int.Parse(httpRequest.GetSession().UserAuthId);

					if(oldData.UserId!=userId)
						throw HttpError.Unauthorized("No puede actualizar Tareas de otro usuario");

					proxy.Update(request);
				});

				List<Tarea> data = new List<Tarea>();
				data.Add(request);
			
				return new Response<Tarea>(){
					Data=data
				};

			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Tarea>>(e,"PutTareaError");
			}
		}


		public override object OnDelete (Tarea request)
		{
			try{
		
				Factory.Execute(proxy=>{
					var oldData= proxy.FirstOrDefaultById<Tarea>(request.Id);

					if(oldData==default(Tarea))
						throw HttpError.NotFound(string.Format("No existe tarea con Id:'{0}'", request.Id));

					var httpRequest= RequestContext.Get<IHttpRequest>();

					var userId = int.Parse(httpRequest.GetSession().UserAuthId);

					if(oldData.UserId!=userId)
						throw HttpError.Unauthorized("No puede Borrar Tareas de otro usuario");

					proxy.Delete<Tarea>(q=>q.Id==request.Id);

				});

				List<Tarea> data = new List<Tarea>();
				data.Add(request);
			
				return new Response<Tarea>(){
					Data=data
				};

			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Tarea>>(e,"DeleteTareaError");
			}
		}

	}
}

