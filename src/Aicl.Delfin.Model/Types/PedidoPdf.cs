using ServiceStack.ServiceHost;

namespace Aicl.Delfin.Model.Types
{
	[RestService("/Pedido/pdf/{Consecutivo}","get,post")]
	public class PedidoPdf
	{
		public PedidoPdf ()
		{
		}
		public int Consecutivo {get;set;}
	}
}

