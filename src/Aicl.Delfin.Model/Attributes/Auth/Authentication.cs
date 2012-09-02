using System;
using ServiceStack.ServiceHost;

namespace Aicl.Delfin.Model.Types
{
	[RestService("/login/{UserName}/{Password}","post,get")]
	[RestService("/login","post,get")]
	[RestService("/logout","delete")]
	public partial class Authentication
	{
	}
}