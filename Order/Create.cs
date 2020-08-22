using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using MobilePOS.Order.Struck;

namespace MobilePOS.Order
{
    public class Create
    {
        #region Fields
        /// <summary>
        /// Instance of Create Order Class.
        /// </summary>
        private static Create instance;
        public static Create Instance
        {
            get
            {
                if (instance == null)
                    instance = new Create();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public Create()
        {
            client = new HttpClient();
        }
        #endregion
        #region Methods
        public async void Order(string wp_id, string session_key, string stid, string pdid, string opid, string qty, string msg, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("wpid", wp_id);
            dict.Add("snky", session_key);
            dict.Add("stid", stid);
            dict.Add("pdid", pdid);
            dict.Add("opid", opid);
            dict.Add("qty", qty);
            dict.Add("msg", msg);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(BaseClass.BaseDomainUrl + "/mobilepos/v1/customer/order/insert", content);
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
