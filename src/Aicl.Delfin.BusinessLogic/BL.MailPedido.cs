using System.Linq;
using System.Linq.Expressions;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.Delfin.DataAccess;
using Mono.Linq.Expressions;
using System.Collections.Generic;
using System;
using ServiceStack.ServiceInterface;
using ServiceStack.Common;
using ServiceStack.Common.Web;
using ServiceStack.Markdown;
using System.Text;

namespace Aicl.Delfin.BusinessLogic
{
	public static partial class BL
	{
		#region Get
        public static MailPedidoResponse Get(this MailPedido request,
		                                              Factory factory,
		                                              IHttpRequest httpRequest)
        {

			factory.Execute(proxy=>{
				Pedido pedido= proxy.FirstOrDefault<Pedido>(q=>q.Consecutivo==request.Consecutivo);
				if (pedido==default(Pedido))
				{
					throw HttpError.NotFound(string.Format("No existe Oferta con Consecutivo: '{0}'", request.Consecutivo));
				}

				if(!pedido.FechaEnvio.HasValue)
				{
					//throw HttpError.Unauthorized(
						//string.Format("Oferta con Consecutivo:'{0}' No esta en estado ENVIADA", request.Consecutivo));
				}


				List<PedidoItem> items=
					proxy.Get<PedidoItem>(q=>q.IdPedido==pedido.Id).OrderBy(f=>f.IdServicio).ToList();

				Console.WriteLine(ItemsTable(items));

			});




			return new MailPedidoResponse{

			};
		}
		#endregion Get

		public static MvcHtmlString ItemsTable(List<PedidoItem> items)
		{

			var sb = new StringBuilder(@"<head>
 <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
</head>
");
			var grupos = from p in items
				group p by p.NombreServicio ;

			foreach(var grupo in grupos){
				sb.AppendFormat("<fieldset>\n\t<legend>Servicio:{0}</legend>\n", grupo.Key);
				sb.AppendFormat("\t<table style=\"margin: 0.5em; border-collapse: collapse;\">\n\t\t<thead>\n\t\t\t<tr>");
				sb.AppendFormat("\n\t\t\t\t<th style=\"padding: .3em; border: 1px #ccc solid;\">Descripcion</th>");
				sb.AppendFormat("\n\t\t\t\t<th style=\"padding: .3em; border: 1px #ccc solid;\">Tiempo<br />Entrega</th>");
				sb.AppendFormat("\n\t\t\t\t<th style=\"padding: .3em; border: 1px #ccc solid;\">Cantidad</th>");
				sb.AppendFormat("\n\t\t\t\t<th style=\"padding: .3em; border: 1px #ccc solid;\">Precio<br />Unitario<br />$</th>");
				sb.AppendFormat("\n\t\t\t\t<th style=\"padding: .3em; border: 1px #ccc solid;\">Descuento<br />%</th>");
				sb.AppendFormat("\n\t\t\t\t<th style=\"padding: .3em; border: 1px #ccc solid;\">Precio<br />Con<br />Descuento<br />$</th>");
				sb.AppendFormat("\n\t\t\t\t<th style=\"padding: .3em; border: 1px #ccc solid;\">Procedimiento</th>");
				sb.Append("\n\t\t\t</tr>\n\t\t</thead>\n\t\t<tbody>\n");
				foreach(var item in grupo){
					sb.Append("\t\t\t<tr>");
					sb.AppendFormat("\n\t\t\t\t<td style=\"padding: .3em; border: 1px #ccc solid;><p style=\"text-align: left;\">{0}</p></td>",
					                string.Concat(item.Descripcion,
					              item.Nota.IsNullOrEmpty()?"": "<br /><b>Nota:"+item.Nota+"</b>" ));
					sb.AppendFormat("\n\t\t\t\t<td style=\"padding: .3em; border: 1px #ccc solid;\"><p style=\"text-align: center;\">{0}</p></td>",
					                string.Format("{0} días hábiles",item.DiasEntrega));
					sb.AppendFormat("\n\t\t\t\t<td style=\"padding: .3em; border: 1px #ccc solid;\"><p style=\"text-align: center;\">{0}</p></td>",
					                item.Cantidad);
					sb.AppendFormat("\n\t\t\t\t<td style=\"padding: .3em; border: 1px #ccc solid;\"><p style=\"text-align: right;\">{0}</p></td>",
					               string.Format("{0:##,000.00}", item.ValorUnitario));
					sb.AppendFormat("\n\t\t\t\t<td style=\"padding: .3em; border: 1px #ccc solid;\"><p style=\"text-align: center;\">{0}</p></td>",
					                string.Format("{0:##,0.00}",item.Descuento));
					sb.AppendFormat("\n\t\t\t\t<td style=\"padding: .3em; border: 1px #ccc solid;\"><p style=\"text-align: right;\">{0}</p></td>",
					               string.Format("{0:##,000.00}", item.CostoInversion));
					sb.AppendFormat("\n\t\t\t\t<td style=\"padding: .3em; border: 1px #ccc solid; width: 50%\"><p style=\"text-align:justify; font-size:85%;\">{0}</p></td>",
					                item.DescripcionProcedimiento);
					sb.Append("\n\t\t\t</tr>");
				}
				sb.AppendFormat("\n\t\t</tbody>\n\t</table>\n");
				sb.Append("</fieldset>");
			}

			return MvcHtmlString.Create(sb.ToString());
		}


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
