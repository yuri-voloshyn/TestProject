using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TestProject.Data.Enums;
using TestProject.Data.Models;
using TestProject.Dto.Models;

namespace TestProject.Controllers
{
	[Route("v1/[controller]")]
	[ApiController]
	public class OrderController : Controller
	{
		private readonly ApplicationDbContext db;

		public OrderController(ApplicationDbContext db)
		{
			this.db = db;
		}

		[HttpGet]
		[Produces(typeof(IEnumerable<OrderDto>))]
		public async Task<IActionResult> Get(long? id)
		{
			var query = db.Orders.AsQueryable();
			if (id != null)
				query = query.Where(a => a.OxId == id.Value);

			return Ok(await query.Select(a => new OrderDto
			{
				OxId = a.OxId,
				OrderDatetime = a.OrderDatetime,
				OrderStatus = a.OrderStatus,
				InvoiceNumber = a.InvoiceNumber,
			}).ToListAsync());
		}

		[HttpPost(("Import"))]
		[Consumes("application/xml")]
		public async Task<IActionResult> Import([FromBody]OrderXml model)
		{
			foreach (var orderItem in model.Orders)
			{
				var order = new Orders
				{
					OxId = orderItem.OxId,
					OrderDatetime = orderItem.OrderDatetime,
				};
				db.Add(order);
				foreach (var billingAddressItem in orderItem.BillingAddresses)
				{
					var billingAddress = new BillingAddresses
					{
						OrderOx = order,
						Email = billingAddressItem.Email,
						Fullname = billingAddressItem.Fullname,
						Country = billingAddressItem.Country.Code,
						City = billingAddressItem.City,
						Street = billingAddressItem.Street,
						HomeNumber = billingAddressItem.HomeNumber,
						Zip = billingAddressItem.Zip,
					};
					db.Add(billingAddress);
				}
			}

			await db.SaveChangesAsync();

			return Ok();
		}

		[HttpGet("{id}/SetOrderStatus")]
		public async Task<IActionResult> SetOrderStatus(long id, OrderStatus status)
		{
			var entity = await db.Orders.Where(a => a.OxId == id).FirstOrDefaultAsync();
			if (entity == null)
				return NotFound();

			entity.OrderStatus = (byte)status;
			await db.SaveChangesAsync();

			return Ok();
		}

		[HttpGet("{id}/SetInvoiceNumber")]
		public async Task<IActionResult> SetInvoiceNumber(long id, int invoiceNumber)
		{
			var entity = await db.Orders.Where(a => a.OxId == id).FirstOrDefaultAsync();
			if (entity == null)
				return NotFound();

			entity.InvoiceNumber = invoiceNumber;
			await db.SaveChangesAsync();

			return Ok();
		}
	}
}
