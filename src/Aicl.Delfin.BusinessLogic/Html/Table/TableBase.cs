namespace Aicl.Cayita
{
	public abstract class TableBase:TagBase
	{
		public int RowsCount{get; protected set;}

		public HtmlRowStyle RowStyle{get;set;}
		public HtmlRowStyle AlternateRowStyle { get;set;}

		protected internal TableBase (string tagName):base(tagName){
			Style = new HtmlTableStyle();
			RowsCount=0;
			RowStyle = new HtmlRowStyle();
			AlternateRowStyle= new HtmlRowStyle();
		}

		public virtual RowBase CreateRow(string alternateRowCss=null){
			HtmlRow row = new HtmlRow();
			ApplyStyleToRow (row, alternateRowCss);
			RowsCount++;
			return row;
		}

		public void AddRow(RowBase row){
			InnerHtml=InnerHtml+row.ToString();
		}


		protected void ApplyStyleToRow ( RowBase row, string alternateRowCss=null)
		{
			if (RowsCount % 2 == 0)
				row.Style = RowStyle;
			else {
				row.Style = AlternateRowStyle ?? RowStyle;
				if (!string.IsNullOrEmpty (alternateRowCss))
					row.Attributes ["class"] = alternateRowCss;
			}
		}

	}
}

