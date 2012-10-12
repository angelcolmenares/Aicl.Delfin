using ServiceStack.ServiceHost;

namespace Aicl.Delfin.Model.Types
{
	[RestService("/Pedido/mail/{Consecutivo}","get,post")]
	public class PedidoMail
	{
		public PedidoMail ()
		{
		}

		public int Consecutivo {get;set;}

		public string Asunto {get;set;}

		public string TextoInicial {get;set;}

	}
}

