using Aicl.Delfin.Model.Types;
using Aicl.Delfin.DataAccess;
using System.Linq.Expressions;
using System;
using ServiceStack.Common.Web;
using ServiceStack.Common.Utils;
using ServiceStack.DesignPatterns.Model;
using System.Web;


namespace Aicl.Delfin.BusinessLogic
{
    public static partial class BL
    {
        public static readonly int ResponsePageSize=10;
		public static readonly string Cotizacion="Cotizacion";
		public static readonly string RoleCoordinador="Coordinador";
		public static readonly string Prefijo="CMKS";
		public static readonly string AdminUser="admin";
		public static readonly double LockSeconds=5;

		internal static readonly string HtmlSpace = "&nbsp;";
		internal static readonly string DateFormat = "dd.MM.yyyy";

		public static T CheckExistAndActivo<T>(this DALProxy proxy, int id, Expression<Func<T,string>> field )
			where T : IHasActivo, new()
		
		{
			if (id==default(int))
				throw HttpError.Unauthorized(string.Format("Debe Indicar el Id para: '{0}",typeof(T).Name));

			var record = proxy.FirstOrDefault<T>(q=>q.Id==id);
				if(record==null)
				throw HttpError.NotFound(string.Format("No existe '{0}' con Id: '{1}'", typeof(T).Name, id));

			if(!record.Activo)
			{
				object fieldValue=GetValue<T>(record, field);

				throw HttpError.Unauthorized(string.Format("Registro Inactivo '{0} :'{1}-{2}'",
				                                           typeof(T).Name,
				                                           id, fieldValue));
			}

			return record;
		}

		static string FieldName<T>(Expression<Func<T,string>> field){
			var lambda = (field as LambdaExpression);
			if(lambda==null) return string.Empty;
			var me = (lambda.Body as MemberExpression);
			return me==null? string.Empty: me.Member.Name ;
		}

		static object GetValue<T>(T record, Expression<Func<T,string>> field){

			var fn=FieldName<T>(field); 
			if(string.IsNullOrEmpty(fn))
				throw HttpError.NotFound(string.Format("Expresion Incorrecta '{0}' para '{1}'",
				                                       field.ToString(), typeof(T).Name));
			
			var pi = ReflectionUtils.GetPropertyInfo(typeof(T), fn);
			return pi.GetValue(record, new object[]{});

		}

		public static string GetLockKey<T>(this T request) where T : IHasId<int>
		{
			return string.Format("urn:lock:{0}:Id:{1}",typeof(T).Name, request.Id); 
		}


		public static string Decode(this string text)
		{
			return HttpUtility.UrlDecode(text,System.Text.Encoding.UTF8);
		}

		public static string Encode(this string text)
		{
			return HttpUtility.UrlEncode(text,System.Text.Encoding.UTF8);
		}


		public static string ValueOrHtmlSpace(this string value){
			return !string.IsNullOrEmpty(value)?value:HtmlSpace;
		}

		public static string Format(this DateTime date){
			return date!=default(DateTime)? date.ToString(DateFormat):string.Empty;
		}

		public static string Format(this DateTime? date){
			return date.HasValue? Format(date.Value):Format(new DateTime());
		}



    }
}