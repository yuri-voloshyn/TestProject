using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestProject.Dto.Models
{
	[XmlRoot(ElementName = "orders")]
	public class OrderXml
	{
		[XmlElement(ElementName = "order")]
		public List<Order> Orders { get; set; }
	}

	public class Order
	{
		[XmlElement(ElementName = "oxid")]
		public long OxId { get; set; }

		[XmlElement(ElementName = "orderdate")]
		public DateTime OrderDatetime { get; set; }

		[XmlElement(ElementName = "billingaddress")]
		public List<BillingAddress> BillingAddresses { get; set; }
	}

	public class BillingAddress
	{
		[XmlElement(ElementName = "billemail")]
		public string Email { get; set; }

		[XmlElement(ElementName = "billfname")]
		public string Fullname { get; set; }

		[XmlElement(ElementName = "billstreet")]
		public string Street { get; set; }

		[XmlElement(ElementName = "billstreetnr")]
		public short HomeNumber { get; set; }

		[XmlElement(ElementName = "billcity")]
		public string City { get; set; }

		[XmlElement(ElementName = "country")]
		public Country Country { get; set; }

		[XmlElement(ElementName = "billzip")]
		public int Zip { get; set; }
	}

	public class Country
	{
		[XmlElement(ElementName = "geo")]
		public string Code { get; set; }
	}
}
