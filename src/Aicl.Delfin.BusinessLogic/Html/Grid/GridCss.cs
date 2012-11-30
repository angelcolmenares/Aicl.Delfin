using System;

namespace Aicl.Cayita
{
	public class GridCss
	{
		public string Name {get;set;} // table class=

		public string AlternateRow {get;set;}

		public GridCss (){
		}

	}

	public class CayitaGridWhite:GridCss{
		public CayitaGridWhite():base(){
			Name="cayitagridwhite";
			AlternateRow="alt";
		}
	}

	public class CayitaGridGrey:GridCss{
		public CayitaGridGrey():base(){
			Name="cayitagridgrey";
			AlternateRow="alt";
		}
	}


	public class CayitaGridBlue:GridCss{
		public CayitaGridBlue():base(){
			Name="cayitagridblue";
			AlternateRow="alt";
		}
	}

}

