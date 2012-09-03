using System;
﻿using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.BusinessLogic;


namespace Aicl.Delfin.Interface
{
	public class CiudadService:AppRestService<Ciudad>
	{
		public override object OnGet (Ciudad request)
		{
			try{
				return request.Get(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Ciudad>>(e,"GetCiudadError");
			}
		}


		public override object OnPost (Ciudad request)
		{
			try{
				return request.Post(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Ciudad>>(e,"PostCiudadError");
			}
		}


		public override object OnPut (Ciudad request)
		{
			try{
				return request.Put(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Ciudad>>(e,"PutCiudadError");
			}
		}

		public override object OnDelete (Ciudad request)
		{
			try{
				return request.Delete(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Ciudad>>(e,"DeleteCiudadError");
			}
		}

	}
}

