using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

namespace PocztaDesktop.EndPoint
{
    class GetEndPoint
    {
        //private string _url = "http://localhost:5118/api/Items";
        public static string getEndPoint()
        {
            //creating http client
            using (HttpClient _httpclient = new HttpClient())   //httpclient bedzie dostepny tylko w nawiasach klamrowych a using samo zajmie sie zwolnieniem pamieci
            {
                //ustawienie formatu danych na json
                _httpclient.DefaultRequestHeaders.Add("Accept", "application/json");

                //Creating the request
                Task<HttpResponseMessage> httpResponse = _httpclient.GetAsync("http://localhost:5118/api/Items");
                HttpResponseMessage httpResponseMessage = httpResponse.Result;
                //return (httpResponseMessage.ToString());

                //status code
                HttpStatusCode statusCode = httpResponseMessage.StatusCode;
                //statusCode to teskt np.NOT FOUND
                //(int)statusCode zwroci nr bledu np. 404

                //response content
                HttpContent content = httpResponseMessage.Content;
                string data = content.ReadAsStringAsync().Result;
                return data;
            }
        }

    }
}
