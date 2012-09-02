using System;
using ServiceStack.DataAnnotations;
using ServiceStack.DesignPatterns.Model;

namespace Aicl.Delfin.Model.Types
{
	public partial class AuthRoleUser:IHasId<Int32>,IHasIntUserId{

		public AuthRoleUser(){}

		[PrimaryKey]
		[AutoIncrement]
		public Int32 Id { get; set;} 

		public Int32 AuthRoleId { get; set;} 

		public Int32 UserId { get; set;} 

	}
}
