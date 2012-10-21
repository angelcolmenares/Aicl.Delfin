using ServiceStack.ServiceHost;
using ServiceStack.DesignPatterns.Model;
using ServiceStack.DataAnnotations;
using System;

namespace Aicl.Delfin.Model.Types
{
	[RestService("/RolePermission/create","post")]
	[RestService("/RolePermission/read","get")]
	[RestService("/RolePermission/destroy/{Id}","delete")]
	[JoinTo(typeof(AuthPermission),"AuthPermissionId","Id")]
	[Alias("AuthRolePermission")]
	public class RolePermission:IHasId<Int32>
	{
		public RolePermission ()
		{
		}

		[PrimaryKey]
		[AutoIncrement]
		public Int32 Id { get; set;} 

		public Int32 AuthRoleId { get; set;} 

		public Int32 AuthPermissionId { get; set;} 

		[BelongsTo(typeof(AuthPermission))]
		public string Name {get;set;}

	}
}

