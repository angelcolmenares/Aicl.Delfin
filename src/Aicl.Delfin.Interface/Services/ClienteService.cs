using System;
﻿using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.BusinessLogic;


namespace Aicl.Delfin.Interface
{
	public class ClienteService:AppRestService<Cliente>
	{
		public override object OnGet (Cliente request)
		{
			try{
				return request.Get(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Cliente>>(e,"GetClienteError");
			}
		}

		public override object OnPost (Cliente request)
		{
			try{
				return request.Post(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Cliente>>(e,"PostClienteError");
			}
		}


		public override object OnPut (Cliente request)
		{
			try{
				return request.Put(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Cliente>>(e,"PutClienteError");
			}
		}

		public override object OnDelete (Cliente request)
		{
			try{
				return request.Delete(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<Cliente>>(e,"DeleteClienteError");
			}
		}



	}
}

