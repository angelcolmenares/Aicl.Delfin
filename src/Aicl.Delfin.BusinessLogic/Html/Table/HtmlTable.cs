namespace Aicl.Cayita
{

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

}


/*
font-size: 15px; 
font-weight: bold; 
font-style : normal || italic 
font-family: verdana,arial,sans-serif;

*/

