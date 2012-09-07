using System;
using ServiceStack.DataAnnotations;
using ServiceStack.DesignPatterns.Model;
using ServiceStack.ServiceHost;
using System.ComponentModel.DataAnnotations;

namespace Aicl.Delfin.Model.Types
{
	[JoinTo(typeof(Servicio),"IdServicio","Id", Order=0) ]
	[JoinTo(typeof(Procedimiento),"IdProcedimiento","Id", Order=1) ]
	[RestService("/PedidoItem/create","post")]
	[RestService("/PedidoItem/read","get")]
	[RestService("/PedidoItem/update/{Id}","put")]
	[RestService("/PedidoItem/destroy/{Id}","delete" )]
	public class PedidoItem:IHasId<int>
	{
		public PedidoItem ()
		{
		}

		[AutoIncrement]
		public int Id { get; set; }

		public int IdServicio { get; set;}

		public int IdProcedimiento { get; set;}

		public int Cantidad {get; set;}

		[StringLength(256)]
		public string Descripcion {get;set;} // rango y otras cosas

		[StringLength(256)]
		public string Nota {get;set;} // rango y otras cosas

		[BelongsTo(typeof(Servicio),"Nombre")]
		public string NombreServicio {get; set;}

		[BelongsTo(typeof(Procedimiento),"Descripcion")]
		public string DescripcionProcedimiento {get; set;}

		[DecimalLength(10,6)]
		public decimal Descuento {get; set;}

		[DecimalLength(5,2)]
		public decimal PorcentajeIva {get; set;}

		[DecimalLength(18,2)]
		public decimal ValorBase {get; set;	}

		[Ignore]
		[DecimalLength(18,2)]
		public decimal ValorUnitario{
			get{
				return Math.Floor(ValorBase*(1.00m+PorcentajeIva/100.00m));
			}
		}

		[Ignore]
		[DecimalLength(18,2)]
		public decimal ValorIva {
			get{
				return ValorUnitario-ValorBase;
			}
		}
	}
}