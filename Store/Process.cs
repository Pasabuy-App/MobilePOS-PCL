using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using MobilePOS.Order.Struck;

namespace MobilePOS.Store
{
    public class Process
    {
        #region Fields
        /// <summary>
        /// Instance of Process Order of Store (Received, Cancelled, Shipping) Class.
        /// </summary>
        private static Process instance;
        public static Process Instance
        {
            get
            {
                if (instance == null)
                    instance = new Process();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public Process()
        {
            client = new HttpClient();
        }
        #endregion
        #region Methods
        public async void Order(string wp_id, string session_key, string stid, string odid, string stage, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("wpid", wp_id);
            dict.Add("snky", session_key);
            dict.Add("stid", stid);
            dict.Add("odid", odid);
            dict.Add("stage", stage);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(BaseClass.BaseDomainUrl + "/mobilepos/v1/store/order/process", content);
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
