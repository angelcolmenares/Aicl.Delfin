using System;
﻿using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.BusinessLogic;
using ServiceStack.ServiceInterface;

namespace Aicl.Delfin.Interface
{
	[PermissionAttribute(ApplyTo.Post,"Servicio.update")]
	[RequiresAuthenticateAttribute(ApplyTo.Get)]
	[PermissionAttribute(ApplyTo.Put,"Servicio.update")]
	[PermissionAttribute(ApplyTo.Delete, "Servicio.update")]
	public class ServicioProcedimientoService:AppRestService<ServicioProcedimiento>
	{
		public override object OnGet (ServicioProcedimiento request)
		{
			try{
				return request.Get(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<ServicioProcedimiento>>(e,"GetServicioProcedimientoError");
			}
		}


		public override object OnPost (ServicioProcedimiento request)
		{
			try{
				return request.Post(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<ServicioProcedimiento>>(e,"PostServicioProcedimientoError");
			}
		}


		public override object OnPut (ServicioProcedimiento request)
		{
			try{
				return request.Put(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<ServicioProcedimiento>>(e,"PutServicioProcedimientoError");
			}
		}

		public override object OnDelete (ServicioProcedimiento request)
		{
			try{
				return request.Delete(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<ServicioProcedimiento>>(e,"DeleteServicioProcedimientoError");
			}
		}

	}
}

