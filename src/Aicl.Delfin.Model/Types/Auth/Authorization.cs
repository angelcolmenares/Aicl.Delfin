using ServiceStack.ServiceHost;

namespace Aicl.Delfin.Model.Types
{
	[RestService("/Authorization/read","get")]
	public partial class Authorization
	{
		public Authorization ()
		{
		}
		
		public int UserId{ get; set;}
	}
}