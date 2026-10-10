using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lesson7.Models;

public partial class OrderDetail
{
    [Key]
    public int OrderDetailId { get; set; }

    public string? OrderId { get; set; }

    public string? BookId { get; set; }

    public int? Quantity { get; set; }

    public int? Price { get; set; }

    public int? TotalMoney { get; set; }

    public virtual Book? Book { get; set; }

    public virtual OrderBook? Order { get; set; }
}
