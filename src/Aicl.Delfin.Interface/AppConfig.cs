using ServiceStack.Configuration;

namespace Aicl.Delfin.Interface
{
	public class AppConfig
	{

		public string Channel {get;set;}

		public string MailLogToken{get;set;}
				
		public AppConfig(IResourceManager resources)
		{
			Channel= resources.Get<string>("CHANNEL","mail_log_channel");
				
		}



				
	}
}