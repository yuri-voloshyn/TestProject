using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.Dto.Models
{
	public class OrderDto
	{
		public long OxId { get; set; }
		public DateTime OrderDatetime { get; set; }
		public byte? OrderStatus { get; set; }
		public int? InvoiceNumber { get; set; }
	}
}
