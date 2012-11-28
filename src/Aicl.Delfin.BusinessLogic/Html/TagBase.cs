using System;
using ServiceStack.Markdown;

namespace Aicl.Cayita
{
	public abstract class TagBase:TagBuilder{  

		public TagBase(string tagName):base(tagName){}

		public virtual ElementStyleBase Style {get;set;}

		public string Id {
			get{
				string id;
				return Attributes.TryGetValue("id", out id)? id:string.Empty;
			}
			set{
				Attributes["id"]= value;
			}
		}

		public virtual void AddHtmlTag(TagBase tag){
			InnerHtml= InnerHtml+ tag;
		}

		public override string ToString ()
		{
			BuildStyleAttribue();
			return base.ToString(TagRenderMode.Normal);
		}

		protected void BuildStyleAttribue(){
			if( Style!=default(ElementStyleBase) ){
				var s = Style.ToString();
				if( !string.IsNullOrEmpty(s)) Attributes["style"]= Style.ToString();
			}
		}

	}

}

