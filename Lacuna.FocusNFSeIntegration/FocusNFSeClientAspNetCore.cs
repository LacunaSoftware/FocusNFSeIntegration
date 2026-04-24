using Lacuna.FocusNFSeIntegration;
using Microsoft.Extensions.Logging;
using System.Net.Http;

internal class FocusNFSeAspNetCore : FocusNFSeClient {

	public FocusNFSeAspNetCore(ILogger<FocusNFSeClient> logger, IHttpClientFactory clientFactory) : base(logger, clientFactory) {
	}
}
