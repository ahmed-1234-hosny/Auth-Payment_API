using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
	public class PaymentItem
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public decimal Price { get; set; }
		public int Quantity { get; set; }

		public int PaymentId { get; set; }
		public Payment Payment { get; set; } = null!;
	}
}
