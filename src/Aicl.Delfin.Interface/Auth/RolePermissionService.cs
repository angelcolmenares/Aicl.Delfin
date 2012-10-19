using System;
using Aicl.Delfin.Model.Types;
using ServiceStack.ServiceInterface;
using Aicl.Delfin.BusinessLogic;
using Aicl.Delfin.Model.Operations;
using ServiceStack.ServiceHost;
using ServiceStack.Common.Web;

namespace Aicl.Delfin.Interface
{
	[RequiresAuthenticate]
	public class RolePermissionService:AppRestService<RolePermission>
	{
		[RoleAttribute(ApplyTo.Post,RoleNames.Admin)]
		[RoleAttribute(ApplyTo.Delete, RoleNames.Admin)]
		public override object OnGet (RolePermission request)
		{
			try{
				return request.Get(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<RolePermission>>(e,"GetRolePermissionError");
			}
		}


		public override object OnPost (RolePermission request)
		{
			try{
				return request.Post(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<RolePermission>>(e,"PostRolePermissionError");
			}
		}


		public override object OnPut (RolePermission request)
		{
			return HttpError.Unauthorized("RolePermission Put: Operacion no permitida");
		}

		public override object OnDelete (RolePermission request)
		{
			try{
				return request.Delete(Factory, RequestContext.Get<IHttpRequest>());
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<RolePermission>>(e,"DeleteRolePermissionError");
			}
		}


	}
}

