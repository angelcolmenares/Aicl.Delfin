using System.Web;
using System.Linq;
using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.DataAccess;
using System.Collections.Generic;
using System;
using ServiceStack.ServiceInterface;
using ServiceStack.Common;
using ServiceStack.Common.Web;
using Aicl.Delfin.Report;
using System.Net.Mail;
using ServiceStack.ServiceInterface.Auth;
using System.IO;

namespace Aicl.Delfin.BusinessLogic
{
	public static partial class BL
	{
		#region Get
        public static MailPedidoResponse Get(this MailPedido request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest,
		                                     Mailer mailService)
        {

			factory.Execute(proxy=>{
				Pedido pedido= proxy.FirstOrDefault<Pedido>(q=>q.Consecutivo==request.Consecutivo);
				if (pedido==default(Pedido))
				{
					throw HttpError.NotFound(string.Format("No existe Oferta con Consecutivo: '{0}'", request.Consecutivo));
				}

				if(!pedido.FechaEnvio.HasValue)
				{
					throw HttpError.Unauthorized(
						string.Format("Oferta con Consecutivo:'{0}' No esta en estado ENVIADA", request.Consecutivo));
				}


				List<PedidoItem> items=
					proxy.Get<PedidoItem>(q=>q.IdPedido==pedido.Id).OrderBy(f=>f.IdServicio).ToList();


				var oferta = new OfertaHtml();
				var userSession = httpRequest.GetSession();   // el de esta session...
				IAuthSession user= new AuthUserSession();  // el que lo envio !!!

				if(userSession.Id!=pedido.IdEnviadoPor.ToString()){
					var userAuth= proxy.FirstOrDefault<UserAuth>(q=>q.Id==pedido.IdEnviadoPor);
					if(userAuth==default(UserAuth)){
						userAuth = new UserAuth(){
							DisplayName="indefinido",
							LastName="indefinido"
						};

					}
					user.PopulateWith(userAuth);
				}
				else{
					user.PopulateWith(userSession);
				}

				var empresa = proxy.GetEmpresa(); 

				var html = oferta.ConstruirHtmlReport(empresa,
				                                      user,
				                                      pedido,
				                                      items,
				                                      HttpUtility.UrlDecode(request.TextoInicial,System.Text.Encoding.Default));

				MailMessage message = new MailMessage();
				message.Subject=  !request.Asunto.IsNullOrEmpty()?
					request.Asunto:
						string.Format("Envio Oferta No:{0}", pedido.Consecutivo.ToString().PadLeft(8,'0'));

				message.ReplyToList.Add(userSession.Email);
				message.From= new MailAddress(userSession.Email);

				var mc= !pedido.MailContacto.IsNullOrEmpty()?pedido.MailContacto:userSession.Email;

				message.To.Add(mc);

				if(! pedido.MailDestinatario.IsNullOrEmpty() &&

				   (mc.Trim().ToUpper()!=pedido.MailDestinatario.Trim().ToUpper()) ){
					message.CC.Add(pedido.MailDestinatario);
				}

				message.Bcc.Add(userSession.Email);

				if(!empresa.ApplicationMailBox.IsNullOrEmpty()){
					message.Bcc.Add(empresa.ApplicationMailBox);
				}

				message.Body= html;
				message.IsBodyHtml=true;

				OfertaPdf pdf = new OfertaPdf();

				string logo = Path.Combine(Path.Combine(httpRequest.ApplicationFilePath, "resources"), "logo.png");
				string file = Path.Combine(Path.Combine(httpRequest.ApplicationFilePath,"App_Data"),
				                           string.Format("oferta-{0}.pdf",pedido.Consecutivo));

				try{
					pdf.CreatePDF(empresa,user,pedido,items,logo,"CMK-S", 
				              file);
				}
				catch(Exception){

				}

				message.Attachments.Add(new Attachment(file));

				mailService.Send(message);


			});

			return new MailPedidoResponse{
				CorreoMensaje=Path.Combine(Path.Combine(httpRequest.ApplicationFilePath, "resources"), "logo.png")+";"+
					Path.Combine(Path.Combine(httpRequest.ApplicationFilePath,"App_Data"),
				                           string.Format("oferta-{0}.pdf","X"))
			};
		}
		#endregion Get

	}
}

/*
<p style="text-align:center">
   Este texto está centrado.
 </p> 

 <p style="text-align:left"> 
   Este texto está alineado a la izquierda.
 </p> 

 <p style="text-align:right"> 
    Este texto está alineado a la derecha.
 </p> 

 <p style="text-align:justify"> 
    Este texto está justificado,
    Margenes alineados a derecha e izquierda.
 </p>
*/
