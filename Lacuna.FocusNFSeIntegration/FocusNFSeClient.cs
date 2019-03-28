using Lacuna.FocusNFSeIntegration.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Lacuna.FocusNFSeIntegration {
	public class FocusNFSeClient {

		private HttpClient _httpClient;

		private readonly FocusNFSeIntegrationOptions options;

		protected HttpClient HttpClient {
			get {
				if (_httpClient == null) {
					_httpClient = new HttpClient {
						BaseAddress = new Uri(options.IsSandbox ? options.SandboxEndpoint : options.Endpoint)
					};
					_httpClient.DefaultRequestHeaders.Accept.Clear();
					_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", options.IsSandbox ? options.SandboxToken : options.Token);
				}
				return _httpClient;
			}
		}

		public FocusNFSeClient(FocusNFSeIntegrationOptions options) {
			this.options = options;
		}

		/// <summary>
		/// Submits a NFSe. Must be verified if the NFSe was accept in further retrieval requests.
		/// </summary>
		public async Task<NFSeResponse> CreateNFSeAsync(string reference, NFSeRequest request) {
			var data = JsonConvert.SerializeObject(request);
			var requestUri = $"?ref={reference}";

			var postResponse = await performHttpRequestAsync(HttpMethod.Post, requestUri,
				() => HttpClient.PostAsJsonAsync(requestUri, data)
			);

			var stream = await postResponse.Content.ReadAsStreamAsync();

			using (var reader = new StreamReader(stream)) {
				var jsonResp = reader.ReadToEnd();
				var response = JsonConvert.DeserializeObject<NFSeResponse>(jsonResp);

				if (response.Errors != null) {
					throw new FocusNFSeIntegrationApiException(HttpMethod.Post, new Uri(HttpClient.BaseAddress, requestUri), "Response error", "Error on response", response.Errors);
				}

				return response;
			}
		}

		/// <summary>
		/// Retrieves a NFSe using its unique reference
		/// </summary>
		public async Task<NFSeDetailsResponse> RetrieveNFSeAsync(string reference) {
			var requestUri = $"/{reference}?completa=0";

			var resp = await performHttpRequestAsync(HttpMethod.Get, requestUri,
				() => HttpClient.GetAsync(requestUri)
			);

			var stream = await HttpClient.GetStreamAsync(requestUri);

			using (var reader = new StreamReader(stream)) {
				var jsonretorno = reader.ReadToEnd();
				var response = JsonConvert.DeserializeObject<NFSeDetailsResponse>(jsonretorno);

				if (response.Errors != null) {
					throw new FocusNFSeIntegrationApiException(HttpMethod.Get, new Uri(HttpClient.BaseAddress, requestUri), "Response error", "Error on response", response.Errors);
				}

				return response;
			}
		}

		/// <summary>
		/// Cancels a NFSe using its unique reference
		/// </summary>
		public async Task<NFSeOnlyStatusResponse> CancelNFSeAsync(string reference) {
			var requestUri = $"/{reference}";

			var resp = await performHttpRequestAsync(HttpMethod.Delete, requestUri,
				() => HttpClient.DeleteAsync(requestUri)
			);

			var stream = await HttpClient.GetStreamAsync(requestUri);

			using (var reader = new StreamReader(stream)) {
				var jsonretorno = reader.ReadToEnd();
				var response = JsonConvert.DeserializeObject<NFSeOnlyStatusResponse>(jsonretorno);

				if (response.Errors != null) {
					throw new FocusNFSeIntegrationApiException(HttpMethod.Delete, new Uri(HttpClient.BaseAddress, requestUri), "Response error", "Error on response", response.Errors);
				}

				return response;
			}
		}

		/// <summary>
		/// Sends a NFSe to the e-mails inside the given list
		/// </summary>
		public async Task ResendEmailAsync(string reference, IEnumerable<string> emails) {

			if (emails == null) {
				throw new Exception("A list of e-mails must be provided. Min: 1, Max: 10.");
			}

			if (emails.Count() > 10 || emails.Count() < 1) {
				var emailCount = emails.Count();
				throw new Exception($"A list of e-mails must be provided with at least 1 email and no more than 10 emails. Emails in the list: {emailCount}");
			}

			var emailData = JsonConvert.SerializeObject(emails);
			var requestUri = $"/{reference}/email";

			var postResponse = await performHttpRequestAsync(HttpMethod.Post, requestUri,
				() => HttpClient.PostAsJsonAsync(requestUri, emailData)
			);

			var stream = await postResponse.Content.ReadAsStreamAsync();

			using (var reader = new StreamReader(stream)) {
				var jsonResp = reader.ReadToEnd();
	
			}
		}

		private async Task<HttpResponseMessage> performHttpRequestAsync(HttpMethod verb, string requestUri, Func<Task<HttpResponseMessage>> asyncFunc) {
			var uri = new Uri(HttpClient.BaseAddress, requestUri);
			HttpResponseMessage httpResponse;
			try {
				httpResponse = await asyncFunc();
			} catch (Exception ex) {
				throw new FocusNFSeIntegrationUnreachableException(verb, uri, ex);
			}
			if (!httpResponse.IsSuccessStatusCode) {
				throw new FocusNFSeIntegrationHttpException(verb, uri, httpResponse.StatusCode, httpResponse.ReasonPhrase);
			}
			return httpResponse;
		}

	}
}
