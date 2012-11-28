using ServiceStack.Markdown;

namespace Aicl.Cayita
{

	public class HtmlIframe:TagBase{
		public HtmlIframe():base("iframe"){}

		public string Url {
			get{
				string src;
				return Attributes.TryGetValue("src", out src)? src:string.Empty;
			}
			set{
				Attributes["src"]=value;
			}
		}

	}


	public class HtmlImage:TagBase{
		public HtmlImage():base("img"){}

		public string AlternateText{
			get{
				string alt;
				return Attributes.TryGetValue("alt", out alt)? alt:string.Empty;
			}
			set{
				Attributes["alt"]=value;
			}
		}

		public string Url {
			get{
				string src;
				return Attributes.TryGetValue("src", out src)? src:string.Empty;
			}
			set{
				Attributes["src"]=value;
			}
		}

	}


	public class HtmlLink:TagBase{

		public HtmlLink():base("a"){
			Style = new HtmlStyle();
		}

		public string Url {
			get{
				string url;
				return Attributes.TryGetValue("href", out url)? url:string.Empty;
			}
			set{
				Attributes["href"]=value;
			}
		}

		// "_blank|_self|_parent|_top|framename"
		public string Target {
			get{
				string target;
				return Attributes.TryGetValue("target", out target)? target:string.Empty;
			}
			set{
				Attributes["target"]=value;
			}
		}

		public string Text {
			get{
				return InnerHtml;
			}
			set{
				InnerHtml=value;
			}
		}

		public override string ToString ()
		{
			BuildStyleAttribue();
			return base.ToString(TagRenderMode.SelfClosing);

		}

	}


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

	}

	public class HtmlSpan:TagBase{

		public HtmlSpan():base("span"){
			Style = new HtmlStyle();
		}

	}

}

