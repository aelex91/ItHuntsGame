using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Assets.Scripts.Extensions
{
    public static class GameObjectExtensions
    {
        public static void Guard(this ILayoutElement element, string message = null)
        {
            ThrowIfNull(element, message);
        }

        public static void Guard(this object element, string gameObjectName, string message = null)
        {
            var msg = $"##{gameObjectName}## -> {message}";

            ThrowIfNull(element, msg);
        }

        private static void ThrowIfNull(object element, string message)
        {
            if (element == null)
            {
                var msg = message ?? $"element can't be null in order to run!";

                throw new ArgumentNullException($"{msg}");
            }
        }
    }
}
