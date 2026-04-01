using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
	public class Payment
	{
		public int Id { get; set; }
		public string StripeSessionId { get; set; } = string.Empty;
		public string Status { get; set; } = "Pending";
		public decimal TotalAmount { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public int UserId { get; set; }
		public User User { get; set; } = null!;

		public List<PaymentItem> Items { get; set; } = new();
	}
}
