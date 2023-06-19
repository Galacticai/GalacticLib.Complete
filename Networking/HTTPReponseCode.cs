using System.ComponentModel.DataAnnotations;

namespace GalacticLib.Networking;

public enum HTTPResponseCode {
    /// <summary> 100 • Continue </summary>
    [Display(
        GroupName = "Information responses",
        Name = "Continue",
        Description = "This interim response indicates that the client should continue the request or ignore the response if the request is already finished.")]
    Continue = 100,
    /// <summary> 101 • Switching Protocols </summary>
    [Display(
        GroupName = "Information responses",
        Name = "Switching Protocols",
        Description = "This code is sent in response to an Upgrade request header from the client and indicates the protocol the server is switching to.")]
    SwitchingProtocols = 101,
    /// <summary> 102 • Processing (WebDAV) </summary>
    [Display(
        GroupName = "Information responses",
        Name = "Processing (Webdav)",
        Description = "This code indicates that the server has received and is processing the request, but no response is available yet.")]
    Processing = 102,
    /// <summary> 103 • Early Hints Experimental </summary>
    [Display(
        GroupName = "Information responses",
        Name = "Early Hints Experimental",
        Description = "This status code is primarily intended to be used with the Link header, letting the user agent start preloading resources while the server prepares a response.")]
    EarlyHintsExperimental = 103,
    /// <summary> 200 • OK </summary>
    [Display(
        GroupName = "Successful responses",
        Name = "OK",
        Description = "The request succeeded. The result meaning of \"success\" depends on the HTTP method:")]
    OK = 200,
    /// <summary> 201 • Created </summary>
    [Display(
        GroupName = "Successful responses",
        Name = "Created",
        Description = "The request succeeded, and a new resource was created as a result. This is typically the response sent after POST requests, or some PUT requests.")]
    Created = 201,
    /// <summary> 202 • Accepted </summary>
    [Display(
        GroupName = "Successful responses",
        Name = "Accepted",
        Description = "The request has been received but not yet acted upon. It is noncommittal, since there is no way in HTTP to later send an asynchronous response indicating the outcome of the request. It is intended for cases where another process or server handles the request, or for batch processing.")]
    Accepted = 202,
    /// <summary> 203 • Non-Authoritative Information </summary>
    [Display(
        GroupName = "Successful responses",
        Name = "Non-Authoritative Information",
        Description = "This response code means the returned metadata is not exactly the same as is available from the origin server, but is collected from a local or a third-party copy. This is mostly used for mirrors or backups of another resource. Except for that specific case, the 200 OK response is preferred to this status.")]
    NonAuthoritativeInformation = 203,
    /// <summary> 204 • No Content </summary>
    [Display(
        GroupName = "Successful responses",
        Name = "No Content",
        Description = "There is no content to send for this request, but the headers may be useful. The user agent may update its cached headers for this resource with the new ones.")]
    NoContent = 204,
    /// <summary> 205 • Reset Content </summary>
    [Display(
        GroupName = "Successful responses",
        Name = "Reset Content",
        Description = "Tells the user agent to reset the document which sent this request.")]
    ResetContent = 205,
    /// <summary> 206 • Partial Content </summary>
    [Display(
        GroupName = "Successful responses",
        Name = "Partial Content",
        Description = "This response code is used when the Range header is sent from the client to request only part of a resource.")]
    PartialContent = 206,
    /// <summary> 207 • Multi-Status (WebDAV) </summary>
    [Display(
        GroupName = "Successful responses",
        Name = "Multi-Status (Webdav)",
        Description = "Conveys information about multiple resources, for situations where multiple status codes might be appropriate.")]
    MultiStatus = 207,
    /// <summary> 208 • Already Reported (WebDAV) </summary>
    [Display(
        GroupName = "Successful responses",
        Name = "Already Reported (Webdav)",
        Description = "Used inside a <dav:propstat> response element to avoid repeatedly enumerating the internal members of multiple bindings to the same collection.")]
    AlreadyReported = 208,
    /// <summary> 226 • IM Used (HTTP Delta encoding) </summary>
    [Display(
        GroupName = "Successful responses",
        Name = "IM Used (HTTP Delta Encoding)",
        Description = "The server has fulfilled a GET request for the resource, and the response is a representation of the result of one or more instance-manipulations applied to the current instance.")]
    IMUsed = 226,
    /// <summary> 300 • Multiple Choices </summary>
    [Display(
        GroupName = "Redirection messages",
        Name = "Multiple Choices",
        Description = "The request has more than one possible response. The user agent or user should choose one of them. (There is no standardized way of choosing one of the responses, but HTML links to the possibilities are recommended so the user can pick.)")]
    MultipleChoices = 300,
    /// <summary> 301 • Moved Permanently </summary>
    [Display(
        GroupName = "Redirection messages",
        Name = "Moved Permanently",
        Description = "The URL of the requested resource has been changed permanently. The new URL is given in the response.")]
    MovedPermanently = 301,
    /// <summary> 302 • Found </summary>
    [Display(
        GroupName = "Redirection messages",
        Name = "Found",
        Description = "This response code means that the URI of requested resource has been changed temporarily. Further changes in the URI might be made in the future. Therefore, this same URI should be used by the client in future requests.")]
    Found = 302,
    /// <summary> 303 • See Other </summary>
    [Display(
        GroupName = "Redirection messages",
        Name = "See Other",
        Description = "The server sent this response to direct the client to get the requested resource at another URI with a GET request.")]
    SeeOther = 303,
    /// <summary> 304 • Not Modified </summary>
    [Display(
        GroupName = "Redirection messages",
        Name = "Not Modified",
        Description = "This is used for caching purposes. It tells the client that the response has not been modified, so the client can continue to use the same cached version of the response.")]
    NotModified = 304,
    /// <summary> 305 • Use Proxy Deprecated </summary>
    [Display(
        GroupName = "Redirection messages",
        Name = "Use Proxy Deprecated",
        Description = "Defined in a previous version of the HTTP specification to indicate that a requested response must be accessed by a proxy. It has been deprecated due to security concerns regarding in-band configuration of a proxy.")]
    UseProxyDeprecated = 305,
    /// <summary> 306 • unused </summary>
    [Display(
        GroupName = "Redirection messages",
        Name = "Unused",
        Description = "This response code is no longer used; it is just reserved. It was used in a previous version of the HTTP/1.1 specification.")]
    unused = 306,
    /// <summary> 307 • Temporary Redirect </summary>
    [Display(
        GroupName = "Redirection messages",
        Name = "Temporary Redirect",
        Description = "The server sends this response to direct the client to get the requested resource at another URI with the same method that was used in the prior request. This has the same semantics as the 302 Found HTTP response code, with the exception that the user agent must not change the HTTP method used: if a POST was used in the first request, a POST must be used in the second request.")]
    TemporaryRedirect = 307,
    /// <summary> 308 • Permanent Redirect </summary>
    [Display(
        GroupName = "Redirection messages",
        Name = "Permanent Redirect",
        Description = "This means that the resource is now permanently located at another URI, specified by the Location: HTTP Response header. This has the same semantics as the 301 Moved Permanently HTTP response code, with the exception that the user agent must not change the HTTP method used: if a POST was used in the first request, a POST must be used in the second request.")]
    PermanentRedirect = 308,
    /// <summary> 400 • Bad Request </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Bad Request",
        Description = "The server cannot or will not process the request due to something that is perceived to be a client error (e.g., malformed request syntax, invalid request message framing, or deceptive request routing).")]
    BadRequest = 400,
    /// <summary> 401 • Unauthorized </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Unauthorized",
        Description = "Although the HTTP standard specifies \"unauthorized\", semantically this response means \"unauthenticated\". That is, the client must authenticate itself to get the requested response.")]
    Unauthorized = 401,
    /// <summary> 402 • Payment Required Experimental </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Payment Required Experimental",
        Description = "This response code is reserved for future use. The initial aim for creating this code was using it for digital payment systems, however this status code is used very rarely and no standard convention exists.")]
    PaymentRequiredExperimental = 402,
    /// <summary> 403 • Forbidden </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Forbidden",
        Description = "The client does not have access rights to the content; that is, it is unauthorized, so the server is refusing to give the requested resource. Unlike 401 Unauthorized, the client's identity is known to the server.")]
    Forbidden = 403,
    /// <summary> 404 • Not Found </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Not Found",
        Description = "The server cannot find the requested resource. In the browser, this means the URL is not recognized. In an API, this can also mean that the endpoint is valid but the resource itself does not exist. Servers may also send this response instead of 403 Forbidden to hide the existence of a resource from an unauthorized client. This response code is probably the most well known due to its frequent occurrence on the web.")]
    NotFound = 404,
    /// <summary> 405 • Method Not Allowed </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Method Not Allowed",
        Description = "The request method is known by the server but is not supported by the target resource. For example, an API may not allow calling DELETE to remove a resource.")]
    MethodNotAllowed = 405,
    /// <summary> 406 • Not Acceptable </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Not Acceptable",
        Description = "This response is sent when the web server, after performing server-driven content negotiation, doesn't find any content that conforms to the criteria given by the user agent.")]
    NotAcceptable = 406,
    /// <summary> 407 • Proxy Authentication Required </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Proxy Authentication Required",
        Description = "This is similar to 401 Unauthorized but authentication is needed to be done by a proxy.")]
    ProxyAuthenticationRequired = 407,
    /// <summary> 408 • Request Timeout </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Request Timeout",
        Description = "This response is sent on an idle connection by some servers, even without any previous request by the client. It means that the server would like to shut down this unused connection. This response is used much more since some browsers, like Chrome, Firefox 27+, or IE9, use HTTP pre-connection mechanisms to speed up surfing. Also note that some servers merely shut down the connection without sending this message.")]
    RequestTimeout = 408,
    /// <summary> 409 • Conflict </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Conflict",
        Description = "This response is sent when a request conflicts with the current state of the server.")]
    Conflict = 409,
    /// <summary> 410 • Gone </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Gone",
        Description = "This response is sent when the requested content has been permanently deleted from server, with no forwarding address. Clients are expected to remove their caches and links to the resource. The HTTP specification intends this status code to be used for \"limited-time, promotional services\". APIs should not feel compelled to indicate resources that have been deleted with this status code.")]
    Gone = 410,
    /// <summary> 411 • Length Required </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Length Required",
        Description = "Server rejected the request because the Content-Length header field is not defined and the server requires it.")]
    LengthRequired = 411,
    /// <summary> 412 • Precondition Failed </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Precondition Failed",
        Description = "The client has indicated preconditions in its headers which the server does not meet.")]
    PreconditionFailed = 412,
    /// <summary> 413 • Payload Too Large </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Payload Too Large",
        Description = "Request entity is larger than limits defined by server. The server might close the connection or return an Retry-After header field.")]
    PayloadTooLarge = 413,
    /// <summary> 414 • URI Too Long </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "URI Too Long",
        Description = "The URI requested by the client is longer than the server is willing to interpret.")]
    URITooLong = 414,
    /// <summary> 415 • Unsupported Media Type </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Unsupported Media Type",
        Description = "The media format of the requested data is not supported by the server, so the server is rejecting the request.")]
    UnsupportedMediaType = 415,
    /// <summary> 416 • Range Not Satisfiable </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Range Not Satisfiable",
        Description = "The range specified by the Range header field in the request cannot be fulfilled. It's possible that the range is outside the size of the target URI's data.")]
    RangeNotSatisfiable = 416,
    /// <summary> 417 • Expectation Failed </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Expectation Failed",
        Description = "This response code means the expectation indicated by the Expect request header field cannot be met by the server.")]
    ExpectationFailed = 417,
    /// <summary> 418 • I'm a teapot </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "I'm A Teapot",
        Description = "The server refuses the attempt to brew coffee with a teapot.")]
    ImATeapot = 418,
    /// <summary> 421 • Misdirected Request </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Misdirected Request",
        Description = "The request was directed at a server that is not able to produce a response. This can be sent by a server that is not configured to produce responses for the combination of scheme and authority that are included in the request URI.")]
    MisdirectedRequest = 421,
    /// <summary> 422 • Unprocessable Content (WebDAV) </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Unprocessable Content (Webdav)",
        Description = "The request was well-formed but was unable to be followed due to semantic errors.")]
    UnprocessableContent = 422,
    /// <summary> 423 • Locked (WebDAV) </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Locked (Webdav)",
        Description = "The resource that is being accessed is locked.")]
    Locked = 423,
    /// <summary> 424 • Failed Dependency (WebDAV) </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Failed Dependency (Webdav)",
        Description = "The request failed due to failure of a previous request.")]
    FailedDependency = 424,
    /// <summary> 425 • Too Early Experimental </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Too Early Experimental",
        Description = "Indicates that the server is unwilling to risk processing a request that might be replayed.")]
    TooEarlyExperimental = 425,
    /// <summary> 426 • Upgrade Required </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Upgrade Required",
        Description = "The server refuses to perform the request using the current protocol but might be willing to do so after the client upgrades to a different protocol. The server sends an Upgrade header in a 426 response to indicate the required protocol(s).")]
    UpgradeRequired = 426,
    /// <summary> 428 • Precondition Required </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Precondition Required",
        Description = "The origin server requires the request to be conditional. This response is intended to prevent the 'lost update' problem, where a client GETs a resource's state, modifies it and PUTs it back to the server, when meanwhile a third party has modified the state on the server, leading to a conflict.")]
    PreconditionRequired = 428,
    /// <summary> 429 • Too Many Requests </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Too Many Requests",
        Description = "The user has sent too many requests in a given amount of time (\"rate limiting\").")]
    TooManyRequests = 429,
    /// <summary> 431 • Request Header Fields Too Large </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Request Header Fields Too Large",
        Description = "The server is unwilling to process the request because its header fields are too large. The request may be resubmitted after reducing the size of the request header fields.")]
    RequestHeaderFieldsTooLarge = 431,
    /// <summary> 451 • Unavailable For Legal Reasons </summary>
    [Display(
        GroupName = "Client error responses",
        Name = "Unavailable For Legal Reasons",
        Description = "The user agent requested a resource that cannot legally be provided, such as a web page censored by a government.")]
    UnavailableForLegalReasons = 451,
    /// <summary> 500 • Internal Server Error </summary>
    [Display(
        GroupName = "Server error responses",
        Name = "Internal Server Error",
        Description = "The server has encountered a situation it does not know how to handle.")]
    InternalServerError = 500,
    /// <summary> 501 • Not Implemented </summary>
    [Display(
        GroupName = "Server error responses",
        Name = "Not Implemented",
        Description = "The request method is not supported by the server and cannot be handled. The only methods that servers are required to support (and therefore that must not return this code) are GET and HEAD.")]
    NotImplemented = 501,
    /// <summary> 502 • Bad Gateway </summary>
    [Display(
        GroupName = "Server error responses",
        Name = "Bad Gateway",
        Description = "This error response means that the server, while working as a gateway to get a response needed to handle the request, got an invalid response.")]
    BadGateway = 502,
    /// <summary> 503 • Service Unavailable </summary>
    [Display(
        GroupName = "Server error responses",
        Name = "Service Unavailable",
        Description = "The server is not ready to handle the request. Common causes are a server that is down for maintenance or that is overloaded. Note that together with this response, a user-friendly page explaining the problem should be sent. This response should be used for temporary conditions and the Retry-After HTTP header should, if possible, contain the estimated time before the recovery of the service. The webmaster must also take care about the caching-related headers that are sent along with this response, as these temporary condition responses should usually not be cached.")]
    ServiceUnavailable = 503,
    /// <summary> 504 • Gateway Timeout </summary>
    [Display(
        GroupName = "Server error responses",
        Name = "Gateway Timeout",
        Description = "This error response is given when the server is acting as a gateway and cannot get a response in time.")]
    GatewayTimeout = 504,
    /// <summary> 505 • HTTP Version Not Supported </summary>
    [Display(
        GroupName = "Server error responses",
        Name = "HTTP Version Not Supported",
        Description = "The HTTP version used in the request is not supported by the server.")]
    HTTPVersionNotSupported = 505,
    /// <summary> 506 • Variant Also Negotiates </summary>
    [Display(
        GroupName = "Server error responses",
        Name = "Variant Also Negotiates",
        Description = "The server has an internal configuration error: the chosen variant resource is configured to engage in transparent content negotiation itself, and is therefore not a proper end point in the negotiation process.")]
    VariantAlsoNegotiates = 506,
    /// <summary> 507 • Insufficient Storage (WebDAV) </summary>
    [Display(
        GroupName = "Server error responses",
        Name = "Insufficient Storage (Webdav)",
        Description = "The method could not be performed on the resource because the server is unable to store the representation needed to successfully complete the request.")]
    InsufficientStorage = 507,
    /// <summary> 508 • Loop Detected (WebDAV) </summary>
    [Display(
        GroupName = "Server error responses",
        Name = "Loop Detected (Webdav)",
        Description = "The server detected an infinite loop while processing the request.")]
    LoopDetected = 508,
    /// <summary> 510 • Not Extended </summary>
    [Display(
        GroupName = "Server error responses",
        Name = "Not Extended",
        Description = "Further extensions to the request are required for the server to fulfill it.")]
    NotExtended = 510,
    /// <summary> 511 • Network Authentication Required </summary>
    [Display(
        GroupName = "Server error responses",
        Name = "Network Authentication Required",
        Description = "Indicates that the client needs to authenticate to gain network access.")]
    NetworkAuthenticationRequired = 511,
}