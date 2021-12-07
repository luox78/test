using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shared;

namespace Consumer
{
    public partial class DemoConsumer
    {
        string[] CollectAllRoutekey()
        {
            var allTypes = Assembly.GetExecutingAssembly().GetTypes();
            var result   = new List<string>();
            foreach (var type in allTypes)
            {
                var typeInfo = type.GetTypeInfo();
                if (typeInfo.IsClass && typeInfo.GetInterfaces()
                        .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMessageHandler<>)))
                {
                    var messageType = type.GetTypeInfo().GetInterfaces().First().GetGenericArguments()[0];
                    result.Add(messageType.ToString());
                    _types.Add(messageType.ToString(), messageType);
                }
            }

            return result.Distinct().ToArray();
        }

        public string[] FetchTopics => CollectAllRoutekey();
    }
}