using System.Collections.Generic;
using Aicl.Delfin.Model.Types;

namespace Aicl.Delfin.Setup
{
	public class FormasPago:List<FormaPago>
	{
		public FormasPago ():base(new FormaPago[]{
			new FormaPago{Descripcion="100% anticipado", Activo=true, Modo="Contado", DiasCredito=0 },
			new FormaPago{Descripcion="50% al inicio 50% contra entrega", Activo=true, Modo="Contado", DiasCredito=0 },
			new FormaPago{Descripcion="Credito 30 días", Activo=true, Modo="Crédito", DiasCredito=30 },
			new FormaPago{Descripcion="Credito 60 días", Activo=true, Modo="Crédito", DiasCredito=60 },
			new FormaPago{Descripcion="Credito 8 días", Activo=true, Modo="Crédito", DiasCredito=8 },
			new FormaPago{Descripcion="Credito 15 días", Activo=true, Modo="Crédito", DiasCredito=15 },
			new FormaPago{Descripcion="Credito 90 días", Activo=false, Modo="Crédito", DiasCredito=90 },

		})
		{
		}
	}
}