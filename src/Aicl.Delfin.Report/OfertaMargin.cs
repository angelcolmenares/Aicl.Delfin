using System;

namespace Aicl.Delfin.Report
{
	public class OfertaMargin
	{
		public OfertaMargin ():this(5,5,90,15)
		{

		}

		public OfertaMargin (float left, float right, float top, float bottom)
		{
			Left= left;
			Right = right;
			Top = top;
			Bottom = bottom;
		}

		public float Left {get; set;}
		public float Right {get; set;}
		public float Top {get; set;}
		public float Bottom {get;set;}
	}
}

