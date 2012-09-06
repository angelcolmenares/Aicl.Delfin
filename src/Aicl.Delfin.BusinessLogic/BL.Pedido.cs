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
            	
                var predicate = PredicateBuilder.True<Pedido>();

				if(request.Consecutivo!=default(int))
				{
					predicate= q=>q.Consecutivo==request.Consecutivo;
				}
				else
				{
					if(!request.NombreCliente.IsNullOrEmpty())
						predicate= q=>q.NombreCliente.StartsWith(request.NombreCliente);

					if(!request.NitCliente.IsNullOrEmpty())
						predicate= predicate.AndAlso(q=>q.NitCliente.StartsWith(request.NitCliente));
				}

                var visitor = ReadExtensions.CreateExpression<Pedido>();
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
				request.FechaActualizacion=request.FechaActualizacion;
				request.IdAceptadoPor=null;
				request.IdAnuladoPor=null;
				request.IdEnviadoPor=null;

				var fp = proxy.CheckExistAndActivo<FormaPago>(request.IdFormaPago, f=>f.Descripcion);
				request.DescripcionFormaPago= fp.Descripcion;

				var contacto = proxy.CheckExistAndActivo<Contacto>(request.IdContacto, f=>f.Nombre);
				request.NombreContacto= contacto.Nombre;
				request.CargoContacto= contacto.Cargo;
				request.CelularContacto= contacto.Celular;
				request.CodigoPostalContacto= contacto.CodigoPostal;
				request.DireccionContacto= contacto.Direccion;
				request.FaxContacto= contacto.Fax;
				request.MailContacto= contacto.Mail;
				request.TelefonoContacto=contacto.Telefono;

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
				proxy.Update(request);
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
			factory.Execute(proxy=>{
				proxy.Delete<Pedido>(q=>q.Id==request.Id);
			});

			List<Pedido> data = new List<Pedido>();
			data.Add(request);
			
			return new Response<Pedido>(){
				Data=data
			};	
		}
		#endregion Delete

	}
}

/*

var fp = proxy.FirstOrDefaultById<FormaPago>(request.IdFormaPago);
				if(fp==default(FormaPago))
					throw HttpError.NotFound(string.Format("No existe FormaPago con Id: '{0}'", request.IdFormaPago));

				if(!fp.Activo)
					throw HttpError.Unauthorized(string.Format("FormaPago :'{0}-{1}' se encuentra inactiva", request.IdFormaPago,fp.Descripcion));

				request.DescripcionFormaPago= fp.Descripcion;

				var contacto = proxy.FirstOrDefaultById<Contacto>(request.IdContacto);

				if(contacto==default(Contacto))
					throw HttpError.NotFound(string.Format("No existe Contacto con Id: '{0}'", request.IdContacto));

				if (!contacto.Activo)
					throw HttpError.Unauthorized(string.Format("Contacto :'{0}-{1}' se encuentra inactivo",
					                                           request.IdContacto, contacto.Nombre));
*/					                                           