using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.DesignPatterns.Model;

namespace Aicl.Delfin.Model.Types
{
	public partial class AuthRole:IHasId<Int32>{

		public AuthRole(){}

		[PrimaryKey]
		[AutoIncrement]
		public Int32 Id { get; set;} 

		[Required]
		[StringLength(30)]
		public String Name { get; set;} 

		[StringLength(15)]
		public String Directory { get; set;} 
				
		[StringLength(2)]
		public String ShowOrder { get; set;} 

		[Required]
		[StringLength(30)]
		public String Title { get; set;} 

	}
}