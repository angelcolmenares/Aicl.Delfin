using System;
using ServiceStack.ServiceHost;

namespace Aicl.Galapago.Model.Types
{
	[RestService("/UserAuth/create","post")]
	[RestService("/UserAuth/read","get")]
	[RestService("/UserAuth/read/{Id}","get")]
	[RestService("/UserAuth/update/{Id}","put")]
	[RestService("/UserAuth/destroy/{Id}","delete")]
	public partial class UserAuth
	{
	}
}