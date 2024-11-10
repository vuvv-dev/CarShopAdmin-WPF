using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Entities;

public sealed class CarBranch
{
    [Key]
    public int Id { get; set; }
    public string BranchName { get; set; }
    public string BranchLogo { get; set; }
    public IEnumerable<Car> Cars { get; set; }
}
