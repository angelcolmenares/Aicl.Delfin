using ServiceStack.ServiceHost;

namespace Aicl.Delfin.Model.Types
{
	[RestService("/MailEvent/{MailLogToken}","post")]
	public class MailEvent
	{
		public MailEvent (){}

		public string MailLogToken{get;set; }

		public string Token{get;set; }

		public string Event{get;set; }

		public string Recipient {get;set;}

		public string Domain {get;set;}

		public string Reason {get;set;}

		public string Code {get;set;}

		public string Description {get;set;}

		public int TimeStamp {get;set;}
	}
}

