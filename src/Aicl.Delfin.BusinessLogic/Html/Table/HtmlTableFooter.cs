using System;

namespace Aicl.Cayita
{
	public class HtmlTableFooter:TableBase{

		public HtmlTableFooter():base("tfoot"){}

		public override RowBase CreateRow(string alternateRowCss=null){

			HtmlFooterRow row = new HtmlFooterRow();
			ApplyStyleToRow (row, alternateRowCss);
			RowsCount++;
			return row;
		}
	}
}

