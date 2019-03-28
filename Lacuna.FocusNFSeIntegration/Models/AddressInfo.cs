using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lacuna.FocusNFSeIntegration.Models {
	public class AddressInfo {
		/// <summary>
		/// Client address
		/// </summary>
		[JsonProperty("logradouro")]
		public string Street { get; set; }

		/// <summary>
		/// Address number
		/// </summary>
		[JsonProperty("numero")]
		public string Number { get; set; }

		/// <summary>
		/// Address complement
		/// </summary>
		[JsonProperty("complemento")]
		public string Complement { get; set; }

		/// <summary>
		/// Neighborhood
		/// </summary>
		[JsonProperty("bairro")]
		public string Neighborhood { get; set; }

		/// <summary>
		/// Client's city IBGE code
		/// </summary>
		[JsonProperty("codigo_municipio")]
		public string CityCode { get; set; }

		/// <summary>
		/// State info, just 2 characters
		/// </summary>
		[JsonProperty("uf")]
		public string UF { get; set; }

		/// <summary>
		/// Cep, just numbers
		/// </summary>
		[JsonProperty("cep")]
		public string PostalCode { get; set; }
	}
}
