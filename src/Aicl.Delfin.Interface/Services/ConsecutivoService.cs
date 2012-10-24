using System;
﻿using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.BusinessLogic;
using ServiceStack.ServiceInterface;


namespace Aicl.Delfin.Interface
{
	[PermissionAttribute(ApplyTo.Post,"Consecutivo.create")]
	[RequiresAuthenticateAttribute(ApplyTo.Get)]
	[PermissionAttribute(ApplyTo.Put,"Consecutivo.update")]
	[PermissionAttribute(ApplyTo.Delete, "Consecutivo.destroy")]
	public class ConsecutivoService:AppRestService<Consecutivo>
	{
		public override object OnGet (Consecutivo request)
		{
			try{
				return request.Get(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Consecutivo>>(e,"GetConsecutivoError");
			}
		}


		public override object OnPost (Consecutivo request)
		{
			try{
				return request.Post(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Consecutivo>>(e,"PostConsecutivoError");
			}
		}


		public override object OnPut (Consecutivo request)
		{
			try{
				return request.Put(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Consecutivo>>(e,"PutConsecutivoError");
			}
		}

		public override object OnDelete (Consecutivo request)
		{
			try{
				return request.Delete(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Consecutivo>>(e,"DeleteConsecutivoError");
			}
		}

	}
}

