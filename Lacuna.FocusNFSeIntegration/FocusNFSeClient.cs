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

		private const string MunicipalPath = "/v2/nfse";
		private const string NationalPath = "/v2/nfsen";

		/// <summary>
		/// Submits a NFSe in the municipal layout. Must be verified if the NFSe was accepted in further
		/// retrieval requests.
		/// </summary>
		public Task<NFSeResponse> CreateNFSeAsync(string reference, NFSeRequest request) => createAsync(MunicipalPath, reference, request);

		/// <summary>
		/// Retrieves a municipal layout NFSe using its unique reference
		/// </summary>
		public Task<NFSeDetailsResponse> RetrieveNFSeAsync(string reference) => retrieveAsync(MunicipalPath, reference);

		/// <summary>
		/// Cancels a municipal layout NFSe using its unique reference
		/// </summary>
		public Task<NFSeOnlyStatusResponse> CancelNFSeAsync(string reference) => cancelAsync(MunicipalPath, reference);

		/// <summary>
		/// Submits a DPS in the national layout. Must be verified if the NFSe was accepted in further
		/// retrieval requests.
		/// </summary>
		public Task<NFSeResponse> CreateNationalNFSeAsync(string reference, NFSeNationalRequest request) => createAsync(NationalPath, reference, request);

		/// <summary>
		/// Retrieves a national layout NFSe using its unique reference
		/// </summary>
		public Task<NFSeDetailsResponse> RetrieveNationalNFSeAsync(string reference) => retrieveAsync(NationalPath, reference);

		/// <summary>
		/// Cancels a national layout NFSe using its unique reference
		/// </summary>
		public Task<NFSeOnlyStatusResponse> CancelNationalNFSeAsync(string reference) => cancelAsync(NationalPath, reference);

		private async Task<NFSeResponse> createAsync(string basePath, string reference, object request) {
			var body = JsonConvert.SerializeObject(request,
							new JsonSerializerSettings {
								NullValueHandling = NullValueHandling.Ignore
							});

			var requestUri = $"{basePath}?ref={reference}";

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

		private async Task<NFSeDetailsResponse> retrieveAsync(string basePath, string reference) {
			var requestUri = $"{basePath}/{reference}?completa=0";

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

		private async Task<NFSeOnlyStatusResponse> cancelAsync(string basePath, string reference) {
			var requestUri = $"{basePath}/{reference}";

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
				logger.LogInformation("NFSe response processed successfully. Method: {Method}, Url: {Endpoint}, ResponseType: {ResponseType}", method, endpoint, typeof(T).Name);
				afterDeserialization?.Invoke(httpResponse, client, result);
				return result;
			} catch (Exception ex) when (ex is not FocusNFSeIntegrationApiException) {
				logger.LogError(ex, "Error processing NFSe response. Method: {Method}, Url: {Endpoint}, ResponseContent: {ResponseContent}", method, endpoint, responseContent);
				var error = JsonConvert.DeserializeObject<NFSeError>(responseContent);
				throw new FocusNFSeIntegrationApiException(
					method,
					new Uri(client.BaseAddress, endpoint),
					"Response error",
					"Error on response",
					new List<string> { formatError(error) }
				);
			}
		}

		private static void handleErrorResponse(HttpMethod method, Uri uri, string code, string message, List<NFSeError> errors) {
			if (errors != null) {
				throw new FocusNFSeIntegrationApiException(method, uri, code, message, errors.ConvertAll(formatError));
			}
		}

		private static string formatError(NFSeError error) {
			var formatted = $"Codigo: {error.Code} - Mensagem: {error.Message}";
			return string.IsNullOrWhiteSpace(error.Correction) ? formatted : $"{formatted} - Correcao: {error.Correction}";
		}
	}
}
