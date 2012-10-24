using System;
﻿using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.BusinessLogic;
using ServiceStack.ServiceInterface;

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
				return request.Get(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<PedidoItem>>(e,"GetPedidoItemError");
			}
		}


		public override object OnPost (PedidoItem request)
		{
			try{
				return request.Post(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<PedidoItem>>(e,"PostPedidoItemError");
			}
		}


		public override object OnPut (PedidoItem request)
		{
			try{
				return request.Put(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<PedidoItem>>(e,"PutPedidoItemError");
			}
		}

		public override object OnDelete (PedidoItem request)
		{
			try{
				return request.Delete(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<PedidoItem>>(e,"DeletePedidoItemError");
			}
		}

	}
}

