using System;
using Aicl.Delfin.Model.Types;
using ServiceStack.ServiceInterface;
using Aicl.Delfin.BusinessLogic;
using Aicl.Delfin.Model.Operations;
using ServiceStack.ServiceHost;

namespace Aicl.Delfin.Interface
{
	[RequiresAuthenticate]
	public class UserService:AppRestService<User>
	{
		//[PermissionAttribute(ApplyTo.Get,"User.read")]
		[RoleAttribute(ApplyTo.Post,RoleNames.Admin)]
		//[PermissionAttribute(ApplyTo.Put,"User.update")]
		[RoleAttribute(ApplyTo.Delete, RoleNames.Admin)]
		public override object OnGet (User request)
		{
			try{
				return request.Get(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<User>>(e,"GetUserError");
			}
		}


		public override object OnPost (User request)
		{
			try{
				return request.Post(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<User>>(e,"PostUserError");
			}
		}


		public override object OnPut (User request)
		{
			try{
				return request.Put(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<User>>(e,"PutUserError");
			}
		}

		public override object OnDelete (User request)
		{
			try{
				return request.Delete(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<User>>(e,"DeleteUserError");
			}
		}


	}
}

