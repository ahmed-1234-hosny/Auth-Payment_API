using Application.Dtos;

namespace Application.Interfaces;

public interface IPaymentService
{
    Task<CheckoutResponseDto> CreateCheckoutSessionAsync(CreateCheckoutDto dto, int userId);
}
