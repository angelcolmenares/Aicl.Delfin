

namespace Aicl.Cayita
{
	public abstract class TableBase:TagBase
	{
		protected int RowsCount{get;set;}

		public HtmlRowStyle RowStyle{get;set;}
		public HtmlRowStyle AlternateRowStyle { get;set;}

		protected internal TableBase (string tagName):base(tagName){
			Style = new HtmlTableStyle();
			RowsCount=0;
			RowStyle = new HtmlRowStyle();
			AlternateRowStyle= new HtmlRowStyle();
		}

		public virtual RowBase CreateRow(){
			HtmlRow row = new HtmlRow();
			row.Style= RowsCount%2==0?
				RowStyle:
				AlternateRowStyle??RowStyle;
			RowsCount++;
			return row;
		}

		public void AddRow(RowBase row){
			InnerHtml=InnerHtml+row.ToString();
		}

	}
}

