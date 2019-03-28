#if NETSTANDARD2_0
using Microsoft.Extensions.Options;

namespace Lacuna.FocusNFSeIntegration {
	internal class FocusNFSeClientAspNetCore: FocusNFSeClient {
		public FocusNFSeClientAspNetCore(IOptions<FocusNFSeIntegrationOptions> options) : base(options.Value) {
		}
	}
}
#endif
