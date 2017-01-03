using Jabberwocky.Autofac.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Xml.Serialization;

namespace Informa.Library.Salesforce.V2
{
    public class HttpClientHelper : IHttpClientHelper
    {
        /// <summary>
        /// Holds media type header.
        /// </summary>
        private static readonly MediaTypeWithQualityHeaderValue xmlMediaTypeHeader = new MediaTypeWithQualityHeaderValue(@"application/xml");

        /// <summary>
        /// Constant for authorization scheme
        /// </summary>
        private const string AuthorizationScheme = "bearer";

        /// <summary>
        /// The time out
        /// </summary>
        private const string TimeOut = "Timeout : ";

        /// <summary>
        /// Holds instance of DelegatingHandler.
        /// </summary>
        private DelegatingHandler handler;

        /// <summary>
        /// Holds instance of HttpClientHandler.
        /// </summary>
        private HttpClientHandler httpClientHandler;

        /// <summary>
        /// Holds instance of this.httpClient.
        /// </summary>
        private HttpClient httpClient;

        /// <summary>
        /// Executes the Get request and returns the response message.
        /// </summary>
        /// <param name="url">Uri of the resource</param>
        /// <returns>Response message</returns>
        public HttpResponseMessage ExecuteGet(Uri url)
        {
            return this.ExecuteGet(url, xmlMediaTypeHeader);
        }

        /// <summary>
        /// Executes the Get request and returns the response message.
        /// </summary>
        /// <param name="url">Uri of the resource</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <returns>
        /// Response message
        /// </returns>
        public HttpResponseMessage ExecuteGet(Uri url, MediaTypeWithQualityHeaderValue mediaTypeHeader)
        {
            this.InitializeHttpClient();
            try
            {
                this.httpClient.DefaultRequestHeaders.Accept.Clear();
                this.httpClient.DefaultRequestHeaders.Accept.Add(mediaTypeHeader);
                var task = this.httpClient.GetAsync(url);
                task.Wait();
                var response = task.Result;
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                var message = response.Content.ReadAsStringAsync().Result;
                if (url != null)
                {
                    throw new ApplicationException(url + "\n" + message);
                }
                else
                {
                    throw new ApplicationException(message);
                }
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Gets the response of type T.
        /// </summary>
        /// <typeparam name="T">Type name of T</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <returns>Response of type T</returns>
        public T GetResponse<T>(Uri url)
        {
            return GetResponse<T>(url, xmlMediaTypeHeader);
        }

        /// <summary>
        /// Gets the response of type T.
        /// </summary>
        /// <typeparam name="T">Type name of T</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <returns>
        /// Response of type T
        /// </returns>
        public T GetResponse<T>(Uri url, MediaTypeWithQualityHeaderValue mediaTypeHeader)
        {
            this.InitializeHttpClient();
            try
            {
                var xmlMediaTypeFormatter = new XmlMediaTypeFormatter();
                xmlMediaTypeFormatter.UseXmlSerializer = true;
                var formatter = mediaTypeHeader == xmlMediaTypeHeader ? xmlMediaTypeFormatter : (MediaTypeFormatter)new JsonMediaTypeFormatter();
                this.httpClient.DefaultRequestHeaders.Accept.Clear();
                this.httpClient.DefaultRequestHeaders.Accept.Add(mediaTypeHeader);
                var task = this.httpClient.GetAsync(url);
                task.Wait();
                var response = task.Result;
                if (response.IsSuccessStatusCode)
                {
                    var contentTask = response.Content.ReadAsAsync<T>(new[] { formatter });
                    contentTask.Wait();
                    return contentTask.Result;
                }

                var message = response.Content.ReadAsStringAsync().Result;

                if (url != null)
                {
                    throw new ApplicationException(url + "\n" + message);
                }
                else
                {
                    throw new ApplicationException(message);
                }
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <typeparam name="T">Type name of T</typeparam>
        /// <param name="url">The URL to hit.</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <param name="millisecondsTimeout">The milliseconds timeout.</param>
        /// <returns>Response of type T</returns>
        public T GetResponse<T>(Uri url, MediaTypeWithQualityHeaderValue mediaTypeHeader, int millisecondsTimeout)
        {
            this.InitializeHttpClient();
            try
            {
                var xmlMediaTypeFormatter = new XmlMediaTypeFormatter();
                xmlMediaTypeFormatter.UseXmlSerializer = true;
                var formatter = mediaTypeHeader == xmlMediaTypeHeader ? xmlMediaTypeFormatter : (MediaTypeFormatter)new JsonMediaTypeFormatter();
                this.httpClient.DefaultRequestHeaders.Accept.Clear();
                this.httpClient.DefaultRequestHeaders.Accept.Add(mediaTypeHeader);
                var task = this.httpClient.GetAsync(url);
                var wasInTime = true;
                if (millisecondsTimeout > 0)
                {
                    wasInTime = task.Wait(millisecondsTimeout);
                }
                else
                {
                    task.Wait();
                }

                if (wasInTime)
                {
                    var response = task.Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var contentTask = response.Content.ReadAsAsync<T>(new[] { formatter });
                        contentTask.Wait();
                        return contentTask.Result;
                    }

                    var message = response.Content.ReadAsStringAsync().Result;

                    if (url != null)
                    {
                        throw new ApplicationException(url + "\n" + message);
                    }
                    else
                    {
                        throw new ApplicationException(message);
                    }
                }
                else
                {
                    throw new ApplicationException(TimeOut + url);
                }
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <typeparam name="T">Type name of T</typeparam>
        /// <param name="url">The URL of the resource</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <param name="millisecondsTimeout">The milliseconds timeout.</param>
        /// <param name="headerValue">The header value.</param>
        /// <returns>Response of type T</returns>
        public T GetResponse<T>(Uri url, MediaTypeWithQualityHeaderValue mediaTypeHeader, int? millisecondsTimeout, AuthenticationHeaderValue headerValue)
        {
            this.InitializeHttpClient();
            try
            {
                var xmlMediaTypeFormatter = new XmlMediaTypeFormatter();
                xmlMediaTypeFormatter.UseXmlSerializer = true;
                var formatter = mediaTypeHeader == xmlMediaTypeHeader ? xmlMediaTypeFormatter : (MediaTypeFormatter)new JsonMediaTypeFormatter();
                this.httpClient.DefaultRequestHeaders.Accept.Clear();
                this.httpClient.DefaultRequestHeaders.Accept.Add(mediaTypeHeader);
                this.httpClient.DefaultRequestHeaders.Authorization = headerValue;
                var task = this.httpClient.GetAsync(url);
                var wasInTime = true;
                if (millisecondsTimeout > 0)
                {
                    wasInTime = task.Wait(millisecondsTimeout.Value);
                }
                else
                {
                    task.Wait();
                }

                if (wasInTime)
                {
                    var response = task.Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var contentTask = response.Content.ReadAsAsync<T>(new[] { formatter });
                        contentTask.Wait();
                        return contentTask.Result;
                    }

                    var message = response.Content.ReadAsStringAsync().Result;
                    if (url != null)
                    {
                        throw new ApplicationException(url + "\n" + message);
                    }
                    else
                    {
                        throw new ApplicationException(message);
                    }
                }
                else
                {
                    throw new ApplicationException(TimeOut + url);
                }
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Gets the response of type T.
        /// </summary>
        /// <typeparam name="T">Type name of T</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <param name="headers">The headers.</param>
        /// <returns>
        /// Response of type T
        /// </returns>
        /// <exception cref="System.ApplicationException">
        /// </exception>
        public T GetResponse<T>(Uri url, MediaTypeWithQualityHeaderValue mediaTypeHeader, Dictionary<string, string> headers)
        {
            this.InitializeHttpClient();
            try
            {
                var xmlMediaTypeFormatter = new XmlMediaTypeFormatter();
                xmlMediaTypeFormatter.UseXmlSerializer = true;
                var formatter = mediaTypeHeader == xmlMediaTypeHeader ? xmlMediaTypeFormatter : (MediaTypeFormatter)new JsonMediaTypeFormatter();
                this.httpClient.DefaultRequestHeaders.Accept.Clear();
                this.httpClient.DefaultRequestHeaders.Accept.Add(mediaTypeHeader);
                SetRequestHeaders(headers, this.httpClient);
                var task = this.httpClient.GetAsync(url);
                task.Wait();
                var response = task.Result;
                if (response.IsSuccessStatusCode)
                {
                    var contentTask = response.Content.ReadAsAsync<T>(new[] { formatter });
                    contentTask.Wait();
                    return contentTask.Result;
                }

                var message = response.Content.ReadAsStringAsync().Result;

                if (url != null)
                {
                    throw new ApplicationException(url + "\n" + message);
                }
                else
                {
                    throw new ApplicationException(message);
                }
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Gets the response of type T.
        /// </summary>
        /// <param name="url">Uri of the resource</param>
        /// <returns>
        /// Response of type T
        /// </returns>
        public HttpResponseMessage GetResponse(Uri url)
        {
            return this.GetResponse(url, xmlMediaTypeHeader);
        }

        /// <summary>
        /// Gets the response of type T.
        /// </summary>
        /// <param name="url">Uri of the resource</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <returns>
        /// Response of type T
        /// </returns>
        /// <exception cref="System.ApplicationException">
        /// </exception>
        public HttpResponseMessage GetResponse(Uri url, MediaTypeWithQualityHeaderValue mediaTypeHeader)
        {
            this.InitializeHttpClient();
            try
            {
                this.httpClient.DefaultRequestHeaders.Accept.Clear();
                this.httpClient.DefaultRequestHeaders.Accept.Add(mediaTypeHeader);
                var task = this.httpClient.GetAsync(url);

                task.Wait();

                var response = task.Result;
                if (response.IsSuccessStatusCode)
                {
                    return new HttpResponseMessage() { Content = response.Content };
                }

                var message = response.Content.ReadAsStringAsync().Result;
                if (url != null)
                {
                    throw new ApplicationException(url + "\n" + message);
                }
                else
                {
                    throw new ApplicationException(message);
                }
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Update the request.
        /// </summary>
        /// <typeparam name="T">Type name of T</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="value">Value of type</param>
        public void PutRequest<T>(Uri url, T value)
        {
            this.PutRequest(url, value, xmlMediaTypeHeader);
        }

        /// <summary>
        /// Puts the request.
        /// </summary>
        /// <typeparam name="T">Type name of T</typeparam>
        /// <param name="url">The URL value.</param>
        /// <param name="value">The value of Type T.</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        public void PutRequest<T>(Uri url, T value, MediaTypeWithQualityHeaderValue mediaTypeHeader)
        {
            this.InitializeHttpClient();
            try
            {
                if (mediaTypeHeader != null && url != null)
                {
                    var xmlMediaTypeFormatter = new XmlMediaTypeFormatter { UseXmlSerializer = true };
                    var formatter = mediaTypeHeader == xmlMediaTypeHeader ? xmlMediaTypeFormatter : (MediaTypeFormatter)new JsonMediaTypeFormatter();
                    this.httpClient.DefaultRequestHeaders.Accept.Clear();
                    this.httpClient.DefaultRequestHeaders.Accept.Add(mediaTypeHeader);
                    var task = this.httpClient.PutAsync(url.ToString(), value, formatter, mediaTypeHeader.MediaType);
                    task.Wait();
                    var response = task.Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return;
                    }

                    var message = response.Content.ReadAsStringAsync().Result;
                    throw new ApplicationException(url + "\n" + message);
                }
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Puts the request.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="url">The URL value.</param>
        /// <param name="value">The value of Type T.</param>
        /// <returns>
        /// Http response message
        /// </returns>
        public T1 PutRequest<T1, T2>(Uri url, T2 value)
        {
            this.InitializeHttpClient();
            try
            {
                if (url != null)
                {
                    var formatter = new XmlMediaTypeFormatter { UseXmlSerializer = true };
                    this.httpClient.DefaultRequestHeaders.Accept.Clear();
                    this.httpClient.DefaultRequestHeaders.Accept.Add(xmlMediaTypeHeader);
                    var task = this.httpClient.PutAsync(url.ToString(), value, formatter, xmlMediaTypeHeader.MediaType);
                    task.Wait();
                    var response = task.Result;
                    if (response.IsSuccessStatusCode)
                    {
                        if (typeof(T1) == typeof(HttpResponseMessage))
                        {
                            object res = response;
                            return (T1)res;
                        }
                        else
                        {
                            var t = response.Content.ReadAsAsync<T1>(new[] { formatter });
                            t.Wait();
                            return t.Result;
                        }
                    }

                    var message = response.Content.ReadAsStringAsync().Result;
                    throw new ApplicationException(url + "\n" + message);
                }

                return default(T1);
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Puts the request.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="url">The URL value.</param>
        /// <param name="value">The value of Type T.</param>
        /// <param name="millisecondsTimeout">The milliseconds timeout.</param>
        /// <returns>
        /// Http response message
        /// </returns>
        /// <exception cref="System.ApplicationException"></exception>
        public T1 PutRequest<T1, T2>(Uri url, T2 value, int? millisecondsTimeout)
        {
            this.InitializeHttpClient();
            try
            {
                if (url != null)
                {
                    var formatter = new XmlMediaTypeFormatter { UseXmlSerializer = true };
                    this.httpClient.DefaultRequestHeaders.Accept.Clear();
                    this.httpClient.DefaultRequestHeaders.Accept.Add(xmlMediaTypeHeader);
                    var task = this.httpClient.PutAsync(url.ToString(), value, formatter, xmlMediaTypeHeader.MediaType);
                    var wasInTime = true;
                    if (millisecondsTimeout > 0)
                    {
                        wasInTime = task.Wait(millisecondsTimeout.Value);
                    }
                    else
                    {
                        task.Wait();
                    }

                    if (wasInTime)
                    {
                        var response = task.Result;
                        if (response.IsSuccessStatusCode)
                        {
                            if (typeof(T1) == typeof(HttpResponseMessage))
                            {
                                object res = response;
                                return (T1)res;
                            }
                            else
                            {
                                var t = response.Content.ReadAsAsync<T1>(new[] { formatter });
                                t.Wait();
                                return t.Result;
                            }
                        }

                        var message = response.Content.ReadAsStringAsync().Result;
                        throw new ApplicationException(url + "\n" + message);
                    }
                    else
                    {
                        throw new ApplicationException(TimeOut + url);
                    }
                }

                return default(T1);
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Puts the request.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="url">The URL value.</param>
        /// <param name="value">The value.</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <returns>
        /// Http response message
        /// </returns>
        public T1 PutRequest<T1, T2>(Uri url, T2 value, MediaTypeWithQualityHeaderValue mediaTypeHeader)
        {
            this.InitializeHttpClient();
            try
            {
                if (mediaTypeHeader != null && url != null)
                {
                    var xmlMediaTypeFormatter = new XmlMediaTypeFormatter { UseXmlSerializer = true };
                    var formatter = mediaTypeHeader == xmlMediaTypeHeader ? xmlMediaTypeFormatter : (MediaTypeFormatter)new JsonMediaTypeFormatter();
                    this.httpClient.DefaultRequestHeaders.Accept.Clear();
                    this.httpClient.DefaultRequestHeaders.Accept.Add(mediaTypeHeader);
                    var task = this.httpClient.PutAsync(url.ToString(), value, formatter, mediaTypeHeader.MediaType);
                    task.Wait();
                    var response = task.Result;
                    if (response.IsSuccessStatusCode)
                    {
                        if (typeof(T1) == typeof(HttpResponseMessage))
                        {
                            object res = response;
                            return (T1)res;
                        }
                        else
                        {
                            var t = response.Content.ReadAsAsync<T1>(new[] { formatter });
                            t.Wait();
                            return t.Result;
                        }
                    }

                    var message = response.Content.ReadAsStringAsync().Result;
                    throw new ApplicationException(url + "\n" + message);
                }

                return default(T1);
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Puts the request.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="url">The URL value.</param>
        /// <param name="value">The value.</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <param name="millisecondsTimeout">The milliseconds timeout.</param>
        /// <returns>
        /// Http response message
        /// </returns>
        /// <exception cref="System.ApplicationException"></exception>
        public T1 PutRequest<T1, T2>(Uri url, T2 value, MediaTypeWithQualityHeaderValue mediaTypeHeader, int? millisecondsTimeout)
        {
            this.InitializeHttpClient();
            try
            {
                if (mediaTypeHeader != null && url != null)
                {
                    var xmlMediaTypeFormatter = new XmlMediaTypeFormatter { UseXmlSerializer = true };
                    var formatter = mediaTypeHeader == xmlMediaTypeHeader ? xmlMediaTypeFormatter : (MediaTypeFormatter)new JsonMediaTypeFormatter();
                    this.httpClient.DefaultRequestHeaders.Accept.Clear();
                    this.httpClient.DefaultRequestHeaders.Accept.Add(mediaTypeHeader);
                    var task = this.httpClient.PutAsync(url.ToString(), value, formatter, mediaTypeHeader.MediaType);
                    var wasInTime = true;
                    if (millisecondsTimeout > 0)
                    {
                        wasInTime = task.Wait(millisecondsTimeout.Value);
                    }
                    else
                    {
                        task.Wait();
                    }

                    if (wasInTime)
                    {
                        var response = task.Result;
                        if (response.IsSuccessStatusCode)
                        {
                            if (typeof(T1) == typeof(HttpResponseMessage))
                            {
                                object res = response;
                                return (T1)res;
                            }
                            else
                            {
                                var t = response.Content.ReadAsAsync<T1>(new[] { formatter });
                                t.Wait();
                                return t.Result;
                            }
                        }

                        var message = response.Content.ReadAsStringAsync().Result;
                        throw new ApplicationException(url + "\n" + message);
                    }
                    else
                    {
                        throw new ApplicationException(TimeOut + url);
                    }
                }

                return default(T1);
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Puts the request.
        /// </summary>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="url">The URL value.</param>
        /// <param name="value">The value of Type T.</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <returns>
        /// Http response message
        /// </returns>
        public HttpResponseMessage PutResponseRequest<T2>(Uri url, T2 value, MediaTypeWithQualityHeaderValue mediaTypeHeader)
        {
            this.InitializeHttpClient();
            try
            {
                if (mediaTypeHeader != null && url != null)
                {
                    var xmlMediaTypeFormatter = new XmlMediaTypeFormatter { UseXmlSerializer = true };
                    var formatter = mediaTypeHeader.MediaType == xmlMediaTypeHeader.MediaType ? xmlMediaTypeFormatter : (MediaTypeFormatter)new JsonMediaTypeFormatter();
                    this.httpClient.DefaultRequestHeaders.Accept.Clear();
                    this.httpClient.DefaultRequestHeaders.Accept.Add(xmlMediaTypeHeader);
                    var task = this.httpClient.PutAsync(url.ToString(), value, formatter, mediaTypeHeader.MediaType);
                    task.Wait();
                    var response = task.Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return new HttpResponseMessage() { Content = response.Content };
                    }

                    var message = response.Content.ReadAsStringAsync().Result;
                    throw new ApplicationException(url + "\n" + message);
                }

                return null;
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Puts the response request.
        /// </summary>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="url">The URL value.</param>
        /// <param name="value">The value of Type T.</param>
        /// <param name="contentMediaTypeHeader">The content media type header.</param>
        /// <param name="acceptMediaTypeHeader">The accept media type header.</param>
        /// <returns>
        /// HttpResponse Message
        /// </returns>
        public HttpResponseMessage PutResponseRequest<T2>(Uri url, T2 value, MediaTypeWithQualityHeaderValue contentMediaTypeHeader, MediaTypeWithQualityHeaderValue acceptMediaTypeHeader)
        {
            this.InitializeHttpClient();
            try
            {
                if (contentMediaTypeHeader != null && acceptMediaTypeHeader != null && url != null)
                {
                    var xmlMediaTypeFormatter = new XmlMediaTypeFormatter { UseXmlSerializer = true };
                    var formatter = contentMediaTypeHeader.MediaType == xmlMediaTypeHeader.MediaType ? xmlMediaTypeFormatter : (MediaTypeFormatter)new JsonMediaTypeFormatter();
                    this.httpClient.DefaultRequestHeaders.Accept.Clear();
                    this.httpClient.DefaultRequestHeaders.Accept.Add(acceptMediaTypeHeader);
                    var task = this.httpClient.PutAsync(url.ToString(), value, formatter, contentMediaTypeHeader.MediaType);
                    task.Wait();
                    var response = task.Result;
                    return response;
                }

                return null;
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Puts the response request.
        /// </summary>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="url">The URL value.</param>
        /// <param name="value">The value of Type T.</param>
        /// <param name="contentMediaTypeHeader">The content media type header.</param>
        /// <param name="acceptMediaTypeHeader">The accept media type header.</param>
        /// <param name="headers">The headers.</param>
        /// <returns>
        /// HttpResponse Message
        /// </returns>
        public HttpResponseMessage PutResponseRequest<T2>(Uri url, T2 value, MediaTypeWithQualityHeaderValue contentMediaTypeHeader, MediaTypeWithQualityHeaderValue acceptMediaTypeHeader, Dictionary<string, string> headers)
        {
            this.InitializeHttpClient();
            try
            {
                if (contentMediaTypeHeader != null && acceptMediaTypeHeader != null && url != null)
                {
                    if (headers != null)
                    {
                        this.httpClient.DefaultRequestHeaders.Clear();
                        foreach (var header in headers)
                        {
                            this.httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }

                    var xmlMediaTypeFormatter = new XmlMediaTypeFormatter { UseXmlSerializer = true };
                    var formatter = contentMediaTypeHeader.MediaType == xmlMediaTypeHeader.MediaType ? xmlMediaTypeFormatter : (MediaTypeFormatter)new JsonMediaTypeFormatter();
                    this.httpClient.DefaultRequestHeaders.Accept.Clear();
                    this.httpClient.DefaultRequestHeaders.Accept.Add(acceptMediaTypeHeader);
                    var task = this.httpClient.PutAsync(url.ToString(), value, formatter, contentMediaTypeHeader.MediaType);
                    task.Wait();
                    var response = task.Result;
                    return response;
                }

                return null;
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Posts the request.
        /// </summary>
        /// <typeparam name="T">Type name of T</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="value">Value of type T</param>
        public void PostRequest<T>(Uri url, T value)
        {
            this.PostRequest(url, value, xmlMediaTypeHeader);
        }

        /// <summary>
        /// Posts the request.
        /// </summary>
        /// <typeparam name="T">Type name of T</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="value">Value of type T</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        public void PostRequest<T>(Uri url, T value, MediaTypeWithQualityHeaderValue mediaTypeHeader)
        {
            this.InitializeHttpClient();
            try
            {
                if (mediaTypeHeader != null && url != null)
                {
                    var xmlMediaTypeFormatter = new XmlMediaTypeFormatter { UseXmlSerializer = true };
                    var formatter = mediaTypeHeader == xmlMediaTypeHeader ? xmlMediaTypeFormatter : (MediaTypeFormatter)new JsonMediaTypeFormatter();
                    this.httpClient.DefaultRequestHeaders.Accept.Clear();
                    this.httpClient.DefaultRequestHeaders.Accept.Add(mediaTypeHeader);
                    var task = this.httpClient.PostAsync(url.ToString(), value, formatter, mediaTypeHeader.MediaType);
                    task.Wait();
                    var response = task.Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return;
                    }

                    var message = response.Content.ReadAsStringAsync().Result;
                    throw new ApplicationException(url + "\n" + message);
                }
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Deletes the request.
        /// </summary>
        /// <param name="url">Uri of the resource</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        public void DeleteRequest(Uri url, MediaTypeWithQualityHeaderValue mediaTypeHeader)
        {
            this.DeleteRequest(url, 0, mediaTypeHeader);
        }

        /// <summary>
        /// Deletes the request.
        /// </summary>
        /// <param name="url">Uri of the resource</param>
        public void DeleteRequest(Uri url)
        {
            this.DeleteRequest(url, 0, xmlMediaTypeHeader);
        }

        /// <summary>
        /// Deletes the request.
        /// </summary>
        /// <param name="url">Uri of the resource</param>
        /// <param name="timeoutDuration">Duration of the timeout.</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        public void DeleteRequest(Uri url, int timeoutDuration, MediaTypeWithQualityHeaderValue mediaTypeHeader)
        {
            this.InitializeHttpClient();
            try
            {
                this.httpClient.DefaultRequestHeaders.Accept.Clear();
                this.httpClient.DefaultRequestHeaders.Accept.Add(mediaTypeHeader);
                var task = this.httpClient.DeleteAsync(url);
                var wasInTime = true;
                if (timeoutDuration > 0)
                {
                    wasInTime = task.Wait(timeoutDuration);
                }
                else
                {
                    task.Wait();
                }

                if (wasInTime)
                {
                    var response = task.Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return;
                    }

                    var message = response.Content.ReadAsStringAsync().Result;
                    if (url != null)
                    {
                        throw new ApplicationException(url + "\n" + message);
                    }
                    else
                    {
                        throw new ApplicationException(message);
                    }
                }
                else
                {
                    throw new ApplicationException(TimeOut + url);
                }
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Posts the request.
        /// </summary>
        /// <typeparam name="T1">Type name of T1</typeparam>
        /// <typeparam name="T2">Type name of T2</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="value">Value of type T2</param>
        /// <returns>
        /// Http response message
        /// </returns>
        public T1 PostData<T1, T2>(Uri url, T2 value)
        {
            return PostData<T1, T2>(url, value, xmlMediaTypeHeader);
        }

        /// <summary>
        /// Posts the request.
        /// </summary>
        /// <typeparam name="T1">Type name of T1</typeparam>
        /// <typeparam name="T2">Type name of T2</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="value">Value of type T2</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <returns>
        /// Http response message
        /// </returns>
        public T1 PostData<T1, T2>(Uri url, T2 value, MediaTypeWithQualityHeaderValue mediaTypeHeader)
        {
            this.InitializeHttpClient();
            try
            {
                if (mediaTypeHeader != null && url != null)
                {
                    var xmlMediaTypeFormatter = new XmlMediaTypeFormatter { UseXmlSerializer = true };
                    var formatter = mediaTypeHeader.MediaType == xmlMediaTypeHeader.MediaType ? xmlMediaTypeFormatter : (MediaTypeFormatter)new JsonMediaTypeFormatter();
                    this.httpClient.DefaultRequestHeaders.Accept.Clear();
                    this.httpClient.DefaultRequestHeaders.Accept.Add(mediaTypeHeader);
                    var task = this.httpClient.PostAsync(url.ToString(), value, formatter, mediaTypeHeader.MediaType);
                    task.Wait();
                    var response = task.Result;
                    if (response.IsSuccessStatusCode)
                    {
                        if (typeof(T1) == typeof(HttpResponseMessage))
                        {
                            object res = response;
                            return (T1)res;
                        }
                        else
                        {
                            var t = response.Content.ReadAsAsync<T1>(new[] { formatter });
                            t.Wait();
                            return t.Result;
                        }
                    }

                    var message = response.Content.ReadAsStringAsync().Result;
                    throw new ApplicationException(url + "\n" + message);
                }

                return default(T1);
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Posts the request.
        /// </summary>
        /// <typeparam name="T1">Type name of T1</typeparam>
        /// <typeparam name="T2">Type name of T2</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="value">Value of type T2</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <param name="headerValue">The header value.</param>
        /// <returns>
        /// Http response message
        /// </returns>
        /// <exception cref="System.ApplicationException"></exception>
        public T1 PostData<T1, T2>(Uri url, T2 value, MediaTypeWithQualityHeaderValue mediaTypeHeader, AuthenticationHeaderValue headerValue)
        {
            this.InitializeHttpClient();
            try
            {
                if (mediaTypeHeader != null && url != null)
                {
                    var xmlMediaTypeFormatter = new XmlMediaTypeFormatter { UseXmlSerializer = true };
                    var formatter = mediaTypeHeader.MediaType == xmlMediaTypeHeader.MediaType ? xmlMediaTypeFormatter : (MediaTypeFormatter)new JsonMediaTypeFormatter();
                    this.httpClient.DefaultRequestHeaders.Accept.Clear();
                    this.httpClient.DefaultRequestHeaders.Accept.Add(mediaTypeHeader);
                    this.httpClient.DefaultRequestHeaders.Authorization = headerValue;
                    var task = this.httpClient.PostAsync(url.ToString(), value, formatter, mediaTypeHeader.MediaType);
                    task.Wait();
                    var response = task.Result;
                    if (response.IsSuccessStatusCode)
                    {
                        if (typeof(T1) == typeof(HttpResponseMessage))
                        {
                            object res = response;
                            return (T1)res;
                        }
                        else
                        {
                            var t = response.Content.ReadAsAsync<T1>(new[] { formatter });
                            t.Wait();
                            return t.Result;
                        }
                    }

                    var message = response.Content.ReadAsStringAsync().Result;
                    throw new ApplicationException(url + "\n" + message);
                }

                return default(T1);
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Posts the request.
        /// </summary>
        /// <typeparam name="T1">Type name of T1</typeparam>
        /// <typeparam name="T2">Type name of T2</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="value">Value of type T2</param>
        /// <param name="contentMediaTypeHeader">The content media type header.</param>
        /// <param name="acceptMediaTypeHeader">The accept media type header.</param>
        /// <returns>
        /// Http response message
        /// </returns>
        public T1 PostData<T1, T2>(Uri url, T2 value, MediaTypeWithQualityHeaderValue contentMediaTypeHeader, MediaTypeWithQualityHeaderValue acceptMediaTypeHeader)
        {
            this.InitializeHttpClient();
            try
            {
                if (contentMediaTypeHeader != null && acceptMediaTypeHeader != null && url != null)
                {
                    var xmlMediaTypeFormatter = new XmlMediaTypeFormatter { UseXmlSerializer = true };
                    var formatter = contentMediaTypeHeader.MediaType == xmlMediaTypeHeader.MediaType ? xmlMediaTypeFormatter : (MediaTypeFormatter)new JsonMediaTypeFormatter();
                    this.httpClient.DefaultRequestHeaders.Accept.Clear();
                    this.httpClient.DefaultRequestHeaders.Accept.Add(acceptMediaTypeHeader);
                    var task = this.httpClient.PostAsync(url.ToString(), value, formatter, contentMediaTypeHeader.MediaType);
                    task.Wait();
                    var response = task.Result;
                    if (response.IsSuccessStatusCode)
                    {
                        if (typeof(T1) == typeof(HttpResponseMessage))
                        {
                            object res = response;
                            return (T1)res;
                        }
                        else
                        {
                            var t = response.Content.ReadAsAsync<T1>(new[] { formatter });
                            t.Wait();
                            return t.Result;
                        }
                    }

                    var message = response.Content.ReadAsStringAsync().Result;
                    throw new ApplicationException(url + "\n" + message);
                }

                return default(T1);
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Posts the request.
        /// </summary>
        /// <typeparam name="T2">Type name of T2</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="value">Value of type T2</param>
        /// <returns>
        /// Http response message
        /// </returns>
        public HttpResponseMessage PostResponseData<T2>(Uri url, T2 value)
        {
            return PostResponseData<T2>(url, value, xmlMediaTypeHeader);
        }

        /// <summary>
        /// Posts the request.
        /// </summary>
        /// <typeparam name="T2">Type name of T2</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="value">Value of type T2</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <returns>
        /// Http response message
        /// </returns>
        public HttpResponseMessage PostResponseData<T2>(Uri url, T2 value, MediaTypeWithQualityHeaderValue mediaTypeHeader)
        {
            this.InitializeHttpClient();
            try
            {
                if (mediaTypeHeader != null && url != null)
                {
                    var xmlMediaTypeFormatter = new XmlMediaTypeFormatter { UseXmlSerializer = true };
                    var formatter = mediaTypeHeader.MediaType == xmlMediaTypeHeader.MediaType ? xmlMediaTypeFormatter : (MediaTypeFormatter)new JsonMediaTypeFormatter();
                    this.httpClient.DefaultRequestHeaders.Accept.Clear();
                    this.httpClient.DefaultRequestHeaders.Accept.Add(mediaTypeHeader);
                    var task = this.httpClient.PostAsync(url.ToString(), value, formatter, mediaTypeHeader.MediaType);
                    task.Wait();
                    var response = task.Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return new HttpResponseMessage() { Content = response.Content };
                    }

                    var message = response.Content.ReadAsStringAsync().Result;
                    throw new ApplicationException(url + "\n" + message);
                }

                return null;
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Posts the request.
        /// </summary>
        /// <typeparam name="T2">Type name of T2</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="value">Value of type T2</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <param name="millisecondsTimeout">The milliseconds timeout.</param>
        /// <returns>
        /// Http response message
        /// </returns>
        /// <exception cref="System.ApplicationException"></exception>
        public HttpResponseMessage PostResponseData<T2>(Uri url, T2 value, MediaTypeWithQualityHeaderValue mediaTypeHeader, int? millisecondsTimeout)
        {
            this.InitializeHttpClient();
            try
            {
                if (mediaTypeHeader != null && url != null)
                {
                    var xmlMediaTypeFormatter = new XmlMediaTypeFormatter { UseXmlSerializer = true };
                    var formatter = mediaTypeHeader.MediaType == xmlMediaTypeHeader.MediaType ? xmlMediaTypeFormatter : (MediaTypeFormatter)new JsonMediaTypeFormatter();
                    this.httpClient.DefaultRequestHeaders.Accept.Clear();
                    this.httpClient.DefaultRequestHeaders.Accept.Add(mediaTypeHeader);
                    var task = this.httpClient.PostAsync(url.ToString(), value, formatter, mediaTypeHeader.MediaType);
                    var wasInTime = true;
                    if (millisecondsTimeout > 0)
                    {
                        wasInTime = task.Wait(millisecondsTimeout.Value);
                    }
                    else
                    {
                        task.Wait();
                    }

                    if (wasInTime)
                    {
                        var response = task.Result;
                        if (response.IsSuccessStatusCode)
                        {
                            return new HttpResponseMessage() { Content = response.Content };
                        }

                        var message = response.Content.ReadAsStringAsync().Result;
                        throw new ApplicationException(url + "\n" + message);
                    }
                    else
                    {
                        throw new ApplicationException(TimeOut + url);
                    }
                }

                return null;
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Posts the request.
        /// </summary>
        /// <typeparam name="T2">Type name of T2</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="value">Value of type T2</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <param name="millisecondsTimeout">The milliseconds timeout.</param>
        /// <param name="headerValue">The header value.</param>
        /// <returns>
        /// Http response message
        /// </returns>
        /// <exception cref="System.ApplicationException">
        /// </exception>
        public HttpResponseMessage PostResponseData<T2>(Uri url, T2 value, MediaTypeWithQualityHeaderValue mediaTypeHeader, int? millisecondsTimeout, AuthenticationHeaderValue headerValue)
        {
            this.InitializeHttpClient();
            try
            {
                if (mediaTypeHeader != null && url != null)
                {
                    var xmlMediaTypeFormatter = new XmlMediaTypeFormatter { UseXmlSerializer = true };
                    var formatter = mediaTypeHeader.MediaType == xmlMediaTypeHeader.MediaType ? xmlMediaTypeFormatter : (MediaTypeFormatter)new JsonMediaTypeFormatter();
                    this.httpClient.DefaultRequestHeaders.Accept.Clear();
                    this.httpClient.DefaultRequestHeaders.Accept.Add(mediaTypeHeader);
                    this.httpClient.DefaultRequestHeaders.Authorization = headerValue;
                    var task = this.httpClient.PostAsync(url.ToString(), value, formatter, mediaTypeHeader.MediaType);
                    var wasInTime = true;
                    if (millisecondsTimeout > 0)
                    {
                        wasInTime = task.Wait(millisecondsTimeout.Value);
                    }
                    else
                    {
                        task.Wait();
                    }

                    if (wasInTime)
                    {
                        var response = task.Result;
                        if (response.IsSuccessStatusCode)
                        {
                            return new HttpResponseMessage() { Content = response.Content };
                        }

                        var message = response.Content.ReadAsStringAsync().Result;
                        throw new ApplicationException(url + "\n" + message);
                    }
                    else
                    {
                        throw new ApplicationException(TimeOut + url);
                    }
                }

                return null;
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Posts the request.
        /// </summary>
        /// <typeparam name="T1">Type name of T1</typeparam>
        /// <typeparam name="T2">Type name of T2</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="value">Value of type T2</param>
        /// <param name="headerParameter">The authentication header value.</param>
        /// <param name="headers">The headers.</param>
        /// <returns>
        /// Http response message
        /// </returns>
        public T1 PostData<T1, T2>(Uri url, T2 value, string headerParameter, Dictionary<string, string> headers)
        {
            this.InitializeHttpClient();
            try
            {
                if (url != null)
                {
                    this.httpClient.DefaultRequestHeaders.Accept.Clear();
                    this.httpClient.DefaultRequestHeaders.Accept.Add(xmlMediaTypeHeader);
                    this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, headerParameter);
                    var xmlMediaTypeFormatter = new XmlMediaTypeFormatter { UseXmlSerializer = true };
                    if (headers != null)
                    {
                        this.httpClient.DefaultRequestHeaders.Clear();
                        foreach (var header in headers)
                        {
                            this.httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }

                    var task = this.httpClient.PostAsync(url.ToString(), value, xmlMediaTypeFormatter, xmlMediaTypeHeader.MediaType);
                    task.Wait();
                    var response = task.Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var t = response.Content.ReadAsAsync<T1>(new[] { xmlMediaTypeFormatter });
                        t.Wait();
                        return t.Result;
                    }

                    var message = response.Content.ReadAsStringAsync().Result;
                    throw new ApplicationException(url + "\n" + message);
                }

                return default(T1);
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Generic post request.
        /// </summary>
        /// <typeparam name="T">Response Type</typeparam>
        /// <param name="url">Uri of the resource.</param>
        /// <param name="nameValueCollection">The name value collection.</param>
        /// <returns>
        /// Http response message
        /// </returns>
        public T PostData<T>(Uri url, IEnumerable<KeyValuePair<string, string>> nameValueCollection)
        {
            this.InitializeHttpClient();
            try
            {
                using (var content = new FormUrlEncodedContent(nameValueCollection))
                {
                    var xmlMediaTypeFormatter = new XmlMediaTypeFormatter { UseXmlSerializer = true };
                    var task = this.httpClient.PostAsync(url, content);
                    task.Wait();
                    var response = task.Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var t = response.Content.ReadAsAsync<T>();
                        t.Wait();
                        return t.Result;
                    }

                    var message = response.Content.ReadAsStringAsync().Result;
                    if (url != null)
                    {
                        throw new ApplicationException(url + "\n" + message);
                    }
                    else
                    {
                        throw new ApplicationException(message);
                    }
                }
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Generic post request.
        /// </summary>
        /// <typeparam name="T">Response Type</typeparam>
        /// <param name="url">Uri of the resource.</param>
        /// <param name="value">The value.</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <returns>
        /// Http response message
        /// </returns>
        public string PostDataValue<T>(Uri url, T value, MediaTypeWithQualityHeaderValue mediaTypeHeader)
        {
            this.InitializeHttpClient();
            try
            {
                if (mediaTypeHeader != null && url != null)
                {
                    var xmlMediaTypeFormatter = new XmlMediaTypeFormatter { UseXmlSerializer = true };
                    var formatter = mediaTypeHeader == xmlMediaTypeHeader ? xmlMediaTypeFormatter : (MediaTypeFormatter)new JsonMediaTypeFormatter();
                    this.httpClient.DefaultRequestHeaders.Accept.Clear();
                    this.httpClient.DefaultRequestHeaders.Accept.Add(mediaTypeHeader);
                    var task = this.httpClient.PostAsync(url.ToString(), value, formatter, mediaTypeHeader.MediaType);
                    task.Wait();
                    var response = task.Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var t = response.Content.ReadAsStringAsync();
                        t.Wait();
                        return response.Headers.Location.OriginalString;
                    }

                    var message = response.Content.ReadAsStringAsync().Result;
                    throw new ApplicationException(url + "\n" + message);
                }

                return null;
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Gets the data response.
        /// </summary>
        /// <typeparam name="T">Type name of T</typeparam>
        /// <param name="url">The Uri of the resource</param>
        /// <param name="authenticationHeaderValue">The authentication header value.</param>
        /// <param name="headers">The headers.</param>
        /// <returns>Response of type T</returns>
        public T GetDataResponse<T>(Uri url, AuthenticationHeaderValue authenticationHeaderValue, Dictionary<string, string> headers)
        {
            this.InitializeHttpClient();
            try
            {
                T result = default(T);
                if (url != null)
                {
                    var formatter = SetMediaFormatter(authenticationHeaderValue, this.httpClient);
                    SetRequestHeaders(headers, this.httpClient);

                    var task = this.httpClient.GetAsync(url);
                    task.Wait();
                    var response = task.Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseobject = response.Content.ReadAsStringAsync().Result;
                        XmlRootAttribute xRoot = new XmlRootAttribute();
                        xRoot.ElementName = "user";
                        xRoot.IsNullable = true;

                        XmlSerializer serializer = new XmlSerializer(typeof(T), xRoot);
                        T output;
                        using (TextReader reader = new StringReader(responseobject))
                        {
                            output = (T)serializer.Deserialize(reader);
                        }

                        return output;
                        //return ReturnTaskResult<T>(formatter, response);
                    }

                    SetResponseMessage(response);
                }

                return result;
            }
            finally
            {
                this.DisposeHttpClient();
            }
        }

        /// <summary>
        /// Disposes the HTTP client.
        /// </summary>
        public void DisposeHttpClient()
        {
            if (this.httpClient != null)
            {
                this.httpClient.Dispose();
            }

            if (this.handler != null)
            {
                this.handler.Dispose();
            }

            if (this.httpClientHandler != null)
            {
                this.httpClientHandler.Dispose();
            }
        }

        #region Private Methods

        /// <summary>
        /// Sets the media formatter.
        /// </summary>
        /// <param name="authenticationHeaderValue">The authentication header value.</param>
        /// <param name="httpClient">The HTTP client.</param>
        /// <returns>Media Formatter</returns>
        private static MediaTypeFormatter SetMediaFormatter(AuthenticationHeaderValue authenticationHeaderValue, HttpClient httpClient)
        {
            var xmlMediaTypeFormatter = new XmlMediaTypeFormatter { UseXmlSerializer = true };
            MediaTypeFormatter formatter = xmlMediaTypeFormatter;
            httpClient.DefaultRequestHeaders.Accept.Add(xmlMediaTypeHeader);

            httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            return formatter;
        }

        /// <summary>
        /// Sets the request headers.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <param name="httpClient">The HTTP client.</param>
        private static void SetRequestHeaders(Dictionary<string, string> headers, HttpClient httpClient)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
        }

        /// <summary>
        /// Sets the response message.
        /// </summary>
        /// <param name="response">The response.</param>
        private static void SetResponseMessage(HttpResponseMessage response)
        {
            var message = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(message))
            {
                message = response.ToString();
            }

            throw new ApplicationException(message);
        }

        /// <summary>
        /// Returns the task result.
        /// </summary>
        /// <typeparam name="T">Result type T</typeparam>
        /// <param name="formatter">The formatter.</param>
        /// <param name="response">The response.</param>
        /// <returns>Return type T</returns>
        private static T ReturnTaskResult<T>(MediaTypeFormatter formatter, HttpResponseMessage response)
        {
            var contentTask = response.Content.ReadAsAsync<T>(new[] { formatter });
            contentTask.Wait();
            return contentTask.Result;
        }

        /// <summary>
        /// Initializes the HTTP client.
        /// </summary>
        private void InitializeHttpClient()
        {
            this.httpClient = new HttpClient();

            this.httpClient.Timeout = TimeSpan.FromMinutes(5);
        }

        #endregion Private Methods
    }
}
