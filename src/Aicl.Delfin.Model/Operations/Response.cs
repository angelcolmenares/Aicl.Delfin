using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Aicl.Delfin.Model.Types;

namespace Aicl.Delfin.Model.Operations
{
	public class Response<T>:IHasResponseStatus where T:new()
	{
        long? totalCount;

		public Response ()
		{
			ResponseStatus= new ResponseStatus();
			Data= new List<T>();
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public List<T> Data {get; set;}

        public long? TotalCount {
            get {return totalCount.HasValue? totalCount.Value: Data.Count;}
            set { totalCount=value;}
        }

	}


	public class HtmlResponse:IHasResponseStatus 
	{
        
		public HtmlResponse ()
		{
			ResponseStatus= new ResponseStatus();
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public string  Html {get; set;}       

	}

	public class MailEventResponse:IHasResponseStatus 
	{
        
		public MailEventResponse ()
		{
			ResponseStatus= new ResponseStatus();
			MailEvent = new MailEvent();
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public MailEvent  MailEvent {get; set;}       

	}


}