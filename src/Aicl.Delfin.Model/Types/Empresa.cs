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
        public string Nombre {get;set;}

		[StringLength(64)]
        public string Alias {get;set;}

		[StringLength(64)]
        public string Lema {get;set;}

		[StringLength(16)]
        [Required]
        public string Nit {get;set;}

		[StringLength(128)]
        [Required]
        public string Direccion {get;set;}

		[StringLength(128)]
        [Required]
        public string DireccionAntigua {get;set;}

		[StringLength(32)]
        [Required]
        public string Ciudad {get;set;}

		[StringLength(32)]
        [Required]
        public string Pais {get;set;}

		[StringLength(32)]
        public string Telefono {get;set;}

		[StringLength(16)]
        public string Fax {get;set;}

		[StringLength(128)]
        public string Mail {get;set;}

		[StringLength(64)]
        public string Web {get;set;}

		[StringLength(128)]
		public string CuentaBancaria {get;set;}

		[StringLength(64)]
		public string ResponsableOfertasNombre {get;set;}

		[StringLength(128)]
		public string ResponsableOfertasMail {get;set;}

		[StringLength(16)]
		public string ResponsableOfertasCelular {get;set;}

		[StringLength(64)]
		public string ResponsableOfertasCargo {get;set;}

		[StringLength(32)]
		public string MailServerUrl { get; set;}

		[StringLength(128)]
		public string MailServerUser { get; set;}

		[StringLength(128)]
		public string MailServerPassword { get; set;}

		public int MailServerPort { get; set;}

		public bool  MailServerEnableSsl { get; set;}

		[StringLength(64)]
		public string ApplicationHost{get;set;}


		[StringLength(128)]
		public string ApplicationMailBox{get;set;}
	}
}
