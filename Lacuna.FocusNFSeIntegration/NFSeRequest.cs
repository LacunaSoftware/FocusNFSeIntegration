using Lacuna.FocusNFSeIntegration.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lacuna.FocusNFSeIntegration {
	public class NFSeRequest {

		/// <summary>
		/// NFSe emission date
		/// </summary>
		[JsonProperty("data_emissao")]
		[JsonConverter(typeof(CustomDateTimeConverter))]
		public DateTime EmissionDate { get; set; }

		[JsonProperty("natureza_operacao")]
		public string OperationKind { get; set; } = "1";

		[JsonProperty("regime_especial_tributacao")]
		public string SpecialTaxRegime { get; set; }

		[JsonProperty("optante_simples_nacional")]
		public bool UseSimpleTributation { get; set; }

		[JsonProperty("incentivador_cultural")] 
		public bool CultureSupporter { get; set; }

		[JsonProperty("tributacao_rps")]
		public string RpsTribute { get; set; }

		[JsonProperty("codigo_obra")]
		public string ConstructionCode { get; set; }

		[JsonProperty("art")]
		public string ArtCode { get; set; }

		/// <summary>
		/// NFSe provider info
		/// </summary>
		[JsonProperty("prestador")]
		public ProviderInfo Provider { get; set; }

		/// <summary>
		/// NFSe client info
		/// </summary>
		[JsonProperty("tomador")]
		public ClientInfo Client { get; set; }

		/// <summary>
		/// NFSe service info
		/// </summary>
		[JsonProperty("servico")]
		public ServiceInfo Info { get; set; }
	}
}
