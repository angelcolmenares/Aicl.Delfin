using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.DataAccess;
using System.Collections.Generic;

namespace Aicl.Delfin.BusinessLogic
{
	public static partial class BL
	{
		#region Get
        public static Response<Empresa> Get(this Empresa request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
            return factory.Execute(proxy=>{
				                
				request = proxy.GetEmpresa();
				var response =new Response<Empresa>();
				response.Data.Add(request);
				return response;
            });
  
        }
        #endregion 


		#region Post
        public static Response<Empresa> Post(this Empresa request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.PostEmpresa(request);
			});

			List<Empresa> data = new List<Empresa>();
			data.Add(request);
			
			return new Response<Empresa>(){
				Data=data
			};	
		}
		#endregion Post


		#region Put
        public static Response<Empresa> Put(this Empresa request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.PutEmpresa(request);
			});

			List<Empresa> data = new List<Empresa>();
			data.Add(request);
			
			return new Response<Empresa>(){
				Data=data
			};	
		}
		#endregion Put

		#region Delete
        public static Response<Empresa> Delete(this Empresa request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {
			factory.Execute(proxy=>{
				proxy.DeleteEmpresa(request);
			});

			List<Empresa> data = new List<Empresa>();
			data.Add(request);
			
			return new Response<Empresa>(){
				Data=data
			};	
		}
		#endregion Delete

	}
}

