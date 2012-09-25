using ServiceStack.ServiceHost;

namespace Aicl.Delfin.Model.Types
{
	[RestService("/Pedido/mail/{Consecutivo}","get")]
	public class MailPedido
	{
		public MailPedido ()
		{
		}

		public int Consecutivo {get;set;}

		public string Asunto {get;set;}

		public string Mensaje {get;set;}

	}
}

