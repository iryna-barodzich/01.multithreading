using System;

namespace Facade.Task1.OrderPlacement
{
    public class OrderFacade
    {
        private readonly InvoiceSystem invoiceSystem;
        private readonly PaymentSystem paymentSystem;
        private readonly ProductCatalog productCatalog;

        public OrderFacade(InvoiceSystem invoiceSystem, PaymentSystem paymentSystem, ProductCatalog productCatalog)
        {
            this.invoiceSystem = invoiceSystem;
            this.paymentSystem = paymentSystem;
            this.productCatalog = productCatalog;
        }
        public void PlaceOrder(string productId, int quantity, string email)
        {
            var product = this.productCatalog.GetProductDetails(productId);
            paymentSystem.MakePayment(
                new Payment { 
                ProductId = product.Id, 
                    ProductName = product.Name, 
                    Quantity = quantity, 
                    TotalPrice = product.Price 
                });
            invoiceSystem.SendInvoice(
                new Invoice
                {
                    CustomerEmail = email,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Quantity = quantity,
                    TotalPrice = product.Price,
                    InvoiceNumber = new Guid(),
                    SendDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(7)
                });
        }
    }
}
