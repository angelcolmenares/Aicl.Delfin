using System;
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
		public Oferta (){}

		public string ConstruirHtmlReport(Empresa empresa, Pedido pedido, List<PedidoItem> items){
			return ConstruirEncabezado(empresa, pedido)+ 
				ItemsToTable(items)+
					ConstruirResumen(items)+
					ConstruirCondiciones(empresa);
		}

		public string ItemsToTable(List<PedidoItem> items)
		{
			var mvc = ConstruirListaDeItems(items);
			return mvc.ToHtmlString();
		}

		public string ConstruirResumen(List<PedidoItem> items){

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

			return string.Format(@"<fieldset style=""width:40%"">
    <legend style=""padding: 0.2em 0.5em; border:1px solid green; color:green; font-size:100%; text-align:left;"" >Resumen Oferta</legend>
    <table style=""margin: 0.5em; border-collapse: collapse;"">
    <tbody>
        {0}
    </tbody>
    </table>
    <p style=""font-weight: bold"">Nota : El precio no incluye gastos de envio</p>
</fieldset>",ConstruirFilasResumen(filas));



		}

		public string ConstruirEncabezado(Empresa empresa, Pedido pedido)
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

			return string.Format(@"<head>
 <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
</head>
    
	<table style=""margin-top: 0.5em; margin-right: 0.5em; margin-bottom: 0.5em; margin-left: 0.5em; border-collapse: collapse; padding-top: 0.3em; padding-right: 0.3em; padding-bottom: 0.3em; padding-left: 0.3em; width: 100%; "">
		<tbody>
			<tr>
				<td style=""width: 50%;"">
					<table border=""0"" cellpadding=""1"" cellspacing=""1"" style=""width: 100%; "">
						<tbody>
							<tr>
								<td style=""width: 30%; "">
									<p><img alt="""" src=""resources/logo.jpg"" title="""" /></p>
									<p>	NIT:{0}</p>
								</td>
								<td style=""width: 70%; ""></td>
							</tr>
						</tbody>
					</table>
				</td>
				<td style=""width: 50%;padding: .3em; "">
					<table cellpadding=""1"" cellspacing=""1"" style=""width: 100%; "">
						<tbody>
							{1}
						</tbody>
					</table>
				</td>
			</tr>
		</tbody>
	</table>
<br />
",empresa.Nit, filasHtml);
		}

		MvcHtmlString ConstruirListaDeItems(List<PedidoItem> items)
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


		string ConstruirFilasEncabezado(List<Fila> filas){
			StringBuilder html = new StringBuilder();
			foreach( var fila in filas){
				html.AppendFormat(@"<tr>
								<td style=""width: 40%; "">{0}</td>
								<td>{1}</td>
							</tr>", fila.Label, fila.Value);
			}
			return html.ToString();
		}

		string ConstruirFilasResumen(List<Fila> filas){
			StringBuilder html = new StringBuilder();
			foreach(var fila in filas){
				html.AppendFormat(@"<tr>
	    <td style=""width: 40%; "">{0}</td>
	    <td style=""padding: .35em; ""><p style=""text-align: right;"">{1}</p></td>
		</tr>",fila.Label, fila.Value);
		
			}
			
			return html.ToString();

		}


		public string ConstruirCondiciones(Empresa empresa){
			StringBuilder html = new StringBuilder();
			html.Append(@"<fieldset>
			<legend style=""padding: 0.2em 0.5em; border:1px solid green; color:green; font-size:100%; text-align:left;"">Condiciones</legend>
			
			<p>
				Si esta de acuerdo con la oferta, por favor enviar:</p>
			<ul>
				<li>
					Comunicaci&oacute;n escrita (Carta, Orden de Compra, Orden de Servicio u Orden de Trabajo)</li>
				<li>
					Soporte de Pago ( via fax o e-mail): &nbsp;Consignaci&oacute;n en la Cuenta Corriente No. 04915730 del Banco de Bogota</li>
			</ul>
			<p>
				La comunicaci&oacute;n debe contener la siguiente informacion:</p>
			<ul>
				<li>
					Razon Social completa de la empresa, Direccion &nbsp;y Nit.</li>
				<li>
					N&uacute;mero de la presente oferta.</li>
				<li>
					C&oacute;digo o N&uacute;mero de Identificaci&oacute;n del equipo (en caso de que no tenga este c&oacute;digo, este ser&aacute; asignado por Colmetrik Ltda.)</li>
				<li>
					Direcci&oacute;n exacta (donde estan ubicados los equipos) - Informaci&oacute;n para el certificado.</li>
				<li>
					Nombres de las personas encargadas de los equipos o departamenteo de metrolog&iacute;a para comunicarnos en caso de necesidad.</li>
				<li>
					Al enviar los equipos favor anexar copia de esta oferta.</li>
			</ul>
			<p>
				Tambien se da por entendida la aceptaci&oacute;n por parte del cliente si est&eacute; da una autorizaci&oacute;n verbal (telef&oacute;nica), o trae el instrumento para calibrar o realiza el pago correspodiente.</p>
			<p>
				Si tiene alguna inquietud comun&iacute;quese con nosotros. No se emiten juicios profesionales sobre los resultados de la calibracion.</p>
			<p>
				Milena Gracia Barreto</p>
			<p>
				Asistente Comercial</p>
			<p>
				venta@colmetrik.com</p>
		</fieldset>
		<p>
			Calle 72 No. 55-54 Piso 3. Tel: (571) 5476161-3108231 Telefax:(571) 5491573 www.colmetrik.com e-mail:colmetrik@colmentrik.com Bogot&aacute; D.C.-Colombia Direccion antigua : Calle 72 NO 43-50 Piso 3.</p>
");
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