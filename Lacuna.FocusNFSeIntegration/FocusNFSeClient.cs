using Lacuna.FocusNFSeIntegration.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Lacuna.FocusNFSeIntegration {
	public class FocusNFSeClient {

		private readonly ILogger<FocusNFSeClient> logger;
		private readonly IHttpClientFactory clientFactory;

		public FocusNFSeClient(ILogger<FocusNFSeClient> logger, IHttpClientFactory clientFactory) {
			this.logger = logger;
			this.clientFactory = clientFactory;
		}

		/// <summary>
		/// Submits a NFSe. Must be verified if the NFSe was accept in further retrieval requests.
		/// </summary>
		public async Task<NFSeResponse> CreateNFSeAsync(string reference, NFSeRequest request) {
			var body = JsonConvert.SerializeObject(request,
							new JsonSerializerSettings {
								NullValueHandling = NullValueHandling.Ignore
							});

			var requestUri = $"/v2/nfse?ref={reference}";

			var data = new StringContent(body, Encoding.UTF8, Constants.MediaType);

			return await sendHttpRequestAsync<NFSeResponse>(
				HttpMethod.Post,
				requestUri,
				data,
				(response, client, obj) => handleErrorResponse(
					HttpMethod.Post,
					new Uri(client.BaseAddress, requestUri),
					"Response error",
					"Error on response",
					obj.Errors
				)
			);
		}

		/// <summary>
		/// Retrieves a NFSe using its unique reference
		/// </summary>
		public async Task<NFSeDetailsResponse> RetrieveNFSeAsync(string reference) {
			var requestUri = $"/v2/nfse/{reference}?completa=0";

			return await sendHttpRequestAsync<NFSeDetailsResponse>(
				HttpMethod.Get,
				requestUri,
				afterDeserialization: (response, client, obj) => handleErrorResponse(
					HttpMethod.Get,
					new Uri(client.BaseAddress, requestUri),
					"Response error",
					"Error on response",
					obj.Errors
				)
			);
		}

		/// <summary>
		/// Cancels a NFSe using its unique reference
		/// </summary>
		public async Task<NFSeOnlyStatusResponse> CancelNFSeAsync(string reference) {
			var requestUri = $"/v2/nfse/{reference}";

			return await sendHttpRequestAsync<NFSeOnlyStatusResponse>(
				HttpMethod.Delete,
				requestUri,
				afterDeserialization: (response, client, obj) => handleErrorResponse(
					HttpMethod.Delete,
					new Uri(client.BaseAddress, requestUri),
					"Response error",
					"Error on response",
					obj.Errors
				)
			);
		}

		/// <summary>
		/// Sends a NFSe to the e-mails inside the given list
		/// </summary>
		//public async Task ResendEmailAsync(string reference, EmailSendRequest request) {

		//	if (request.Emails == null) {
		//		throw new Exception("A list of e-mails must be provided. Min: 1, Max: 10.");
		//	}

		//	if (request.Emails.Count() > 10 || request.Emails.Count() < 1) {
		//		var emailCount = request.Emails.Count();
		//		throw new Exception($"A list of e-mails must be provided with at least 1 email and no more than 10 emails. Emails in the list: {emailCount}");
		//	}

		//	var emailData = JsonConvert.SerializeObject(request);
		//	var requestUri = $"/v2/nfse/{reference}/email";

		//	var postResponse = await performHttpRequestAsync(HttpMethod.Post, requestUri,
		//		() => HttpClient.PostAsync(requestUri, new StringContent(emailData))
		//	);

		//	var stream = await postResponse.Content.ReadAsStreamAsync();

		//	using (var reader = new StreamReader(stream)) {
		//		var jsonResp = reader.ReadToEnd();

		//	}
		//}

		private async Task<T> sendHttpRequestAsync<T>(HttpMethod method, string endpoint, HttpContent content = null, Action<HttpResponseMessage, HttpClient, T> afterDeserialization = null) {
			using var client = clientFactory.CreateClient(Constants.FactoryClientName);
			HttpResponseMessage httpResponse = null;

			try {
				httpResponse = method switch {
					var m when m == HttpMethod.Get => await client.GetAsync(endpoint),
					var m when m == HttpMethod.Post => await client.PostAsync(endpoint, content),
					var m when m == HttpMethod.Delete => await client.DeleteAsync(endpoint),
					_ => throw new NotSupportedException($"HTTP method {method} not supported.")
				};
			} catch (Exception ex) {
				logger.LogError("Error calling Focus API. Method: {method}, Url: {endpoint}, Message: {Message}", method, endpoint, ex.Message);
				throw new FocusNFSeIntegrationUnreachableException(method, new Uri(client.BaseAddress, endpoint), ex);
			}

			var responseContent = await httpResponse.Content.ReadAsStringAsync();

			if (!httpResponse.IsSuccessStatusCode) {
				logger.LogError("Not sucessfull status code {StatusCode}: {stringContent}", httpResponse.StatusCode, responseContent);
				throw new FocusNFSeIntegrationHttpException(
					method,
					new Uri(client.BaseAddress, endpoint),
					httpResponse.StatusCode,
					httpResponse.ReasonPhrase,
					content: responseContent
				);
			}

			try {
				var result = JsonConvert.DeserializeObject<T>(responseContent);
				afterDeserialization?.Invoke(httpResponse, client, result);
				return result;
			} catch {
				var error = JsonConvert.DeserializeObject<NFSeError>(responseContent);
				throw new FocusNFSeIntegrationApiException(
					method,
					new Uri(client.BaseAddress, endpoint),
					"Response error",
					"Error on response",
					new List<string> { $"Codigo: {error.Code} - Mensagem: {error.Message}" }
				);
			}
		}

		private static void handleErrorResponse(HttpMethod method, Uri uri, string code, string message, List<NFSeError> errors) {
			if (errors != null) {
				throw new FocusNFSeIntegrationApiException(method, uri, code, message, errors.ConvertAll(e => $"Codigo: {e.Code} - Mensagem: {e.Message}"));
			}
		}
	}
}
