using System;

namespace Aicl.Cayita
{
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
			int? t, r, b, l=null;
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
}

