using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Domain.Repositories;

public class PaymentRepository : ARepositoryAsync<Payment>
{
    public PaymentRepository(DbContext context) : base(context)
    {
    }
}


