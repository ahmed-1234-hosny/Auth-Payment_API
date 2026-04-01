using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
	public class CreateCheckoutDto
	{
		public List<CheckoutItemDto> Items { get; set; } = new();
	}

	public class CheckoutItemDto
	{
		public string Name { get; set; } = string.Empty;
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}

	public class CheckoutResponseDto
	{
		public string CheckoutUrl { get; set; } = string.Empty;
		public string SessionId { get; set; } = string.Empty;
	}
}
