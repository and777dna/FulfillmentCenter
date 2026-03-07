namespace FulfillmentCenter.Enums;

//TODO review: If these values are ever surfaced to clients or logs, raw enum names work, but adding [EnumMember(Value = "...")] or similar annotations is good practice for API serialization consistency
public enum OrderStatus
{
    Created,
    Processing,
    ReadyToShip,
    Shipped, //TODO review: Shipped overlaps with ShipmentStatus.Shipped
    Delivered,
    Cancelled
}