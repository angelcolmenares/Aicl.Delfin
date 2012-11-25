using System;
using ServiceStack.Markdown;

namespace Aicl.Delfin.Html
{
	public class HtmlParagragh:TagBase{

		public HtmlParagragh():base("p"){
			Style = new HtmlStyle();
		}

		public string Text {
			get{
				return InnerHtml;
			}
			set{
				InnerHtml=value;
			}
		}
	}

	public class HtmlDiv:TagBase{

		public HtmlDiv():base("div"){
			Style = new HtmlDivStyle();
		}

		public void AddHtmlTag(TagBase tag){
			InnerHtml= InnerHtml+ tag;
		}

	}

	public class HtmlTable:TableBase
	{
		public HtmlTable ():base("table"){
			Header= new HtmlTableHeader();
			Footer = new HtmlTableFooter();
		}

		public HtmlTableHeader Header {
			get; set;
		}

		public HtmlTableFooter Footer {
			get; set;
		}

		public override string ToString ()
		{

			if(Footer!=default(HtmlTableFooter)){
				InnerHtml= Footer.ToString()+ InnerHtml;
			}

			if(Header!=default(HtmlTableHeader)){
				InnerHtml= Header.ToString()+ InnerHtml;
			}

			return base.ToString();
		}

	}

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

	public class HtmlTableFooter:TableBase{

		public HtmlTableFooter():base("tfoot"){}

		public override RowBase CreateRow(){

			HtmlFooterRow row = new HtmlFooterRow();
			row.Style= RowsCount%2==0?
				RowStyle:
				AlternateRowStyle??RowStyle;
			RowsCount++;
			return row;
		}
	}

	//--
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

	public class HtmlRow:RowBase{
		protected internal HtmlRow():base(){}
	}

	public class HtmlHeaderRow:RowBase{
		protected internal HtmlHeaderRow():base(){}

		public override CellBase CreateCell(){
			HtmlHeaderCell cell = new HtmlHeaderCell();
			if(CellStyle!=default(HtmlCellStyle)) cell.Style= CellStyle;
			return cell;
		}
	}

	public class HtmlFooterRow:RowBase{
		protected internal HtmlFooterRow():base(){}

		public override CellBase CreateCell(){
			HtmlFooterCell cell = new HtmlFooterCell();
			if(CellStyle!=default(HtmlCellStyle)) cell.Style= CellStyle;
			return cell;
		}
	}

	public abstract class RowBase:TagBuilder{

		protected internal RowBase():base("tr"){
			Style= new HtmlRowStyle();
		}

		public StyleBase Style{get;set;}

		public HtmlCellStyle CellStyle{get;set;}

		public string Id {
			get{
				string id;
				return Attributes.TryGetValue("id", out id)? id:string.Empty;
			}
			set{
				Attributes["id"]= value;
			}
		}

		public virtual CellBase CreateCell(){
			HtmlCell cell = new HtmlCell();
			if(CellStyle!=default(HtmlCellStyle)) cell.Style= CellStyle;
			return cell;
		}

		public virtual void AddCell(CellBase cell){
			InnerHtml=InnerHtml +cell.ToString();
		}

		public int? RowSpan {get;set;}

		public override string ToString ()
		{
			if (RowSpan.HasValue && RowSpan.Value!=default(int))
				Attributes["rowspan"]=RowSpan.Value.ToString();

			if( Style!=default(StyleBase) ){
				var s = Style.ToString();
				if( !string.IsNullOrEmpty(s)) Attributes["style"]= Style.ToString();
			}

			return base.ToString(TagRenderMode.Normal);
		}

	}


	public class HtmlRowStyle:StyleBase{
		public HtmlRowStyle():base(){
		}
	}


	public class HtmlCell:CellBase{
		internal protected HtmlCell():base("td"){}
	}

	public class HtmlHeaderCell:CellBase
	{
		public HtmlHeaderCell():base("th"){}

	}

	public class HtmlFooterCell:CellBase{
		public HtmlFooterCell():base("th"){}
	}



	public abstract class CellBase:TagBase{

		internal protected CellBase(string tagName):base(tagName){
			Style = new HtmlCellStyle();
		}

		public int? ColumnSpan{
			get{ 
				int? r=null;
				string v;
				if(Attributes.TryGetValue("colspan", out v)){
					int i;
					if (int.TryParse(v, out i)) r=i; 
				}
				return r;
			}
			set{
				if(value.HasValue && value.Value!=default(int))
					Attributes["colspan"]= value.Value.ToString();
				else
					if(Attributes.ContainsKey("colspan")) Attributes.Remove("colspan");
			}
		}

		public void AddHtmlTag(TagBase tag){
			InnerHtml= InnerHtml+ tag;
		}

		public void SetValue(object value){
			InnerHtml= value!=null?value.ToString():"";
		}

	}


	public class HtmlCellStyle:ElementStyleBase{

		public HtmlCellStyle():base(){
			Border= new CellBorderProperty();
		}
	}


	public class CellBorderProperty:BorderPropertyBase{
		public CellBorderProperty():base(){}
	}


	public abstract class TagBase:TagBuilder{  

		public TagBase(string tagName):base(tagName){}

		public virtual ElementStyleBase Style {get;set;}

		public string Id {
			get{
				string id;
				return Attributes.TryGetValue("id", out id)? id:string.Empty;
			}
			set{
				Attributes["id"]= value;
			}
		}


		public override string ToString ()
		{
			if( Style!=default(ElementStyleBase) ){
				var s = Style.ToString();
				if( !string.IsNullOrEmpty(s)) Attributes["style"]= Style.ToString();
			}

			return base.ToString(TagRenderMode.Normal);
		}
	}


	public class HtmlTableStyle:ElementStyleBase{

		public HtmlTableStyle():base(){
			Border= new TableBorderProperty();
		}

	}

	public class HtmlDivStyle:ElementStyleBase{

		public HtmlDivStyle():base(){
			Border= new DivBorderProperty();
		}
	}

	public class HtmlStyle:ElementStyleBase{
		public HtmlStyle():base(){}
	}


	public abstract class StyleBase{  

		public StyleBase()
		{
			Width = new WidthProperty();
			Height= new HeightProperty();
			Padding = new PaddingProperty();
			Margin = new MarginProperty();

			Color=string.Empty;
			BackgroundColor=string.Empty;

		}

		public WidthProperty Width {get;set;}
		public HeightProperty Height  {get;set;}
		public PaddingProperty Padding {get;set;}
		public MarginProperty Margin {get;set;}

		public string BackgroundColor {get;set;}

		public int? FontSize{get;set;}
		public string FontWeight{get;set;}
		public string FontStyle{get;set;}
		public string FontFamily{get;set;}
		public string TextAlign {get;set;}

		public string Color {get;set;}

		public override string ToString ()
		{
			var r= string.Format ("{0}{1}{2}{3}", Width, Height, Padding,Margin);

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

			var v = r.Trim();
			return string.IsNullOrEmpty(v)?string.Empty:v;

		}
	}


	public abstract class ElementStyleBase:StyleBase{  //antiguo ElementStyle 

		public ElementStyleBase():base(){}

		public virtual BorderPropertyBase Border {get;set;}

		public override string ToString ()
		{
			var r= base.ToString();
			var v = r + (Border==default(BorderPropertyBase)?string.Empty: Border.ToString());
			return string.IsNullOrEmpty(v)?string.Empty:v;

		}
	}


	public class WidthProperty:MeasurePropertyBase{

		public WidthProperty():base("width"){}
	
	}

	public class HeightProperty:MeasurePropertyBase{

		public HeightProperty():base("height"){}

	}

	public abstract class MeasurePropertyBase{

		public MeasurePropertyBase(string property){
			Unit="px";
			Property= property;
		}

		public int? Value {get;set;}
		public string Property {get;set;}

		string Unit {get;set;}

		public override string ToString ()
		{
			return Value.HasValue? string.Format("{0}:{1}{2};",Property,Value,Unit):string.Empty;
		}
	}

	public class MarginProperty:SideProperty{
		public MarginProperty():base("margin"){}
	}

	public class BorderWidthProperty:SideProperty{
		public BorderWidthProperty():base("border-width"){}
	}

	public class BorderRadiusProperty:SideProperty{
		public BorderRadiusProperty():base("border-radius"){
		}
	}

	public class PaddingProperty:SideProperty{
		public PaddingProperty():base("padding"){}
	}

	public class SideProperty:SideBase{

		public SideProperty(string property):base(){
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


	public abstract class SideBase{

		public SideBase()
		{
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


	public abstract class BorderPropertyBase{

		public BorderPropertyBase(){}

		public BorderWidthProperty Width {get;set;}
		public BorderRadiusProperty Radius {get;set;}
		public string Style {get;set;}
		public string Color {get;set;}
		public override string ToString(){
		
			var bw= Width==default(BorderWidthProperty)? string.Empty: Width.ToString();
			var st = string.IsNullOrEmpty(Style)?string.Empty: string.Format("border-style:{0};",Style);
			var cl = string.IsNullOrEmpty(Color)?string.Empty: string.Format("border-color:{0};",Color);
			var br= Radius==default(BorderRadiusProperty)? string.Empty: Radius.ToString();
			return string.Format("{0}{1}{2}{3}",
			                     bw,st,cl,br).Trim();
		}
	}


	public class DivBorderProperty:BorderPropertyBase{
		public DivBorderProperty():base(){}
	}

	public class TableBorderProperty:BorderPropertyBase{
		public TableBorderProperty():base(){}

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
			return base.ToString()+  string.Format("{0}{1}",bc,bs).Trim();
		}
	}

}


/*
font-size: 15px; 
font-weight: bold; 
font-style : normal || italic 
font-family: verdana,arial,sans-serif;

*/

