using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Shared.Context
{
    public class FeatureCollection : IFeatureCollection
    {
        private readonly IDictionary<Type, object> _features;
        //private readonly ConcurrentDictionary<Type, object> _features;

        public FeatureCollection()
        {
            _features = new Dictionary<Type, object>();
        }

        public TFeature Get<TFeature>()
        {
            return (TFeature)_features[typeof(TFeature)];
        }

        /// <inheritdoc />
        public void Set<TFeature>(TFeature instance)
        {
            _features[typeof(TFeature)] = instance;
        }
    }
}