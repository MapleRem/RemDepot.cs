namespace ShopSystem.Api.Models
{
    
    namespace ShopSystem.Api.Models
{
    public enum UserRole
    {
        Admin,
        Customer,
        Dev
    }
}
    
    public enum UserRole
    {
        Customer,
        Seller,
        Admin,
        Courier
    }

    public enum OrderStatus
    {
        Pending,
        Paid,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }

    public enum PaymentMethod
    {
        CashOnDelivery,
        CreditCard,
        GCash,
        BankTransfer
    }

    public enum ShipmentStatus
    {
        Preparing,
        InTransit,
        Delivered,
        Failed
    }
}
