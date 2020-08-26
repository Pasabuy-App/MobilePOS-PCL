using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using MobilePOS.Order.Struct;

namespace MobilePOS.Store
{
    public class ByOdid
    {
        #region Fields
        /// <summary>
        /// Instance of List of Order by Order ID Class.
        /// </summary>
        private static ByOdid instance;
        public static ByOdid Instance
        {
            get
            {
                if (instance == null)
                    instance = new ByOdid();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public ByOdid()
        {
            client = new HttpClient();
        }
        #endregion
        #region Methods
        public async void Listing(string wp_id, string session_key, string odid, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("wpid", wp_id);
            dict.Add("snky", session_key);
            dict.Add("odid", odid);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(BaseClass.BaseDomainUrl + "/mobilepos/v1/store/select", content);
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                Token token = JsonConvert.DeserializeObject<Token>(result);

                bool success = token.status == "success" ? true : false;
                string data = token.status == "success" ? result : token.message;
                callback(success, data);
            }
            else
            {
                callback(false, "Network Error! Check your connection.");
            }
        }
        #endregion
    }
}
