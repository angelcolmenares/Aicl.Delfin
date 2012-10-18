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
			Permissions= new List<AuthPermission>();
			Roles = new List<AuthRole>();

		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public List<AuthPermission> Permissions {get; set;}
		public List<AuthRole> Roles {get; set;}
		public string DisplayName { get; set;}
		

	}
}