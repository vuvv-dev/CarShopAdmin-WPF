using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Entities;

public sealed class BillStatus
{
    [Key]
    public int Id { get; set; }

    public string BillStatusName { get; set; }

    public IEnumerable<Bill> Bills { get; set; }
}
