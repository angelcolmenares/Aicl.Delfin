using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.DesignPatterns.Model;
using ServiceStack.ServiceHost;

namespace Aicl.Delfin.Model.Types
{
	[RestService("/Cliente/create","post")]
	[RestService("/Cliente/read","get")]
	[RestService("/Cliente/read/Nombre/{Nombre}","get")]
	[RestService("/Cliente/read/Nit/{Nit}","get")]
	[RestService("/Cliente/update/{Id}","put")]
	[RestService("/Cliente/destroy/{Id}","delete" )]
	public class Cliente:IHasId<int>
	{
		public Cliente ()
		{
		}

		[AutoIncrement]
		public int Id { get; set; }

		[StringLength(16)]
        [Required]
        [Index(true)]
        public string Nit {get;set;}

        [StringLength(128)]
        [Required]
		[Index]
        public string Nombre {get;set;}

		public bool Activo { get; set;}


	}
}

