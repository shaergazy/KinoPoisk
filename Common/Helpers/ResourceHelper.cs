using System.Reflection;
using System.Resources;

namespace Common.Helpers
{
    public static class ResourceHelper
    {
        private static readonly ResourceManager ResourceManager;

        static ResourceHelper()
        {
            ResourceManager = new ResourceManager("Common.Resources.MailResource", Assembly.GetExecutingAssembly());
        }

        public static string GetString(string key)
        {
            return ResourceManager.GetString(key);
        }
    }
}
