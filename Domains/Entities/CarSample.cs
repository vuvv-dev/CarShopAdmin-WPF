using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Entities;

public sealed class CarSample
{
    [Key]
    public int Id { get; set; }
    public string SampleName { get; set; }

    public IEnumerable<Car> Cars { get; set; }
}
