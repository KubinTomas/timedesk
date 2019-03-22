using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace TImeDesk.Models.Convertors
{

    public class CurrencyConvertor
    {   
        //Read data from CNB, reprase Data
        public static decimal ConvertCurrency(decimal amount, string from, string to)
        {
            if (from == to)
                return amount;

            string[] resArray = getData().Split('|');

            List<string> finalArray = new List<string>();

            //Same as switch adding currencies, 1 currency = 2 size
            string[] currencies = new string[6];

            for (int i = 0; i < resArray.Length; i++)
            {
                if (resArray[i].Contains("\n"))
                {
                    string[] tmp = resArray[i].Split('\n');
                    finalArray.Add(tmp[0]);
                }
                else
                {
                    finalArray.Add(resArray[i]);
                }
            }
            finalArray.Add("1");
            finalArray.Add("CZK");
            finalArray.Add("1");


            decimal numFrom = 1;
            decimal numTo = 1;

            for (int i = 0; i < finalArray.Count; i++)
            {
                if (from == finalArray[i])
                {
                    numFrom = decimal.Parse(finalArray[i+1]) / decimal.Parse(finalArray[i-1]);
                }


                if (to == finalArray[i])
                {

                    numTo = decimal.Parse(finalArray[i + 1]) / decimal.Parse(finalArray[i - 1]);


                }

            }
            decimal res = (numFrom / numTo) * amount;

            return res;
        }

        /// <summary>
        /// Return string of currency from CNB
        /// </summary>
        /// <returns></returns>
        private static string getData()
        {

            DateTime today = DateTime.Today;
            var URL = "http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt?date=" + today.ToString("dd.MM.yyyy");
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(URL);
            myRequest.Method = "GET";

            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string result = sr.ReadToEnd();

            sr.Close();
            myResponse.Close();

            return result;
        }
    }

}
