using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceHost;

namespace Aicl.Delfin.Model.Types
{

	[RestService("/Servicio/create","post")]
	[RestService("/Servicio/read","get")]
	[RestService("/Servicio/read/Nombre/{Nombre}","get")]
	[RestService("/Servicio/update/{Id}","put")]
	[RestService("/Servicio/destroy/{Id}","delete" )]
	public class Servicio:IHasActivo
	{
		public Servicio ()
		{
		}

		[AutoIncrement]
		public int Id { get; set; }

        [StringLength(128)]
        [Required]
		[Index]
        public string Nombre {get;set;}

		public bool Activo {get; set;}

	}
}

