using System;
﻿using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.BusinessLogic;
using ServiceStack.ServiceInterface;

namespace Aicl.Delfin.Interface
{
	[PermissionAttribute(ApplyTo.Get,"Pedido.read")]
	[PermissionAttribute(ApplyTo.Post,"Pedido.read")]
	public class PedidoPdfService:AppRestService<PedidoPdf>
	{
		public override object OnGet(PedidoPdf request)
		{
			try{
				var user = GetUser();
				return request.Get(Factory, RequestContext.Get<IHttpRequest>(), user);
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<PedidoPdf>>(e,"GetPedidoPdfError");
			}
		}

		public override object OnPost(PedidoPdf request)
		{
			return Get(request);
		}

	}
}

