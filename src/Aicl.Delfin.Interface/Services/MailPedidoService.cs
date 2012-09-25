using System;
﻿using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.BusinessLogic;
using ServiceStack.Common.Web;
using System.IO;

namespace Aicl.Delfin.Interface
{
	public class MailPedidoService:AppRestService<MailPedido>
	{
		public override object OnGet(MailPedido request)
		{
			try{
				return request.Get(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<MailPedido>>(e,"GetMailPedidoError");
			}
		}
	}
}


/*

FileInfo targetFile = new FileInfo("AppHost.cs");

			return new HttpResult(targetFile, asAttachment:true);
*/