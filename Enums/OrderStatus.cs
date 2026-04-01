namespace FulfillmentCenter.Enums;

//TODO review: If these values are ever surfaced to clients or logs, raw enum names work, but adding [EnumMember(Value = "...")] or similar annotations is good practice for API serialization consistency
public enum OrderStatus
{
    Created = 0,
    Processing = 1,
    ReadyToShip = 2,//TODO: FROM HERE TO CREATE SHIP
    Shipped = 3, //TODO review: Shipped overlaps with ShipmentStatus.Shipped
    Delivered = 4,
    Cancelled = 5
}