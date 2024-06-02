using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaueantAPI.Models;

public partial class Restaurant
{
    [Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }
    [Required]
    public string? RestaurantCode { get; set; }
    [Required]
    public string? RestaurantName { get; set; }

    public string? RestaurantNameAr { get; set; }
}
