using System;

namespace Aicl.Delfin.Html
{
	public  static  class Renderers
	{
		public static HtmlParagragh Format(this bool value, string trueValue="Yes", string falseValue="No",
		                                   HtmlStyle trueStyle=null, 
		                                   HtmlStyle falseStyle=null){
			HtmlStyle style = (value? trueStyle: falseStyle)?? new HtmlStyle(){TextAlign="center" };

			return new HtmlParagragh{
				Style=style,
				Text= value? trueValue: falseValue
			};
		}

		public static HtmlParagragh Format(this bool? value, string trueValue="Yes", string falseValue="No",
		                                   HtmlStyle trueStyle=null, 
		                                   HtmlStyle falseStyle=null){

			if(value.HasValue) return Format (value.Value,trueValue, falseValue, trueStyle,falseStyle);

			return new HtmlParagragh(){
				Text="",
				Style=falseStyle?? new HtmlStyle(){TextAlign="center" }
			};
		}


		public static HtmlParagragh Format(this decimal value, string format="##,0.00", 
		                             HtmlStyle trueStyle=null, 
		                             HtmlStyle falseStyle=null,
		                             Func<decimal,bool> condition=null
		                             )
		{
			HtmlStyle style;

			if( condition!=null){
				style= (condition(value)? trueStyle: falseStyle )??new HtmlStyle(){TextAlign="right" };
			}
			else{
				style =  new HtmlStyle(){TextAlign="right" };
				if(value<0) style.Color="red";
			}

			return new HtmlParagragh(){
				Text=  value.ToString(format),
				Style= style
			};
		}

		public static HtmlParagragh Format(this decimal? value, string format="##,0.00", 
		                             HtmlStyle trueStyle=null, 
		                             HtmlStyle falseStyle=null,
		                             Func<decimal,bool> condition=null
		                             )
		{
			if(value.HasValue) return Format(value.Value, format,trueStyle, falseStyle, condition);

			return new HtmlParagragh(){
				Text="",
				Style=falseStyle?? new HtmlStyle(){TextAlign="right" }
			};
		}


		public static HtmlParagragh Format(this int value, string format="##,0", 
		                             HtmlStyle trueStyle=null, 
		                             HtmlStyle falseStyle=null,
		                             Func<int,bool> condition=null
		                             )
		{
			HtmlStyle style;

			if( condition!=null){
				style= (condition(value)? trueStyle: falseStyle )??new HtmlStyle(){TextAlign="center" };
			}
			else{
				style =  new HtmlStyle(){TextAlign="center" };
				if(value<0) style.Color="red";
			}


			return new HtmlParagragh(){
				Text=  value.ToString(format),
				Style= style
			};
		}

		public static HtmlParagragh Format(this int? value, string format="##,0", 
		                             HtmlStyle trueStyle=null, 
		                             HtmlStyle falseStyle=null,
		                             Func<int,bool> condition=null
		                             )
		{
			if(value.HasValue) return Format(value.Value, format,trueStyle, falseStyle, condition);

			return new HtmlParagragh(){
				Text="",
				Style= falseStyle?? new HtmlStyle(){TextAlign="center"}
			};
		}

		public static HtmlParagragh Format (this DateTime date, string format="dd.MM.yyyy", 
		                             HtmlStyle trueStyle=null, 
		                             HtmlStyle falseStyle=null,
		                             Func<DateTime,bool> condition=null
		                             )
		{
			HtmlStyle style;

			if( condition!=null){
				style= (condition(date)? trueStyle: falseStyle )??new HtmlStyle(){TextAlign="center" };
			}
			else
				style =  new HtmlStyle(){TextAlign="center"};

			return new HtmlParagragh(){
				Text= date!=default(DateTime)? date.ToString(format):"",
				Style= style
			};
		}

		public static HtmlParagragh Format (this DateTime? date, string format="dd.MM.yyyy",
		                             HtmlStyle trueStyle=null, 
		                             HtmlStyle falseStyle=null,
		                             Func<DateTime,bool> condition=null){
			return 
				Format((date.HasValue)? date.Value:default(DateTime),
				       format,  trueStyle, falseStyle, condition  );
				
		}






	}
}

