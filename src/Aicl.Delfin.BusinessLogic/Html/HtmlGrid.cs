using System;
using System.Collections.Generic;

namespace Aicl.Delfin.Html
{

	public class HtmlGrid<T>:GridBase<T>{

		public HtmlGrid():base(){
			Style = new HtmlGridStyle();
			Columns = new List<GridColumnBase<T>>();
		}

		public override  GridColumnBase<T> CreateGridColumn(){

			GridColumn<T> gc = new GridColumn<T>();

			if(Style!=default(GridStyleBase)){
				gc.CellStyle=Style.CellStyle;

				gc.HeaderCellStyle= Style.HeaderCellStyle;  // th
				gc.HeaderTextSytle= Style.HeaderTextStyle;  // p

				gc.FooterCellStyle= Style.FooterCellStyle;  // th

			} 

			return gc;
		}

	}

	public abstract class GridBase <T>
	{
		public string Title {get;set;}

		public string FootNote {get;set;}

		public IList<T> DataSource {get;set;}

		public GridStyleBase Style {get;set;}

		protected IList<GridColumnBase<T>> Columns {get;set;}

		public abstract GridColumnBase<T> CreateGridColumn();

		public virtual void AddGridColum(GridColumnBase<T> gridColumn){
			Columns.Add(gridColumn);
		}

		public GridBase (){}

		public override string ToString ()
		{
			HtmlTable table = new HtmlTable();

			table.Style= Style.TableStyle;
			table.RowStyle= Style.RowStyle;
			table.AlternateRowStyle= Style.AlternateRowStyle;

			table.Header.Style= Style.HeaderStyle;

			table.Footer.Style= Style.FooterStyle;

			if(!string.IsNullOrEmpty( Title)){
				var tr = table.Header.CreateRow();
				var th =  tr.CreateCell();
				th.ColumnSpan= (Columns!=null && Columns.Count>0) ?Columns.Count: 1;

				th.SetValue( Style.TitleStyle!=default(HtmlStyle)?
				            (new HtmlParagragh{Text=Title, Style= Style.TitleStyle}).ToString():
							Title
				);
				tr.AddCell(th);
				table.Header.AddRow(tr);

			}

			if(DataSource==null || Columns==null || Columns.Count==0) return table.ToString();

			var trh = table.Header.CreateRow();
			foreach(var column in Columns){
				var th = trh.CreateCell();
				th.Style = column.HeaderCellStyle;
				if (!string.IsNullOrEmpty (column.HeaderText)){
					th.SetValue(column.HeaderText);
				}
				else{
					th.Attributes.Add("height","0");
					th.SetValue(Renderers.HtmlSpace);
				}
				trh.AddCell(th);
			}
			table.Header.AddRow(trh);


			var rowIndex=0;
			foreach(var data in DataSource){

				var dr = table.CreateRow();

				foreach(var column in Columns){
					var dt = dr.CreateCell();
					dr.CellStyle = column.CellStyle;
					if (column.CellRenderFunc!=null){
						dt.SetValue( column.CellRenderFunc(data,rowIndex));
					}
					else{
						dt.SetValue("");
					}
					dr.AddCell(dt);
				}
				table.AddRow(dr);
				rowIndex++;
			}

			//--
			var trf = table.Footer.CreateRow();
			foreach(var column in Columns){
				var th = trf.CreateCell();
				th.Style = column.FooterCellStyle;
				if (column.FooterRenderFunc!=null){
					th.SetValue(column.FooterRenderFunc());
				}
				else{
					th.Attributes.Add("height","0");
					th.SetValue(Renderers.HtmlSpace);
				}
				trf.AddCell(th);
			}
			table.Footer.AddRow(trf);


			if(!string.IsNullOrEmpty(FootNote)){
				var tr = table.Footer.CreateRow();
				var th =  tr.CreateCell();
				th.ColumnSpan= (Columns!=null && Columns.Count>0) ?Columns.Count: 1;

				th.SetValue( Style.FootNoteStyle!=default(HtmlStyle)?
				            (new HtmlParagragh{Text=FootNote, Style= Style.FootNoteStyle}).ToString():
							FootNote
				);
				tr.AddCell(th);
				table.Footer.AddRow(tr);
			}

			//--

			return table.ToString();
		}
	}

	//-------------------------------------------------------------------------
	public class HtmlGridStyle:GridStyleBase{
		public HtmlGridStyle():base(){}
	}

	public abstract class GridStyleBase{

		public HtmlStyle TitleStyle {get;set;}  //p

		public HtmlStyle FootNoteStyle {get;set;}  //p

		public HtmlRowStyle RowStyle {get;set;}  // tr

		public HtmlRowStyle AlternateRowStyle {get;set;} //tr

		public HtmlTableStyle TableStyle {get;set;}      //table


		// GridColumn Style:

		public HtmlCellStyle CellStyle {get;set;}  // td

		public HtmlTableStyle HeaderStyle {get;set;} // thead style

		public HtmlCellStyle HeaderCellStyle {get;set;}  // cell style  th

		public HtmlTableStyle FooterStyle {get;set;} // tfoot style

		public HtmlCellStyle FooterCellStyle {get;set;}  // cell style  th

		public HtmlStyle HeaderTextStyle {get;set;}  //  p

		public GridStyleBase(){
			RowStyle = new HtmlRowStyle();
			AlternateRowStyle = new HtmlRowStyle();
			CellStyle = new HtmlCellStyle();
			TableStyle = new HtmlTableStyle();
			HeaderStyle= new HtmlTableStyle();
			FooterStyle = new HtmlTableStyle();
			TitleStyle = new HtmlStyle();
			FootNoteStyle = new HtmlStyle();
			HeaderCellStyle= new HtmlCellStyle();
			FooterCellStyle = new HtmlCellStyle();
			HeaderTextStyle = new HtmlStyle();

		}

	}

	//-------------------------------------------------------------------------
	public class GridColumn<T>:GridColumnBase<T>{

		protected internal GridColumn():base(){}

	}

	public abstract class GridColumnBase <T>
	{
		public string HeaderText{get;set;}

		public HtmlStyle HeaderTextSytle{get;set;}  //p

		public HtmlCellStyle HeaderCellStyle {get;set;}  // cell style  th for header 

		public HtmlCellStyle FooterCellStyle {get;set;}  // cell style  th for summary rows

		public HtmlCellStyle CellStyle {get;set;}  // stilo de las celdas con el valor 

		public  Func<T,int,object> CellRenderFunc{
			get;set;
		}

		public  Func<object> FooterRenderFunc{ // footer

			get;set;
		}

		protected internal GridColumnBase(){
			CellStyle= new HtmlCellStyle();
			HeaderCellStyle=new HtmlCellStyle();
			HeaderTextSytle = new HtmlStyle();
			FooterCellStyle = new HtmlCellStyle();
		}

	}


}


