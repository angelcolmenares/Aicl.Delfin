using System;

namespace Aicl.Delfin.Html
{
	public  static  class Renderers
	{
		public static object Format (this DateTime date, string format="dd.MM.yyyy", 
		                             HtmlStyle styleIf=null, 
		                             HtmlStyle styleElse=null,
		                             Func<DateTime,bool> condition=null
		                             )
		{
			HtmlStyle style;

			if( condition!=null){
				style= (condition(date)? styleIf: styleElse );
			}
			else
				style =  new HtmlStyle(){TextAlign="center"};

			return new HtmlParagragh(){
				Text= date!=default(DateTime)? date.ToString(format):"",
				Style= style
			};
		}

		public static object Format (this DateTime? date, string format="dd.MM.yyyy",
		                             HtmlStyle styleIf=null, 
		                             HtmlStyle styleElse=null,
		                             Func<DateTime,bool> condition=null){
			return 
				Format((date.HasValue)? date.Value:default(DateTime),
				       format,  styleIf, styleElse, condition  );
				
		}






	}
}

