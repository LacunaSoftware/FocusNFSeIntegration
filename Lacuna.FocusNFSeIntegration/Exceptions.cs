using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Lacuna.FocusNFSeIntegration {
	public abstract class FocusNFSeIntegrationException : Exception {

		public HttpMethod Verb { get; set; }

		public Uri Uri { get; set; }

		public FocusNFSeIntegrationException(string message, HttpMethod verb, Uri uri, Exception innerException = null) : base(message, innerException) {
			Verb = verb;
			Uri = uri;
		}
	}

	public class FocusNFSeIntegrationUnreachableException : FocusNFSeIntegrationException {

		public FocusNFSeIntegrationUnreachableException(HttpMethod verb, Uri uri, Exception innerException = null) : base($"Bradesco API {verb} {uri} is unreachable", verb, uri, innerException) {
		}
	}

	public class FocusNFSeIntegrationHttpException : FocusNFSeIntegrationException {

		public HttpStatusCode StatusCode { get; private set; }

		public string ErrorMessage { get; private set; }

		public FocusNFSeIntegrationHttpException(HttpMethod verb, Uri uri, HttpStatusCode statusCode, string errorMessage = null, Exception innerException = null) : base(formatExceptionMessage(verb, uri, statusCode, errorMessage), verb, uri, innerException) {
			StatusCode = statusCode;
			ErrorMessage = errorMessage;
		}

		private static string formatExceptionMessage(HttpMethod verb, Uri uri, HttpStatusCode statusCode, string errorMessage) {
			var sb = new StringBuilder();
			sb.AppendFormat("Bradesco API {0} {1} returned HTTP error {2}", verb.Method, uri.AbsoluteUri, (int)statusCode);
			if (Enum.IsDefined(typeof(HttpStatusCode), statusCode)) {
				sb.AppendFormat(" ({0})", statusCode);
			}
			if (!string.IsNullOrWhiteSpace(errorMessage)) {
				sb.AppendFormat(": {0}", errorMessage);
			}
			return sb.ToString();
		}
	}

	public class FocusNFSeIntegrationApiException : FocusNFSeIntegrationException {

		public string Code { get; set; }

		public string FocusMessage { get; set; }

		public List<string> Errors { get; set; } = null;

		public FocusNFSeIntegrationApiException(HttpMethod verb, Uri uri, string code, string message, List<string> errors = null)
			: base(formatErrorMessage(verb, uri, code, message, errors), verb, uri) {
			Code = code;
			FocusMessage = message;
			Errors = errors;
		}

		private static string formatErrorMessage(HttpMethod verb, Uri uri, string code, string message, List<string> errors) {
			if (errors != null) {
				return string.Format("Focus NFSe API {0} {1} returned an error. Focus NFSe error code: {2}. Message: {3}", verb, uri, code, message);
			} else {
				return string.Format("Focus NFSe API {0} {1} returned an error. Focus NFSe error code: {2}. Message: {3}. Errors returned: {4}", verb, uri, code, message, string.Join(", ", errors.ToArray()));
			}
		}
	}

}
