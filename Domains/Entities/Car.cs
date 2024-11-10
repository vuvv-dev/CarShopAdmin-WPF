using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Entities;

public sealed class Car
{
    [Key]
    public int Id { get; set; }
    public string CarName { get; set; }
    public string CarCode { get; set; }
    public string CarDescription { get; set; }
    public string MadeInDate { get; set; }
    public string Color { get; set; }
    public string CarImage { get; set; }
    public string CarPrice { get; set; }
    public int StorageAmount { get; set; }

    //Ref
    public int CarBranchId { get; set; }
    public int CarStatusId { get; set; }
    public int CarSampleId { get; set; }

    //navigation property
    public CarBranch CarBranch { get; set; }
    public CarStatus CarStatus { get; set; }
    public CarSample CarSample { get; set; }
}
