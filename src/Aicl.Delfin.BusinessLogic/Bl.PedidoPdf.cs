using System.Linq;
using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.DataAccess;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.Common;
using ServiceStack.Common.Web;
using Aicl.Delfin.Report;
using ServiceStack.ServiceInterface.Auth;
using System.IO;

namespace Aicl.Delfin.BusinessLogic
{
	public  static partial class BL
	{
		#region Get
        public static object Get(this PedidoPdf request,
		                         Factory factory,
		                         IHttpRequest httpRequest)
        {

			return factory.Execute(proxy=>{
				Pedido pedido= proxy.FirstOrDefault<Pedido>(q=>q.Consecutivo==request.Consecutivo);
				if (pedido==default(Pedido))
				{
					throw HttpError.NotFound(string.Format("No existe Oferta con Consecutivo: '{0}'", request.Consecutivo));
				}


				List<PedidoItem> items=
					proxy.Get<PedidoItem>(q=>q.IdPedido==pedido.Id).OrderBy(f=>f.IdServicio).ToList();


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


				OfertaPdf pdf = new OfertaPdf();

				string logo = Path.Combine(Path.Combine(httpRequest.ApplicationFilePath, "resources"), "logo.png");
				string file = Path.Combine(Path.Combine(httpRequest.ApplicationFilePath,"App_Data"),
				                           string.Format("oferta-{0}.pdf",pedido.Consecutivo));

				using (var stream =  new MemoryStream()){


					pdf.CreatePDF(empresa,user,pedido,items,logo,BL.Prefijo,
				              stream, new OfertaMargin(5,5,100,30));

					stream.Position=0;

					using(var fileStream = new FileStream(file, FileMode.Create )){
						stream.CopyTo(fileStream);
						fileStream.Close();
						return new HttpResult( new FileInfo(file), asAttachment:true);
					}

				}

			});

		}
		#endregion Get
	}



}

