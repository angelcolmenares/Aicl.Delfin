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
				gc.HeaderCellStyle= Style.HeaderCellStyle;
				gc.HeaderTextSytle= Style.HeaderTextStyle;
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

			Console.WriteLine( "Columnas '{0}'", Columns.Count);

			if(DataSource==null || Columns==null || Columns.Count==0) return table.ToString();


			var trh = table.Header.CreateRow();
			foreach(var column in Columns){
				Console.WriteLine("Creando columna {0} - {1}", column.HeaderText, column.HeaderCellStyle);
				var th = trh.CreateCell();
				if (!string.IsNullOrEmpty (column.HeaderText)){
					th.SetValue(column.HeaderText);
					th.Style = column.HeaderCellStyle;
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

			return table.ToString();
		}
	}

	//-------------------------------------------------------------------------
	public class HtmlGridStyle:GridStyleBase{
		public HtmlGridStyle():base(){}
	}

	public abstract class GridStyleBase{

		public HtmlStyle TitleStyle {get;set;}

		public HtmlStyle FootNoteStyle {get;set;}

		public HtmlRowStyle RowStyle {get;set;}

		public HtmlRowStyle AlternateRowStyle {get;set;}

		public HtmlTableStyle TableStyle {get;set;}


		// GridColumn Style:

		public HtmlCellStyle CellStyle {get;set;}

		public HtmlTableStyle HeaderStyle {get;set;} // thead style

		public HtmlCellStyle HeaderCellStyle {get;set;}  // cell style  th

		public HtmlTableStyle FooterStyle {get;set;} // tfoot style

		public HtmlCellStyle FooterCellStyle {get;set;}  // cell style  th

		public HtmlStyle HeaderTextStyle {get;set;}  // 


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

		public HtmlStyle HeaderTextSytle{get;set;}

		public HtmlCellStyle HeaderCellStyle {get;set;}  // cell style  th

		public HtmlCellStyle CellStyle {get;set;}  // stilo de las celdas con el valor 

		public  Func<T,int,object> CellRenderFunc{
			get;set;
		}

		public  Func<T,object> SummaryRenderFunc{ // footer

			get;set;
		}

		protected internal GridColumnBase(){
			CellStyle= new HtmlCellStyle();
			HeaderCellStyle=new HtmlCellStyle();
			HeaderTextSytle = new HtmlStyle();
		}



		//public virtual SqlExpressionVisitor<T> Select<TKey>(Expression<Func<T, TKey>> fields){

		// Header : Texto y Estilo
		// Cell:
		// 	Value: Que voy a Mostrar
		// 	Renderer : como lo voy a mostar
		// Footer : Texto y Estilo
	}


}


