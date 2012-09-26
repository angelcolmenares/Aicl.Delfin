using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceHost;
using ServiceStack.DesignPatterns.Model;

namespace Aicl.Delfin.Model.Types
{

	[RestService("/Empresa/create","post")]
	[RestService("/Empresa/read","get")]
	[RestService("/Empresa/update/{Id}","put")]
	[RestService("/Empresa/destroy/{Id}","delete" )]
	public class Empresa:IHasId<int>
	{
		public Empresa ()
		{
		}

		[AutoIncrement]
		public int Id { get; set; }

		[StringLength(64)]
        [Required]
        [Index(true)]
        public string Nombre {get;set;}

		[StringLength(64)]
        [Index(true)]
        public string Alias {get;set;}

		[StringLength(16)]
        [Required]
        [Index(true)]
        public string Nit {get;set;}

		[StringLength(128)]
        [Required]
        [Index(true)]
        public string Direccion {get;set;}

		[StringLength(32)]
		public string MailServerUrl { get; set;}

		[StringLength(128)]
		public string MailServerUser { get; set;}

		[StringLength(128)]
		public string MailServerPassword { get; set;}

		public int MailServerPort { get; set;}

		public bool  MailServerEnableSsl { get; set;}

		[StringLength(64)]
		public string ResponsableOfertasNombre {get;set;}

		[StringLength(128)]
		public string ResponsableOfertasMail {get;set;}

		[StringLength(16)]
		public string ResponsableOfertasCelular {get;set;}

		[StringLength(64)]
		public string ResponsableOfertasCargo {get;set;}

	}
}
