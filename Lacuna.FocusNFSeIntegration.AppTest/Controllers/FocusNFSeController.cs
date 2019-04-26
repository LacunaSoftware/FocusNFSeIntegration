using Lacuna.FocusNFeIntegration.AppTest.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lacuna.FocusNFSeIntegration.AppTest.Controllers {

	[Route("api/[controller]")]
	public class FocusNFSeController : Controller {

		private readonly FocusNFSeClient focusClient;
		private readonly IOptions<FocusNFSeIntegrationOptions> focusOptions;

		public FocusNFSeController(FocusNFSeClient focusClient, IOptions<FocusNFSeIntegrationOptions> focusOptions) {
			this.focusClient = focusClient;
			this.focusOptions = focusOptions;
		}

		[HttpPost("{reference}")]
		public async Task<IActionResult> SubmitNFSeAsync(string reference, [FromQuery]bool hasCnpj = true) {
			var req = TestHelpers.GenerateRequest(focusOptions.Value, hasCnpj);

			var retorno = await focusClient.CreateNFSeAsync(reference, req);
			return Ok(retorno);
		}

		[HttpGet("{reference}")]
		public async Task<IActionResult> GetNFSeAsync(string reference) {
			var now = DateTime.Now;
			var retorno = await focusClient.RetrieveNFSeAsync(reference);
			return Ok(retorno);
		}

		[HttpDelete("{reference}")]
		public async Task<IActionResult> CancelNFSeAsync(string reference) {
			var now = DateTime.Now;
			var retorno = await focusClient.CancelNFSeAsync(reference);
			return Ok(retorno);
		}

		//[HttpPost("email/{reference}")]
		//public async Task<IActionResult> EmailNFSeAsync(string reference, [FromBody]EmailSendRequest request) {
		//	await focusClient.ResendEmailAsync(reference, request);
		//	return Ok();
		//}
	}
}
