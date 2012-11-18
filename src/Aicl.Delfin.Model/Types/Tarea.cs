using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.DesignPatterns.Model;
using ServiceStack.ServiceHost;

namespace Aicl.Delfin.Model.Types
{

	[RestService("/Tarea/create","post")]
	[RestService("/Tarea/read","get")]
	[RestService("/Tarea/update/{Id}","put")]
	[RestService("/Tarea/destroy/{Id}","delete" )]
	[JoinTo(typeof(Cliente),"IdCliente","Id", Order=0, JoinType=JoinType.Left) ]
	public class Tarea:IHasId<int>, IHasIntUserId
	{
		public Tarea ()
		{
		}

		[AutoIncrement]
		public int Id { get; set; }

		public int UserId { get; set; }

		public int? IdCliente {get; set;}

        [StringLength(128)]
        [Required]
		public string Tema {get;set;}

		public bool Cumplida {get;set;}

		public DateTime Fecha {get; set;}

		[BelongsTo(typeof(Cliente),"Nombre")]
		public string NombreCliente {get; set;}

		[BelongsTo(typeof(Cliente),"Nit")]
		public string NitCliente {get; set;}

	}
}

