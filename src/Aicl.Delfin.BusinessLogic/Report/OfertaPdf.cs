using System.Linq;
using ServiceStack.Common;
using iTextSharp.text;
using Aicl.Delfin.Model.Types;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections.Generic;
using ServiceStack.ServiceInterface.Auth;
using System.Text;
using System;
using Aicl.Cayita;

namespace Aicl.Delfin.Report
{
	public class OfertaPdf
	{
		internal readonly string FontFamilyName = "unifont";

		Font f7;
		Font f8;
		Font f10;

		public OfertaPdf():this(AppDomain.CurrentDomain.BaseDirectory){}

		public OfertaPdf (string applicationFilePath)
		{
			//var fontPath= Path.Combine( Path.Combine(applicationFilePath, "resources"), "Ubuntu-R.ttf");
			var fontPath= Path.Combine( Path.Combine(applicationFilePath, "resources"), "GOTHIC.TTF");
			//FontFactory.Register("/usr/share/fonts/truetype/unifont/unifont.ttf","unifont");
			//FontFactory.Register("//usr/share/fonts/truetype/ttf-dejavu/DejaVuSans.ttf","unifont");
			//FontFactory.Register("//usr/share/fonts/truetype/ttf-liberation/LiberationSans-Regular.ttf","unifont");
			FontFactory.Register(fontPath,FontFamilyName);
			f7 = FontFactory.GetFont(FontFamilyName,BaseFont.IDENTITY_H, BaseFont.EMBEDDED,7);
			f8 = FontFactory.GetFont(FontFamilyName,BaseFont.IDENTITY_H, BaseFont.EMBEDDED,8);
			f10 = FontFactory.GetFont(FontFamilyName,BaseFont.IDENTITY_H, BaseFont.EMBEDDED,10);
		}

		public void CreatePDF(Empresa empresa, User user, Pedido pedido,
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
					Prefijo = prefijo,
					//FontFamilyName= FontFamilyName,
					F10=f10,
					F7=f7,
					F8=f8
				};

	            PDFWriter.PageEvent = PageEventHandler;           

	            document.Open();


				PdfPTable solicitadPor = new PdfPTable(1);
				PdfPCell cell = new PdfPCell(new Phrase("Solicitado Por:",f8));
				cell.HorizontalAlignment= PdfPCell.ALIGN_CENTER;
				solicitadPor.AddCell(cell);

				cell = new PdfPCell(new Phrase(pedido.NombreCliente,f8));
				cell.Border= PdfPCell.NO_BORDER;
				solicitadPor.AddCell(cell);
				cell = new PdfPCell(new Phrase(pedido.NitCliente,f8));
				cell.Border= PdfPCell.NO_BORDER;
				solicitadPor.AddCell(cell);
				cell = new PdfPCell(new Phrase(pedido.NombreContacto,f8));
				cell.Border= PdfPCell.NO_BORDER;
				solicitadPor.AddCell(cell);
				cell = new PdfPCell(new Phrase(pedido.MailContacto,f8));
				cell.Border= PdfPCell.NO_BORDER;
				solicitadPor.AddCell(cell);

				PdfPTable destinatario = new PdfPTable(1);
				cell = new PdfPCell(new Phrase("Destinatario:", f8));
				cell.HorizontalAlignment= PdfPCell.ALIGN_CENTER;
				destinatario.AddCell(cell);

				cell = new PdfPCell(new Phrase(pedido.NombreDestinatario, f10));
				cell.Border= PdfPCell.NO_BORDER;
				destinatario.AddCell(cell);

				cell = new PdfPCell(new Phrase(pedido.CargoDestinatario,f8));
				cell.Border= PdfPCell.NO_BORDER;
				destinatario.AddCell(cell);

				cell = new PdfPCell(new Phrase(pedido.DireccionDestinatario+"-"+pedido.NombreCiudad,f8));
				cell.Border= PdfPCell.NO_BORDER;
				destinatario.AddCell(cell);

				cell = new PdfPCell(new Phrase(string.Format("{0}{1}",
						              pedido.TelefonoDestinatario,
						              (!pedido.TelefonoDestinatario.IsNullOrEmpty() && !pedido.FaxDestinatario.IsNullOrEmpty())?
						              "-"+pedido.FaxDestinatario:
						              pedido.FaxDestinatario)+ "-" + pedido.CelularDestinatario ,f8));
				cell.Border= PdfPCell.NO_BORDER;
				destinatario.AddCell(cell);

				cell = new PdfPCell(new Phrase(pedido.MailDestinatario,f8));
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
				//ConstruirNotaGastosEnvio(document, pedido);
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

			var cell = new PdfPCell(new Phrase("Resumen Oferta", f10));
			cell.Colspan = 6;
			itemTable.AddCell(cell);
			itemTable.HeaderRows=1;

			foreach(var fila in filas){
				cell = new PdfPCell(new Phrase(fila.Label,f10));
				cell.HorizontalAlignment=2;
				itemTable.AddCell(cell);

				cell = new PdfPCell(new Phrase(fila.Value.ToString(),f10));
				itemTable.AddCell(cell);
			}

			document.Add(itemTable);

		}
		/*
		public void ConstruirNotaGastosEnvio(Document document, Pedido pedido){
			string nota = string.Format("Nota : El precio {0}incluye gastos de envio",
			                            pedido.IncluyeGastosEnvio? " ": "NO ");

			var pr = new Paragraph(nota, f8);
			pr.IndentationLeft=18;
			document.Add(pr);
		}
		*/

		public void ConstruirCondiciones(Document document, Empresa empresa, User user){
			var html = new StringBuilder();
			html.AppendFormat(
@"Condiciones:
1 Los precios se basan en la mejor información disponible proporcionada por el cliente y pueden ser revisados tras la inspección del equipo.
2 Las condiciones del servicio se definen en el formato FLPA0208 adjunto, por favor leerlo detenidamente y diligenciarlo.
3 Colmetrik no recoge equipos, ni asume gastos de los envíos de estos.
4 Los pagos se deben realizar en el {0} a nombre de {1}, ya sea por transferencia electrónica  o consignación.
5 Si acepta el servicio descrito, favor enviar Orden de Compra indicando en ella el número de la oferta y los ítem aceptados para el proceso de Calibración.
6 La información que el cliente envía para la elaboración de la oferta y para la emisión de los certificados, formato FLPA0206 y FLPA0207 respectivamente, debe ser clara y correcta.
7 La recepción de los equipos y la programación del servicio de Calibración se realiza de acuerdo a la disponibilidad del laboratorio y es informada por Colmetrik después de la aceptación del servicio.
8 En esta oferta no se emiten Juicios Profesionales sobre los resultados de la calibración.", empresa.CuentaBancaria, empresa.Nombre);

			var p = new Paragraph(html.ToString(),f7);
			p.IndentationLeft=18;
			p.IndentationRight=18;
			document.Add(p);
		}

		public void ConstruirObservacion(Document document, Empresa empresa, Pedido pedido, User user){
			if(pedido.Observacion.IsNullOrEmpty()) return ;

			var html = 	"Observación: "+ pedido.Observacion;
			var font=FontFactory.GetFont(FontFamilyName,BaseFont.IDENTITY_H, BaseFont.EMBEDDED,7,Font.BOLD);
			var p = new Paragraph(html,font);
			//var p = new Paragraph(html,new Font(Font.FontFamily.HELVETICA, 7, Font.BOLD));
			p.IndentationLeft=18;
			p.IndentationRight=18;
			document.Add(p);
		}


		public void ConstruirFirma(Document document, Empresa empresa, User user){
			StringBuilder html = new StringBuilder();
			html.AppendFormat(
@"
{0}
{1}
{2}
",		
			user.FirstName+" "+user.LastName, user.Cargo, user.Email);
			var font=FontFactory.GetFont(FontFamilyName,BaseFont.IDENTITY_H, BaseFont.EMBEDDED,9,Font.BOLDITALIC);
			var p = new Paragraph(html.ToString(),font);
			//var p = new Paragraph(html.ToString(),new Font(Font.FontFamily.HELVETICA, 9, Font.BOLDITALIC));
			p.IndentationLeft=18;
			p.IndentationRight=18;
			document.Add(p);
		}


	}
}

//http://itext-general.2136553.n4.nabble.com/Pdf-Table-footer-and-bottom-margin-td4321110.html

