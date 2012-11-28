namespace Aicl.Cayita
{
	public class HtmlRow:RowBase{
		protected internal HtmlRow():base(){}
	}

	public class HtmlHeaderRow:RowBase{
		protected internal HtmlHeaderRow():base(){}

		public override CellBase CreateCell(){
			HtmlHeaderCell cell = new HtmlHeaderCell();
			if(CellStyle!=default(HtmlCellStyle)) cell.Style= CellStyle;
			return cell;
		}
	}

	public class HtmlFooterRow:RowBase{
		protected internal HtmlFooterRow():base(){}

		public override CellBase CreateCell(){
			HtmlFooterCell cell = new HtmlFooterCell();
			if(CellStyle!=default(HtmlCellStyle)) cell.Style= CellStyle;
			return cell;
		}
	}

}

