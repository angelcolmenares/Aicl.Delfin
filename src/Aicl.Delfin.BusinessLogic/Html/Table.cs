using System;
using ServiceStack.Markdown;
namespace Aicl.Delfin.Html
{

	public abstract class TagWithStyle:TagBuilder{

		public TagWithStyle(string tagName):base(tagName){

		}

		public ElementStyle Style {
			get;set;
		}

		protected string ToHtml(){

			if(Style!=default(ElementStyle)){
				var style= Style.ToString();
				if(!string.IsNullOrEmpty(style)) Attributes["style"]= style;
			}

			return base.ToString(TagRenderMode.Normal);
		}

		public override string ToString ()
		{
			return ToHtml();
		}

	}

	public class Paragrag:TagWithStyle{

		public Paragrag():base("p"){}

		public string Text {get;set;}

		public override string ToString(){

			if(!string.IsNullOrEmpty(Text)) InnerHtml =Text;
			return base.ToHtml();
		}

	}

	#region Table
	public class Table:TagBuilder
	{
		public TableStyle Style{get;set;}

		public RowStyle RowStyle{get;set;}

		public TableHeader Header {get;set;}
		public TableFooter Footer {get;set;}

		int RowsCount{get;set;}

		public Table ():base("table")
		{
			RowsCount=0;
			InnerHtml=string.Empty;
		}

		internal Table(string tagName):base(tagName){
			RowsCount=0;
			InnerHtml=string.Empty;
		}

		public void AddRow(Row row){
			InnerHtml=InnerHtml+row.ToString( RowsCount%2==0? RowStyle: AlternateRowStyle??RowStyle);
			RowsCount++;
		}

		public override string ToString ()
		{
			if(Style!=default(TableStyle)){
				var style= Style.ToString();
				if(!string.IsNullOrEmpty(style)) Attributes["style"]= style ;
			}

			if(Footer!=default(TableFooter)){
				InnerHtml= Footer.ToString()+ InnerHtml;
			}

			if(Header!=default(TableHeader)){
				InnerHtml= Header.ToString()+ InnerHtml;
			}

			return base.ToString(TagRenderMode.Normal);
		}

		public static TableStyle DefaultTableStyle {
			get {return new TableStyle{
					//style="border-collapse:separate;padding:1px 1px 1px 1px;border-spacing:10px;border-width:1px;border-style:solid;border-color:black;background-color:white; border-radius:5px;" 
					BorderStyle = new TableBorder{
						Width=new BorderWidth(){
							AllSides=1
						},
						Style ="solid",
						AllBorderSpacing=0,
						Radius= new BorderRadius(){
							AllSides=10
						},
						Color="black"
					},
					Padding = new ElementPadding{
						AllSides=10
					}

				};
			}
		}

		public static RowStyle DefaultRowStyle {
			get {
				return new RowStyle();
			}
		}

		public  RowStyle AlternateRowStyle { get;set;}

		public  RowStyle DefaultAlternateRowStyle {
			get {
				return new RowStyle(){BackgroundColor="#E1EEF4"};
			}
		}

		public static CellStyle DefaultCellStyle {
			get {return new CellStyle{
					Padding = new ElementPadding{
						AllSides=2
					}
				};
			}
		}

	}
	#endregion Table

	#region Row
	public class Row:TagBuilder
	{
		public CellStyle CellStyle {get;set;}

		public RowStyle Style{get;set;}

		public Row():base("tr"){
			InnerHtml=string.Empty;
		}

		public void AddCell(TagCell cell){
			InnerHtml=InnerHtml+cell.ToString(CellStyle); 
		}

		public int? RowSpan {get;set;}

		internal string ToString(RowStyle rowStyle){

			if (RowSpan.HasValue && RowSpan.Value!=default(int))
				Attributes["rowspan"]=RowSpan.Value.ToString();

			RowStyle rs = Style??rowStyle;
			if(rs!=default(RowStyle)){
				var style= rs.ToString();
				if(!string.IsNullOrEmpty(style)) Attributes["style"]= style;
			}

			return base.ToString(TagRenderMode.Normal);
		}
	}

	public class TableHeader:Table{

		public TableHeader():base("thead"){

		}

	}

	public class TableFooter:Table{

		public TableFooter():base("tfoot"){}
	}


	#endregion Row


	#region Cell
	public abstract class TagCell:TagBuilder
	{
		public CellStyle Style{get;set;}

		public TagCell(string tag):base(tag){
			InnerHtml=string.Empty;
		}

		public TagCell(string tag,object value):base(tag){
			InnerHtml= value.ToString();
		}

		public object Value{
			get{ return InnerHtml;}
			set{ InnerHtml=value.ToString();}
		}

		public int? ColumnSpan{get;set;}

		internal string ToString(CellStyle cellStyle){

			if(ColumnSpan.HasValue && ColumnSpan.Value!=default(int))
				Attributes["colspan"]=ColumnSpan.Value.ToString();

			CellStyle cs = Style??cellStyle;
			if(cs!=default(CellStyle)){
				var style= cs.ToString();
				if(!string.IsNullOrEmpty(style)) Attributes["style"]= style;
			}
			return base.ToString(TagRenderMode.Normal);
		}
	}

	public class Cell:TagCell
	{
		public Cell():base("td"){}
		public Cell(object value):base("td",value){}
	}

	public class ColumnHeader:TagCell
	{
		public ColumnHeader():base("th"){}
		public ColumnHeader(object value):base("th",value){}
	}

	public class ColumnFooter:ColumnHeader{
		public ColumnFooter():base(){}
	}

	#endregion Cell

	#region Border
	public class Border{

		public Border(){
			Width = new BorderWidth();
			Radius = new BorderRadius();
		}

		public BorderWidth Width {get;set;}
		public BorderRadius Radius {get;set;}
		public string Style {get;set;}
		public string Color {get;set;}
		public override string ToString(){
		
			var bw= Width==default(BorderWidth)? string.Empty: Width.ToString();
			var st = string.IsNullOrEmpty(Style)?string.Empty: string.Format("border-style:{0};",Style);
			var cl = string.IsNullOrEmpty(Color)?string.Empty: string.Format("border-color:{0};",Color);
			var br= Radius==default(BorderRadius)? string.Empty: Radius.ToString();
			return string.Format("{0}{1}{2}{3}",
			                     bw,st,cl,br);
		}
	}
	#endregion Border

	#region TableBorder
	public class TableBorder:Border{
		public TableBorder():base(){}

		public int? AllBorderSpacing {get;set;}
		public int? HorizontalBorderSpacing {get;set;}
		public int? VerticalBorderSpacing {get;set;}

		public string Collapse{get;set;}

		public override string ToString ()
		{
			string bs=string.Empty;
			if(VerticalBorderSpacing.HasValue){
				bs=string.Format("border-spacing:{0}px {1}px",HorizontalBorderSpacing??(AllBorderSpacing??0), VerticalBorderSpacing );
			}
			else if(HorizontalBorderSpacing.HasValue){
				bs=string.Format("border-spacing:{0}px{1}",HorizontalBorderSpacing,
				                 AllBorderSpacing.HasValue? string.Format(" {0}px",AllBorderSpacing):string.Empty );
			}
			else{
				bs= AllBorderSpacing.HasValue? string.Format("border-spacing:{0}px",AllBorderSpacing):string.Empty;
			}

			var bc = string.IsNullOrEmpty(Collapse)?string.Empty: string.Format("border-collapse:{0};",Collapse);
			return base.ToString()+  string.Format("{0}{1}",bc,bs);
		}
	}
	#endregion TableBorder

	#region RowBorder
	public class RowBorder:Border{
		public RowBorder():base(){}
	}
	#endregion RowBorder


	#region CellBorder
	public class CellBorder:RowBorder{
		public CellBorder():base(){}
	}
	#endregion CellBorder


	#region ElementStyle
	public class ElementStyle{

		public ElementStyle(){
			Width = new ElementWidth();
			Height= new ElementHeight();
			Padding = new ElementPadding();
			Color=string.Empty;
			BackgroundColor=string.Empty;
		}

		public ElementWidth Width {get;set;}
		public ElementHeight Height  {get;set;}
		public ElementPadding Padding {get;set;}
		public string BackgroundColor {get;set;}
		public int? FontSize{get;set;}
		public string FontWeight{get;set;}
		public string FontStyle{get;set;}
		public string FontFamily{get;set;}
		public string TextAlign {get;set;}


/*
font-size: 15px; 
font-weight: bold; 
font-style : normal || italic 
font-family: verdana,arial,sans-serif;

*/
		public string Color {get;set;}

		public override string ToString ()
		{
			var r= string.Format ("{0}{1}{2}", Width, Height, Padding);
			if(!string.IsNullOrEmpty(BackgroundColor)) 
				r=string.Format("{0} background-color:{1};",r, BackgroundColor);
			if(!string.IsNullOrEmpty(Color)) 
				r=string.Format("{0} color:{1};",r, Color);
			if(FontSize.HasValue)
				r=string.Format("{0} font-size:{1};",r, FontSize.Value);
			if(!string.IsNullOrEmpty(FontWeight)) 
				r=string.Format("{0} font-weight:{1};",r, FontWeight);
			if(!string.IsNullOrEmpty(FontStyle)) 
				r=string.Format("{0} font-style:{1};",r, FontStyle);
			if(!string.IsNullOrEmpty(FontFamily)) 
				r=string.Format("{0} font-family:{1};",r, FontFamily);
			if(!string.IsNullOrEmpty(TextAlign)) 
				r=string.Format("{0} text-align:{1};",r, TextAlign);


			return r.Trim();
		}
	}
	#endregion ElementStyle

	public class ElementWidth{
		public ElementWidth(){
			Unit="%";
		}

		public int? Width {get;set;}
		public string Unit {get;set;}

		public override string ToString ()
		{
			return Width.HasValue? string.Format("width:{0}{1};",Width.Value,Unit):string.Empty;
		}
	}

	public class ElementHeight{
		public ElementHeight(){
			Unit="px";
		}

		public int? Height {get;set;}
		string Unit {get;set;}

		public override string ToString ()
		{
			return Height.HasValue? string.Format("height:{0}{1};",Height.Value,Unit):string.Empty;
		}
	}

	public class BorderWidth:ElementSide{
		public BorderWidth(){
			Unit="px";
		}
		public override string ToString(){
			var r= base.ToString();
			return (string.IsNullOrEmpty(r))?
				string.Empty:
					string.Format("border-width:{0}",r);
		}
	}

	public class BorderRadius:ElementSide{
		public BorderRadius(){
			Unit="px";
		}
		public override string ToString(){
			var r= base.ToString();
			return (string.IsNullOrEmpty(r))?
				string.Empty:
					string.Format("border-radius:{0}",r);
		}
	}


	public class ElementPadding:ElementSide{
		public ElementPadding(){
			Unit="px";
		}
		public override string ToString(){
			var r= base.ToString();
			return (string.IsNullOrEmpty(r))?
				string.Empty:
					string.Format("padding:{0}",r);
		}
	}

	public class ElementSideProperty:ElementSide{

		public ElementSideProperty(string property){
			Unit="px";
			Property=property;
		}
		public string Property {get;set;}
		public override string ToString(){
			var r= base.ToString();
			return (string.IsNullOrEmpty(r))?
				string.Empty:
					string.Format("{0}:{1}", Property, r);
		}
	}

	public abstract class ElementSide{
		public ElementSide(){
			Unit="px";
		}

		public int? AllSides {get;set;}
		public int? Top {get;set;}
		public int? Right {get;set;}
		public int? Bottom {get;set;}
		public int? Left {get;set;}

		public string Unit {get;set;}

		public override string ToString ()
		{
			int? t, r, b, l;
			if(Left.HasValue){
				l=Left.Value;
				b= Bottom??(AllSides??0);
				r= Right??(AllSides??0);
				t= Top??(AllSides??0);
			}
			else if(Bottom.HasValue){
				l =AllSides;
				b=Bottom.Value;
				r= Right??(AllSides??0);
				t= Top??(AllSides??0);
			}
			else if(Right.HasValue){
				l =AllSides;
				b =AllSides;
				r=Right.Value;
				t= Top??(AllSides??0);
			}
			else if(Top.HasValue){
				l =AllSides;
				b =AllSides;
				r =AllSides;
				t=Top.Value;
			}
			else{
				t =AllSides;
			}

			var rs= string.Format("{0}{1}{2}{3}",
			                     t.HasValue?t.Value.ToString()+Unit+" ":string.Empty,
			                     r.HasValue?r.Value.ToString()+Unit+" ":string.Empty,
			                     b.HasValue?b.Value.ToString()+Unit+" ":string.Empty,
			                     l.HasValue?l.Value.ToString()+Unit:string.Empty).Trim();

			return string.IsNullOrEmpty(rs)?string.Empty:rs+";";
		}
	}

	public class TableStyle:ElementStyle{

		public TableStyle():base(){
					
		}
		public TableBorder BorderStyle {get;set;}

		public override string ToString ()
		{
			return base.ToString() + ((BorderStyle==default(TableBorder))?"":BorderStyle.ToString());
		}
	}

	public class RowStyle:ElementStyle{

		public RowStyle():base(){}


		public override string ToString ()
		{
			return base.ToString() ;
		}
	}

	//-------------------------------------------------------------------------------

	public class CellStyle:ElementStyle{

		public CellStyle():base(){}

		public CellBorder BorderStyle {get;set;}


		public override string ToString ()
		{
			return 
				base.ToString()+
					((BorderStyle==default(CellBorder))?"":BorderStyle.ToString());
		}
	}


}

/*
font-size: 15px; 
font-weight: bold; 
font-style : normal || italic 
font-family: verdana,arial,sans-serif;

.datagrid table 
{
border-collapse: collapse; 
text-align: left; 
width: 100%; 
}

.datagrid 
{
font: normal 12px/150% Times New Roman, Times, serif; 
background: #fff; 
overflow: hidden; 
border: 4px solid #006699; 
-webkit-border-radius: 10px; 
-moz-border-radius: 10px; 
border-radius: 10px; 
}
.datagrid table td, .datagrid table th 
{ 
padding: 7px 6px; 
}
.datagrid table thead th 
{
background:-webkit-gradient( linear, left top, left bottom, color-stop(0.05, #006699), color-stop(1, #00557F) );
background:-moz-linear-gradient( center top, #006699 5%, #00557F 100% );
filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#006699', endColorstr='#00557F');
background-color:#006699; 
color:#FFFFFF; 
font-size: 15px; 
font-weight: bold; 
border-left: 3px solid #008ACF;
} 
.datagrid table thead th:first-child 
{ 
border: none; 
}
.datagrid table tbody td 
{ 
color: #00496B; 
border-left: 1px solid #E1EEF4;
font-size: 12px;
font-weight: normal; 
}
.datagrid table tbody .alt td 
{ 
background: #F4F489;
color: #691321; 
}
.datagrid table tbody td:first-child 
{ border-left: none; }
.datagrid table tbody tr:last-child td { border-bottom: none; }.datagrid table tfoot td div { border-top: 1px solid #006699;background: #E1EEF4;} .datagrid table tfoot td { padding: 0; font-size: 12px } .datagrid table tfoot td div{ padding: 2px; }.datagrid table tfoot td ul { margin: 0; padding:0; list-style: none; text-align: right; }.datagrid table tfoot  li { display: inline; }.datagrid table tfoot li a { text-decoration: none; display: inline-block;  padding: 2px 8px; margin: 1px;color: #FFFFFF;border: 1px solid #006699;-webkit-border-radius: 3px; -moz-border-radius: 3px; border-radius: 3px; background:-webkit-gradient( linear, left top, left bottom, color-stop(0.05, #006699), color-stop(1, #00557F) );background:-moz-linear-gradient( center top, #006699 5%, #00557F 100% );filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#006699', endColorstr='#00557F');background-color:#006699; }.datagrid table tfoot ul.active, .datagrid table tfoot ul a:hover { text-decoration: none;border-color: #006699; color: #FFFFFF; background: none; background-color:#00557F;}


TABLE
border-width (px)	1px 2px 3px 4px 5px thin medium thick 0px
border-style	outset none hidden dotted dashed solid double ridge groove inset
border-color	gray white blue green black red custom:	
background-color	white #FFFFF0 #FAF0E6 #FFF5EE #FFFAFA custom:	

border-spacing	2px 1px 3px 4px 5px 0px  solo a  la tabla
border-collapse	separate collapse

TD/TH
border-width (px)	1px 2px 3px 4px 5px thin medium thick 0px
border-style	inset none hidden dotted dashed solid double ridge groove outset
border-color	gray white blue green black red custom:	
background-color	white #FFFFF0 #FAF0E6 #FFF5EE #FFFAFA custom:	

padding	1px 2px 3px 4px 5px 0px
-moz-border-radius	0px 3px 6px 9px 12px

<table width="200" cellpadding="0" cellspacing="0" border="0"
style="background-color: #9C084A">
.datagrid table { border-collapse: collapse; text-align: left; width: 100%; } .datagrid {font: normal 12px/150% Times New Roman, Times, serif; background: #fff; overflow: hidden; border: 4px solid #FFFFFF; -webkit-border-radius: 7px; -moz-border-radius: 7px; border-radius: 7px; }.datagrid table td, .datagrid table th { padding: 3px 10px; }.datagrid table thead th {background:-webkit-gradient( linear, left top, left bottom, color-stop(0.05, #006699), color-stop(1, #00557F) );background:-moz-linear-gradient( center top, #006699 5%, #00557F 100% );filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#006699', endColorstr='#00557F');background-color:#006699; color:#FFFFFF; font-size: 15px; font-weight: bold; border-left: 1px solid #0070A8; } .datagrid table thead th:first-child { border: none; }.datagrid table tbody td { color: #00496B; border-left: 1px solid #E1EEF4;font-size: 12px;border-bottom: 2px solid #F44E11;font-weight: normal; }.datagrid table tbody .alt td { background: #E1EEF4; color: #00496B; }.datagrid table tbody td:first-child { border-left: none; }.datagrid table tbody tr:last-child td { border-bottom: none; }.datagrid table tfoot td div { border-top: 1px solid #FFFFFF;background: #E1EEF4;} .datagrid table tfoot td { padding: 0; font-size: 12px } .datagrid table tfoot td div{ padding: 2px; }.datagrid table tfoot td ul { margin: 0; padding:0; list-style: none; text-align: right; }.datagrid table tfoot  li { display: inline; }.datagrid table tfoot li a { text-decoration: none; display: inline-block;  padding: 2px 8px; margin: 1px;color: #FFFFFF;border: 1px solid #006699;-webkit-border-radius: 3px; -moz-border-radius: 3px; border-radius: 3px; background:-webkit-gradient( linear, left top, left bottom, color-stop(0.05, #006699), color-stop(1, #00557F) );background:-moz-linear-gradient( center top, #006699 5%, #00557F 100% );filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#006699', endColorstr='#00557F');background-color:#006699; }.datagrid table tfoot ul.active, .datagrid table tfoot ul a:hover { text-decoration: none;border-color: #006699; color: #FFFFFF; background: none; background-color:#00557F;}


*/