using System;
using System.Linq;
using ServiceStack.Markdown;
using System.Text;
using Aicl.Delfin.Model.Types;
using System.Collections.Generic;
using ServiceStack.Common;
using ServiceStack.ServiceInterface.Auth;

namespace Aicl.Delfin.Report
{
	public class OfertaHtml
	{

		public OfertaHtml (){}

		public string ConstruirHtmlReport(Empresa empresa, IAuthSession user,
		                                  Pedido pedido, List<PedidoItem> items,
		                                  string textoInicial=default(string)){

			return string.Format(@"<!DOCTYPE html>
<html lang=""es"">
	<head>
		<meta http-equiv=""Content-Type"" content=""text/html"" charset=""utf-8"" />
	</head>
	<body>
		{0}{1}{2}{3}{4}
	</body>
</html>",
			                            ConstruirEncabezado(empresa, pedido, textoInicial),
			                            ConstruirTablaCliente(pedido),
			                            ItemsToTable(items),
			                            ConstruirResumen(pedido, items),
										ConstruirCondiciones(empresa,pedido,user));
		}


		public string ItemsToTable(List<PedidoItem> items)
		{
			var mvc = ConstruirListaDeItems(items);
			return mvc.ToHtmlString();
		}

		public string ConstruirTablaCliente(Pedido pedido){
			return string.Format(@"<table border=""0"" cellpadding=""1"" cellspacing=""1"" style=""width: 100%; "">
			<tbody>
				<tr>
					<td style=""width: 50%; "">
						{0}
					</td>
					<td style=""width: 50%; "">
						{1}
					</td>
				</tr>
			</tbody>
		</table>
", ConstruirSolicitadoPor(pedido), ConstruirDestinatario(pedido));

		}

		public string ConstruirSolicitadoPor(Pedido pedido){
			var filas = new List<Fila>(
				new Fila[]{
				new Fila{Value=pedido.NombreCliente.ValueOrHtmlSpace()},
				new Fila{Value=pedido.NitCliente.ValueOrHtmlSpace()},
				new Fila{Value=pedido.NombreContacto.ValueOrHtmlSpace()},
				new Fila{Value=pedido.MailContacto.ValueOrHtmlSpace()},
				new Fila{Value=Extensions.HtmlSpace},
				new Fila{Value=Extensions.HtmlSpace},
				new Fila{Value=Extensions.HtmlSpace}
			});

			return string.Format(@"							<table style=""margin: 0.5em; border-collapse: collapse; width: 100%; height: 200px; "">
																<thead>
																	<tr>
																		<th colspan=""2"" style=""padding: .3em; border: 1px #ccc solid;"">Solicitado por:</th>
																	</tr>
																</thead>	
								<tbody style=""margin: 0.5em; border-collapse: collapse;  border: 1px #ccc solid; "">
									{0}
								</tbody>
							</table>",
			                     ConstruirFilasSolicitadoPor(filas));
		}


		public string ConstruirDestinatario(Pedido pedido){
			var filas = new List<Fila>(
				new Fila[]{
				new Fila{Value=pedido.NombreDestinatario.ValueOrHtmlSpace()},
				new Fila{Value=pedido.CargoDestinatario.ValueOrHtmlSpace()},
				new Fila{Value=pedido.DireccionDestinatario.ValueOrHtmlSpace()},
				new Fila{Value=pedido.NombreCiudad.ValueOrHtmlSpace()},
				new Fila{Value=
					(string.Format("{0}{1}",
					              pedido.TelefonoDestinatario,
					              (!pedido.TelefonoDestinatario.IsNullOrEmpty() && !pedido.FaxDestinatario.IsNullOrEmpty())?
					              "-"+pedido.FaxDestinatario:
					              pedido.FaxDestinatario)).ValueOrHtmlSpace()},
				new Fila{Value=pedido.CelularDestinatario.ValueOrHtmlSpace()},
				new Fila{Value=pedido.MailDestinatario.ValueOrHtmlSpace()}
			});

			return string.Format(@"						<table style=""margin: 0.5em; border-collapse: collapse; width: 100%;height: 200px; "">
																<thead>
																	<tr>
																		<th colspan=""2"" style=""padding: .3em; border: 1px #ccc solid;"">Destinatario:</th>
																	</tr>
																</thead>	
								<tbody style=""margin: 0.5em; border-collapse: collapse;  border: 1px #ccc solid; "">
									{0}
								</tbody>
							</table>",
			                     ConstruirFilasSolicitadoPor(filas));
		}

		public string ConstruirResumen(Pedido pedido, List<PedidoItem> items){

			var valores = 
				(from p in items
        		group p by p.IdPedido into g 
        		select new {
				Id = g.Key, 
				Subtotal = g.Sum(p => p.CostoInversion),
				Iva = g.Sum(p => p.ValorIva)
				}).First();

			var filas = new List<Fila>(
				new Fila[]{
				new Fila{Label="Subtotal:", Value= string.Format("$ {0:##,0.00}", valores.Subtotal) },
				new Fila{Label="IVA:", Value= string.Format("$ {0:##,0.00}",valores.Iva)},
				new Fila{Label="Total:", Value= string.Format("$ {0:##,0.00}",valores.Subtotal+ valores.Iva)},
			});

			return string.Format(@"
    <table cellpadding=""1"" cellspacing=""1""  style=""margin: 0.5em; border-collapse: collapse; width:40% "">
		<thead>
			<tr>
				<th colspan=""2"" style=""padding: .3em; border: 1px #ccc solid;"">Resumen Oferta:</th>
			</tr>
		</thead>	
    <tbody style=""margin: 0.5em; border-collapse: collapse;  border: 1px #ccc solid; "">
        {0}
    </tbody>
    </table>
    <p style=""font-weight: bold"">Nota : El precio {1}incluye gastos de envio</p>",
			                     ConstruirFilasResumen(filas), pedido.IncluyeGastosEnvio? " ": "NO ");

		}

		public string ConstruirEncabezado(Empresa empresa, Pedido pedido, 
		                                  string textoInicial=default(string))
		{
			var filas = new List<Fila>(
				new Fila[]{
				new Fila{Label="Oferta No:", Value=pedido.Consecutivo.ToString().PadLeft(8,'0') },
				new Fila{Label="Fecha Envio:", Value=pedido.FechaEnvio.HasValue?pedido.FechaEnvio.Value.ToString("dd.MM.yyyy"):"Pendiente"},
				new Fila{Label="Válida Hasta:", Value=pedido.VigenteHasta.HasValue? pedido.VigenteHasta.Value.ToString("dd.MM.yyyy"):"Pendiente"},
				new Fila{Label ="Forma de Pago:", Value=pedido.DescripcionFormaPago},
				new Fila{Label="Fecha Aceptación:", Value=pedido.FechaAceptacion.HasValue? pedido.FechaAceptacion.Value.ToString("dd.MM.yyyy"):"Pendiente"},
				new Fila{Label="Enviado Por:", Value=pedido.NombreEnviadoPor}
			});

			var filasHtml = ConstruirFilasEncabezado(filas);

			var filasFormato = new List<Fila>(
				new Fila[]{
				new Fila{Value="FLPA0201" },
				new Fila{Value="EDICION:08 (2012-09-14)"},
				new Fila{Value=string.Empty.ValueOrHtmlSpace()},
				new Fila{Value=string.Empty.ValueOrHtmlSpace()},
				new Fila{Value=string.Empty.ValueOrHtmlSpace()},
				new Fila{Value=string.Empty.ValueOrHtmlSpace()}
			});

			var filasFormatoHtml = ConstruirFilasFormato(filasFormato);

			return string.Format(@"<head>
 <meta http-equiv=""Content-Type"" content=""text/html"" charset=""utf-8"" />
</head>
    {0}
	<table style=""border-collapse: collapse; width: 100%; "">
		<tbody>
			<tr>
				<td style=""width: 20%;"">
					<table border=""0"" cellpadding=""1"" cellspacing=""1"" style=""width: 100%; "">
						<tbody>
							<tr>
								<td>
									<p><img alt=""{1}"" src=""{2}"" title="""" width=""60%"" /></p>
									<p>	NIT:{3}</p>
								</td>
							</tr>
						</tbody>
					</table>
				</td>
				<td style=""padding: .3em; "">
					<table cellpadding=""1"" cellspacing=""1"" style=""border: 1px #ccc solid; "">
						<tbody>
							{4}
						</tbody>
					</table>
				</td>
				<td style=""padding: .3em; "">
					<table cellpadding=""1"" cellspacing=""1"">
						<tbody>
							{5}
						</tbody>
					</table>
				</td>
			</tr>
		</tbody>
	</table>
<br />
",
			                     textoInicial.IsNullOrEmpty()?"":"<p lang=\"es\">"+textoInicial+"</p>",
			                     empresa.Nombre,
			                     empresa.ApplicationHost.IsNullOrEmpty()?"resources/logo.png": empresa.ApplicationHost+"/resources/logo.png",
			                     empresa.Nit, 
			                     filasHtml,
			                     filasFormatoHtml);
		}

		MvcHtmlString ConstruirListaDeItems(List<PedidoItem> items)
		{

			var th = "\n\t\t\t\t<th style=\"padding: .3em; border: 1px #ccc solid;\">{0}</th>";

			var sb = new StringBuilder();
			var grupos = from p in items
				group p by p.NombreServicio ;

			foreach(var grupo in grupos){
				//sb.AppendFormat("<fieldset>\n\t<legend style=\"padding: 0.2em 0.5em; border:1px solid green; color:green; font-size:90%; text-align:left;\" >Servicio:{0}</legend>\n", grupo.Key);
				sb.AppendFormat("\t<table style=\"margin: 0.5em; border-collapse: collapse; width: 100%\">\n\t\t<thead>\n\t\t\t<tr>");
				sb.AppendFormat("<th colspan=\"8\" style=\"padding: .3em; border: 1px #ccc solid;\"><p style=\"text-align: left;\">Servicio:{0}</p></th>",grupo.Key);
				sb.Append("\n\t\t\t</tr>");
				sb.Append("\n\t\t\t<tr>");
				sb.AppendFormat(th,"Descripcion");
				sb.AppendFormat(th,"Tiempo<br />Entrega");
				sb.AppendFormat(th,"Cantidad");
				sb.AppendFormat(th,"Precio<br />Unitario<br />$");
				sb.AppendFormat(th,"Descuento<br />%");
				sb.AppendFormat(th,"Precio<br />Con<br />Descuento<br />$");
				sb.AppendFormat(th,"Iva");
				sb.AppendFormat(th,"Procedimiento");
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
				//sb.Append("</fieldset>");
			}

			return MvcHtmlString.Create(sb.ToString());
		}


		string ConstruirFilasEncabezado(List<Fila> filas){
			StringBuilder html = new StringBuilder();
			foreach( var fila in filas){
				html.AppendFormat(@"<tr>
								<td style=""width: 140px; padding-left:0.3em; "">{0}</td>
								<td>{1}</td>
							</tr>", fila.Label, fila.Value);
			}
			return html.ToString();
		}

		string ConstruirFilasResumen(List<Fila> filas){
			StringBuilder html = new StringBuilder();
			foreach(var fila in filas){
				html.AppendFormat(@"<tr>
	    <td style=""width: 35%;padding-left:0.4em; "">{0}</td>
	    <td style=""padding-right:0.4em""><p style=""text-align: right;"">{1}</p></td>
		</tr>",fila.Label, fila.Value);
		
			}
		
			return html.ToString();
		}

		string ConstruirFilasSolicitadoPor(List<Fila> filas){
			StringBuilder html= new StringBuilder();
			foreach(var fila in filas){
				html.AppendFormat(@"<tr>
	<td style=""width: 0%; ""></td>
	<td style=""padding: .35em; "">{0}</td>
</tr>
",fila.Value);
			}
			return html.ToString();
		}

		string ConstruirFilasFormato(List<Fila> filas){
			StringBuilder html= new StringBuilder();
			foreach(var fila in filas){
				html.AppendFormat(@"<tr>
	<td>{0}</td>
</tr>
",fila.Value);
			}
			return html.ToString();
		}

		public string ConstruirCondiciones(Empresa empresa,Pedido pedido, IAuthSession user){
			StringBuilder html = new StringBuilder();
			html.AppendFormat(@"<table style=""border-collapse: collapse; width: 100%; "">
									<thead>
										<tr>
											<th  style=""padding: .3em; border: 1px #ccc solid;"">Condiciones:</th>
										</tr>
									</thead>	
									<tbody style=""margin: 0.5em; border-collapse: collapse;  border: 1px #ccc solid; "">
									<tr>
										<td>			
			<p>
				Si esta de acuerdo con la oferta, por favor enviar:</p>
			<ul>
				<li>
					Comunicaci&oacute;n escrita (Carta, Orden de Compra, Orden de Servicio u Orden de Trabajo)</li>
				<li>
					Soporte de Pago ( via fax o e-mail): &nbsp;Consignaci&oacute;n en la {0}</li>
			</ul>
			<p>
				La comunicaci&oacute;n debe contener la siguiente informacion:</p>
			<ul>
				<li>
					Razon Social completa de la empresa, Direccion &nbsp;y Nit.</li>
				<li>
					N&uacute;mero de la presente oferta.</li>
				<li>
					C&oacute;digo o N&uacute;mero de Identificaci&oacute;n del equipo (en caso de que no tenga este c&oacute;digo, este ser&aacute; asignado por {1})</li>
				<li>
					Direcci&oacute;n exacta (donde estan ubicados los equipos) - Informaci&oacute;n para el certificado.</li>
				<li>
					Nombres de las personas encargadas de los equipos o departamento de metrolog&iacute;a para comunicarnos en caso de necesidad.</li>
				<li>
					Al enviar los equipos favor anexar copia de esta oferta.</li>
			</ul>
			<p>
				Tambien se da por entendida la aceptaci&oacute;n por parte del cliente si est&eacute; da una autorizaci&oacute;n verbal (telef&oacute;nica), o trae el instrumento para calibrar o realiza el pago correspodiente.</p>
			<p>
				Si tiene alguna inquietud comun&iacute;quese con nosotros. No se emiten juicios profesionales sobre los resultados de la calibracion.</p>
			{13}	
			<p>{2}</p>
			<p>{3}</p>
			<p>{4}</p>
		</td>
		</tr>
		</tbody>
		</table>

	<p>{5} Tel:{6} Telefax:{7} {8} e-mail:{9} {10}-{11}</p>
	<p> Direccion antigua: {12}</p>
",
			                  empresa.CuentaBancaria, 
			                  empresa.Nombre, user.DisplayName, user.LastName, user.Email,
			                  empresa.Direccion,empresa.Telefono, empresa.Fax, 
			                  empresa.Web,empresa.Mail, empresa.Ciudad, empresa.Pais, empresa.DireccionAntigua,
			                  pedido.Observacion.IsNullOrEmpty()?
			                  "<br />":
			                  string.Format("<p style=\"font-weight:bold;\">Observación:{0}</p><br />",pedido.Observacion));
			return html.ToString();
		}

	}


	public class Fila{
		public string Label {get;set;}
		public object Value {get; set;}
	}

}


/*
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
*/