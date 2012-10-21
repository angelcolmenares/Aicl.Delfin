using System;
using Aicl.Delfin.Model.Types;
using ServiceStack.ServiceInterface;
using Aicl.Delfin.BusinessLogic;
using Aicl.Delfin.Model.Operations;
using ServiceStack.ServiceHost;

namespace Aicl.Delfin.Interface
{
	[RequiresAuthenticate]
	[RoleAttribute(ApplyTo.Post,RoleNames.Admin)]
	[RoleAttribute(ApplyTo.Put,RoleNames.Admin)]
	[RoleAttribute(ApplyTo.Delete, RoleNames.Admin)]
	public class AuthPermissionService:AppRestService<AuthPermission>
	{

		public override object OnGet (AuthPermission request)
		{
			try{
				return request.Get(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<AuthPermission>>(e,"GetAuthPermissionError");
			}
		}


		public override object OnPost (AuthPermission request)
		{
			try{
				return request.Post(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<AuthPermission>>(e,"PostAuthPermissionError");
			}
		}


		public override object OnPut (AuthPermission request)
		{
			try{
				return request.Put(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<AuthPermission>>(e,"PutAuthPermissionError");
			}
		}

		public override object OnDelete (AuthPermission request)
		{
			try{
				return request.Delete(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<AuthPermission>>(e,"DeleteAuthPermissionError");
			}
		}


	}
}