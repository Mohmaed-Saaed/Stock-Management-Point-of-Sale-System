using CoreLayer.Models.Operations;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PresentationLayer.Utility
{
    public class InvoiceDocument : IDocument
    {
        private readonly SalesInvoice _invoice;

        public InvoiceDocument(SalesInvoice invoice)
        {
            _invoice = invoice;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public DocumentSettings GetSettings() => DocumentSettings.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                // Receipt-like size (80mm width)
                page.Size(200, PageSizes.A4.Height); // ~half A4 width
                page.Margin(10);
                page.ContinuousSize(200); // Height fits content

                page.DefaultTextStyle(x => x.FontSize(8));

                // HEADER
                page.Header().Column(col =>
                {
                    col.Item().AlignCenter().Text("My Store").Bold().FontSize(12);
                    col.Item().AlignCenter().Text($"Branch: {_invoice.Branch?.Name}");
                    col.Item().AlignCenter().Text(_invoice.Branch?.Address);
                    col.Item().AlignCenter().Text(_invoice.Branch?.PhoneNumber);
                    col.Item().AlignCenter().Text("-------------------------------");
                });

                // CONTENT
                page.Content().Column(col =>
                {
                    col.Spacing(5);

                    // Invoice info
                    col.Item().Text($"Invoice #: {_invoice.Code}");
                    col.Item().Text($"Date: {_invoice.Date} {_invoice.Time}");
                    if (_invoice.RetailCustomer != null)
                        col.Item().Text($"Customer: {_invoice.RetailCustomer.Name}");
                    col.Item().Text($"Total Qty: {_invoice.TotalQuantity}");

                    col.Item().LineHorizontal(1);

                    // Items table
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(4); // Item
                            c.RelativeColumn(2); // Qty
                            c.RelativeColumn(3); // Price
                            c.RelativeColumn(3); // Total
                        });

                        // Header row
                        table.Header(h =>
                        {
                            h.Cell().Text("Item").Bold();
                            h.Cell().AlignRight().Text("Qty").Bold();
                            h.Cell().AlignRight().Text("Price").Bold();
                            h.Cell().AlignRight().Text("Total").Bold();
                        });

                        // Spacer row between header and items
                        table.Cell().ColumnSpan(4).Height(5);

                        // Items
                        foreach (var item in _invoice.OperationItems)
                        {
                            table.Cell().Text(item.Item?.Name).FontSize(7);
                            table.Cell().AlignRight().Text(item.Quantity.ToString()).FontSize(7);

                            // Price cell
                            table.Cell().AlignRight().Column(col =>
                            {
                                if (item.DiscountRate > 0)
                                {
                                    var price = (item.SellingPrice) * (1 - item.DiscountRate / 100.0);
                                    col.Item().Text($"{price:0.00}")
                                        .Bold()
                                        .FontSize(7);

                                    col.Item().Text($"{item.SellingPrice:0.00}")
                                        .FontSize(6)
                                        .Strikethrough()
                                        .FontColor(Colors.Grey.Medium);
                                    col.Item().Height(5);
                                }
                                else
                                {
                                    col.Item().Text($"{item.SellingPrice:0.00}").FontSize(7);
                                    col.Item().Height(5);
                                }
                            });

                            // Total column = final price × qty
                            table.Cell().AlignRight().Text(item.TotalPrice.ToString("0.00")).FontSize(7);
                        }
                    });


                    col.Item().LineHorizontal(1);

                    // Totals
                    col.Item().AlignRight().Text($"Subtotal: {_invoice.TotalAmount:0.00}");
                    if (_invoice.DiscountSalesInvoices.Count > 0)
                    {
                        col.Item().AlignRight().Text($"{_invoice.DiscountSalesInvoices.ToList()[0].Discount.Name} Discount: {_invoice.TotalDiscountAmount ?? 0:0.00}");
                    }

                    col.Item().LineHorizontal(1);
                    col.Item().AlignRight().Text($"Grand Total: {_invoice.GrandTotal:0.00}").Bold();
                    col.Item().AlignRight().Text($"Rounded Grand Total: {_invoice.RoundedGrandTotal:0.00}").Bold();

                    // Payment summary
                    col.Item().AlignRight().Text($"Paid Cash: {_invoice.PaidCash:0.00}");
                    col.Item().AlignRight().Text($"Change: {_invoice.Change:0.00}");

                    col.Item().LineHorizontal(1);

                    col.Item().AlignCenter().Text("Item Prices Include Taxes").Italic();
                });

                // FOOTER
                page.Footer()
                    .AlignCenter()
                    .Text("Thank you for shopping with us!");
            });
        }
    }
}
