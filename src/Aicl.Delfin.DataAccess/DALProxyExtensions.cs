using Aicl.Delfin.Model.Types;
using ServiceStack.Common.Web;
using ServiceStack.Common;

namespace Aicl.Delfin.DataAccess
{
	public static class DALProxyExtensions
	{
		static string cryptoKey="8esjais863deDae";

		public static Consecutivo GetNext(this DALProxy proxy, string documento, int? incremento=1){

			var consecutivo = proxy.FirstOrDefault<Consecutivo>(q=>q.Documento==documento);

			if(consecutivo==default(Consecutivo)){
				throw HttpError.NotFound(string.Format("No existe registro de consecutivo para: '{0}'", documento));
			}


			consecutivo.Valor+=incremento.HasValue?incremento.Value:1;

			proxy.Update(consecutivo,ev=>ev.Update(f=>f.Valor).Where(q=>q.Id==consecutivo.Id));

			return consecutivo;
		}

		public static Empresa GetEmpresa(this DALProxy proxy)
		{
			Empresa empresa = proxy.FirstOrDefault<Empresa>(q=>true);

			if(empresa!=default(Empresa)){
				if (!empresa.MailServerPassword.IsNullOrEmpty()){
					empresa.MailServerPassword=
						Cryptor.Desencriptar(empresa.MailServerPassword,
						                     cryptoKey);
				}
			}

			return empresa;
		}


		public static void PostEmpresa(this DALProxy proxy, Empresa empresa)
		{
			if(!empresa.MailServerPassword.IsNullOrEmpty()){
				empresa.MailServerPassword=
						Cryptor.Encriptar(empresa.MailServerPassword,
						                     cryptoKey);
			}
			proxy.Create(empresa);
		}

		public static void PutEmpresa(this DALProxy proxy, Empresa empresa)
		{
			if(!empresa.MailServerPassword.IsNullOrEmpty()){
				empresa.MailServerPassword=
						Cryptor.Encriptar(empresa.MailServerPassword,
						                     cryptoKey);
			}
			proxy.Update(empresa, ev=> ev.Where(q=>q.Id==empresa.Id));
		}

		public static void DeleteEmpresa(this DALProxy proxy, Empresa empresa)
		{
			proxy.Delete<Empresa>(q=>q.Id==empresa.Id);
		}

	}
}

