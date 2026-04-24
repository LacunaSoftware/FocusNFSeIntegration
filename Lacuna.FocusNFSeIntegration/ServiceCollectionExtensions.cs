using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Text;

namespace Lacuna.FocusNFSeIntegration {

	public static class FocusNFSeIntegrationExtensions {

		public static IServiceCollection AddFocusNFSeIntegration(this IServiceCollection services, Action<FocusNFSeIntegrationOptions> action = null) {

			if (action != null) {
				services.Configure(action);
			}

			services.AddHttpClient(Constants.FactoryClientName)
				.ConfigureHttpClient((serviceProvider, c) => {
					var options = serviceProvider.GetRequiredService<IOptions<FocusNFSeIntegrationOptions>>().Value;

					c.BaseAddress = new Uri(
						options.IsSandbox ? options.SandboxEndpoint : options.Endpoint
					);

					c.DefaultRequestHeaders.Accept.Clear();
					c.DefaultRequestHeaders.Add("Accept-Charset", Constants.CharSet);
					var authHeaderBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(options.IsSandbox ? options.SandboxToken : options.Token));
					c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderBase64);
				});

			services.AddSingleton<FocusNFSeClient, FocusNFSeAspNetCore>();

			return services;
		}
	}
}
