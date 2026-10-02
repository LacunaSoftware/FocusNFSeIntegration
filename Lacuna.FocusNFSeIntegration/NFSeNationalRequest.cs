using Newtonsoft.Json;
using System;

namespace Lacuna.FocusNFSeIntegration {

	/// <summary>
	/// A DPS (Declaração de Prestação de Serviço) in the national layout, sent to /v2/nfsen. Unlike the
	/// municipal layout, it is flat: provider, client and service fields sit side by side.
	/// </summary>
	/// <remarks>
	/// Field reference: https://campos.focusnfe.com.br/nfse_nacional/EmissaoDPSXml.html
	/// </remarks>
	public class NFSeNationalRequest {

		/// <summary>
		/// DPS emission date and time, with its offset.
		/// </summary>
		[JsonProperty("data_emissao")]
		public DateTimeOffset EmissionDate { get; set; }

		/// <summary>
		/// Date the service was rendered in. Must not be after the emission date.
		/// </summary>
		[JsonProperty("data_competencia")]
		[JsonConverter(typeof(CustomDateTimeConverter))]
		public DateTime CompetenceDate { get; set; }

		/// <summary>
		/// DPS series. Numbered by Focus when omitted.
		/// </summary>
		[JsonProperty("serie_dps")]
		public int? DpsSeries { get; set; }

		/// <summary>
		/// DPS number. Numbered by Focus when omitted.
		/// </summary>
		[JsonProperty("numero_dps")]
		public long? DpsNumber { get; set; }

		/// <summary>
		/// Who issues the DPS: 1 provider, 2 client, 3 intermediary. Focus assumes the provider when
		/// omitted.
		/// </summary>
		[JsonProperty("emitente_dps")]
		public int? EmitterType { get; set; }

		/// <summary>
		/// IBGE code (7 digits) of the city issuing the NFSe.
		/// </summary>
		[JsonProperty("codigo_municipio_emissora")]
		public string IssuingCityCode { get; set; }

		// Provider

		[JsonProperty("cnpj_prestador")]
		public string ProviderCnpj { get; set; }

		[JsonProperty("inscricao_municipal_prestador")]
		public string ProviderCitySubscription { get; set; }

		[JsonProperty("codigo_opcao_simples_nacional")]
		public SimpleNationalOption SimpleNationalOption { get; set; } = SimpleNationalOption.NotOptant;

		/// <summary>
		/// Only for Simples Nacional optants.
		/// </summary>
		[JsonProperty("regime_tributario_simples_nacional")]
		public int? SimpleNationalTaxRegime { get; set; }

		[JsonProperty("regime_especial_tributacao")]
		public int SpecialTaxRegime { get; set; } = 0;

		// Client

		[JsonProperty("cnpj_tomador")]
		public string ClientCnpj { get; set; }

		[JsonProperty("cpf_tomador")]
		public string ClientCpf { get; set; }

		[JsonProperty("inscricao_municipal_tomador")]
		public string ClientCitySubscription { get; set; }

		[JsonProperty("razao_social_tomador")]
		public string ClientName { get; set; }

		[JsonProperty("codigo_municipio_tomador")]
		public string ClientCityCode { get; set; }

		[JsonProperty("cep_tomador")]
		public string ClientPostalCode { get; set; }

		[JsonProperty("logradouro_tomador")]
		public string ClientStreet { get; set; }

		[JsonProperty("numero_tomador")]
		public string ClientNumber { get; set; }

		[JsonProperty("complemento_tomador")]
		public string ClientComplement { get; set; }

		[JsonProperty("bairro_tomador")]
		public string ClientNeighborhood { get; set; }

		[JsonProperty("telefone_tomador")]
		public string ClientPhone { get; set; }

		[JsonProperty("email_tomador")]
		public string ClientEmail { get; set; }

		// Service

		/// <summary>
		/// IBGE code (7 digits) of the city where the service was rendered.
		/// </summary>
		[JsonProperty("codigo_municipio_prestacao")]
		public string ServiceCityCode { get; set; }

		/// <summary>
		/// National ISSQN tribute code (cTribNac), 6 digits: item, subitem and breakdown of the
		/// service list.
		/// </summary>
		[JsonProperty("codigo_tributacao_nacional_iss")]
		public string NationalTaxCode { get; set; }

		/// <summary>
		/// The city's own ISSQN tribute code (cTribMun).
		/// </summary>
		[JsonProperty("codigo_tributacao_municipal_iss")]
		public string CityTributeCode { get; set; }

		[JsonProperty("descricao_servico")]
		public string Description { get; set; }

		/// <summary>
		/// NBS code (Nomenclatura Brasileira de Serviços), 9 digits.
		/// </summary>
		[JsonProperty("codigo_nbs")]
		public string NbsCode { get; set; }

		[JsonProperty("informacoes_complementares")]
		public string AdditionalInformation { get; set; }

		[JsonProperty("valor_servico")]
		public double ServiceValue { get; set; }

		[JsonProperty("desconto_incondicionado")]
		public double? UnconditionedDiscount { get; set; }

		[JsonProperty("desconto_condicionado")]
		public double? ConditionedDiscount { get; set; }

		// ISSQN

		[JsonProperty("tributacao_iss")]
		public IssTaxation IssTaxation { get; set; } = IssTaxation.Taxable;

		[JsonProperty("tipo_retencao_iss")]
		public IssRetention IssRetention { get; set; } = IssRetention.NotRetained;

		/// <summary>
		/// ISSQN rate (%). Filled by the national system when the city of incidence is parameterized
		/// there; must be informed otherwise.
		/// </summary>
		[JsonProperty("percentual_aliquota_relativa_municipio")]
		public double? IssAliquota { get; set; }

		// Approximate tax totals (Lei da Transparência)

		[JsonProperty("percentual_total_tributos_federais")]
		public double? FederalTaxPercent { get; set; }

		[JsonProperty("percentual_total_tributos_estaduais")]
		public double? StateTaxPercent { get; set; }

		[JsonProperty("percentual_total_tributos_municipais")]
		public double? MunicipalTaxPercent { get; set; }

		/// <summary>
		/// Only for Simples Nacional optants.
		/// </summary>
		[JsonProperty("percentual_total_tributos_simples_nacional")]
		public double? SimpleNationalTaxPercent { get; set; }

		// IBS / CBS (reforma tributária)

		/// <summary>
		/// Purpose of the emission. 0: regular NFSe.
		/// </summary>
		[JsonProperty("finalidade_emissao")]
		public int EmissionPurpose { get; set; } = 0;

		/// <summary>
		/// Whether the operation is for personal use or consumption.
		/// </summary>
		[JsonProperty("consumidor_final")]
		public int FinalConsumer { get; set; }

		/// <summary>
		/// Operation indicator code (cIndOp), 6 digits.
		/// </summary>
		[JsonProperty("codigo_indicador_operacao")]
		public string OperationIndicator { get; set; }

		/// <summary>
		/// 0: the recipient is the client itself; 1: someone else.
		/// </summary>
		[JsonProperty("indicador_destinatario")]
		public int RecipientIndicator { get; set; } = 0;

		/// <summary>
		/// IBS/CBS tax situation code (CST), 3 digits.
		/// </summary>
		[JsonProperty("ibs_cbs_situacao_tributaria")]
		public string IbsCbsTaxSituation { get; set; }

		/// <summary>
		/// IBS/CBS tax classification code (cClassTrib), 6 digits.
		/// </summary>
		[JsonProperty("ibs_cbs_classificacao_tributaria")]
		public string IbsCbsTaxClassification { get; set; }
	}

	public enum SimpleNationalOption {
		NotOptant = 1,
		Mei = 2,
		MeEpp = 3,
	}

	public enum IssTaxation {
		Taxable = 1,
		Immunity = 2,
		Export = 3,
		NoIncidence = 4,
	}

	public enum IssRetention {
		NotRetained = 1,
		RetainedByClient = 2,
		RetainedByIntermediary = 3,
	}
}
