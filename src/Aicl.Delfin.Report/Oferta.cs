using System.Linq;
using ServiceStack.Markdown;
using System.Text;
using Aicl.Delfin.Model.Types;
using System.Collections.Generic;
using ServiceStack.Common;



namespace Aicl.Delfin.Report
{
	public class Oferta
	{

		/*
		string pageTemplate =
@"@model System.Collections.Generic.List<Aicl.Delfin.Model.Types.PedidoItem>
<head>
 <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
</head>
    @PItem.ItemsTable(Model)
";
*/
		public Oferta ()
		{

		}

		public string ItemsToTable(List<PedidoItem> items)
		{

			var mvc = ToTable(items);
			return ConstruirEncabezado()+ mvc.ToHtmlString();
		}


		MvcHtmlString ToTable(List<PedidoItem> items)
		{

			var th = "\n\t\t\t\t<th style=\"padding: .3em; border: 1px #ccc solid;\">{0}</th>";

			var sb = new StringBuilder();
			var grupos = from p in items
				group p by p.NombreServicio ;

			foreach(var grupo in grupos){
				sb.AppendFormat("<fieldset>\n\t<legend style=\"padding: 0.2em 0.5em; border:1px solid green; color:green; font-size:90%; text-align:left;\" >Servicio:{0}</legend>\n", grupo.Key);
				sb.AppendFormat("\t<table style=\"margin: 0.5em; border-collapse: collapse;\">\n\t\t<thead>\n\t\t\t<tr>");
				sb.AppendFormat(th,"Descripcion");
				sb.AppendFormat("\n\t\t\t\t<th style=\"padding: .3em; border: 1px #ccc solid;\">Tiempo<br />Entrega</th>");
				sb.AppendFormat("\n\t\t\t\t<th style=\"padding: .3em; border: 1px #ccc solid;\">Cantidad</th>");
				sb.AppendFormat("\n\t\t\t\t<th style=\"padding: .3em; border: 1px #ccc solid;\">Precio<br />Unitario<br />$</th>");
				sb.AppendFormat("\n\t\t\t\t<th style=\"padding: .3em; border: 1px #ccc solid;\">Descuento<br />%</th>");
				sb.AppendFormat("\n\t\t\t\t<th style=\"padding: .3em; border: 1px #ccc solid;\">Precio<br />Con<br />Descuento<br />$</th>");
				sb.AppendFormat("\n\t\t\t\t<th style=\"padding: .3em; border: 1px #ccc solid;\">Iva</th>");
				sb.AppendFormat("\n\t\t\t\t<th style=\"padding: .3em; border: 1px #ccc solid;\">Procedimiento</th>");
				sb.Append("\n\t\t\t</tr>\n\t\t</thead>\n\t\t<tbody>\n");
				foreach(var item in grupo){
					sb.Append("\t\t\t<tr>");
					sb.AppendFormat("\n\t\t\t\t<td style=\"padding: .3em; border: 1px #ccc solid;\"><p style=\"text-align: left;\">{0}</p></td>",
					                string.Concat(item.Descripcion,
					              item.Nota.IsNullOrEmpty()?"": "<br /><b>Nota:"+item.Nota+"</b>" ));
					sb.AppendFormat("\n\t\t\t\t<td style=\"padding: .3em; border: 1px #ccc solid;\"><p style=\"text-align: center;\">{0}</p></td>",
					                string.Format("{0} días hábiles",item.DiasEntrega));
					sb.AppendFormat("\n\t\t\t\t<td style=\"padding: .3em; border: 1px #ccc solid;\"><p style=\"text-align: center;\">{0}</p></td>",
					                item.Cantidad);
					sb.AppendFormat("\n\t\t\t\t<td style=\"padding: .3em; border: 1px #ccc solid;\"><p style=\"text-align: right;\">{0}</p></td>",
					               string.Format("{0:##,000.00}", item.CostoUnitario));
					sb.AppendFormat("\n\t\t\t\t<td style=\"padding: .3em; border: 1px #ccc solid;\"><p style=\"text-align: center;\">{0}</p></td>",
					                string.Format("{0:##,0.00}",item.Descuento));
					sb.AppendFormat("\n\t\t\t\t<td style=\"padding: .3em; border: 1px #ccc solid;\"><p style=\"text-align: right;\">{0}</p></td>",
					               string.Format("{0:##,0.00}", item.CostoInversion));
					sb.AppendFormat("\n\t\t\t\t<td style=\"padding: .3em; border: 1px #ccc solid;\"><p style=\"text-align: right;\">{0}</p></td>",
					               string.Format("{0:##,0.00}", item.ValorIva));
					sb.AppendFormat("\n\t\t\t\t<td style=\"padding: .3em; border: 1px #ccc solid; width: 50%\"><p style=\"text-align:justify; font-size:85%;\">{0}</p></td>",
					                item.DescripcionProcedimiento);
					sb.Append("\n\t\t\t</tr>");
				}
				sb.AppendFormat("\n\t\t</tbody>\n\t</table>\n");
				sb.Append("</fieldset>");
			}

			return MvcHtmlString.Create(sb.ToString());
		}

		string ConstruirEncabezado()
		{
			return @"<head>
 <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
<style type=""text/css"">
fieldset {
  padding: 1em;
  border:1px solid green;
  font:80%/1 sans-serif;
  }
label {
  float:left;
  width:25%;
  margin-right:0.5em;
  padding-top:0.2em;
  text-align:right;
  font-weight:bold;
  }
legend {
  padding: 0.2em 0.5em;
  border:1px solid green;
  color:green;
  font-size:90%;
  text-align:left;
  }
</style>
</head>
		<table style=""margin-top: 0.5em; margin-right: 0.5em; margin-bottom: 0.5em; margin-left: 0.5em; border-collapse: collapse; padding-top: 0.3em; padding-right: 0.3em; padding-bottom: 0.3em; padding-left: 0.3em; width: 100%; "">
			<tbody>
				<tr>
					<td style=""width: 50%;"">
						<table border=""0"" cellpadding=""1"" cellspacing=""1"" style=""width: 100%; "">
							<tbody>
								<tr>
									<td style=""width: 30%; "">
										<p>
											<img alt="""" src=""http://0.0.0.0:8080/resources/logo.jpg"" title="""" /></p>
										<p>
											NIT:900.008.963-9</p>
									</td>
									<td style=""width: 70%; "">
										&nbsp;</td>
								</tr>
							</tbody>
						</table>
					</td>
					<td style=""width: 50%;padding: .3em; "">
						<table cellpadding=""1"" cellspacing=""1"" style=""width: 100%; "">
							<tbody>
								<tr>
									<td style=""width: 40%; "">
										Oferta No:</td>
									<td>
										00001</td>
								</tr>
								<tr>
									<td>
										Fecha de Envio:</td>
									<td>
										01.12.2012</td>
								</tr>
								<tr>
									<td>
										Valida Hasta:</td>
									<td>
										20.12.2012</td>
								</tr>
								<tr>
									<td>
										Forma de Pago:</td>
									<td>
										100 % anticipado</td>
								</tr>
								<tr>
									<td>
										Fecha Aceptacion:</td>
									<td>
										Pendiente</td>
								</tr>
								<tr>
									<td>
										Elaborada Por:</td>
									<td>
										Usuario X</td>
								</tr>
							</tbody>
						</table>
					</td>
				</tr>
			</tbody>
		</table>
<br />
";
		}
	}




}


