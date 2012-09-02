using System;
﻿
using ServiceStack.ServiceInterface;

using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.DataAccess;

namespace Aicl.Delfin.Interface
{
	public class AppRestService<T>:RestServiceBase<T> where T:new()
	{			
		public Factory Factory{ get; set;}
		
		public override object OnGet (T request)
		{		
			try{
				return  new Response<T>(){
					Data=Factory.Get<T>(request)
				};
			}
			catch(Exception e ){
				return HttpResponse.ErrorResult<Response<T>>(e, "GetError");
			}
		}
		
		public override object OnPost (T request)
		{
			try{		
				return new Response<T>(){
					Data=Factory.Post<T>(request)
				};			
			}
			catch(Exception e ){
				return HttpResponse.ErrorResult<Response<T>>(e, "PostError");
			}
		}
		
		public override object OnPut (T request)
		{
			
			try{
				return new Response<T>(){
					Data=Factory.Put<T>(request)
				};
			}
			catch(Exception e ){
				return HttpResponse.ErrorResult<Response<T>>(e, "PutError");
			}
		}
		
		public override object OnDelete (T request)
		{
		
			try{
				return  new Response<T>(){
					Data=Factory.Delete<T>(request)
				};
			}
			catch(Exception e ){
				return HttpResponse.ErrorResult<Response<T>>(e, "DeleteError");
			}
		}
	
	}
}

