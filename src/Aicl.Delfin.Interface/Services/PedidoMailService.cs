using System;
﻿using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.BusinessLogic;
using Aicl.Delfin.DataAccess;

namespace Aicl.Delfin.Interface
{
	public class PedidoMailService:AppRestService<PedidoMail>
	{
		public Mailer MailService{get;set;}

		public override object OnGet(PedidoMail request)
		{
			try{
				return request.Get(Factory, RequestContext.Get<IHttpRequest>(), MailService);
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<PedidoMail>>(e,"GetPedidoMailError");
			}
		}

		public override object OnPost(PedidoMail request)
		{
			return Get(request);
		}
	}
}


/*

FileInfo targetFile = new FileInfo("AppHost.cs");

			return new HttpResult(targetFile, asAttachment:true);
*/