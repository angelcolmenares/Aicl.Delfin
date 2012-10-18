using System;
using ServiceStack.DesignPatterns.Model;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceHost;

namespace Aicl.Delfin.Model.Types
{
	[RestService("/UserRole/create","post")]
	[RestService("/UserRole/read","get")]
	[RestService("/UserRole/destroy/{Id}","delete")]
	[JoinTo(typeof(AuthRole),"AuthRoleId", "Id")]
	[Alias("AuthRoleUser")]

	public class UserRole:IHasId<Int32>,IHasIntUserId
	{
		public UserRole ()
		{
		}

		[PrimaryKey]
		[AutoIncrement]
		public Int32 Id { get; set;} 

		public Int32 AuthRoleId { get; set;} 

		public Int32 UserId { get; set;} 

		[BelongsTo(typeof(AuthRole))]
		public string Name{get; set;}

	}
}

