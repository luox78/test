using Shared.Context;

namespace Shared
{
    public class TransportMessage
    {
        public object Message { get; set; }

        public byte[] BodyBytes { get; set; }

        public string Topic { get; set; }

        public IFeatureCollection Features { get; set; } = new FeatureCollection();
    }
}