using ServiceStack.ServiceHost;

namespace Aicl.Delfin.Model.Types
{
	[RestService("/AuthPermission/create","post")]
	[RestService("/AuthPermission/read","get")]
	[RestService("/AuthPermission/update/{Id}","put")]
	[RestService("/AuthPermission/destroy/{Id}","delete")]
	public partial class AuthPermission
	{
	}
}