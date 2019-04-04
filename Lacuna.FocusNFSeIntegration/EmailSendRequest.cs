using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lacuna.FocusNFSeIntegration {
	public class EmailSendRequest {
		[JsonProperty("emails")]
		public List<string> Emails { get; set; }
	}
}
