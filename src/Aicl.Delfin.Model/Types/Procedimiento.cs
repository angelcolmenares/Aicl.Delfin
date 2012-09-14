using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceHost;

namespace Aicl.Delfin.Model.Types
{

	[RestService("/Procedimiento/create","post")]
	[RestService("/Procedimiento/read","get")]
	[RestService("/Procedimiento/read/Nombre/{Nombre}","get")]
	[RestService("/Procedimiento/update/{Id}","put")]
	[RestService("/Procedimiento/destroy/{Id}","delete" )]
	public class Procedimiento:IHasActivo
	{
		public Procedimiento ()
		{
		}

		[AutoIncrement]
		public int Id { get; set; }

        [StringLength(96)]
        [Required]
		[Index]
        public string Nombre {get;set;}

		[StringLength(4096)]
		[Index]
        public string Descripcion {get;set;}

		[DecimalLength(18,2)]
		public decimal ValorUnitario {get; set;} // incluye el iva

		[DecimalLength(5,2)]
		public decimal PorcentajeIva {get; set;}

		public bool Activo {get; set;}


		[Ignore]
		[DecimalLength(18,2)]
		public decimal ValorBase {
			get {
				return Math.Ceiling(ValorUnitario/(1.00m+(PorcentajeIva/100.00m)));
			}
		}

		[Ignore]
		[DecimalLength(18,2)]
		public decimal ValorIva {
			get {
				return ValorUnitario-ValorBase;
			}
		}

	}
}