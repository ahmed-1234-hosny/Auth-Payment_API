using Application.Interfaces;
using Domain.Models;
using Infrastucture.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Repo
{
	public class PaymentRepo:IPaymentRepository
	{
		private readonly AppDBContext _context;
		public PaymentRepo(AppDBContext context)
		{
			_context = context;
		}
		public async Task AddAsync(Payment payment)
			=> await _context.Payment.AddAsync(payment);
		public async Task SaveChangesAsync()
			=> await _context.SaveChangesAsync();

	}
}
