using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lacuna.FocusNFSeIntegration.Models {

	public class ServiceInfo {

		[JsonProperty("aliquota")]
		public double? Aliquota { get; set; }

		[JsonProperty("discriminacao")]
		public string Description { get; set; }

		[JsonProperty("iss_retido")]
		public bool IssRetained { get; set; }

		[JsonProperty("item_lista_servico")]
		public string ServiceListItem { get; set; }

		[JsonProperty("codigo_tributario_municipio")]
		public string CityTributeCode { get; set; }

		[JsonProperty("valor_servicos")]
		public double ServiceValue { get; set; }

		// Additional fields

		[JsonProperty("valor_deducoes")]
		public double? DeductionValue { get; set; }

		[JsonProperty("valor_pis")]
		public double? PisValue { get; set; }

		[JsonProperty("valor_cofins")]
		public double? CofinsValue { get; set; }

		[JsonProperty("valor_inss")]
		public double? InssValue { get; set; }

		[JsonProperty("valor_ir")]
		public double? IRValue { get; set; }

		[JsonProperty("valor_csll")]
		public double? CsllValue { get; set; }

		[JsonProperty("valor_iss")]
		public double? IssValue { get; set; }

		[JsonProperty("valor_iss_retido")]
		public double? IssValueRetained { get; set; }

		[JsonProperty("outras_retencoes")]
		public double? OtherRetentions { get; set; }

		[JsonProperty("base_calculo")]
		public double? IssCalculationBasis { get; set; }

		[JsonProperty("desconto_incondicionado")]
		public double? UnconditionedDiscount { get; set; }

		[JsonProperty("desconto_condicionado")]
		public double? ConditionedDiscount { get; set; }

		[JsonProperty("codigo_cnae")]
		public string CnaeCode { get; set; } = null;

		[JsonProperty("codigo_municipio")]
		public string CityCode { get; set; } = null;

		[JsonProperty("percentual_total_tributos")]
		public double? TotalTaxPercent { get; set; }

		[JsonProperty("fonte_total_tributos")]
		public string TotalTaxSource { get; set; } = null;
	}
}
