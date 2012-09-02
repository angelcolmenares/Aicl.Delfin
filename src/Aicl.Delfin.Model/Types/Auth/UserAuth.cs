using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.DesignPatterns.Model;

namespace Aicl.Galapago.Model.Types
{
	[Alias("USERAUTH")]
	public partial class UserAuth:IHasId<Int32>{

		public UserAuth(){}

		[Alias("ID")]
		[Sequence("USERAUTH_ID_GEN")]
		[PrimaryKey]
		[AutoIncrement]
		public Int32 Id { get; set;} 

		[Alias("USERNAME")]
		[StringLength(40)]
		public String Username { get; set;} 

		[Alias("EMAIL")]
		[StringLength(80)]
		public String Email { get; set;} 

		[Alias("PRIMARYEMAIL")]
		[StringLength(80)]
		public String Primaryemail { get; set;} 

		[Alias("FIRSTNAME")]
		[StringLength(60)]
		public String Firstname { get; set;} 

		[Alias("LASTNAME")]
		[StringLength(60)]
		public String Lastname { get; set;} 

		[Alias("DISPLAYNAME")]
		[StringLength(60)]
		public String Displayname { get; set;} 

		[Alias("SALT")]
		[StringLength(512)]
		public String Salt { get; set;} 

		[Alias("PASSWORDHASH")]
		[StringLength(512)]
		public String Passwordhash { get; set;} 

		[Alias("DIGESTHA1HASH")]
		[StringLength(512)]
		public String Digestha1hash { get; set;} 

		[Alias("ROLES")]
		[StringLength(30)]
		public String Roles { get; set;} 

		[Alias("PERMISSIONS")]
		[StringLength(30)]
		public String Permissions { get; set;} 

		[Alias("CREATEDDATE")]
		public DateTime Createddate { get; set;} 

		[Alias("MODIFIEDDATE")]
		public DateTime Modifieddate { get; set;} 

		[Alias("META")]
		[StringLength(1024)]
		public String Meta { get; set;} 

	}
}
