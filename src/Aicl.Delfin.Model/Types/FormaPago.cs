using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using ServiceStack.ServiceHost;

namespace Aicl.Delfin.Model.Types
{
	[RestService("/FormaPago/create","post")]
	[RestService("/FormaPago/read","get")]
	[RestService("/FormaPago/update/{Id}","put")]
	[RestService("/FormaPago/destroy/{Id}","delete" )]
	public class FormaPago:IHasActivo
	{

		public FormaPago ()
		{
		}

		[AutoIncrement]
		public int Id {get; set;}

		[Required]
		[StringLength(16)]
		public string Modo { get; set;}

		[Required]
		[StringLength(48)]
		public string Descripcion { get; set;}

		public int DiasCredito {get;set;}

		public bool Activo {get;set;}
	}
}

