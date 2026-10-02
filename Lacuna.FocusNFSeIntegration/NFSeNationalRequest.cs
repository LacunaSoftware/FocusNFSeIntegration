using Newtonsoft.Json;
using System;

namespace Lacuna.FocusNFSeIntegration {

	/// <summary>
	/// A DPS (Declaração de Prestação de Serviço) in the national layout, sent to /v2/nfsen. Unlike the
	/// municipal layout, it is flat: provider, client and service fields sit side by side.
	/// </summary>
	/// <remarks>
	/// Field reference, with the allowed values of every coded field: <see href="https://campos.focusnfe.com.br/nfse_nacional/EmissaoDPSXml.html"/>.
	/// Coded fields are plain numbers, as Focus takes them; the values used are listed on each.
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
		/// Who issues the DPS (tpEmit). Focus assumes the provider when omitted.
		/// <list type="bullet">
		/// <item>1: provider</item>
		/// <item>2: client</item>
		/// <item>3: intermediary</item>
		/// </list>
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

		/// <summary>
		/// Standing in the Simples Nacional (opSimpNac).
		/// <list type="bullet">
		/// <item>1: not an optant</item>
		/// <item>2: optant, MEI</item>
		/// <item>3: optant, ME/EPP</item>
		/// </list>
		/// </summary>
		[JsonProperty("codigo_opcao_simples_nacional")]
		public int SimpleNationalOption { get; set; } = 1;

		/// <summary>
		/// How a Simples Nacional optant collects its taxes (regApTribSN). Only for optants.
		/// <list type="bullet">
		/// <item>1: federal and municipal taxes through the Simples Nacional</item>
		/// <item>2: federal taxes through the Simples Nacional, ISSQN outside it</item>
		/// <item>3: federal and municipal taxes outside the Simples Nacional</item>
		/// </list>
		/// </summary>
		[JsonProperty("regime_tributario_simples_nacional")]
		public int? SimpleNationalTaxRegime { get; set; }

		/// <summary>
		/// Special municipal tax regime (regEspTrib).
		/// <list type="bullet">
		/// <item>0: none</item>
		/// <item>1: cooperative act</item>
		/// <item>2: estimate</item>
		/// <item>3: municipal microenterprise</item>
		/// <item>4: notary or registrar</item>
		/// <item>5: self-employed professional</item>
		/// <item>6: professional partnership</item>
		/// <item>9: other</item>
		/// </list>
		/// </summary>
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
		public decimal ServiceValue { get; set; }

		[JsonProperty("desconto_incondicionado")]
		public decimal? UnconditionedDiscount { get; set; }

		[JsonProperty("desconto_condicionado")]
		public decimal? ConditionedDiscount { get; set; }

		// ISSQN

		/// <summary>
		/// ISSQN taxation of the service (tribISSQN).
		/// <list type="bullet">
		/// <item>1: taxable</item>
		/// <item>2: immunity</item>
		/// <item>3: service export</item>
		/// <item>4: no incidence</item>
		/// </list>
		/// </summary>
		[JsonProperty("tributacao_iss")]
		public int IssTaxation { get; set; } = 1;

		/// <summary>
		/// ISSQN withholding (tpRetISSQN).
		/// <list type="bullet">
		/// <item>1: not withheld</item>
		/// <item>2: withheld by the client</item>
		/// <item>3: withheld by the intermediary</item>
		/// </list>
		/// </summary>
		[JsonProperty("tipo_retencao_iss")]
		public int IssRetention { get; set; } = 1;

		/// <summary>
		/// ISSQN rate (%). Filled by the national system when the city of incidence is parameterized
		/// there; must be informed otherwise.
		/// </summary>
		[JsonProperty("percentual_aliquota_relativa_municipio")]
		public decimal? IssAliquota { get; set; }

		// Approximate tax totals (Lei da Transparência)

		[JsonProperty("percentual_total_tributos_federais")]
		public decimal? FederalTaxPercent { get; set; }

		[JsonProperty("percentual_total_tributos_estaduais")]
		public decimal? StateTaxPercent { get; set; }

		[JsonProperty("percentual_total_tributos_municipais")]
		public decimal? MunicipalTaxPercent { get; set; }

		/// <summary>
		/// Only for Simples Nacional optants.
		/// </summary>
		[JsonProperty("percentual_total_tributos_simples_nacional")]
		public decimal? SimpleNationalTaxPercent { get; set; }

		// IBS / CBS (reforma tributária)

		/// <summary>
		/// Purpose of the emission (finNFSe).
		/// <list type="bullet">
		/// <item>0: regular NFSe</item>
		/// </list>
		/// </summary>
		[JsonProperty("finalidade_emissao")]
		public int EmissionPurpose { get; set; } = 0;

		/// <summary>
		/// Whether the operation is for personal use or consumption (indFinal).
		/// <list type="bullet">
		/// <item>0: no</item>
		/// <item>1: yes</item>
		/// </list>
		/// </summary>
		[JsonProperty("consumidor_final")]
		public int FinalConsumer { get; set; }

		/// <summary>
		/// Operation indicator code (cIndOp), 6 digits.
		/// </summary>
		[JsonProperty("codigo_indicador_operacao")]
		public string OperationIndicator { get; set; }

		/// <summary>
		/// Who receives the service (indDest).
		/// <list type="bullet">
		/// <item>0: the client itself</item>
		/// <item>1: someone else, or another establishment of the client</item>
		/// </list>
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
}
