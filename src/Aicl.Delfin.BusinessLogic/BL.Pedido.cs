using System.Linq.Expressions;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.DataAccess;
using Mono.Linq.Expressions;
using System.Collections.Generic;
using System;
using ServiceStack.ServiceInterface;
using ServiceStack.Common;
using ServiceStack.Common.Web;

namespace Aicl.Delfin.BusinessLogic
{
	public static partial class BL
	{
		#region Get
        public static Response<Pedido> Get(this Pedido request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {

            return factory.Execute(proxy=>{

				long? totalCount=null;

				var paginador= new Paginador(httpRequest);
            	
				var visitor = ReadExtensions.CreateExpression<Pedido>();

                var predicate = PredicateBuilder.True<Pedido>();

				if(request.Consecutivo!=default(int))
				{
					predicate= q=>q.Consecutivo==request.Consecutivo;
				}
				else
				{
					visitor.OrderByDescending(f=>f.Consecutivo);

					if(!request.NombreCliente.IsNullOrEmpty()){
						predicate= q=>q.NombreCliente.StartsWith(request.NombreCliente);
						visitor.OrderBy(f=>f.NombreCliente);
					}

					if(!request.NitCliente.IsNullOrEmpty()){
						predicate= predicate.AndAlso(q=>q.NitCliente.StartsWith(request.NitCliente));
						visitor.OrderBy(f=>f.NitCliente);
					}
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
                
				return new Response<Pedido>(){
                	Data=proxy.Get(visitor),
                	TotalCount=totalCount
            	};

            });
  
        }
        #endregion 


		#region Post
        public static Response<Pedido> Post(this Pedido request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{

				var session = httpRequest.GetSession();

				request.IdCreadoPor= int.Parse(session.UserAuthId);
				request.Id=0;
				request.FechaAceptacion=null;
				request.FechaEnvio=null;
				request.FechaAnulado=null;
				request.FechaCreacion=DateTime.Today;
				request.FechaActualizacion=request.FechaCreacion;
				request.IdAceptadoPor=null;
				request.IdAnuladoPor=null;
				request.IdEnviadoPor=null;

				var fp = proxy.CheckExistAndActivo<FormaPago>(request.IdFormaPago, f=>f.Descripcion);
				request.DescripcionFormaPago= fp.Descripcion;

				var contacto = proxy.CheckExistAndActivo<Contacto>(request.IdContacto, f=>f.Nombre);
				request.NombreContacto= contacto.Nombre;

				var ciudad = CheckCiudad(proxy, request);
				request.NombreCiudad= ciudad.Nombre;
				request.CodigoCiudad= ciudad.Codigo;

				var cliente= proxy.CheckExistAndActivo<Cliente>(contacto.IdCliente, f=>f.Nombre);
				request.NitCliente=cliente.Nit;
				request.NombreCliente= cliente.Nombre;

				using (proxy.AcquireLock(Consecutivo.GetLockKey(BL.Cotizacion), BL.LockSeconds))
                {
					proxy.BeginDbTransaction();
					request.Consecutivo= proxy.GetNext(BL.Cotizacion).Valor;
					proxy.Create(request);
					proxy.CommitDbTransaction();
				}

			});

			List<Pedido> data = new List<Pedido>();
			data.Add(request);
			
			return new Response<Pedido>(){
				Data=data
			};	
		}
		#endregion Post


		#region Put
        public static Response<Pedido> Put(this Pedido request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				using (proxy.AcquireLock(request.GetLockKey(), BL.LockSeconds))
                {
					var old = proxy.FirstOrDefaultById<Pedido>(request.Id);

					if( old==default(Pedido)){
						throw HttpError.NotFound(string.Format("No existe Pedido con Id: '{0}'", request.Id));
					}

					if(old.FechaEnvio.HasValue){
						throw HttpError.Unauthorized(string.Format("Pedido '{0}' Id:'{1} No puede ser Actualizado. Estado:Enviado ", request.Consecutivo, request.Id));
					}

					if(old.FechaAnulado.HasValue){
						throw HttpError.Unauthorized(string.Format("Pedido '{0}' Id:'{1} No puede ser Actualizado. Estado:Anulado ", request.Consecutivo, request.Id));
					}

					request.FechaActualizacion= DateTime.Today;

					if(request.IdCiudadDestinatario!=default(int) &&
						request.IdCiudadDestinatario!=old.IdCiudadDestinatario){

						var ciudad = CheckCiudad(proxy, request);
						request.NombreCiudad= ciudad.Nombre;
						request.CodigoCiudad= ciudad.Codigo;
					}


					if(request.IdFormaPago!=default(int) &&
					   request.IdFormaPago!=old.IdFormaPago)
					{
						var fp = proxy.CheckExistAndActivo<FormaPago>(request.IdFormaPago, f=>f.Descripcion);
						request.DescripcionFormaPago= fp.Descripcion;
					}

					if(request.IdContacto!=default(int) &&
					   request.IdContacto!=old.IdContacto)
					{
						var contacto = proxy.CheckExistAndActivo<Contacto>(request.IdContacto, f=>f.Nombre);
						request.NombreContacto= contacto.Nombre;

						var cliente= proxy.CheckExistAndActivo<Cliente>(contacto.IdCliente, f=>f.Nombre);
						request.NitCliente=cliente.Nit;
						request.NombreCliente= cliente.Nombre;
					}

					proxy.Update(request,ev=>ev.Update(f=> 
					    new {
							f.CargoDestinatario, f.CelularDestinatario, f.DiasDeVigencia,
							f.DireccionDestinatario, f.FaxDestinatario, f.IdCiudadDestinatario,f.IdContacto,
							f.IdFormaPago,f.MailDestinatario, f.NitCliente, f.NombreDestinatario,f.TelefonoDestinatario,
							f.FechaActualizacion
						}).Where(q=>q.Id==request.Id)
					);

				}
			});

			List<Pedido> data = new List<Pedido>();
			data.Add(request);
			
			return new Response<Pedido>(){
				Data=data
			};	
		}
		#endregion Put

		#region Delete
        public static Response<Pedido> Delete(this Pedido request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			throw HttpError.Unauthorized("Operacion Borrar no autorizada para Pedidos");
		}
		#endregion Delete

		#region Patch
        public static Response<Pedido> Patch(this Pedido request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			List<string> actions = new List<string>(new string[]{"ENVIAR","ACEPTAR","ANULAR"});

			var action= httpRequest.PathInfo.Substring(httpRequest.PathInfo.LastIndexOf("/")+1).ToUpper();

			if(actions.IndexOf(action)<0)
				throw HttpError.Unauthorized(string.Format("Operacion: '{0}' no autorizada en Pedidos", action));

			var session = httpRequest.GetSession();

			factory.Execute(proxy=>{

				using (proxy.AcquireLock(request.GetLockKey(), BL.LockSeconds))
                {

					var old = proxy.FirstOrDefaultById<Pedido>(request.Id);

					if( old==default(Pedido)){
						throw HttpError.NotFound(string.Format("No existe Pedido con Id: '{0}'", request.Id));
					}

					if(old.FechaAnulado.HasValue){
						throw HttpError.Unauthorized(string.Format("Operacion:'{0}' No permitida. Pedido '{1}' Id:'{2} se encuentra anulado",action, request.Consecutivo, request.Id));
					}

					request.PopulateWith(old);

					if(action=="ENVIAR")
					{
						if(old.FechaEnvio.HasValue)
							throw HttpError.Conflict("Pedido ya se encuentra en estado Enviado");
						request.FechaEnvio=DateTime.Today;
						request.IdEnviadoPor= int.Parse(session.UserAuthId);
					}
					else if (action=="ACEPTAR")
					{
						if(old.FechaAceptacion.HasValue)
							throw HttpError.Conflict("Pedido ya se encuentra en estado Aceptado");

						if(!old.FechaEnvio.HasValue)
							throw HttpError.Conflict("El Pedido primero debe ser enviado");

						request.FechaAceptacion=DateTime.Today;
						request.IdAceptadoPor= int.Parse(session.UserAuthId);
					}
					else
					{
						request.FechaAnulado= DateTime.Today;
						request.IdAnuladoPor= int.Parse(session.UserAuthId);
					}

					proxy.Update(request, ev=>ev.Update(
						f=> new {
						f.FechaEnvio,f.FechaAceptacion,f.FechaAnulado, f.VigenteHasta,
						f.IdAceptadoPor, f.IdAnuladoPor, f.IdEnviadoPor
					}).Where(q=>q.Id==request.Id));
				}

			});

			List<Pedido> data = new List<Pedido>();
			data.Add(request);
			
			return new Response<Pedido>(){
				Data=data
			};	
		}
		#endregion Patch

            

		static Ciudad CheckCiudad(DALProxy proxy,  Pedido request)
		{
			if(request.IdCiudadDestinatario==0)
					throw HttpError.Unauthorized("Debe Indicar la ciudad Destino");
			var ciudad = proxy.FirstOrDefaultById<Ciudad>(request.IdCiudadDestinatario);
				if(ciudad==default(Ciudad))
					throw HttpError.NotFound(string.Format("No existe Ciudad con Id: '{0}'", request.IdCiudadDestinatario));
			return ciudad;
		}

	}
}
