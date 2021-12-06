using Shared.Context;

namespace Shared
{
    public class TransportContext
    {
        public IFeatureCollection Features { get; set; } = new FeatureCollection();
    }
}