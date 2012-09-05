using Aicl.Delfin.Model.Types;
using ServiceStack.Common.Web;

namespace Aicl.Delfin.DataAccess
{
	public static class DALProxyExtensions
	{
		public static Consecutivo GetNext(this DALProxy proxy, string documento, int? incremento=1){

			var consecutivo = proxy.FirstOrDefault<Consecutivo>(q=>q.Documento==documento);

			if(consecutivo==default(Consecutivo)){
				throw HttpError.NotFound(string.Format("No existe registro de consecutivo para: '{0}'", documento));
			}


			consecutivo.Valor+=incremento.HasValue?incremento.Value:1;

			proxy.Update(consecutivo,ev=>ev.Update(f=>f.Valor).Where(q=>q.Id==consecutivo.Id));

			return consecutivo;
		}

	}
}

