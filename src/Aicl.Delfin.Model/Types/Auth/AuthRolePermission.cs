using System;
using ServiceStack.DataAnnotations;
using ServiceStack.DesignPatterns.Model;

namespace Aicl.Delfin.Model.Types
{
	public partial class AuthRolePermission:IHasId<Int32>{

		public AuthRolePermission(){}

		[PrimaryKey]
		[AutoIncrement]
		public Int32 Id { get; set;} 

		public Int32 AuthRoleId { get; set;} 

		public Int32 AuthPermissionId { get; set;} 

	}
}
