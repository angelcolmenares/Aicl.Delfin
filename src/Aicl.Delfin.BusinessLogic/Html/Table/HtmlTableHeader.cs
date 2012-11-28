namespace Aicl.Cayita
{
	public class HtmlTableHeader:TableBase
	{
		protected internal HtmlTableHeader ():base("thead"){}

		public override RowBase CreateRow(){
			HtmlHeaderRow row = new HtmlHeaderRow();
			row.Style= RowsCount%2==0?
				RowStyle:
				AlternateRowStyle??RowStyle;
			RowsCount++;
			return row;
		}
	}

}

