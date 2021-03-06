﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lacuna.FocusNFSeIntegration.Models {
	public class ClientInfo {

		/// <summary>
		/// Client's Cpf number
		/// </summary>
		[JsonProperty("cpf")]
		public string Cpf { get; set; }

		/// <summary>
		/// Client's Cnpj number
		/// </summary>
		[JsonProperty("cnpj")]
		public string Cnpj { get; set; }

		/// <summary>
		/// Client's city subscription number
		/// </summary>
		[JsonProperty("inscricao_municipal")]
		public string CitySubscription { get; set; }

		/// <summary>
		/// Client's state subscription number
		/// </summary>
		[JsonProperty("inscricao_estadual")]
		public string StateSubscription { get; set; }

		/// <summary>
		/// Client's company name
		/// </summary>
		[JsonProperty("razao_social")]
		public string CompanyNameOrClientName { get; set; }

		/// <summary>
		/// Client's phone
		/// </summary>
		[JsonProperty("phone")]
		public string Phone { get; set; }

		/// <summary>
		/// Client's e-mail
		/// </summary>
		[JsonProperty("email")]
		public string Email { get; set; }

		/// <summary>
		/// Client's address
		/// </summary>
		[JsonProperty("endereco")]
		public AddressInfo AddressInfo { get; set; }
	}
}
