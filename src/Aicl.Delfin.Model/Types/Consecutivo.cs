using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using ServiceStack.ServiceHost;
using ServiceStack.DesignPatterns.Model;

namespace Aicl.Delfin.Model.Types
{

	[RestService("/Consecutivo/create","post")]
	[RestService("/Consecutivo/read","get")]
	[RestService("/Consecutivo/update/{Id}","put")]
	[RestService("/Consecutivo/destroy/{Id}","delete" )]
	public class Consecutivo:IHasId<int>
	{
		public Consecutivo ()
		{
		}

		[AutoIncrement]
		public int Id {get;set;}

		[StringLength(32)]
        [Required]
		[Index(true)]
		public string Documento {get;set;}

		[StringLength(4)]
        [Required]
		[Index(true)]
		public string Prefijo {get;set;}

		public int Valor {get;set;}

		public static string GetLockKey(string documento)
		{
			return string.Format("urn:lock:Consecutivo:Documento:{0}",documento); 
		}

	}
}

