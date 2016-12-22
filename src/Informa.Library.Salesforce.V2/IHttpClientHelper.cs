using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Informa.Library.Salesforce.V2
{
    public interface IHttpClientHelper
    {
        /// <summary>
        /// Gets the response message.
        /// </summary>
        /// <param name="url">Uri of the resource</param>
        /// <returns>Response message</returns>
        HttpResponseMessage ExecuteGet(Uri url);

        /// <summary>
        /// Gets the response message.
        /// </summary>
        /// <param name="url">Uri of the resource</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <returns>Response message</returns>
        HttpResponseMessage ExecuteGet(Uri url, MediaTypeWithQualityHeaderValue mediaTypeHeader);

        /// <summary>
        /// Gets the response of type T.
        /// </summary>
        /// <typeparam name="T">Type name of T</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <returns>Response of type T</returns>
        T GetResponse<T>(Uri url);

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <param name="url">Uri of the resource</param>
        /// <returns>HttpResponse Message</returns>
        HttpResponseMessage GetResponse(Uri url);

        /// <summary>
        /// Gets the response of type T.
        /// </summary>
        /// <typeparam name="T">Type name of T</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <returns>Response of type T</returns>
        T GetResponse<T>(Uri url, MediaTypeWithQualityHeaderValue mediaTypeHeader);

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <typeparam name="T">Type name of T</typeparam>
        /// <param name="url">The URL to hit.</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <param name="millisecondsTimeout">The milliseconds timeout.</param>
        /// <returns>Response of type T</returns>
        T GetResponse<T>(Uri url, MediaTypeWithQualityHeaderValue mediaTypeHeader, int millisecondsTimeout);

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <typeparam name="T">Type name of T</typeparam>
        /// <param name="url">The URL of the resource.</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <param name="millisecondsTimeout">The milliseconds timeout.</param>
        /// <param name="headerValue">The header value.</param>
        /// <returns>Response of type T</returns>
        T GetResponse<T>(Uri url, MediaTypeWithQualityHeaderValue mediaTypeHeader, int? millisecondsTimeout, AuthenticationHeaderValue headerValue);

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <param name="url">Uri of the resource</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <returns>HttpResponse Message</returns>
        HttpResponseMessage GetResponse(Uri url, MediaTypeWithQualityHeaderValue mediaTypeHeader);

        /// <summary>
        /// Update the request.
        /// </summary>
        /// <typeparam name="T">Type name of T</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="value">Value of type</param>
        void PutRequest<T>(Uri url, T value);

        /// <summary>
        /// Puts the request.
        /// </summary>
        /// <typeparam name="T">Type name of T</typeparam>
        /// <param name="url">The URL value.</param>
        /// <param name="value">The value of Type T.</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        void PutRequest<T>(Uri url, T value, MediaTypeWithQualityHeaderValue mediaTypeHeader);

        /// <summary>
        /// Puts the request.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="url">The URL value.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// Http response message
        /// </returns>
        T1 PutRequest<T1, T2>(Uri url, T2 value);

        /// <summary>
        /// Puts the request.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="url">The URL value.</param>
        /// <param name="value">The value.</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <returns>Http response message</returns>
        T1 PutRequest<T1, T2>(Uri url, T2 value, MediaTypeWithQualityHeaderValue mediaTypeHeader);

        /// <summary>
        /// Puts the response request.
        /// </summary>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="value">The value.</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <returns>
        /// HttpResponse Message
        /// </returns>
        HttpResponseMessage PutResponseRequest<T2>(Uri url, T2 value, MediaTypeWithQualityHeaderValue mediaTypeHeader);

        /// <summary>
        /// Puts the response request.
        /// </summary>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="value">The value.</param>
        /// <param name="contentMediaTypeHeader">The content media type header.</param>
        /// <param name="acceptMediaTypeHeader">The accept media type header.</param>
        /// <returns> HttpResponse Message</returns>
        HttpResponseMessage PutResponseRequest<T2>(Uri url, T2 value, MediaTypeWithQualityHeaderValue contentMediaTypeHeader, MediaTypeWithQualityHeaderValue acceptMediaTypeHeader);

        /// <summary>
        /// Puts the response request.
        /// </summary>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="value">The value.</param>
        /// <param name="contentMediaTypeHeader">The content media type header.</param>
        /// <param name="acceptMediaTypeHeader">The accept media type header.</param>
        /// <param name="headers">The headers.</param>
        /// <returns>
        /// HttpResponse Message
        /// </returns>
        HttpResponseMessage PutResponseRequest<T2>(Uri url, T2 value, MediaTypeWithQualityHeaderValue contentMediaTypeHeader, MediaTypeWithQualityHeaderValue acceptMediaTypeHeader, Dictionary<string, string> headers);

        /// <summary>
        /// Posts the request.
        /// </summary>
        /// <typeparam name="T">Type name of T</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="value">Value of type T</param>
        void PostRequest<T>(Uri url, T value);

        /// <summary>
        /// Posts the request.
        /// </summary>
        /// <typeparam name="T">Type name of T</typeparam>
        /// <param name="url">The URL of the resource.</param>
        /// <param name="value">The value.</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        void PostRequest<T>(Uri url, T value, MediaTypeWithQualityHeaderValue mediaTypeHeader);

        /// <summary>
        /// Deletes the request.
        /// </summary>
        /// <param name="url">Uri of the resource</param>
        void DeleteRequest(Uri url);

        /// <summary>
        /// Deletes the request.
        /// </summary>
        /// <param name="url">The URL of resource.</param>
        /// <param name="timeoutDuration">Duration of the timeout.</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        void DeleteRequest(Uri url, int timeoutDuration, MediaTypeWithQualityHeaderValue mediaTypeHeader);

        /// <summary>
        /// Deletes the request.
        /// </summary>
        /// <param name="url">Uri of the resource</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        void DeleteRequest(Uri url, MediaTypeWithQualityHeaderValue mediaTypeHeader);

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
        T1 PostData<T1, T2>(Uri url, T2 value);

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
        T1 PostData<T1, T2>(Uri url, T2 value, MediaTypeWithQualityHeaderValue mediaTypeHeader);

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
        T1 PostData<T1, T2>(Uri url, T2 value, MediaTypeWithQualityHeaderValue mediaTypeHeader, AuthenticationHeaderValue headerValue);

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
        T1 PostData<T1, T2>(Uri url, T2 value, MediaTypeWithQualityHeaderValue contentMediaTypeHeader, MediaTypeWithQualityHeaderValue acceptMediaTypeHeader);

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
        T1 PostData<T1, T2>(Uri url, T2 value, string headerParameter, Dictionary<string, string> headers);

        /// <summary>
        /// Generic post request.
        /// </summary>
        /// <typeparam name="T">Response Type</typeparam>
        /// <param name="url">Uri of the resource.</param>
        /// <param name="nameValueCollection">The name value collection.</param>
        /// <returns>
        /// Http response message
        /// </returns>
        T PostData<T>(Uri url, IEnumerable<KeyValuePair<string, string>> nameValueCollection);

        /// <summary>
        /// Posts the data value.
        /// </summary>
        /// <typeparam name="T">Request Type</typeparam>
        /// <param name="url">The URL of resource.</param>
        /// <param name="value">The value.</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <returns>
        /// Location request header
        /// </returns>
        string PostDataValue<T>(Uri url, T value, MediaTypeWithQualityHeaderValue mediaTypeHeader);

        /// <summary>
        /// Posts the response data.
        /// </summary>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="url">The URL of resource.</param>
        /// <param name="value">The value.</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <returns>
        /// HttpResponse Message
        /// </returns>
        HttpResponseMessage PostResponseData<T2>(Uri url, T2 value, MediaTypeWithQualityHeaderValue mediaTypeHeader);

        /// <summary>
        /// Posts the response data.
        /// </summary>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="url">The URL of resource.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// HttpResponse Message
        /// </returns>
        HttpResponseMessage PostResponseData<T2>(Uri url, T2 value);

        /// <summary>
        /// Gets the response of type T.
        /// </summary>
        /// <typeparam name="T">Type name of T</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="authenticationHeaderValue">The authentication header value.</param>
        /// <param name="headers">The headers.</param>
        /// <returns>
        /// Response of type T
        /// </returns>
        T GetDataResponse<T>(Uri url, AuthenticationHeaderValue authenticationHeaderValue, Dictionary<string, string> headers);

        /// <summary>
        /// Disposes the HTTP client.
        /// </summary>
        void DisposeHttpClient();

        /// <summary>
        /// Posts the response data.
        /// </summary>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="value">The value.</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <param name="millisecondsTimeout">The milliseconds timeout.</param>
        /// <returns>Response HTTP Response</returns>
        HttpResponseMessage PostResponseData<T2>(Uri url, T2 value, MediaTypeWithQualityHeaderValue mediaTypeHeader, int? millisecondsTimeout);

        /// <summary>
        /// Posts the response data.
        /// </summary>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="value">The value.</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <param name="millisecondsTimeout">The milliseconds timeout.</param>
        /// <param name="headerValue">The header value.</param>
        /// <returns>
        /// Response HTTP Response
        /// </returns>
        HttpResponseMessage PostResponseData<T2>(Uri url, T2 value, MediaTypeWithQualityHeaderValue mediaTypeHeader, int? millisecondsTimeout, AuthenticationHeaderValue headerValue);

        /// <summary>
        /// Puts the request.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="value">The value.</param>
        /// <param name="millisecondsTimeout">The milliseconds timeout.</param>
        /// <returns>Response of type T1</returns>
        T1 PutRequest<T1, T2>(Uri url, T2 value, int? millisecondsTimeout);

        /// <summary>
        /// Puts the request.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="url">Uri of the resource</param>
        /// <param name="value">The value.</param>
        /// <param name="mediaTypeHeader">The media type header.</param>
        /// <param name="millisecondsTimeout">The milliseconds timeout.</param>
        /// <returns>Response of type T1</returns>
        T1 PutRequest<T1, T2>(Uri url, T2 value, MediaTypeWithQualityHeaderValue mediaTypeHeader, int? millisecondsTimeout);

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
        T GetResponse<T>(Uri url, MediaTypeWithQualityHeaderValue mediaTypeHeader, Dictionary<string, string> headers);
    }
}
