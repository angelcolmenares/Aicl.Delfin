using System;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceHost;
using System.ComponentModel.DataAnnotations;
using ServiceStack.DesignPatterns.Model;

namespace Aicl.Delfin.Model.Types
{
	[JoinTo(typeof(Procedimiento),"IdProcedimiento","Id",Order=0)]
	[JoinTo(typeof(Servicio),"IdServicio","Id", Order=1)]
	[RestService("/ServicioProcedimiento/create","post")]
	[RestService("/ServicioProcedimiento/read","get")]
	[RestService("/ServicioProcedimiento/update/{Id}/{Activo}","put" )]
	[RestService("/ServicioProcedimiento/destroy/{Id}","delete" )]
	public class ServicioProcedimiento:IHasId<int>
	{
		public ServicioProcedimiento ()
		{
		}

		[AutoIncrement]
		public int Id { get; set; }
		        
        public int IdServicio {get;set;}

		public int IdProcedimiento {get; set;}

		#region Servicio
		[BelongsTo(typeof(Servicio),"Nombre")]
		public string NombreServicio {get;set;}

		[BelongsTo(typeof(Servicio),"Activo")]
		public bool ActivoServicio {get; set;}
		#endregion Servicio

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