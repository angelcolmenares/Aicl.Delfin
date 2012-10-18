using ServiceStack.ServiceHost;

namespace Aicl.Delfin.Model.Types
{
	[RestService("/AuthRole/create","post")]
	[RestService("/AuthRole/read","get")]
	[RestService("/AuthRole/update/{Id}","put")]
	[RestService("/AuthRole/destroy/{Id}","delete")]
	public partial class AuthRole
	{
	}
}