using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lacuna.FocusNFSeIntegration {
	public class Status {
		public static readonly string Authorized = "autorizado";
		public static readonly string ProcessingAuthorization = "processando_autorizacao";
		public static readonly string Canceled = "cancelado";
		public static readonly string AuthorizationError = "erro_autorizacao";
		public static readonly string Denied = "denegado";
	}
}
