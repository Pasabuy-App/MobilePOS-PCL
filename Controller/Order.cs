using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using MobilePOS.Controller.Struct;

namespace MobilePOS.Controller
{
    public class Order
    {
        #region Fields
        /// <summary>
        /// Instance of List of Orders by Stage (pending, cancelled, received, completed, shipping, accepted), Store ID, User ID, Orer ID, Date and Operation ID Class.
        /// </summary>
        private static Order instance;
        public static Order Instance
        {
            get
            {
                if (instance == null)
                    instance = new Order();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public Order()
        {
            client = new HttpClient();
        }
        #endregion
        #region Methods
        public async void Listing(string wp_id, string session_key, string stage, string stid, string odid, string date, string opid, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("wpid", wp_id);
            dict.Add("snky", session_key);
            if (stage != "") { dict.Add("stage", stage); }
            if (stid != "") { dict.Add("stage", stid); }
            if (odid != "") { dict.Add("stage", odid); }
            if (date != "") { dict.Add("stage", date); }
            if (opid != "") { dict.Add("stage", opid); }
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(BaseClass.BaseDomainUrl + "/mobilepos/v1/order/listing", content);
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
