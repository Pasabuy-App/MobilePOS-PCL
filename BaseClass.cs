
namespace MobilePOS
{
    public class MPHost
    {
        private static MPHost instance;
        public static MPHost Instance
        {
            get
            {
                if (instance == null)
                    instance = new MPHost();
                return instance;
            }
        }

        private bool isInitialized = false;
        private string baseUrl = "http://localhost";
        public string BaseDomain
        {
            get
            {
                return baseUrl + "/wp-json";
            }
        }

        public void Initialized(string url)
        {
            if (!isInitialized)
            {
                baseUrl = url;
                isInitialized = true;
            }
        }

    }
}
