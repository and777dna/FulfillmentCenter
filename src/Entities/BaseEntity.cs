using System.ComponentModel.DataAnnotations;

namespace FulfillmentCenter.Entities;

public class BaseEntity
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public bool IsDeleted { get; set; }
}