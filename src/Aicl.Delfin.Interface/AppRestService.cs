using System;
﻿
using ServiceStack.ServiceInterface;

using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.DataAccess;
using Aicl.Delfin.Model.Types;
using ServiceStack.ServiceInterface.Auth;

namespace Aicl.Delfin.Interface
{
	public static class TypeExtensionUserDataUrn{

		public static string GetUserDataUrn(this Type type, string sessionId, string key){
			return string.Format("urn:{0}:UserData:{1}:{2}", sessionId, type.Name,key);
		}

		public static string GetUserDataUrn<T>(this T type, string sessionId, string key){
			return string.Format("urn:{0}:UserData:{1}:{2}", sessionId, typeof(T).Name,key);
		}
	}

	public class AppRestService<T>:RestServiceBase<T> where T:new()
	{			
		public Factory Factory{ get; set;}


		public TData GetSessionData<TData>(string key){

			return Factory.Execute(proxy=>{

				var sessionId= this.GetSessionId();

				return proxy.GetUserData<TData>( typeof(TData).GetUserDataUrn(sessionId, key) );

			});

		}

		public void SaveSessionData<TData>(string key, TData data){

			Factory.Execute(proxy=>{
				proxy.SetUserData<TData>(key, data, AuthProvider.DefaultSessionExpiry);
			});

		}

		public User GetUser(){
			return GetSessionData<User>("_user_");
		}
		
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

