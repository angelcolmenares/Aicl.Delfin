using System;

namespace Aicl.Cayita
{
	public class HtmlTableFooter:TableBase{

		public HtmlTableFooter():base("tfoot"){}

		public override RowBase CreateRow(){

			HtmlFooterRow row = new HtmlFooterRow();
			row.Style= RowsCount%2==0?
				RowStyle:
				AlternateRowStyle??RowStyle;
			RowsCount++;
			return row;
		}
	}
}

