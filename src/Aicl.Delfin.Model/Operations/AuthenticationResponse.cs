using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Aicl.Delfin.Model.Types;

namespace Aicl.Delfin.Model.Operations
{
	public class AuthenticationResponse:IHasResponseStatus
	{
		public AuthenticationResponse ()
		{
			ResponseStatus= new ResponseStatus();
			Permissions= new List<string>();
			Roles = new List<AuthRole>();

		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public List<string> Permissions {get; set;}
		public List<AuthRole> Roles {get; set;}
		public string DisplayName { get; set;}
		

	}
}