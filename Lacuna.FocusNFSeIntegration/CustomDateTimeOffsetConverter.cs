using Newtonsoft.Json.Converters;

namespace Lacuna.FocusNFSeIntegration {

	/// <summary>
	/// Date and time with its offset, to the second (AAAA-MM-DDThh:mm:ssTZD), as the national layout
	/// documents it: no fractional seconds.
	/// </summary>
	public class CustomDateTimeOffsetConverter : IsoDateTimeConverter {
		public CustomDateTimeOffsetConverter() {
			DateTimeFormat = "yyyy-MM-ddTHH:mm:sszzz";
		}
	}
}
