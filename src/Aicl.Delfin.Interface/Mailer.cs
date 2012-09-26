using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Aicl.Delfin.Model.Types;

namespace Aicl.Delfin.Interface
{
	public class Mailer
	{		
			
		SmtpClient SmtpServer {get ;set;}
		
		public Mailer ( Empresa empresa )
		{
						
			SmtpServer = new SmtpClient(empresa.MailServerUrl);
			SmtpServer.Port = empresa.MailServerPort;
			SmtpServer.Credentials = 
				new NetworkCredential(empresa.MailServerUser, empresa.MailServerPassword);
			SmtpServer.EnableSsl = empresa.MailServerEnableSsl;
			ServicePointManager.ServerCertificateValidationCallback =
				delegate(object s, X509Certificate certificate,
				X509Chain chain, SslPolicyErrors sslPolicyErrors)
				{ return true; };
			
		}
		
		
		public void Send(MailMessage message){
			SmtpServer.Send(message);
		}
		
		
		
	}
}

