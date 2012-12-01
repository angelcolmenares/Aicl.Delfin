using System;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Aicl.Delfin.Model.Types;
using System.Text;
using Aicl.Cayita;

namespace Aicl.Delfin.Report
{
	public class OfertaPdfEventHandler:PdfPageEventHelper
    {

        // This is the contentbyte object of the writer
        PdfContentByte cb;

        // we will put the final number of pages in a template
        PdfTemplate template;

        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;

        // This keeps track of the creation time
        DateTime printTime = DateTime.Now;

        #region Properties
        
		public string LogoFile{get;set;}
		public Pedido Pedido {get; set;}        
		public Empresa Empresa {get;set;}
		public string Prefijo {get;set;}

		public Font F7 {get;set;}
		public Font F8 {get;set;}
		public Font F10 {get;set;}

		//public string FontFamilyName {get;set;}
        
		#endregion

        // we override the onOpenDocument method
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            printTime = DateTime.Now;
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb = writer.DirectContent;
            template = cb.CreateTemplate(50, 50);    
        }
        
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
            Rectangle pageSize = document.PageSize;        
            
            PdfPTable headerTable = new PdfPTable(2);
			headerTable.SetTotalWidth(new float[]{1,4});
			float cellHeight = document.TopMargin;
			headerTable.TotalWidth = pageSize.Width;


			var leftTable = new PdfPTable(1);  // logo y nit 
			PdfPCell leftTableCell = new PdfPCell( leftTable);
			leftTableCell.FixedHeight = cellHeight;
			leftTableCell.Padding=0;
			leftTableCell.PaddingTop= 5;
			leftTableCell.PaddingLeft = 0;
			leftTableCell.Border= PdfPCell.NO_BORDER;
			headerTable.AddCell(leftTableCell);

			var cell = new PdfPCell(Image.GetInstance(LogoFile),false);
			cell.FixedHeight = cellHeight*80/100;
			cell.HorizontalAlignment= PdfPCell.ALIGN_CENTER;
			cell.Padding=0;
			cell.PaddingTop=0;
			cell.PaddingLeft = 0;
			cell.Border= PdfPCell.NO_BORDER;
			leftTable.AddCell(cell);

			cell = new PdfPCell(new Phrase(Empresa.Nit, F8));
			cell.HorizontalAlignment= PdfPCell.ALIGN_CENTER;
			cell.Border= PdfPCell.NO_BORDER;
			leftTable.AddCell(cell);

			//------------------------------------------------------------------------

			PdfPTable rightTable = new PdfPTable(4);  // Consecutivo, Elaborado por etc...
			rightTable.SetTotalWidth(new float[]{3,4,3,6});
			PdfPCell rightTableCell = new PdfPCell(rightTable);
			rightTableCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
			rightTableCell.Padding = 2;
			rightTableCell.PaddingTop= 10;
            rightTableCell.PaddingBottom = 2;
			rightTableCell.FixedHeight = cellHeight;
			rightTableCell.Border= PdfPCell.NO_BORDER;

            headerTable.AddCell(rightTableCell);

			cell = new PdfPCell(new Phrase("", F10));
			cell.Border= PdfPCell.NO_BORDER;
			cell.Colspan=3;
			rightTable.AddCell(cell);

			var textoFormato=Empresa.Formato;
			cell = new PdfPCell(new Phrase(textoFormato, F10));
			cell.Border= PdfPCell.NO_BORDER;
			rightTable.AddCell(cell);

			cell = new PdfPCell(new Phrase("", F10));
			cell.Border= PdfPCell.NO_BORDER;
			cell.Colspan=3;
			rightTable.AddCell(cell);

			textoFormato=Empresa.Edicion;
			cell = new PdfPCell(new Phrase(textoFormato, F10));
			cell.Border= PdfPCell.NO_BORDER;
			cell.PaddingBottom = 8;
			rightTable.AddCell(cell);

			cell = new PdfPCell(new Phrase("Oferta No:", F10));
			cell.Border= PdfPCell.NO_BORDER;
			rightTable.AddCell(cell);

			cell = new PdfPCell(new Phrase(string.Format("{0}-{1}", Prefijo, Pedido.Consecutivo), F10));
			cell.Border= PdfPCell.NO_BORDER;
			rightTable.AddCell(cell);

			cell = new PdfPCell(new Phrase("Forma de Pago:", F10));
			cell.Border= PdfPCell.NO_BORDER;
			rightTable.AddCell(cell);

			cell = new PdfPCell(new Phrase(Pedido.DescripcionFormaPago, F10));
			cell.Border= PdfPCell.NO_BORDER;
			rightTable.AddCell(cell);


			cell = new PdfPCell(new Phrase("Fecha Envio:", F10));
			cell.Border= PdfPCell.NO_BORDER;
			rightTable.AddCell(cell);

			cell = new PdfPCell(new Phrase(Pedido.FechaEnvio.HasValue? Pedido.FechaEnvio.Value.Format():"BORRADOR !!!",
			                               F10));
			cell.Border= PdfPCell.NO_BORDER;
			rightTable.AddCell(cell);

			cell = new PdfPCell(new Phrase("Fecha Aceptación:", F10));
			cell.Border= PdfPCell.NO_BORDER;
			rightTable.AddCell(cell);

			cell = new PdfPCell(new Phrase(Pedido.FechaAceptacion.Format(), F10));
			cell.Border= PdfPCell.NO_BORDER;
			rightTable.AddCell(cell);

			cell = new PdfPCell(new Phrase("Valida Hasta:", F10));
			cell.Border= PdfPCell.NO_BORDER;
			rightTable.AddCell(cell);

			cell = new PdfPCell(new Phrase(Pedido.VigenteHasta.Format(), F10));
			cell.Border= PdfPCell.NO_BORDER;
			rightTable.AddCell(cell);

			cell = new PdfPCell(new Phrase("Enviado por :", F10));
			cell.Border= PdfPCell.NO_BORDER;
			rightTable.AddCell(cell);

			cell = new PdfPCell(new Phrase(Pedido.NombreEnviadoPor, F10));
			cell.Border= PdfPCell.NO_BORDER;
			rightTable.AddCell(cell);


            cb.SetRGBColorFill(0, 0, 0);
            //HeaderTable.WriteSelectedRows(0, -1, pageSize.GetLeft(40), pageSize.GetTop(50), cb);

			headerTable.WriteSelectedRows( 0, -1,  0,  
			                              pageSize.Height - cellHeight + headerTable.TotalHeight,
			                              writer.DirectContent );
            
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

			Rectangle pageSize = document.PageSize;        
			//var cellHeight = document.BottomMargin*50/100;
			            
			StringBuilder s = new StringBuilder();

			s.AppendFormat(
@"{0} Tel:{1} Telefax:{2} {3} e-mail:{4} {5}-{6}. Direccion antigua: {7}",
				Empresa.Direccion,Empresa.Telefono,
				Empresa.Fax, Empresa.Web, Empresa.Mail, Empresa.Ciudad,
				Empresa.Pais, Empresa.DireccionAntigua);
			/*
			PdfPTable footerTable = new PdfPTable(1);
			footerTable.TotalWidth = pageSize.Width;
						var cell = new PdfPCell(new Phrase(s.ToString(), F8));
			cell.FixedHeight= cellHeight;
			cell.HorizontalAlignment= PdfPCell.ALIGN_CENTER;
			cell.Border= PdfPCell.NO_BORDER;
			footerTable.AddCell(cell);

			footerTable.WriteSelectedRows( 0, -1,  0,  
			                              pageSize.Height - cellHeight + footerTable.TotalHeight,
			                              writer.DirectContent );
			*/


            int pageN = writer.PageNumber;
            String text = "Página " + pageN + " de ";
            float len = bf.GetWidthPoint(text, 8);

			cb.SetRGBColorFill(100, 100, 100);

			cb.BeginText();
            cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 8); 
            cb.SetTextMatrix(pageSize.GetLeft(10), pageSize.GetBottom(15));
            cb.ShowTextAligned(1,s.ToString(),400,18,0);
            cb.EndText();
            
            cb.BeginText();
            cb.SetFontAndSize(bf, 8);
            cb.SetTextMatrix(pageSize.GetLeft(10), pageSize.GetBottom(8));
            cb.ShowText(text);
            cb.EndText();

            cb.AddTemplate(template, pageSize.GetLeft(10) + len, pageSize.GetBottom(8));
            
            cb.BeginText();
            cb.SetFontAndSize(bf, 8);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, 
                "Impreso el:" + printTime.ToString(), 
                pageSize.GetRight(10), 
                pageSize.GetBottom(8), 0);
            cb.EndText();
        }


        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            template.BeginText();
            template.SetFontAndSize(bf, 8);
            template.SetTextMatrix(0, 0);
            template.ShowText("" + (writer.PageNumber - 1)); // esto añade el numero total de paginas ....
            template.EndText();
        }
	}
}

