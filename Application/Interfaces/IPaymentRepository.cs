using Domain.Models;

namespace Application.Interfaces;

public interface IPaymentRepository
{
    Task AddAsync(Payment payment);
    Task SaveChangesAsync();
}
