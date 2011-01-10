using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CloudAppSharp
{
    /// <summary>
    /// The exception that is thrown when invalid credentials are provided to the CloudApp service.
    /// </summary>
    public class CloudAppInvalidCredentialsException : Exception
    {
        public CloudAppInvalidCredentialsException(WebException webException)
            : base("Invalid credentials.", webException)
        {
        }
    }

    /// <summary>
    /// The exception that is thrown when an invalid or unexpected response is received from the CloudApp service.
    /// </summary>
    public class CloudAppInvalidResponseException : WebException
    {
        public CloudAppInvalidResponseException(string message, Exception innerException,WebExceptionStatus status, WebResponse response)
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

    /// <summary>
    /// The exception that is thrown when an action fails due to it requiring a CloudApp Pro account.
    /// </summary>
    public class CloudAppProNeededException : Exception
    {
        public CloudAppProNeededException() { }
        public CloudAppProNeededException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// The exception that is thrown when the daily upload limit has been exceeded.
    /// </summary>
    public class CloudAppUploadCountLimitExceededException : Exception
    {
        public CloudAppUploadCountLimitExceededException() { }
    }

    /// <summary>
    /// The exception that is thrown when a file that is being uploaded exceeds the filesize limit.
    /// </summary>
    public class CloudAppUploadSizeLimitExceededException : Exception
    {
        public CloudAppUploadSizeLimitExceededException() { }
    }
}
