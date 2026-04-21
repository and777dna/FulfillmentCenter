using System.ComponentModel.DataAnnotations;

namespace FulfillmentCenter.Entities;

public class BaseEntity
{
    [Required]
    public bool IsDeleted { get; set; }
}