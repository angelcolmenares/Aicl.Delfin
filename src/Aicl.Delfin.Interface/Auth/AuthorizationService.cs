using System;
using System.Collections.Generic;
using System.Linq;
﻿using ServiceStack.CacheAccess;
using ServiceStack.Common;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;

using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
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
		
	}
}