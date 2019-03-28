#if NETSTANDARD2_0

using Microsoft.Extensions.DependencyInjection;
using System;

namespace Lacuna.FocusNFSeIntegration {

	public static class BradescoIntegrationBradescoIntegration {

		public static IServiceCollection AddFocusNFSeIntegration(this IServiceCollection services, Action<FocusNFSeIntegrationOptions> action = null) {

			if (action != null) {
				services.Configure(action);
			}
			services.AddSingleton<FocusNFSeClient, FocusNFSeClientAspNetCore>();

			return services;
		}
	}
}

#endif
