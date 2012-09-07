using System.Collections.Generic;
using Aicl.Delfin.Model.Types;

namespace Aicl.Delfin.Setup
{
	public class Consecutivos:List<Consecutivo>
	{
		public Consecutivos ():base(new Consecutivo[]{
			new Consecutivo{Documento="Consecutivo", Prefijo="CT", Valor=0 },

		})
		{
		}
	}
}