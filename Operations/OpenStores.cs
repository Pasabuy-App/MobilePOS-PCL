using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using MobilePOS.Order.Struct;

namespace MobilePOS.Stores
{
    public class OpenStores
    {
        #region Fields
        /// <summary>
        /// Instance of List of All Open Stores Class.
        /// </summary>
        private static OpenStores instance;
        public static OpenStores Instance
        {
            get
            {
                if (instance == null)
                    instance = new OpenStores();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public OpenStores()
        {
            client = new HttpClient();
        }
        #endregion
        #region Methodss
        public async void List(string wp_id, string session_key, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("wpid", wp_id);
            dict.Add("snky", session_key);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(BaseClass.BaseDomainUrl + "/mobilepos/v1/operation/listing/openstore", content);
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
