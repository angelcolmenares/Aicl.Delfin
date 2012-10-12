using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using ServiceStack.Service;
using ServiceStack.ServiceHost;
using ServiceStack.Text;



namespace Aicl.Delfin.BusinessLogic
{
	public class PdfFileResult:IHasOptions, IStreamWriter
	{
		public Stream ResponseStream { get; private set; }

		public IDictionary<string, string> Options { get; private set; }

		public PdfFileResult (Stream responseStream, string name)
		{
			ResponseStream= responseStream;

            Options = new Dictionary<string, string> {
                 {"Content-Type", "application/octet-stream"},
				{"Content-Disposition", string.Format("attachment; filename=\"{0}\";",name)} 
            };

		}

		public void WriteTo(Stream responseStream)
        {
            
            if (ResponseStream != null)
            {
                ResponseStream.WriteTo(responseStream);
                responseStream.Flush();
                try
                {
                   // ResponseStream.Dispose();
                }
                catch { /*ignore*/ }

                
            }
		}
	}
}

