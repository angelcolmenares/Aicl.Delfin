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
		
		public bool CorreoEnviado {get;set;}

		public string CorreoMensaje {get;set;}

		public bool PdfGenerado {get;set;}

		public string PdfMensaje {get;set;}

		public string OfertaUrl {get;set;}

	}
}