using System;
﻿using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.BusinessLogic;


namespace Aicl.Delfin.Interface
{
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

