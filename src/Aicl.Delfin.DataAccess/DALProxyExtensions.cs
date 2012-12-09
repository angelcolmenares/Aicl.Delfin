using Aicl.Delfin.Model.Types;
using ServiceStack.Common.Web;
using ServiceStack.Common;
using ServiceStack.Configuration;

namespace Aicl.Delfin.DataAccess
{
	public static class DALProxyExtensions
	{

		static readonly string CryptoKey = ConfigUtils.GetAppSetting("CRYPTO_KEY","8esjais863deDae");

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
						                     CryptoKey);
				}
				if(!empresa.PublishKey.IsNullOrEmpty())
					empresa.PublishKey= Cryptor.Desencriptar(empresa.PublishKey, CryptoKey);

				if(!empresa.SubscribeKey.IsNullOrEmpty())
					empresa.SubscribeKey= Cryptor.Desencriptar(empresa.SubscribeKey, CryptoKey);

				if(!empresa.SecretKey.IsNullOrEmpty())
					empresa.SecretKey= Cryptor.Desencriptar(empresa.SecretKey, CryptoKey);

				if(!empresa.MailLogToken.IsNullOrEmpty())
					empresa.MailLogToken= Cryptor.Desencriptar(empresa.MailLogToken, CryptoKey);
			}

			return empresa;
		}


		public static void PostEmpresa(this DALProxy proxy, Empresa empresa)
		{
			if( empresa.DireccionAntigua.IsNullOrEmpty() )
				empresa.DireccionAntigua=string.Empty;

			var ps = empresa.MailServerPassword;

			if(!empresa.MailServerPassword.IsNullOrEmpty()){
				empresa.MailServerPassword=
						Cryptor.Encriptar(empresa.MailServerPassword,
						                     CryptoKey);
			}

			var pk = empresa.PublishKey;
			if(!pk.IsNullOrEmpty())
				empresa.PublishKey=Cryptor.Encriptar(pk, CryptoKey);


			var sk = empresa.SecretKey;
			if(!sk.IsNullOrEmpty())
				empresa.SecretKey=Cryptor.Encriptar(sk, CryptoKey);

			var suk = empresa.SubscribeKey;
			if(!suk.IsNullOrEmpty())
				empresa.SubscribeKey=Cryptor.Encriptar(suk, CryptoKey);

			var tk = empresa.MailLogToken;
			if(!tk.IsNullOrEmpty())
				empresa.MailLogToken=Cryptor.Encriptar(tk, CryptoKey);


			proxy.Create(empresa);

			empresa.MailServerPassword=ps;
			empresa.PublishKey=pk;
			empresa.SecretKey=sk;
			empresa.SubscribeKey=suk;
			empresa.MailLogToken=tk;

		}

		public static void PutEmpresa(this DALProxy proxy, Empresa empresa)
		{
			if( empresa.DireccionAntigua.IsNullOrEmpty() )
				empresa.DireccionAntigua=string.Empty;

			var ps = empresa.MailServerPassword;

			if(!empresa.MailServerPassword.IsNullOrEmpty()){
				empresa.MailServerPassword=
						Cryptor.Encriptar(empresa.MailServerPassword,
						                     CryptoKey);
			}

			var pk = empresa.PublishKey;
			if(!pk.IsNullOrEmpty())
				empresa.PublishKey=Cryptor.Encriptar(pk, CryptoKey);


			var sk = empresa.SecretKey;
			if(!sk.IsNullOrEmpty())
				empresa.SecretKey=Cryptor.Encriptar(sk, CryptoKey);

			var suk = empresa.SubscribeKey;
			if(!suk.IsNullOrEmpty())
				empresa.SubscribeKey=Cryptor.Encriptar(suk, CryptoKey);

			var tk = empresa.MailLogToken;
			if(!tk.IsNullOrEmpty())
				empresa.MailLogToken=Cryptor.Encriptar(tk, CryptoKey);


			proxy.Update(empresa);

			empresa.MailServerPassword=ps;
			empresa.PublishKey=pk;
			empresa.SecretKey=sk;
			empresa.SubscribeKey=suk;
			empresa.MailLogToken=tk;

		}

		public static void DeleteEmpresa(this DALProxy proxy, Empresa empresa)
		{
			proxy.Delete<Empresa>(q=>q.Id==empresa.Id);
		}

	}
}

