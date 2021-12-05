using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shared;

namespace Consumer.AutoCollectAndRouteHandler
{
    public class MessageBootstrap
    {
        public List<string> CollectAllRoutekey()
        {
            var allTypes = Assembly.GetExecutingAssembly().GetTypes();
            var result = new List<string>();
            foreach (var type in allTypes)
            {
                if (typeof(IMessageHandler<>).IsAssignableFrom(type))
                {
                    var messageType = type.GenericTypeArguments[0];
                    result.Add(messageType.ToString());
                }
            }

            return result.Distinct().ToList();
        }
    }
}