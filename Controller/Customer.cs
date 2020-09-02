using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using MobilePOS.Model;

namespace MobilePOS
{
    public class Customer
    {
        #region Fields
        /// <summary>
        /// Instance of Customer Process Ordering Class with cancel, create, delete and update method.
        /// </summary>
        private static Customer instance;
        public static Customer Instance
        {
            get
            {
                if (instance == null)
                    instance = new Customer();
                return instance;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public Customer()
        {
            client = new HttpClient();
        }
        #endregion

        #region Cancel Method
        public async void Cancel(string wp_id, string session_key, string odid, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
                dict.Add("wpid", wp_id);
                dict.Add("snky", session_key);
                dict.Add("odid", odid);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(MPHost.Instance.BaseDomain + "/mobilepos/v1/customer/order/cancel", content);
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

        #region Create Method
        public async void Create(string wp_id, string session_key, string stid, string pdid, string opid, string qty, string msg, Action<bool, string> callback)
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

            var response = await client.PostAsync(MPHost.Instance.BaseDomain + "/mobilepos/v1/customer/order/insert", content);
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

        #region Delete Method
        public async void Delete(string wp_id, string session_key, string odid, string pid, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
                dict.Add("wpid", wp_id);
                dict.Add("snky", session_key);
                dict.Add("odid", odid);
                dict.Add("pid", pid);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(MPHost.Instance.BaseDomain + "/mobilepos/v1/customer/order/delete", content);
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

        #region Update Method
        public async void Update(string wp_id, string session_key, string odid, string pid, string qty, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
                dict.Add("wpid", wp_id);
                dict.Add("snky", session_key);
                dict.Add("odid", odid);
                dict.Add("pid", pid);
                dict.Add("qty", qty);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(MPHost.Instance.BaseDomain + "/mobilepos/v1/customer/order/update", content);
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
