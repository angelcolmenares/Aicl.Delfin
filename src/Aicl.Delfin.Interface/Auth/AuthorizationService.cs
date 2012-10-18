using ServiceStack.ServiceInterface;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.DataAccess;
using Aicl.Delfin.BusinessLogic;
namespace Aicl.Delfin.Interface
{
	[RequiresAuthenticate]
	public class AuthorizationService:RestServiceBase<Authorization>
	{
		public Factory Factory{ get; set;}
		
		public override object OnPost (Authorization request)
		{
			return  request.GetAuthorizations(Factory, RequestContext);	 
		}

		public override object OnGet (Authorization request)
		{
			return  request.GetAuthorizations(Factory, RequestContext);	 
		}
	}
}