namespace EcommerceOrders.Domain.Common;

public record Error(
    ErrorCode Code,
    string Message);