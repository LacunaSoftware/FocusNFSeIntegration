using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lacuna.FocusNFSeIntegration {
	public class NFSeOnlyStatusResponse {

		/// <summary>
		/// Response status
		/// </summary>
		[JsonProperty("status")]
		public string Status { get; set; }

		/// <summary>
		/// Error list if the operation was not successful
		/// </summary>
		[JsonProperty("erros")]
		public List<string> Errors { get; set; }
	}

	public class NFSeResponse: NFSeOnlyStatusResponse {

		/// <summary>
		/// Provider Cnpj number
		/// </summary>
		[JsonProperty("cnpj_prestador")]
		public string ProviderCnpj { get; set; }

		/// <summary>
		/// NFSe reference
		/// </summary>
		[JsonProperty("ref")]
		public string Reference { get; set; }
	}

	public class NFSeDetailsResponse: NFSeResponse {

		/// <summary>
		/// Response status
		/// </summary>
		[JsonProperty("numero")]
		public string Number { get; set; }

		/// <summary>
		/// Verification code
		/// </summary>
		[JsonProperty("codigo_verificacao")]
		public string VerificationCode { get; set; }

		/// <summary>
		/// Emission date
		/// </summary>
		[JsonProperty("data_emissao")]
		public DateTimeOffset EmissionDate { get; set; }

		/// <summary>
		/// NFSe mirror url, pdf format
		/// </summary>
		[JsonProperty("url")]
		public string NFSeMirrorUrl { get; set; }

		/// <summary>
		/// NFSe XML url
		/// </summary>
		[JsonProperty("caminho_xml_nota_fiscal")]
		public string NFSeXmlUrl { get; set; }
	}
}
