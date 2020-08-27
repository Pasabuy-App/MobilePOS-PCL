using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using MobilePOS.Order.Struct;

namespace MobilePOS.Store
{
    public class TotalSales
    {
        #region Fields
        /// <summary>
        /// Instance of Total Sales By Store ID Class.
        /// </summary>
        private static TotalSales instance;
        public static TotalSales Instance
        {
            get
            {
                if (instance == null)
                    instance = new TotalSales();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public TotalSales()
        {
            client = new HttpClient();
        }
        #endregion
        #region Methodss
        public async void GetData(string wp_id, string session_key, string stid, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("wpid", wp_id);
            dict.Add("snky", session_key);
            dict.Add("stid", stid);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(BaseClass.BaseDomainUrl + "/mobilepos/v1/store/total/sales", content);
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
