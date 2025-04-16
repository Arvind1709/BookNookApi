using Microsoft.AspNetCore.Mvc;
using TheBookNookApi.Dtos;

namespace TheBookNookApi.Services.Interfaces
{
    #region IProformaInvoiceService

    /// <summary>
    /// Interface for handling proforma invoice operations such as generation and emailing.
    /// </summary>
    public interface IProformaInvoiceService
    {
        /// <summary>
        /// Generates a proforma invoice for the given order ID.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <returns>A ProformaInvoiceDto containing invoice details.</returns>
        Task<ProformaInvoiceDto> GenerateProformaInvoiceAsync(int orderId);

        /// <summary>
        /// Downloads the proforma invoice in PDF format.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <returns>A file result containing the generated PDF.</returns>
        Task<FileContentResult> DownloadProformaInvoiceAsync(int orderId);

        /// <summary>
        /// Sends the proforma invoice to the customer's email.
        /// </summary>
        /// <param name="toEmail">The recipient email address.</param>
        /// <param name="pdfBytes">The byte array representing the PDF content.</param>
        Task SendInvoiceEmailAsync(string toEmail, byte[] pdfBytes);
    }

    #endregion
}
