using System;
using Aicl.Delfin.Model.Types;
using ServiceStack.ServiceInterface;
using Aicl.Delfin.BusinessLogic;
using Aicl.Delfin.Model.Operations;
using ServiceStack.ServiceHost;
using ServiceStack.Common.Web;

namespace Aicl.Delfin.Interface
{
	[RequiresAuthenticate(ApplyTo.Get)]
	[RoleAttribute(ApplyTo.Post,RoleNames.Admin)]
	[RoleAttribute(ApplyTo.Delete, RoleNames.Admin)]
	public class UserRoleService:AppRestService<UserRole>
	{

		public override object OnGet (UserRole request)
		{
			try{
				return request.Get(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<UserRole>>(e,"GetUserRoleError");
			}
		}


		public override object OnPost (UserRole request)
		{
			try{
				return request.Post(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<UserRole>>(e,"PostUserRoleError");
			}
		}


		public override object OnPut (UserRole request)
		{
			return HttpError.Unauthorized("UserRole Put: Operacion no permitida");
		}

		public override object OnDelete (UserRole request)
		{
			try{
				return request.Delete(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<UserRole>>(e,"DeleteUserRoleError");
			}
		}


	}
}

