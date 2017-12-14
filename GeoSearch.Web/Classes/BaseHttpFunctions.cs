using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;

namespace GeoSearch.Web
{
    public class BaseHttpFunctions
    {
        private const int timeoutTime = 300000;
        
        public static string HttpPost(string URI, string PostData)
        {
            string responseFromServer = null;
            try
            {
                // Create a request using a URL that can receive a post. 
                WebRequest request = WebRequest.Create(URI);
                // set timeout
                request.Timeout = timeoutTime;
                // Set the Method property of the request to POST.
                request.Method = "POST";
                // Create POST data and convert it to a byte array.
                byte[] byteArray = Encoding.UTF8.GetBytes(PostData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "text/xml";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                responseFromServer = reader.ReadToEnd();
                // Display the content.
                Console.WriteLine(responseFromServer);
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (Exception e)
            {
                e.GetType();
                return null;
            }
            return responseFromServer;
        }

        public static string HttpGet(string URI)
        {
            // Create a request using a URL that can receive a get. 
            WebRequest request = WebRequest.Create(URI);
            // set timeout
            request.Timeout = timeoutTime;
            // Set the Method property of the request to Get.
            request.Method = "GET";
            Stream dataStream = null;
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            Console.WriteLine(responseFromServer);
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }
    }
}