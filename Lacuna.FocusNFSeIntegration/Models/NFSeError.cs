using Newtonsoft.Json;
using System.Collections.Generic;

namespace Lacuna.FocusNFSeIntegration.Models {
	public class NFSeError {
		[JsonProperty("codigo")]
		public string Code { get; set; }

		[JsonProperty("mensagem")]
		public string Message { get; set; }

		[JsonProperty("erros")]
		public List<string> Errors { get; set; }
	}
}
