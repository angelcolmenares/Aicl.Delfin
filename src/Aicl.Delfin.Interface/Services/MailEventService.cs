using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using Aicl.PubNub;
using System.Collections.Generic;
using ServiceStack.ServiceHost;
using ServiceStack.Common.Web;

namespace Aicl.Delfin.Interface
{
	public class MailEventService:AppRestService<MailEvent>
	{
		public Channel Channel {get;set;}

		public AppConfig Config {get;set;}

		public override object OnPost (MailEvent request)
		{

			if(request.MailLogToken!=Config.MailLogToken){
				throw HttpError.Unauthorized("Invalid MailLogToken");
			}

			var args = new Dictionary<string, object>();
			Dictionary<string, object> objDict = new Dictionary<string, object>();

			objDict.Add("Data", request);

            args.Add("channel", Config.Channel);
            args.Add("message", objDict);

            // publish Response
            Channel.Publish(args);

			return new MailEventResponse{
				MailEvent= request
			};
		}
	}
}

