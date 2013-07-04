using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using System.Configuration;
using System.Globalization;

namespace CTCT.Util
{
    /// <summary>
    /// Class implementation of REST client.
    /// </summary>
    public class RestClient : IRestClient
    {
        /// <summary>
        /// Make an Http GET request.
        /// </summary>
        /// <param name="url">Request URL.</param>
        /// <param name="accessToken">Constant Contact OAuth2 access token</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <returns>The response body, http info, and error (if one exists).</returns>
        public CUrlResponse Get(string url, string accessToken, string apiKey)
        {
            return this.HttpRequest(url, WebRequestMethods.Http.Get, accessToken, apiKey, null);
        }

        /// <summary>
        /// Make an Http POST request.
        /// </summary>
        /// <param name="url">Request URL.</param>
        /// <param name="accessToken">Constant Contact OAuth2 access token</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="data">Data to send with request.</param>
        /// <returns>The response body, http info, and error (if one exists).</returns>
        public CUrlResponse Post(string url, string accessToken, string apiKey, string data)
        {
            return this.HttpRequest(url, WebRequestMethods.Http.Post, accessToken, apiKey, data);
        }

        /// <summary>
        /// Make an Http PUT request.
        /// </summary>
        /// <param name="url">Request URL.</param>
        /// <param name="accessToken">Constant Contact OAuth2 access token</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="data">Data to send with request.</param>
        /// <returns>The response body, http info, and error (if one exists).</returns>
        public CUrlResponse Put(string url, string accessToken, string apiKey, string data)
        {
            return this.HttpRequest(url, WebRequestMethods.Http.Put, accessToken, apiKey, data);
        }

        /// <summary>
        /// Make an Http DELETE request.
        /// </summary>
        /// <param name="url">Request URL.</param>
        /// <param name="accessToken">Constant Contact OAuth2 access token</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <returns>The response body, http info, and error (if one exists).</returns>
        public CUrlResponse Delete(string url, string accessToken, string apiKey)
        {
            return this.HttpRequest(url, "DELETE", accessToken, apiKey, null);
        }

        private CUrlResponse HttpRequest(string url, string method, string accessToken, string apiKey, string data)
        {
            // Initialize the response
            HttpWebResponse response = null;
            string responseText = null;
            CUrlResponse urlResponse = new CUrlResponse();

            var address = url;

            if (!string.IsNullOrEmpty(apiKey))
            {
                address = string.Format("{0}{1}api_key={2}", url, url.Contains("?") ? "&" : "?", apiKey);
            }

            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
                                                                                             
            request.Method = method;
            request.ContentType = "application/json";
            request.Accept = "application/json";
            // Add token as HTTP header
            request.Headers.Add("Authorization", "Bearer " + accessToken);
            
            if (data != null)
            {
                // Convert the request contents to a byte array and include it
                byte[] requestBodyBytes = System.Text.Encoding.UTF8.GetBytes(data);
                request.GetRequestStream().Write(requestBodyBytes, 0, requestBodyBytes.Length);
            }

            // Now try to send the request
            try
            {
                response = request.GetResponse() as HttpWebResponse;
                // Expect the unexpected
                if (request.HaveResponse == true && response == null)
                {
                    throw new WebException("Response was not returned or is null");
                }
                urlResponse.StatusCode = response.StatusCode;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new WebException("Response with status: " + response.StatusCode + " " + response.StatusDescription);
                }
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    response = (HttpWebResponse)e.Response;
                    urlResponse.IsError = true;
                }
            }
            finally
            {
                if (response != null)
                {
                    // Get the response content
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseText = reader.ReadToEnd();
                    }
                    response.Close();
                    if (urlResponse.IsError && responseText.Contains("error_message"))
                    {
                        urlResponse.Info = CUrlRequestError.FromJSON<IList<CUrlRequestError>>(responseText);
                    }
                    else
                    {
                        urlResponse.Body = responseText;
                    }
                }
            }

            return urlResponse;
        }

        /// <summary>
        /// Post a multipart Http request.
        /// </summary>
        /// <param name="url">Request URL.</param>
        /// <param name="accessToken">Constant Contact OAuth2 access token.</param>
        /// <param name="apiKey">The API key for the application.</param>
        /// <param name="fileToUpload">The file to be uploaded.</param>
        /// <param name="extraParams">Extra parameters to be sent with the request.</param>
        /// <returns>The response body, http info, and error (if one exists).</returns>
        public CUrlResponse HttpPostMultipart(string url, string accessToken, string apiKey, string fileToUpload, NameValueCollection extraParams)
        {
            // Initialize the response
            HttpWebResponse response = null;
            string responseText = null;
            CUrlResponse urlResponse = new CUrlResponse();
            string address = url;
            byte[] buffer;

            if (!String.IsNullOrEmpty(apiKey))
            {
                address = String.Format("{0}{1}api_key={2}", url, url.Contains("?") ? "&" : "?", apiKey);
            }
            
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Method = "POST";
            request.Accept = "application/json";
            
            // Request content type
            var boundary = String.Concat("---------------------------", DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo));
            request.ContentType = String.Concat("multipart/form-data; boundary=", boundary);
            boundary = String.Concat("--", boundary);
            
            // Add token as HTTP header
            request.Headers.Add("Authorization", "Bearer " + accessToken);

            using (Stream requestStream = request.GetRequestStream())
            {
                // Add multipart parameters
                foreach (string name in extraParams.Keys)
                {
                    buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(String.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", name, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(extraParams[name] + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                }

                // Add multipart file
                buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.UTF8.GetBytes(String.Format("Content-Disposition: form-data; name=\"data\" {0}", Environment.NewLine));
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.ASCII.GetBytes(String.Format("Content-Type: text/{0}{1}{1}", Path.GetExtension(fileToUpload).Substring(1), Environment.NewLine));
                requestStream.Write(buffer, 0, buffer.Length);
                using (FileStream file = File.OpenRead(fileToUpload))
                {
                    file.CopyTo(requestStream);
                    buffer = Encoding.ASCII.GetBytes(Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                }

                // Add multipart end
                var boundaryBuffer = Encoding.ASCII.GetBytes(String.Concat(boundary, "--"));
                requestStream.Write(boundaryBuffer, 0, boundaryBuffer.Length);
            }

            // Now try to send the request
            try
            {
                response = request.GetResponse() as HttpWebResponse;
                // Expect the unexpected
                if (request.HaveResponse == true && response == null)
                {
                    throw new WebException("Response was not returned or is null");
                }
                urlResponse.StatusCode = response.StatusCode;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new WebException("Response with status: " + response.StatusCode + " " + response.StatusDescription);
                }
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    response = (HttpWebResponse)e.Response;
                    urlResponse.IsError = true;
                }
            }
            finally
            {
                if (response != null)
                {
                    // Get the response content
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseText = reader.ReadToEnd();
                    }
                    response.Close();
                    if (urlResponse.IsError && responseText.Contains("error_message"))
                    {
                        urlResponse.Info = CUrlRequestError.FromJSON<IList<CUrlRequestError>>(responseText);
                    }
                    else
                    {
                        urlResponse.Body = responseText;
                    }
                }
            }

            return urlResponse;
        }
    }
}
