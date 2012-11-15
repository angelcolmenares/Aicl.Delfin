using System.Linq;
using ServiceStack.Common;
using iTextSharp.text;
using Aicl.Delfin.Model.Types;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections.Generic;
using ServiceStack.ServiceInterface.Auth;
using System.Text;
using Aicl.Delfin.BusinessLogic;
using System;

namespace Aicl.Delfin.Report
{
	public class OfertaPdf
	{
		internal readonly string FontFamilyName = "unifont";

		public OfertaPdf():this(AppDomain.CurrentDomain.BaseDirectory){}

		public OfertaPdf (string applicationFilePath)
		{
			var fontPath= Path.Combine( Path.Combine(applicationFilePath, "resources"), "Ubuntu-R.ttf");
			//FontFactory.Register("/usr/share/fonts/truetype/unifont/unifont.ttf","unifont");
			//FontFactory.Register("//usr/share/fonts/truetype/ttf-dejavu/DejaVuSans.ttf","unifont");
			//FontFactory.Register("//usr/share/fonts/truetype/ttf-liberation/LiberationSans-Regular.ttf","unifont");
			FontFactory.Register(fontPath,FontFamilyName);
		}

		public void CreatePDF(Empresa empresa, IAuthSession user, Pedido pedido,
		                      List<PedidoItem> items,
		                      string logFile, string prefijo, Stream file,
		                      OfertaMargin margin)
        {
            
			//using (var fileStream= new FileStream(file, FileMode.Create)){
				

				Document document = new Document(PageSize.LETTER.Rotate(), 
				                                 margin.Left, margin.Right, margin.Top, margin.Bottom );

				using (PdfWriter PDFWriter= PdfWriter.GetInstance(document,file))
				{
	            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
				PDFWriter.CloseStream=false;

	            // Our custom Header and Footer is done using Event Handler
	            OfertaPdfEventHandler PageEventHandler = new OfertaPdfEventHandler(){
					LogoFile=logFile,
					Empresa = empresa,
					Pedido = pedido,
					Prefijo = prefijo
				};

	            PDFWriter.PageEvent = PageEventHandler;           

	            document.Open();

				PdfPTable solicitadPor = new PdfPTable(1);
				PdfPCell cell = new PdfPCell(new Phrase("Solicitado Por:",new Font{Size=8}));
				cell.HorizontalAlignment= PdfPCell.ALIGN_CENTER;
				solicitadPor.AddCell(cell);

				cell = new PdfPCell(new Phrase(pedido.NombreCliente,new Font{Size=8}));
				cell.Border= PdfPCell.NO_BORDER;
				solicitadPor.AddCell(cell);
				cell = new PdfPCell(new Phrase(pedido.NitCliente,new Font{Size=8}));
				cell.Border= PdfPCell.NO_BORDER;
				solicitadPor.AddCell(cell);
				cell = new PdfPCell(new Phrase(pedido.NombreContacto,new Font{Size=8}));
				cell.Border= PdfPCell.NO_BORDER;
				solicitadPor.AddCell(cell);
				cell = new PdfPCell(new Phrase(pedido.MailContacto,new Font{Size=8}));
				cell.Border= PdfPCell.NO_BORDER;
				solicitadPor.AddCell(cell);

				PdfPTable destinatario = new PdfPTable(1);
				cell = new PdfPCell(new Phrase("Destinatario:", new Font{Size=8}));
				cell.HorizontalAlignment= PdfPCell.ALIGN_CENTER;
				destinatario.AddCell(cell);

				cell = new PdfPCell(new Phrase(pedido.NombreDestinatario, new Font(){Size=10}));
				cell.Border= PdfPCell.NO_BORDER;
				destinatario.AddCell(cell);

				cell = new PdfPCell(new Phrase(pedido.CargoDestinatario,new Font(){Size=8}));
				cell.Border= PdfPCell.NO_BORDER;
				destinatario.AddCell(cell);

				cell = new PdfPCell(new Phrase(pedido.DireccionDestinatario+"-"+pedido.NombreCiudad,new Font(){Size=8}));
				cell.Border= PdfPCell.NO_BORDER;
				destinatario.AddCell(cell);

				cell = new PdfPCell(new Phrase(string.Format("{0}{1}",
						              pedido.TelefonoDestinatario,
						              (!pedido.TelefonoDestinatario.IsNullOrEmpty() && !pedido.FaxDestinatario.IsNullOrEmpty())?
						              "-"+pedido.FaxDestinatario:
						              pedido.FaxDestinatario)+ "-" + pedido.CelularDestinatario ,new Font(){Size=8}));
				cell.Border= PdfPCell.NO_BORDER;
				destinatario.AddCell(cell);

				cell = new PdfPCell(new Phrase(pedido.MailDestinatario,new Font(){Size=8}));
				cell.Border= PdfPCell.NO_BORDER;
				destinatario.AddCell(cell);


				PdfPTable solicitadoDestinatario = new PdfPTable(2);
				solicitadoDestinatario.WidthPercentage=95;
				cell= new PdfPCell(solicitadPor);
				cell.Padding=10;
				cell.Border= PdfPCell.NO_BORDER;
				solicitadoDestinatario.AddCell(cell);

				cell= new PdfPCell(destinatario);
				cell.Padding=10;
				cell.Border= PdfPCell.NO_BORDER;
				solicitadoDestinatario.AddCell(cell);

				document.Add(solicitadoDestinatario);

				ConstruirTablaItems(document,items);
				ConstruirResumen(document, items);
				ConstruirNotaGastosEnvio(document, pedido);
				ConstruirCondiciones(document, empresa, user);
				ConstruirObservacion(document, empresa, pedido, user);
				ConstruirFirma(document, empresa, user);

	            document.Close();
				}
			//}

          }


		public void ConstruirTablaItems(Document document, List<PedidoItem> items){


			List<string> itemHeaders= new List<string>(new string[]{
				"Descripción","Días","Ctd","Precio Unit",
				"Dscnt %","Precio Con Dscnt","IVA","Procedimiento"
			});

			//var grupos = from p in items group p by p.NombreServicio ;

			var unifont10= FontFactory.GetFont(FontFamilyName,BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
			unifont10.Size=10;

			var unifont8= FontFactory.GetFont(FontFamilyName,BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
			unifont8.Size=8;

			var unifont7= FontFactory.GetFont(FontFamilyName,BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
			unifont7.Size=7;


			//foreach(var grupo in grupos){

				PdfPTable  itemTable = new PdfPTable(8);
				itemTable.WidthPercentage=95;
				itemTable.SetTotalWidth(new float[]{4,0.4f,0.4f,0.9f,0.5f,0.95f,0.75f,5});

				//var cell = new PdfPCell(new Phrase(grupo.Key, unifont10 ));
				//cell.Colspan = 8;
				//itemTable.AddCell(cell);
				PdfPCell cell;

				foreach(var header in itemHeaders){
					cell = new PdfPCell(new Phrase(header,unifont8));
					cell.HorizontalAlignment = Element.ALIGN_CENTER;
					itemTable.AddCell(cell);
				}

				itemTable.HeaderRows=1;

				foreach(var item in items){

					cell = new PdfPCell(new Phrase(string.Concat(item.NombreServicio,". ", item.Descripcion.Decode(),
					              item.Nota.IsNullOrEmpty()?"": "\r\nNota:"+item.Nota.Decode() ),unifont8 ));
					itemTable.AddCell(cell);

					cell = new PdfPCell(new Phrase(item.DiasEntrega.ToString(), unifont8));
					cell.HorizontalAlignment = Element.ALIGN_CENTER;
					itemTable.AddCell(cell);

					cell = new PdfPCell(new Phrase(item.Cantidad.ToString(), unifont8));
					cell.HorizontalAlignment = Element.ALIGN_CENTER;
					itemTable.AddCell(cell);

					cell = new PdfPCell(new Phrase(string.Format("{0:##,0}", item.CostoUnitario),unifont8));
					cell.HorizontalAlignment = Element.ALIGN_RIGHT;
					itemTable.AddCell(cell);

					cell = new PdfPCell(new Phrase(string.Format("{0:##,0}", item.Descuento),unifont8));
					cell.HorizontalAlignment = Element.ALIGN_CENTER;
					itemTable.AddCell(cell);

					cell = new PdfPCell(new Phrase(string.Format("{0:##,0}", item.CostoInversion),unifont8));
					cell.HorizontalAlignment = Element.ALIGN_RIGHT;
					itemTable.AddCell(cell);

					cell = new PdfPCell(new Phrase(string.Format("{0:##,0}", item.ValorIva),unifont8));
					cell.HorizontalAlignment = Element.ALIGN_RIGHT;
					itemTable.AddCell(cell);

					cell = new PdfPCell(new Phrase(item.DescripcionProcedimiento,unifont7));
					cell.HorizontalAlignment=Element.ALIGN_JUSTIFIED;
					itemTable.AddCell(cell);

				}

				document.Add(itemTable);
			//}
		}

		public void ConstruirResumen(Document document, List<PedidoItem> items){

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
				new Fila{Label="Subtotal:", Value= string.Format("$ {0:##,0}", valores.Subtotal) },
				new Fila{Label="IVA:", Value= string.Format("$ {0:##,0}",valores.Iva)},
				new Fila{Label="Total:", Value= string.Format("$ {0:##,0}",valores.Subtotal+ valores.Iva)},
			});

			PdfPTable  itemTable = new PdfPTable(6);
			itemTable.WidthPercentage=95;

			var cell = new PdfPCell(new Phrase("Resumen Oferta", new Font(){Size=10}));
			cell.Colspan = 6;
			itemTable.AddCell(cell);
			itemTable.HeaderRows=1;

			foreach(var fila in filas){
				cell = new PdfPCell(new Phrase(fila.Label,new Font(){Size=10}));
				cell.HorizontalAlignment=2;
				itemTable.AddCell(cell);

				cell = new PdfPCell(new Phrase(fila.Value.ToString(),new Font(){Size=10}));
				itemTable.AddCell(cell);
			}

			document.Add(itemTable);

		}

		public void ConstruirNotaGastosEnvio(Document document, Pedido pedido){
			string nota = string.Format("Nota : El precio {0}incluye gastos de envio",
			                            pedido.IncluyeGastosEnvio? " ": "NO ");

			var pr = new Paragraph(nota, new Font{Size=8});
			pr.IndentationLeft=18;
			document.Add(pr);
		}

		public void ConstruirCondiciones(Document document, Empresa empresa, IAuthSession user){
			StringBuilder html = new StringBuilder();
			html.AppendFormat(
@"Si esta de acuerdo con la oferta, por favor enviar:1 Comunicación escrita (Carta, Orden de Compra, Orden de Servicio u Orden de Trabajo). 2 Soporte de Pago ( via fax o e-mail):Consignación en la {0} a nombre de {1}. 3 La comunicación debe contener la siguiente informacion:3.1 Razón Social completa de la empresa, Direccion y Nit. 3.2 Número de la presente oferta. 3.3 Código o Número de Identificación del equipo (en caso de que no tenga este código, este será; asignado por {1}). 3.4 Dirección exacta (donde estan ubicados los equipos) - Información para el certificado. 3.5 Nombres de las personas encargadas de los equipos o departamento de metrología para comunicarnos en caso de necesidad. 3.6 Al enviar los equipos favor anexar copia de esta oferta.
Tambien se da por entendida la aceptación por parte del cliente si éste da una autorización verbal (telefónica), o trae el instrumento para calibrar o realiza el pago correspodiente.
Si tiene alguna inquietud comuníquese con nosotros. No se emiten juicios profesionales sobre los resultados de la calibracion.",		
				empresa.CuentaBancaria, empresa.Nombre);

			var p = new Paragraph(html.ToString(),new Font{Size=7});
			p.IndentationLeft=18;
			p.IndentationRight=18;
			document.Add(p);
		}

		public void ConstruirObservacion(Document document, Empresa empresa, Pedido pedido, IAuthSession user){
			if(pedido.Observacion.IsNullOrEmpty()) return ;

			var html = 	"Observación: "+ pedido.Observacion;
			var p = new Paragraph(html,new Font(Font.FontFamily.HELVETICA, 7, Font.BOLD));
			p.IndentationLeft=18;
			p.IndentationRight=18;
			document.Add(p);
		}


		public void ConstruirFirma(Document document, Empresa empresa, IAuthSession user){
			StringBuilder html = new StringBuilder();
			html.AppendFormat(
@"
{0}
{1}
{2}
",		
			user.DisplayName, user.LastName, user.Email);
			var p = new Paragraph(html.ToString(),new Font(Font.FontFamily.HELVETICA, 9, Font.BOLDITALIC));
			p.IndentationLeft=18;
			p.IndentationRight=18;
			document.Add(p);
		}


	}
}

//http://itext-general.2136553.n4.nabble.com/Pdf-Table-footer-and-bottom-margin-td4321110.html
