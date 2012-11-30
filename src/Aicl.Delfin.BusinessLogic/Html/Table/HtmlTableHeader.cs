namespace Aicl.Cayita
{
	public class HtmlTableHeader:TableBase
	{
		protected internal HtmlTableHeader ():base("thead"){}

		public override RowBase CreateRow(string alternateRowCss=null){
			HtmlHeaderRow row = new HtmlHeaderRow();
			ApplyStyleToRow (row, alternateRowCss);
			RowsCount++;
			return row;
		}
	}

}

