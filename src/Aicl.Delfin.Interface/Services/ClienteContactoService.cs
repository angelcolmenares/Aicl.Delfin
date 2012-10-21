using System;
﻿using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.BusinessLogic;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;

namespace Aicl.Delfin.Interface
{
	[RequiresAuthenticate]
	[RoleAttribute(ApplyTo.Post,"ClienteContacto.create")]
	[PermissionAttribute(ApplyTo.Get,"ClienteContacto.read")]
	[PermissionAttribute(ApplyTo.Put,"ClienteContacto.update")]
	[RoleAttribute(ApplyTo.Delete, "ClienteContacto.destroy")]
	public class ClienteContactoService:AppRestService<ClienteContacto>
	{
		public override object OnGet (ClienteContacto request)
		{
			try{
				return request.Get(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<ClienteContacto>>(e,"GetClienteContactoError");
			}
		}

		public override object OnPost (ClienteContacto request)
		{
			return HttpError.NotFound("PostClienteContacto No Implementado");

		}


		public override object OnPut (ClienteContacto request)
		{

			return HttpError.NotFound("PutClienteContacto No Implementado");

		}

		public override object OnDelete (ClienteContacto request)
		{

			return HttpError.NotFound("DeleteClienteContacto No Implementado");

		}



	}
}

