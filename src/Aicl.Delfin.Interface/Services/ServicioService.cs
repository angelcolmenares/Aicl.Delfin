using System;
﻿using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.BusinessLogic;
using ServiceStack.ServiceInterface;

namespace Aicl.Delfin.Interface
{
	[PermissionAttribute(ApplyTo.Post,"Servicio.create")]
	[RequiresAuthenticateAttribute(ApplyTo.Get)]
	[PermissionAttribute(ApplyTo.Put,"Servicio.update")]
	[PermissionAttribute(ApplyTo.Delete, "Servicio.destroy")]
	[PermissionAttribute(ApplyTo.Patch, "Servicio.patch")]
	public class ServicioService:AppRestService<Servicio>
	{
		public override object OnGet (Servicio request)
		{
			try{
				return request.Get(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Servicio>>(e,"GetServicioError");
			}
		}


		public override object OnPost (Servicio request)
		{
			try{
				return request.Post(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Servicio>>(e,"PostServicioError");
			}
		}


		public override object OnPut (Servicio request)
		{
			try{
				return request.Put(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Servicio>>(e,"PutServicioError");
			}
		}

		public override object OnDelete (Servicio request)
		{
			try{
				return request.Delete(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Servicio>>(e,"DeleteServicioError");
			}
		}

	}
}

