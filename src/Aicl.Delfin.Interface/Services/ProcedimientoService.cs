using System;
﻿using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.BusinessLogic;


namespace Aicl.Delfin.Interface
{
	public class ProcedimientoService:AppRestService<Procedimiento>
	{
		public override object OnGet (Procedimiento request)
		{
			try{
				return request.Get(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Procedimiento>>(e,"GetProcedimientoError");
			}
		}


		public override object OnPost (Procedimiento request)
		{
			try{
				return request.Post(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Procedimiento>>(e,"PostProcedimientoError");
			}
		}


		public override object OnPut (Procedimiento request)
		{
			try{
				return request.Put(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Procedimiento>>(e,"PutProcedimientoError");
			}
		}

		public override object OnDelete (Procedimiento request)
		{
			try{
				return request.Delete(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Procedimiento>>(e,"DeleteProcedimientoError");
			}
		}

	}
}

