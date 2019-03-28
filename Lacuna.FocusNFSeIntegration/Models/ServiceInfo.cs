using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lacuna.FocusNFSeIntegration.Models {
	public class ServiceInfo {
		[JsonProperty("aliquota")]
		public int Aliquota { get; set; }

		[JsonProperty("discriminacao")]
		public string Description { get; set; }

		[JsonProperty("iss_retido")]
		public bool IssRetained { get; set; }

		[JsonProperty("item_lista_servico")]
		public bool ServiceListItem { get; set; }

		[JsonProperty("codigo_tributario_municipio")]
		public bool CityTributeCode { get; set; }

		[JsonProperty("valor_servicos")]
		public decimal ServiceValue { get; set; }
	}
}
