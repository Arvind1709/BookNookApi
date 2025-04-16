using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using TheBookNookApi.AppDbContext;
using TheBookNookApi.Dtos;
using TheBookNookApi.Services.Interfaces;

//using MimeKit;
//using System.Net.Mail;




namespace TheBookNookApi.Services
{
    #region ProformaInvoiceService

    /// <summary>
    /// Service implementation for proforma invoice operations such as generating PDF and sending email.
    /// </summary>
    public class ProformaInvoiceService : IProformaInvoiceService
    {
        private readonly BookNookDbContext _context;
        private readonly IConverter _converter;

        public ProformaInvoiceService(BookNookDbContext context, IConverter converter)
        {
            _context = context;
            _converter = converter;
        }

        public async Task<ProformaInvoiceDto> GenerateProformaInvoiceAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                throw new KeyNotFoundException("Order not found!");
            }

            var proformaInvoice = new ProformaInvoiceDto
            {
                OrderId = order.Id,
                BuyerName = "Customer Name", // This can be fetched from User table if needed
                InvoiceDate = DateTime.UtcNow,
                //Items = order.OrderItems.Select(oi => new ProformaItemDto
                //{
                //    BookTitle = oi.Book.Title,
                //    Quantity = oi.Quantity,
                //    PricePerUnit = oi.Price
                //}).ToList(),

                Items = order.OrderItems.Select(oi => new ProformaItemDto
                {
                    BookTitle = oi.Book?.Title ?? "Unknown",
                    Quantity = oi.Quantity,
                    PricePerUnit = oi.Price
                }).ToList(),

                TotalAmount = order.TotalAmount
            };

            return proformaInvoice;
        }

        public async Task<FileContentResult> DownloadProformaInvoiceAsync(int orderId)
        {
            var proformaInvoice = await GenerateProformaInvoiceAsync(orderId);

            using var stream = new MemoryStream();
            var writer = new iText.Kernel.Pdf.PdfWriter(stream);

            var pdf = new iText.Kernel.Pdf.PdfDocument(writer);
            var doc = new iText.Layout.Document(pdf);

            doc.Add(new iText.Layout.Element.Paragraph("Proforma Invoice").SetFontSize(18).SetBold());
            doc.Add(new iText.Layout.Element.Paragraph($"Order ID: {proformaInvoice.OrderId}"));
            doc.Add(new iText.Layout.Element.Paragraph($"Buyer Name: {proformaInvoice.BuyerName}"));
            doc.Add(new iText.Layout.Element.Paragraph($"Invoice Date: {proformaInvoice.InvoiceDate:dd-MM-yyyy}"));
            doc.Add(new iText.Layout.Element.Paragraph(" ")); // Space

            // Table for items
            var table = new iText.Layout.Element.Table(4);
            table.AddHeaderCell("Book Title");
            table.AddHeaderCell("Quantity");
            table.AddHeaderCell("Price/Unit");
            table.AddHeaderCell("Total");

            foreach (var item in proformaInvoice.Items)
            {
                table.AddCell(item.BookTitle);
                table.AddCell(item.Quantity.ToString());
                table.AddCell(item.PricePerUnit.ToString("C"));
                table.AddCell(item.TotalPrice.ToString("C"));
            }

            doc.Add(table);
            doc.Add(new iText.Layout.Element.Paragraph($"Total Amount: {proformaInvoice.TotalAmount:C}").SetBold());
            doc.Close();

            return new FileContentResult(stream.ToArray(), "application/pdf")
            {
                FileDownloadName = $"ProformaInvoice_Order_{orderId}.pdf"
            };
        }

        public async Task SendInvoiceEmailAsync(string toEmail, byte[] pdfBytes)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Your Name", "your_email@gmail.com"));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = "Proforma Invoice";

            var builder = new BodyBuilder
            {
                HtmlBody = "<p>Please find attached the proforma invoice.</p>"
            };

            // Attach the PDF
            builder.Attachments.Add("ProformaInvoice.pdf", pdfBytes, new ContentType("application", "pdf"));
            message.Body = builder.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("your_email@gmail.com", "your_app_password"); // Use App Password
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }

    #endregion

}
