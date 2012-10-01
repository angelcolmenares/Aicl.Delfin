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
        public static Response<PedidoItem> Get(this PedidoItem request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
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
				request.ValorUnitario=procedimiento.ValorUnitario;
				request.PorcentajeIva=procedimiento.PorcentajeIva;
				request.DescripcionProcedimiento=procedimiento.Descripcion;
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
				request.ValorUnitario=procedimiento.ValorUnitario;
				request.PorcentajeIva=procedimiento.PorcentajeIva;

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
		#endregion Delete

	}
}

