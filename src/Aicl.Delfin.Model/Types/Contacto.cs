using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceHost;

namespace Aicl.Delfin.Model.Types
{

	[JoinTo(typeof(Ciudad),"IdCiudad","Id", Order=0)]
	[RestService("/Contacto/create","post")]
	[RestService("/Contacto/read","get")]
	[RestService("/Contacto/read/Nombre/{Nombre}","get")]
	[RestService("/Contacto/update/{Id}","put")]
	[RestService("/Contacto/destroy/{Id}","delete" )]
	public class Contacto:IHasActivo
	{
		public Contacto ()
		{
		}

		[AutoIncrement]
		public int Id { get; set; }

		public int IdCliente { get; set; }

        [StringLength(128)]
        [Required]
		[Index]
        public string Nombre {get;set;}

		[StringLength(64)]
        [Required]
        public string Cargo {get;set;}

		[StringLength(16)]
        public string Telefono {get;set;}

		[StringLength(16)]
        public string Fax {get;set;}

		[StringLength(16)]
        public string Celular {get;set;}

		[StringLength(128)]
        public string Mail {get;set;}

		[StringLength(256)]
		[Required]
        public string Direccion {get;set;}

		[StringLength(16)]
        public string CodigoPostal {get;set;}

		public bool Activo { get; set;}

		#region Ciudad
		public int IdCiudad {get;set;}

		[BelongsTo(typeof(Ciudad),"Nombre")]
		public string NombreCiudad {get;set;}
		#endregion Ciudad


	}
}

