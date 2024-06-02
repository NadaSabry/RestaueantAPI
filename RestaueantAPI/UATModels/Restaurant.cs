using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestaueantAPI.UATModels;

public partial class Restaurant
{
    [Key]
    public int Id { get; set; }

    public string RestaurantCode { get; set; } = null!;

    public string RestaurantName { get; set; } = null!;

    public string? RestaurantNameAr { get; set; }
}
