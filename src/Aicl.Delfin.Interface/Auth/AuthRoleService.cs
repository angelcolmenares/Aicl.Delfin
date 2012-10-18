using System;
using Aicl.Delfin.Model.Types;
using ServiceStack.ServiceInterface;
using Aicl.Delfin.BusinessLogic;
using Aicl.Delfin.Model.Operations;
using ServiceStack.ServiceHost;

namespace Aicl.Delfin.Interface
{
	[RequiresAuthenticate]
	public class AuthRoleService:AppRestService<AuthRole>
	{
		//[PermissionAttribute(ApplyTo.Get,"AuthRole.read")]
		[RoleAttribute(ApplyTo.Post,RoleNames.Admin)]
		[PermissionAttribute(ApplyTo.Put,RoleNames.Admin)]
		[RoleAttribute(ApplyTo.Delete, RoleNames.Admin)]
		public override object OnGet (AuthRole request)
		{
			try{
				return request.Get(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<AuthRole>>(e,"GetAuthRoleError");
			}
		}


		public override object OnPost (AuthRole request)
		{
			try{
				return request.Post(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<AuthRole>>(e,"PostAuthRoleError");
			}
		}


		public override object OnPut (AuthRole request)
		{
			try{
				return request.Put(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<AuthRole>>(e,"PutAuthRoleError");
			}
		}

		public override object OnDelete (AuthRole request)
		{
			try{
				return request.Delete(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<AuthRole>>(e,"DeleteAuthRoleError");
			}
		}


	}
}