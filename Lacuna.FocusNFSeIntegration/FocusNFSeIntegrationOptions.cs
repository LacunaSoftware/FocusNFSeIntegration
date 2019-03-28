using Lacuna.FocusNFSeIntegration.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lacuna.FocusNFSeIntegration {

	/// <summary>
	/// Focus NFSe Integration options, main configuration
	/// </summary>
	public class FocusNFSeIntegrationOptions {

		/// <summary>
		/// Checked if it's a sandbox environment
		/// </summary>
		public bool IsSandbox { get; set; }

		/// <summary>
		/// Integration production endpoint
		/// </summary>
		public string Endpoint { get; set; }

		/// <summary>
		/// Integration sandbox endpoint
		/// </summary>
		public string SandboxEndpoint { get; set; }

		/// <summary>
		/// Integration production auth token
		/// </summary>
		public string Token { get; set; }

		/// <summary>
		/// Integration sandbox auth token
		/// </summary>
		public string SandboxToken { get; set; }

		/// <summary>
		/// Provider Cnpj number
		/// </summary>
		public string Cnpj { get; set; }

		/// <summary>
		/// Provider's city subscription number
		/// </summary>
		public string CitySubscription { get; set; }

		/// <summary>
		/// Provider's city IBGE code
		/// </summary>
		public string CityCode { get; set; }

		/// <summary>
		/// Service and taxes configuration
		/// </summary>
		[JsonProperty("servico")]
		public ServiceInfo ServiceInfo { get; set; }
	}
}
