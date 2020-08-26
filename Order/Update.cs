using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using MobilePOS.Order.Struct;

namespace MobilePOS.Order
{
    public class Update
    {
        #region Fields
        /// <summary>
        /// Instance of Update Order Class.
        /// </summary>
        private static Update instance;
        public static Update Instance
        {
            get
            {
                if (instance == null)
                    instance = new Update();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public Update()
        {
            client = new HttpClient();
        }
        #endregion
        #region Methods
        public async void Order(string wp_id, string session_key, string odid, string pid, string qty, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("wpid", wp_id);
            dict.Add("snky", session_key);
            dict.Add("odid", odid);
            dict.Add("pid", pid);
            dict.Add("qty", qty);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(BaseClass.BaseDomainUrl + "/mobilepos/v1/customer/order/update", content);
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
