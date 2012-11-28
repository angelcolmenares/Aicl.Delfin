using System.Web;
using System;

namespace Aicl.Cayita
{
	public static class Extensions
	{
		internal static readonly string HtmlSpace = "&nbsp;";

		internal static readonly string DateFormat = "dd.MM.yyyy";

		public static string ValueOrHtmlSpace(this string value){
			return !string.IsNullOrEmpty(value)?value:HtmlSpace;
		}

		public static string Decode(this string text)
		{
			return HttpUtility.UrlDecode(text,System.Text.Encoding.UTF8);
		}

		public static string Encode(this string text)
		{
			return HttpUtility.UrlEncode(text,System.Text.Encoding.UTF8);
		}



		public static string Format(this DateTime date){
			return date!=default(DateTime)? date.ToString(DateFormat):string.Empty;
		}

		public static string Format(this DateTime? date){
			return date.HasValue? Format(date.Value):Format(new DateTime());
		}


	}
}

