namespace EcommerceOrders.Domain.Common;

public enum ErrorCode
{
    ValidationError = 1,
    CustomerNotFound = 2,
    OrderNotFound = 3,
    InsufficientStock = 4,
    Unauthorized = 5,
    BusinessRuleViolation = 6
}