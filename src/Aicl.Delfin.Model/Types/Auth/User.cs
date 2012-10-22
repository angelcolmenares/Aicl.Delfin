using ServiceStack.DataAnnotations;
using ServiceStack.DesignPatterns.Model;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using ServiceStack.Text;

namespace Aicl.Delfin.Model.Types
{
	[RestService("/User/create","post")]
	[RestService("/User/read","get")]
	[RestService("/User/update/{Id}","put")]
	[RestService("/User/destroy/{Id}","delete")]
	[Alias("UserAuth")]
	public class User:IHasId<int>
	{
		UserMeta metadata;

		readonly string dummyPassword="**********";

		public User ()
		{
			Meta= new Dictionary<string, string>();
			Password=dummyPassword;
		}

		public int Id { get; set; }
		public string UserName { get; set; } 
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public string Email { get; set; }

		public Dictionary<string, string> Meta { private get; set; }

		[Ignore]
		public string Password {get ;set;}

		[Ignore]
        public string Cargo {
			get{
				return Metadata.Cargo;
			} 
			set{
				Metadata.Cargo=value;
			} 
		}  

		[Ignore]
		public bool Activo {get{
				return Metadata.Activo;
			} 
			set{
				Metadata.Activo=value;
			}
		}

		[Ignore]
		public DateTime? ExpiresAt {get{
				return Metadata.ExpiresAt;
			} 
			set{
				Metadata.ExpiresAt=value;
			}}


		UserMeta Metadata 
		{
			get{
				if(metadata!=default(UserMeta)) return metadata;
				if (Meta==null) Meta= new Dictionary<string, string>();
				metadata =new UserMeta();
				metadata.PopulateFromMeta(Meta);
				return metadata;
			}
			//set{
			//	Meta[typeof(UserExtraData).Name] = TypeSerializer.SerializeToString(value);
			//}
		}


		public void SetDummyPassword(){
			Password=dummyPassword;
		}


		public bool IsDummyPassword(){
			return dummyPassword==Password;
		}
	}


	public class UserMeta
	{
		public UserMeta()
		{
			Activo=true;
		}
		public string Cargo {get;set;}
		public bool Activo {get;set;}
		public DateTime? ExpiresAt {get;set;}

		public void PopulateFromMeta(Dictionary<string, string> meta){
			if(meta==null) return;
			string str = null;
			meta.TryGetValue(typeof(UserMeta).Name, out str);
            if(str != null) 
			{
				var t = TypeSerializer.DeserializeFromString<UserMeta>(str);
				Activo=t.Activo;
				Cargo=t.Cargo;
				ExpiresAt=t.ExpiresAt;
			}

		}
	}
}

