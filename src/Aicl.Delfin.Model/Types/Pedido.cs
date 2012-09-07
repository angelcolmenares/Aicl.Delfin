using System;
using ServiceStack.DataAnnotations;
using ServiceStack.DesignPatterns.Model;
using Aicl.Delfin.Model.Types;
using System.ComponentModel.DataAnnotations;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.ServiceHost;

namespace Aicl.Delfin.Model.Types
{
	[JoinTo(typeof(Contacto),"IdContacto","Id",Order=0)]
	[JoinTo(typeof(Contacto),typeof(Cliente),"IdCliente","Id", Order=1)]
	[JoinTo(typeof(FormaPago),"IdFormaPago","Id", Order=3)]
	[JoinTo(typeof(UserAuth),"IdCreadoPor","Id", Order=4)]
	[JoinTo(typeof(UserAuth),"IdEnviadoPor","Id", ChildAlias="SendBy", Order=5, JoinType=JoinType.Left)]
	[JoinTo(typeof(UserAuth),"IdAceptadoPor","Id", ChildAlias="AcceptedBy", Order=6, JoinType=JoinType.Left)]
	[JoinTo(typeof(UserAuth),"IdAnuladoPor","Id", ChildAlias="AnuladoPor", Order=7, JoinType=JoinType.Left)]
	[JoinTo(typeof(Ciudad),"IdCiudadDestinatario","Id",  Order=8)]

	[RestService("/Pedido/create","post")]
	[RestService("/Pedido/read","get")]
	[RestService("/Pedido/update/{Id}","put")]
	[RestService("/Pedido/destroy/{Id}","delete" )]
	public class Pedido:IHasId<int>
	{

		public Pedido ()
		{
		}

		[AutoIncrement]
		public int Id { get;set;}

		[Index(true)]
		public int Consecutivo {get;set;}

		public int IdContacto {get; set;}

		public DateTime FechaCreacion {get;set;}

		public DateTime FechaActualizacion {get;set;}

		public DateTime? FechaEnvio {get;set;}

		public DateTime? FechaAceptacion {get;set;}

		public DateTime? FechaAnulado {get;set;}

		public int DiasDeVigencia {get; set;}

		public DateTime? VigenteHasta {
			get{
				return 
					FechaEnvio.HasValue?
						FechaEnvio.Value.AddDays(DiasDeVigencia>0?DiasDeVigencia-1:0):
						FechaEnvio;
			}
		}

		public int IdCreadoPor {get; set;}
		#region CreadoPor
		[BelongsTo(typeof(UserAuth),"DisplayName")]
		public string NombreCreadoPor {get;set;}
		#endregion CreadoPor

		public int? IdEnviadoPor {get;set;}
		#region EnviadoPor
		[BelongsTo(typeof(UserAuth),"DisplayName", ParentAlias="SendBy")]
		public string NombreEnviadoPor {get;set;}
		#endregion CreadoPor

		public int? IdAceptadoPor {get;set;}
		#region AceptadoPor
		[BelongsTo(typeof(UserAuth),"DisplayName", ParentAlias="AcceptedBy")]
		public string NombreAceptadoPor {get;set;}
		#endregion AceptadoPor

		public int? IdAnuladoPor {get;set;}
		#region AnuladoPor
		[BelongsTo(typeof(UserAuth),"DisplayName", ParentAlias="AnuladoPor")]
		public string NombreAnuladoPor {get;set;}
		#endregion AnuladoPor


		public int IdFormaPago{get; set;}

		#region FormaPago
		[BelongsTo(typeof(FormaPago),"Descripcion")]
		[StringLength(48)]
		public string DescripcionFormaPago { get; set;}
		#endregion FormaPago

		#region Contacto
		[BelongsTo(typeof(Contacto),"Nombre")]
		public string NombreContacto {get;set;}
		#endregion Contacto

		#region Cliente
		[BelongsTo(typeof(Cliente),"Nit")]
		public string NitCliente {get;set;}

        [BelongsTo(typeof(Cliente),"Nombre")]
        public string NombreCliente {get;set;}
		#endregion Cliente

		public int IdCiudadDestinatario { get; set;}

		[BelongsTo(typeof(Ciudad),"Nombre")]
		public string NombreCiudadDestinatario {get;set;}

		[StringLength(128)]
        [Required]
        public string NombreDestinatario {get;set;}

		[StringLength(64)]
        [Required]
        public string CargoDestinatario {get;set;}

		[StringLength(16)]
        public string TelefonoDestinatario {get;set;}

		[StringLength(16)]
        public string FaxDestinatario {get;set;}

		[StringLength(16)]
        public string CelularDestinatario {get;set;}

		[StringLength(128)]
        public string MailDestinatario {get;set;}

		[StringLength(256)]
        public string DireccionDestinatario {get;set;}


	}
}

