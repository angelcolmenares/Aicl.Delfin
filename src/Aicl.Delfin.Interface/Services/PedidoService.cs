using System;
﻿using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.BusinessLogic;


namespace Aicl.Delfin.Interface
{
	public class PedidoService:AppRestService<Pedido>
	{
		public override object OnGet (Pedido request)
		{
			try{
				return request.Get(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Pedido>>(e,"GetPedidoError");
			}
		}


		public override object OnPost (Pedido request)
		{
			try{
				return request.Post(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Pedido>>(e,"PostPedidoError");
			}
		}


		public override object OnPut (Pedido request)
		{
			try{
				return request.Put(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Pedido>>(e,"PutPedidoError");
			}
		}

		public override object OnDelete (Pedido request)
		{
			try{
				return request.Delete(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Pedido>>(e,"DeletePedidoError");
			}
		}

	}
}

