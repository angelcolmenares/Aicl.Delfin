using System;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceHost;
using System.ComponentModel.DataAnnotations;

namespace Aicl.Delfin.Model.Types
{
	[JoinTo(typeof(Procedimiento),"IdProcedimiento","Id",Order=0)]
	[RestService("/ServicioProcedimiento/create","post")]
	[RestService("/ServicioProcedimiento/read","get")]
	[RestService("/ServicioProcedimiento/update/{Id}/{Activo}","put" )]
	[RestService("/ServicioProcedimiento/destroy/{Id}","delete" )]
	public class ServicioProcedimiento:IHasActivo
	{
		public ServicioProcedimiento ()
		{
		}

		[AutoIncrement]
		public int Id { get; set; }
		        
        public int IdServicio {get;set;}

		public int IdProcedimiento {get; set;}

		public bool Activo {get;set;}

		#region Procedimiento
		[BelongsTo(typeof(Procedimiento),"Nombre")]
		public string NombreProcedimiento {get;set;}

		[StringLength(4096)]
		[BelongsTo(typeof(Procedimiento),"Descripcion")]
		public string DescripcionProcedimiento {get;set;}

		[DecimalLength(18,2)]
		[BelongsTo(typeof(Procedimiento),"ValorUnitario")]
		public decimal ValorUnitarioProcedimiento {get; set;}

		[DecimalLength(5,2)]
		[BelongsTo(typeof(Procedimiento),"PorcentajeIva")]
		public decimal PorcentajeIvaProcedimiento {get; set;}

		[BelongsTo(typeof(Procedimiento),"Activo")]
		public bool ActivoProcedimiento {get; set;}
		#endregion Procedimiento


		[Ignore]
		[DecimalLength(18,2)]
		public decimal ValorBaseProcedimiento {
			get {
				return Math.Ceiling(ValorUnitarioProcedimiento/(1.00m+PorcentajeIvaProcedimiento/100.00m));
			}
		}

		[Ignore]
		[DecimalLength(18,2)]
		public decimal ValorIvaProcedimiento {
			get {
				return ValorUnitarioProcedimiento-ValorBaseProcedimiento;
			}
		}
	}
}