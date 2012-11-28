using System;
using System.Collections.Generic;

namespace Aicl.Cayita
{

	public class HtmlGrid<T>:GridBase<T>{

		public HtmlGrid():base(){
			Style = new HtmlGridStyle();
			Columns = new List<GridColumnBase<T>>();
		}

		public override GridColumnBase<T> CreateGridColumn(){

			GridColumn<T> gc = new GridColumn<T>();

			if(Style!=default(GridStyleBase)){
				gc.CellStyle=Style.CellStyle;

				gc.HeaderCellStyle= Style.HeaderCellStyle;  // th
				gc.HeaderTextSytle= Style.HeaderTextStyle;  // p

				gc.FooterCellStyle= Style.FooterCellStyle;  // th

			} 

			return gc;
		}

	}



}


