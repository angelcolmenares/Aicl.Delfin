namespace Aicl.Delfin.Html
{

	public class BlueGridStyle:GridStyleBase
	{
		public BlueGridStyle ():base()
		{
			TableStyle.Border= new  TableBorderProperty 
			{
				Width=new BorderWidthProperty(){
					AllSides=1
				},
				Style ="solid",
				AllBorderSpacing=0,
				Radius= new BorderRadiusProperty(){
					AllSides=10
				},
					Color="black"
			};

			TableStyle.Padding.AllSides=10;

			HeaderStyle.BackgroundColor= "#00557F";
			HeaderStyle.Color="#FFFFFF";

			AlternateRowStyle.BackgroundColor= "#E1EEF4";

			TitleStyle.Padding.AllSides=4;

			HeaderCellStyle.Padding.AllSides=4;
			CellStyle.Padding.AllSides=2;



		}

	}
}
//background-color:#00557F; color:#FFFFFF;

/*
public static HtmlRowStyle DefaultAlternateRowStyle {
			get {
				return new HtmlRowStyle(){BackgroundColor="#E1EEF4"};
			}
		}

		public static HtmlTableStyle DefaultTableStyle {
			get {return new HtmlTableStyle{
					Border = new  TableBorderProperty {
						Width=new BorderWidthProperty(){
							AllSides=1
						},
						Style ="solid",
						AllBorderSpacing=0,
						Radius= new BorderRadiusProperty(){
							AllSides=10
						},
						Color="black"
					},
					Padding = new PaddingProperty{
						AllSides=10
					}
				};
			}
		}
*/
