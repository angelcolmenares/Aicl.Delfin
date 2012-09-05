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
	[JoinTo(typeof(FormaPago),"IdFormaPago","Id", Order=2)]
	[JoinTo(typeof(UserAuth),"IdCreadoPor","Id", Order=3)]
	[JoinTo(typeof(UserAuth),"IdEnviadoPor","Id", ParentAlias="SendBy", Order=4, JoinType=JoinType.Left)]
	[JoinTo(typeof(UserAuth),"IdAceptadoPor","Id", ParentAlias="AcceptedBy", Order=5, JoinType=JoinType.Left)]
	[JoinTo(typeof(UserAuth),"IdAnuladoPor","Id", ParentAlias="AnuladoPor", Order=6, JoinType=JoinType.Left)]

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

		[BelongsTo(typeof(Contacto),"Cargo")]
        public string CargoContacto {get;set;}

		[BelongsTo(typeof(Contacto),"Telefono")]
        public string TelefonoContacto {get;set;}

		[BelongsTo(typeof(Contacto),"Fax")]
        public string FaxContacto {get;set;}

		[BelongsTo(typeof(Contacto),"Celular")]
        public string CelularContacto {get;set;}

		[BelongsTo(typeof(Contacto),"Mail")]
        public string MailContacto {get;set;}

		[BelongsTo(typeof(Contacto),"Direccion")]
        public string DireccionContacto {get;set;}

		[BelongsTo(typeof(Contacto),"CodigoPostal")]
        public string CodigoPostalContacto {get;set;}
		#endregion Contacto

		#region Cliente
		[BelongsTo(typeof(Cliente),"Nit")]
		public string NitCliente {get;set;}

        [BelongsTo(typeof(Cliente),"Nombre")]
        public string NombreCliente {get;set;}
		#endregion Cliente

	}
}

