using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.DesignPatterns.Model;
using ServiceStack.ServiceHost;

namespace Aicl.Delfin.Model.Types
{

	[RestService("/Ciudad/create","post")]
	[RestService("/Ciudad/read","get")]
	[RestService("/Ciudad/read/Nombre/{Nombre}","get")]
	[RestService("/Ciudad/update/{Id}","put")]
	[RestService("/Ciudad/destroy/{Id}","delete" )]
	public class Ciudad:IHasId<int>
	{
		public Ciudad ()
		{
		}

		[AutoIncrement]
		public int Id { get; set; }

        [StringLength(128)]
        [Required]
		[Index]
        public string Nombre {get;set;}

		[StringLength(4)]
        public string CodigoPostal {get;set;}

	}
}

