namespace Shared
{
    public class CreateOrderEvent : IMessage
    {
        public string OrderId { get; set; }
        public string Key { get; set; }
    }
}