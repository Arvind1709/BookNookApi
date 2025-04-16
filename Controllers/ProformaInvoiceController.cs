using Microsoft.AspNetCore.Mvc;
using TheBookNookApi.Services.Interfaces;

namespace TheBookNookApi.Controllers
{
    #region ProformaInvoiceController

    /// <summary>
    /// Controller responsible for managing proforma invoices, including generation, downloading, and emailing.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProformaInvoiceController : ControllerBase
    {
        private readonly IProformaInvoiceService _invoiceService;

        public ProformaInvoiceController(IProformaInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        /// <summary>
        /// Generates a proforma invoice for the given order ID.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <returns>A ProformaInvoiceDto containing the invoice details.</returns>
        [HttpGet("GenerateProforma/{orderId}")]
        public async Task<IActionResult> GenerateProformaInvoice(int orderId)
        {
            try
            {
                var proformaInvoice = await _invoiceService.GenerateProformaInvoiceAsync(orderId);
                return Ok(proformaInvoice);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Downloads the proforma invoice as a PDF.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <returns>The generated PDF file for download.</returns>
        [HttpGet("DownloadPI/{orderId}")]
        public async Task<IActionResult> DownloadProformaInvoice(int orderId)
        {
            try
            {
                var pdfFile = await _invoiceService.DownloadProformaInvoiceAsync(orderId);
                return pdfFile;
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Sends the proforma invoice to the customer's email.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <param name="email">The recipient email address.</param>
        /// <returns>A response indicating success or failure.</returns>
        [HttpPost("SendInvoiceEmail/{orderId}")]
        public async Task<IActionResult> SendProformaInvoiceEmail(int orderId, string email)
        {
            try
            {
                var proformaInvoice = await _invoiceService.DownloadProformaInvoiceAsync(orderId);
                await _invoiceService.SendInvoiceEmailAsync(email, proformaInvoice.FileContents);
                return Ok("Invoice sent successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }

    #endregion

}
