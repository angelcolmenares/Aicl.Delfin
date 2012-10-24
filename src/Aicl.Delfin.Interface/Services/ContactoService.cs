using System;
﻿using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.BusinessLogic;
using ServiceStack.ServiceInterface;


namespace Aicl.Delfin.Interface
{

	[PermissionAttribute(ApplyTo.Post,"Cliente.create")]
	[RequiresAuthenticateAttribute(ApplyTo.Get)]
	[PermissionAttribute(ApplyTo.Put,"Cliente.update")]
	[PermissionAttribute(ApplyTo.Delete, "Cliente.destroy")]
	public class ContactoService:AppRestService<Contacto>
	{
		public override object OnGet (Contacto request)
		{
			try{
				return request.Get(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Contacto>>(e,"GetContactoError");
			}
		}


		public override object OnPost (Contacto request)
		{
			try{
				return request.Post(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Contacto>>(e,"PostContactoError");
			}
		}


		public override object OnPut (Contacto request)
		{
			try{
				return request.Put(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Contacto>>(e,"PutContactoError");
			}
		}

		public override object OnDelete (Contacto request)
		{
			try{
				return request.Delete(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Contacto>>(e,"DeleteContactoError");
			}
		}

	}
}

