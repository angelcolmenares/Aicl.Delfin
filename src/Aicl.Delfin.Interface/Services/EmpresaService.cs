using System;
﻿using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.BusinessLogic;

namespace Aicl.Delfin.Interface
{
	public class EmpresaService:AppRestService<Empresa>
	{
		public override object OnGet (Empresa request)
		{
			try{
				return request.Get(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Empresa>>(e,"GetEmpresaError");
			}
		}


		public override object OnPost (Empresa request)
		{
			try{
				return request.Post(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Empresa>>(e,"PostEmpresaError");
			}
		}


		public override object OnPut (Empresa request)
		{
			try{
				return request.Put(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Empresa>>(e,"PutEmpresaError");
			}
		}

		public override object OnDelete (Empresa request)
		{
			try{
				return request.Delete(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Empresa>>(e,"DeleteEmpresaError");
			}
		}

	}
}

