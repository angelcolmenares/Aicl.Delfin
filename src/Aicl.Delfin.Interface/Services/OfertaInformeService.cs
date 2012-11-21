using System;
﻿using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using ServiceStack.ServiceInterface;
using ServiceStack.OrmLite;
using Mono.Linq.Expressions;
using ServiceStack.Common.Web;
using Aicl.Delfin.Html;

namespace Aicl.Delfin.Interface
{
	[RequiresAuthenticateAttribute(ApplyTo.Get)]
	//[PermissionAttribute(ApplyTo.All,"Cliente.update")]
	public class OfertaInformeService:AppRestService<OfertaInformeRequest>
	{
		public override object OnGet (OfertaInformeRequest request)
		{
			try{

				Table t = new Table();

				Row r = new Row();

				Cell c = new Cell("1 Row :primetra celda");
				r.AddCell(c);

				c.Value="1 Row Segunda Celda";
				r.AddCell(c);

				t.AddRow(r);

				//
				r= new Row();
				c.Value="2 Row Primera Celda";
				r.AddCell(c);

				c.Value="2 Row Segunda Celda";
				r.AddCell(c);

				t.AddRow(r);

				Console.WriteLine(t.ToString());

				//------------------------------
				TableHeader h = new TableHeader();
				h.Style = new RowStyle{BackgroundColor="#00557F", Color="#FFFFFF"};
				h.Attributes.Add("someattribute","somevalue");
				ColumnHeader ch = new ColumnHeader(){Value="First"};
				h.AddColumnHeader(ch);

				ch = new ColumnHeader(){Value="Second"};
				h.AddColumnHeader(ch);

				t = new Table(){
					Style= Table.DefaultTableStyle,
					Header= h
				};

				r = new Row();
				c= new Cell(){Value= "1 Row Primera Celda"};
				r.AddCell(c);
				c= new Cell(){Value= "1 Row Segunda Fila"};
				r.AddCell(c);
				t.AddRow(r);

				r = new Row();
				c= new Cell(){Value= "2 Row Primera Celda"};
				r.AddCell(c);
				c= new Cell(){Value= "2 Row Segunda Fila"};
				r.AddCell(c);
				t.AddRow(r);

				r = new Row();
				c= new Cell(){Value= "3 Row Primera Celda"};
				r.AddCell(c);
				c= new Cell(){Value= "3 Row Segunda Fila"};
				r.AddCell(c);
				t.AddRow(r);

				r = new Row();
				c= new Cell(){Value= "4 Row Primera Celda"};
				r.AddCell(c);
				c= new Cell(){Value= "4 Row Segunda Fila"};
				r.AddCell(c);
				t.AddRow(r);
				Console.WriteLine(t.ToString());

				//------------------------------

				t = new Table(){
					Style= Table.DefaultTableStyle ,
					RowStyle= Table.DefaultRowStyle
				};

				var cellStyle= Table.DefaultCellStyle;

				r = new Row(){CellStyle=cellStyle};
				c= new Cell(){Value= "1 Row Primera Celda"};
				r.AddCell(c);
				c= new Cell(){Value= "1 Row Segunda Fila"};
				r.AddCell(c);
				t.AddRow(r);

				r = new Row(){CellStyle=cellStyle};
				c= new Cell(){Value= "2 Row Primera Celda"};
				r.AddCell(c);
				c= new Cell(){Value= "2 Row Segunda Fila"};
				r.AddCell(c);
				t.AddRow(r);

				r = new Row(){CellStyle=cellStyle};
				c= new Cell(){Value= "3 Row Primera Celda"};
				r.AddCell(c);
				c= new Cell(){Value= "3 Row Segunda Fila"};
				r.AddCell(c);
				t.AddRow(r);

				r = new Row(){CellStyle=cellStyle, Style= new RowStyle{BackgroundColor="red"}};
				c= new Cell(){Value= "4 Row Primera Celda"};
				r.AddCell(c);
				c= new Cell(){Value= "4 Row Segunda Fila"};
				r.AddCell(c);
				t.AddRow(r);
				Console.WriteLine(t.ToString());

				//------------------------------

				TableFooter tf = new TableFooter(){Style=new RowStyle{BackgroundColor="#00557F", Color="#FFFFFF"}};
				ColumnFooter cf = new ColumnFooter (){Value="Footer First"};
				tf.AddColumnFooter(cf);

				cf = new ColumnFooter(){Value="Footer Second"};
				tf.AddColumnFooter(cf);

				t = new Table(){
					Style= Table.DefaultTableStyle ,
					RowStyle= Table.DefaultRowStyle,
					Header= h,
					Footer=tf
				};

				cellStyle= Table.DefaultCellStyle;

				r = new Row(){CellStyle=cellStyle};
				c= new Cell(){Value= "1 Row Primera Celda"};
				r.AddCell(c);
				c= new Cell(){Value= "1 Row Segunda Fila"};
				r.AddCell(c);
				t.AddRow(r);

				r = new Row(){CellStyle=cellStyle};
				c= new Cell(){Value= "2 Row Primera Celda"};
				r.AddCell(c);
				c= new Cell(){Value= "2 Row Segunda Fila"};
				r.AddCell(c);
				t.AddRow(r);

				r = new Row(){CellStyle=cellStyle};
				c= new Cell(){Value= "3 Row Primera Celda"};
				r.AddCell(c);
				c= new Cell(){Value= "3 Row Segunda Fila"};
				r.AddCell(c);
				t.AddRow(r);

				r = new Row(){CellStyle=cellStyle, Style= new RowStyle{BackgroundColor="red"}};
				c= new Cell(){Value= "4 Row Primera Celda"};
				r.AddCell(c);
				c= new Cell(){Value= "4 Row Segunda Fila"};
				r.AddCell(c);
				t.AddRow(r);


				r = new Row(){CellStyle=cellStyle, Style= new RowStyle{BackgroundColor="blue"}};
				r.CellStyle= new CellStyle(){Padding= new ElementPadding{Top=10, Bottom=10}};  // apply to all cell of this row
				c= new Cell(){Value= "5 Row Primera Celda"};
				r.AddCell(c);
				c= new Cell(){Value= "5 Row Segunda Fila"};
				r.AddCell(c);
				t.AddRow(r);

				r = new Row(){CellStyle=cellStyle, Style= new RowStyle{BackgroundColor="yellow"}};
				new CellStyle(){Padding= new ElementPadding{Top=10, Bottom=10}};  
				c= new Cell(){Value= "5 Row Primera Celda"};
				r.AddCell(c);

				c= new Cell(){
					Value= "5 Row Segunda Fila",
					Style = new CellStyle(){
						Padding= new ElementPadding{AllSides=20},
						Color="red",
						FontSize=15,
						FontStyle="italic",
						FontWeight="bold"
					} // apply this cell
				};
				r.AddCell(c);
				t.AddRow(r);

				Console.WriteLine(t.ToString());

				//---

				//---



				var visitor = ReadExtensions.CreateExpression<OfertaInforme>();
				var predicate = PredicateBuilder.True<OfertaInforme>();
	
				if(request.Desde!=default(DateTime)){
						predicate= q=>q.FechaEnvio>=request.Desde;
				}
				else
					throw HttpError.Unauthorized("Debe Indicar la fecha de inicio del informe (Desde)");

				if(request.Hasta!=default(DateTime)){
					predicate= predicate.OrElse(q=>q.FechaEnvio<=request.Hasta);
				}
				else
					throw HttpError.Unauthorized("Debe Indicar la fecha de terminacion del informe (Hasta)");

				predicate= predicate.AndAlso(q=>q.FechaAnulado==null);

				visitor.Where(predicate);

				return Factory.Execute(proxy=>{
					                	
					return new  Response<OfertaInforme>(){
						Data=proxy.Get(visitor)
                	};

				});
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<OfertaInforme>>(e,"GetOfertaInformeError");
			}
		}


	}
}
