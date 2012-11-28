using System;

namespace Aicl.Cayita
{

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


	public abstract class BorderPropertyBase{

		public BorderPropertyBase(){
			Width = new BorderWidthProperty();
			Radius = new BorderRadiusProperty();
		}

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


}

