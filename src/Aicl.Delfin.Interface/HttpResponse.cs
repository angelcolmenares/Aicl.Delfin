using System;
using System.Net;
﻿
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Aicl.Delfin.Interface
{
	public static class HttpResponse
	{
		
		public static HttpResult ErrorResult<TResponse>(string message, string  stackTrace, string errorCode)
			where TResponse:IHasResponseStatus, new() 			
		{
			return new HttpResult( new TResponse(){
					ResponseStatus= new ResponseStatus(){
						Message=message,
						StackTrace=stackTrace,
						ErrorCode=errorCode
					}
				},
			HttpStatusCode.InternalServerError);
		}
		
		public static HttpResult ErrorResult<TResponse>(string message,string errorCode)
			where TResponse:IHasResponseStatus, new() 
		{
			return ErrorResult<TResponse>(message, string.Empty, errorCode);
		}
		
		
		public static HttpResult ErrorResult<TResponse>(Exception exception,string errorCode)
			where TResponse:IHasResponseStatus, new() 
		{
			return ErrorResult<TResponse>(exception.Message, exception.StackTrace, errorCode);
		}		
		
	}
		
}