using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lacuna.FocusNFSeIntegration.Models {
	public class ProviderInfo {

		/// <summary>
		/// Provider Cnpj number
		/// </summary>
		[JsonProperty("cnpj")]
		public string Cnpj { get; set; }

		/// <summary>
		/// Provider's city subscription number
		/// </summary>
		[JsonProperty("inscricao_municipal")]
		public string CitySubscription { get; set; }

		/// <summary>
		/// Provider's city IBGE code
		/// </summary>
		[JsonProperty("codigo_municipio")]
		public string CityCode { get; set; }
	}
}
