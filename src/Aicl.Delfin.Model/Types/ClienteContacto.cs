using ServiceStack.DataAnnotations;

namespace Aicl.Delfin.Model.Types
{
	[JoinTo(typeof(Contacto),"IdContacto","Id", JoinType= JoinType.Left, Order=0)]
	[JoinTo(typeof(Contacto), typeof(Ciudad),"IdCiudad","Id", JoinType= JoinType.Left, Order=1)]
	public class ClienteContacto:Cliente
	{
		public ClienteContacto ()
		{
		}

		[BelongsTo(typeof(Contacto),"Id")]
		public int IdContacto { get; set; }

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

		[BelongsTo(typeof(Contacto),"Activo")]
		public bool ActivoContacto { get; set;}

		#region Ciudad
		[BelongsTo(typeof(Contacto))]
		public int IdCiudad {get;set;}

		[BelongsTo(typeof(Ciudad),"Nombre")]
		public string NombreCiudad {get;set;}
		#endregion Ciudad

	}
}

