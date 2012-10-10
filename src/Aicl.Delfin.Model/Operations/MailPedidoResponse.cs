using ServiceStack.ServiceInterface.ServiceModel;

namespace Aicl.Delfin.Model.Operations
{
	public class MailPedidoResponse:IHasResponseStatus 
	{
        

		public MailPedidoResponse ()
		{
			ResponseStatus= new ResponseStatus();

		}
		
		public ResponseStatus ResponseStatus { get; set; }
		




	}
}