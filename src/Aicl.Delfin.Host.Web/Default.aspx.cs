using System;
using System.Configuration;

namespace Aicl.Delfin.Host.Web
{
	public partial class Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			string redirect= ConfigurationManager.AppSettings.Get("RedirectTo");
			if(!string.IsNullOrEmpty(redirect))
				Response.Redirect(redirect);
		}	
	}
}
