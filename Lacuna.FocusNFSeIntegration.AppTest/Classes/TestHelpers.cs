using Lacuna.FocusNFSeIntegration;
using Lacuna.FocusNFSeIntegration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lacuna.FocusNFeIntegration.AppTest.Classes {
	public class TestHelpers {

		public static NFSeRequest GenerateRequest(FocusNFSeIntegrationOptions options) {
			var request = new NFSeRequest {
				EmissionDate = DateTime.Now,
				Provider = GenerateProviderInfo(options),
				Client = GenerateClientInfo(options),
				Info = GenerateServiceInfo(options),
			};

			return request;
		}

		public static ServiceInfo GenerateServiceInfo(FocusNFSeIntegrationOptions options) {
			var value = new Random().NextDouble();
			if(value < 0) {
				value = -value;
			}
			return new ServiceInfo {
				Aliquota = options.ServiceInfo.Aliquota,
				CityTributeCode = options.ServiceInfo.CityTributeCode,
				Description = options.ServiceInfo.Description,
				IssRetained = options.ServiceInfo.IssRetained,
				ServiceListItem = options.ServiceInfo.ServiceListItem,
				ServiceValue = (decimal)value,
			};
		}

		public static ClientInfo GenerateClientInfo(FocusNFSeIntegrationOptions options) {
			return new ClientInfo {
				Cnpj = "55500000000142",
				CompanyName = "Test Company",
				Email = "focusnfse.test@malinator.com",
				AddressInfo = new AddressInfo {
					CityCode = "5300108",
					Complement = "",
					Neighborhood = "Centro",
					Number = "1",
					PostalCode = "70297400",
					Street = "Rua Teste",
					UF = "DF"
				}
			};
		}

		public static ProviderInfo GenerateProviderInfo(FocusNFSeIntegrationOptions options) {
			return new ProviderInfo {
				CityCode = options.CityCode,
				CitySubscription = options.CitySubscription,
				Cnpj = options.Cnpj
			};
		}

		public static List<string> GenerateEmailList() {
			return new List<string>() {
				"nfse.test1@mailinator.com",
				"nfse.test2@mailinator.com",
				"nfse.test3@mailinator.com",
				"nfse.test4@mailinator.com",
			};
		}
	}
}
