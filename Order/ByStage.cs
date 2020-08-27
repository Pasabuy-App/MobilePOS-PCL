using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using MobilePOS.Order.Struct;

namespace MobilePOS.Order
{
    public class ByStage
    {
        #region Fields
        /// <summary>
        /// Instance of List of Orders by Stage (pending, cancelled, received, completed, shipping, accepted) Class.
        /// </summary>
        private static ByStage instance;
        public static ByStage Instance
        {
            get
            {
                if (instance == null)
                    instance = new ByStage();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public ByStage()
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
            if ( stage != "")
            {
                dict.Add("stage", stage);
            }
            if (stid != "")
            {
                dict.Add("stage", stid);
            }
            if (odid != "")
            {
                dict.Add("stage", odid);
            }
            if (date != "")
            {
                dict.Add("stage", date);
            }
            if (opid != "")
            {
                dict.Add("stage", opid);
            }
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
