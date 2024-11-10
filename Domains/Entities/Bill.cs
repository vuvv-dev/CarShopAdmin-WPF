using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Entities;

public sealed class Bill
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int BillStatusId { get; set; }
    public int CarId { get; set; }
    public Double TotalPrice { get; set; }
    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public Customer Customer { get; set; }
    public BillStatus BillStatus { get; set; }
    public Car Car { get; set; }
}
