﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CloudAppSharp
{
    public partial class CloudApp
    {
        /// <summary>
        /// The exception that is thrown when an invalid or unexpected response is received from the CloudApp service.
        /// </summary>
        public class CloudAppInvalidResponseException : WebException
        {
            public CloudAppInvalidResponseException(string message, Exception innerException, WebExceptionStatus status, WebResponse response)
                : base(message, innerException, status, response)
            {
            }
        }
        /// <summary>
        /// The exception that is thrown when a response received from the CloudApp service is complete but had an invalid protocol.
        /// </summary>
        public class CloudAppInvalidProtocolException : WebException
        {
            public CloudAppInvalidProtocolException(HttpStatusCode expectedCode, HttpWebResponse response)
                : base("Expected status " + expectedCode + "; got " + response.StatusCode + " instead",
                    null, WebExceptionStatus.ProtocolError, response)
            {
            }
        }
    }
}
