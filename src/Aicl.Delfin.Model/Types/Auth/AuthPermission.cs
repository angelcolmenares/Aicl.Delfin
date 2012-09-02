using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.DesignPatterns.Model;

namespace Aicl.Delfin.Model.Types
{

	public partial class AuthPermission:IHasId<Int32>{

		public AuthPermission(){}

		[PrimaryKey]
		[AutoIncrement]
		public Int32 Id { get; set;} 

		[Required]
		[StringLength(30)]
		public String Name { get; set;} 

	}
}
