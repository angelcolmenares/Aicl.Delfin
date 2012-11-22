using System;
using System.Collections.Generic;
using ServiceStack.Markdown;

namespace Aicl.Delfin.Html
{
	public class Grid <T>:TagBuilder
	
	{

		List<GridColumn<T>> columns= new List<GridColumn<T>>();
			public Grid  ():base("div") {}

		public ElementStyle Style {get;set;}

		public TableStyle TableStyle {get;set;}  
		//TODO: incluir dentro de TableStyle a RowStyle  y a su ve CellStyle dentro de RowStyle...

		public IList<T> DataSource {get;set;}


		// Todo Crear GridHeader : Text, Style , Propiedad Header... para reemplar a TitleStyle y TitleText
		// Todo Crear GridFooter : Text, Style , Propiedad Footer... Para reemplazar a Note..

		// crear Paragraf TagBuilder con tag= p con Style..... : recibe Texto y Stylo....
		// texto va en InnerTxt

		public ElementStyle TitleStyle {get;set;}
		public string TitleText {get;set;}

		public GridHeader Header{get;set;}
		public GridFooter Footer{get;set;}

		public string Note {get;set;}

		public void AddColumn(GridColumn<T> column){
			columns.Add(column);
		}

		public override string ToString ()
		{
			if(Style!=default(TableStyle)){
				var style= Style.ToString();
				if(!string.IsNullOrEmpty(style)) Attributes["style"]= style ;
			}

			Table table = new Table();

			TableHeader th = new TableHeader();

			Row hr;

			if(Header!=default(GridHeader)){
				th.Style= Header.Style;

				ColumnHeader ch = new ColumnHeader{
					ColumnSpan= columns.Count,
					Value= Header.TitleStyle!=default(ElementStyle)?
					new Paragrag(){Style=Header.TitleStyle }.ToString():
						Header.TitleText
				};
				hr = new Row();
				hr.AddCell(ch);
				th.AddRow(hr);

			}


			hr = new Row();

			foreach(var c in columns){
				if(string.IsNullOrEmpty(c.HeaderText) ) continue;
				var ch = new ColumnHeader
				{
					Value= c.HeaderStyle!=default(ElementStyle)? 
					(new Paragrag(){Text=c.HeaderText, Style=c.HeaderStyle}).ToString():
						c.HeaderText
				};
				hr.AddCell(ch);
			}

			th.AddRow(hr);


			table.Header=th;

			Console.WriteLine(table.ToString());



			string res=string.Empty;

			foreach(var t in DataSource){
				foreach(var c in columns){
					object o = c.CellRenderer(t);
					res= res +string.Format("Type : {0} - Value : {1}", o.GetType(), o);
				}
			}
			return res;
		}
	}



	public class GridHeader{

		public GridHeader(){}

		public TableStyle Style{get;set;}

		public string TitleText{ get;set;}

		public ElementStyle TitleStyle{ get;set;}
	}

	public class GridFooter{

		public GridFooter(){}

		public ElementStyle Style{get;set;}

		public Paragrag Text{ get;set;}

	}

	public class GridColumn <T>
	{
		public GridColumn(){}

		public string HeaderText{get;set;}

		public ElementStyle HeaderStyle {get;set;}

		public ElementStyle CellStyle{get;set;}

		public  Func<T,object> CellRenderer{
			get;set;
		}

		public  Func<T,object> FooterRenderer{

			get;set;
		}

		//public virtual SqlExpressionVisitor<T> Select<TKey>(Expression<Func<T, TKey>> fields){

		// Header : Texto y Estilo
		// Cell:
		// 	Value: Que voy a Mostrar
		// 	Renderer : como lo voy a mostar
		// Footer : Texto y Estilo
	}



	public abstract class GridColumnElement <T>
	{
		public GridColumnElement(){}

	}
}

