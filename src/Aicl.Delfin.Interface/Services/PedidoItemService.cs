using System;
﻿using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.BusinessLogic;
using ServiceStack.ServiceInterface;
using ServiceStack.OrmLite;
using Mono.Linq.Expressions;
using ServiceStack.Common.Web;
using System.Collections.Generic;
using Aicl.Cayita;
using Aicl.Delfin.DataAccess;

namespace Aicl.Delfin.Interface
{

	[PermissionAttribute(ApplyTo.Post,"Pedido.update")]
	[RequiresAuthenticateAttribute(ApplyTo.Get)]
	[PermissionAttribute(ApplyTo.Put,"Pedido.update")]
	[PermissionAttribute(ApplyTo.Delete, "Pedido.update")]
	public class PedidoItemService:AppRestService<PedidoItem>
	{
		public override object OnGet (PedidoItem request)
		{
			try{
				var factory =Factory;
				var httpRequest= RequestContext.Get<IHttpRequest>();
				return factory.Execute(proxy=>{

				long? totalCount=null;

				var paginador= new Paginador(httpRequest);       
                var visitor = ReadExtensions.CreateExpression<PedidoItem>();
				var predicate = PredicateBuilder.True<PedidoItem>();

				if(request.IdPedido!=default(int)){
					predicate=q=>q.IdPedido== request.IdPedido;
					visitor.OrderBy(f=>f.Id);
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
                
				var data =proxy.Get(visitor);

				foreach(var d in data)
				{
					d.Descripcion= d.Descripcion.Decode();
					d.Nota= d.Nota.Decode();
				}

				return new Response<PedidoItem>(){
                	Data=data,
                	TotalCount=totalCount
            	};
            });
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<PedidoItem>>(e,"GetPedidoItemError");
			}
		}


		public override object OnPost (PedidoItem request)
		{
			try{
				var factory =Factory;
				factory.Execute(proxy=>{

				var pedido = proxy.FirstOrDefaultById<Pedido>(request.IdPedido);

				if( pedido==default(Pedido)){
					throw HttpError.NotFound(string.Format("Item no puede ser insertado. No existe Pedido con Id: '{0}'", request.IdPedido));
				}

				if(pedido.FechaEnvio.HasValue){
					throw HttpError.Unauthorized(string.Format("Item no puede ser insertado. Pedido '{0}' Id:'{1} No puede ser Actualizado. Estado:Enviado ", pedido.Consecutivo, pedido.Id));
				}

				if(pedido.FechaAnulado.HasValue){
					throw HttpError.Unauthorized(string.Format("Item no puede ser insertado. Pedido '{0}' Id:'{1} No puede ser Actualizado. Estado:Anulado ", pedido.Consecutivo, pedido.Id));
				}

				var servicio = proxy.CheckExistAndActivo<Servicio>(request.IdServicio, f=>f.Nombre);
				request.NombreServicio= servicio.Nombre;

				var procedimiento = proxy.CheckExistAndActivo<Procedimiento>(request.IdProcedimiento, f=>f.Nombre);
				var empresa = proxy.GetEmpresa(); 
				if(!empresa.CambiarPrecio){
					request.ValorUnitario=procedimiento.ValorUnitario;
				}
				request.PorcentajeIva=procedimiento.PorcentajeIva;
				
				request.DescripcionProcedimiento=procedimiento.Descripcion;
				request.Descripcion= request.Descripcion.Encode();
				request.Nota= request.Nota.Encode();
				proxy.Create(request);
				request.Descripcion= request.Descripcion.Decode();
				request.Nota= request.Nota.Decode();

			});

			List<PedidoItem> data = new List<PedidoItem>();
			data.Add(request);
			
			return new Response<PedidoItem>(){
				Data=data
			};
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<PedidoItem>>(e,"PostPedidoItemError");
			}
		}


		public override object OnPut (PedidoItem request)
		{
			try{
				var factory =Factory;
				factory.Execute(proxy=>{

				var pedido = proxy.FirstOrDefaultById<Pedido>(request.IdPedido);

				if( pedido==default(Pedido)){
					throw HttpError.NotFound(string.Format("Item no puede ser actualizado. No existe Pedido con Id: '{0}'", request.IdPedido));
				}

				if(pedido.FechaEnvio.HasValue){
					throw HttpError.Unauthorized(string.Format("Item no puede ser actualizado. Pedido '{0}' Id:'{1} No puede ser Actualizado. Estado:Enviado ", pedido.Consecutivo, pedido.Id));
				}

				if(pedido.FechaAnulado.HasValue){
					throw HttpError.Unauthorized(string.Format("Item no puede ser actualizado. Pedido '{0}' Id:'{1} No puede ser Actualizado. Estado:Anulado ", pedido.Consecutivo, pedido.Id));
				}

				var old = proxy.FirstOrDefaultById<PedidoItem>(request.Id);

				if( old==default(PedidoItem)){
					throw HttpError.NotFound(string.Format("No existe Item con Id: '{0}'", request.Id));
				}


				var servicio = proxy.CheckExistAndActivo<Servicio>(request.IdServicio, f=>f.Nombre);
				request.NombreServicio= servicio.Nombre;

				
				var procedimiento = proxy.CheckExistAndActivo<Procedimiento>(request.IdProcedimiento, f=>f.Nombre);
				var empresa = proxy.GetEmpresa(); 
				if(!empresa.CambiarPrecio){
					request.ValorUnitario=procedimiento.ValorUnitario;
				}
				request.PorcentajeIva=procedimiento.PorcentajeIva;

				request.Descripcion= request.Descripcion.Encode();
				request.Nota= request.Nota.Encode();
				proxy.Update(request);
				request.Descripcion= request.Descripcion.Decode();
				request.Nota= request.Nota.Decode();

			});

			List<PedidoItem> data = new List<PedidoItem>();
			data.Add(request);
			
			return new Response<PedidoItem>(){
				Data=data
			};	


			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<PedidoItem>>(e,"PutPedidoItemError");
			}
		}

		public override object OnDelete (PedidoItem request)
		{
			try{
				var factory =Factory;
				factory.Execute(proxy=>{
				var pedido = proxy.FirstOrDefaultById<Pedido>(request.IdPedido);

				if( pedido==default(Pedido)){
					throw HttpError.NotFound(string.Format("Item no puede ser borrado. No existe Pedido con Id: '{0}'", request.IdPedido));
				}

				if(pedido.FechaEnvio.HasValue){
					throw HttpError.Unauthorized(string.Format("Item no puede ser borrado. Pedido '{0}' Id:'{1} No puede ser Actualizado. Estado:Enviado ", pedido.Consecutivo, pedido.Id));
				}

				if(pedido.FechaAnulado.HasValue){
					throw HttpError.Unauthorized(string.Format("Item no puede ser borrado. Pedido '{0}' Id:'{1} No puede ser Actualizado. Estado:Anulado ", pedido.Consecutivo, pedido.Id));
				}

				proxy.Delete<PedidoItem>(q=>q.Id==request.Id);
				});

				List<PedidoItem> data = new List<PedidoItem>();
				data.Add(request);
				
				return new Response<PedidoItem>(){
					Data=data
				};	

			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<PedidoItem>>(e,"DeletePedidoItemError");
			}
		}

	}
}

