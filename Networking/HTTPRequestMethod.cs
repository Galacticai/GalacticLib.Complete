using System.ComponentModel;

namespace GalacticLib.Networking;

public enum HTTPRequestMethod {
    /// <summary> The GET method requests a representation of the specified resource. Requests using GET should only retrieve data. </summary>
    [Description("The GET method requests a representation of the specified resource. Requests using GET should only retrieve data.")]
    GET,
    /// <summary> The HEAD method asks for a response identical to a GET request, but without the response body. </summary>
    [Description("The HEAD method asks for a response identical to a GET request, but without the response body.")]
    HEAD,
    /// <summary> The POST method submits an entity to the specified resource, often causing a change in state or side effects on the server. </summary>
    [Description("The POST method submits an entity to the specified resource, often causing a change in state or side effects on the server.")]
    POST,
    /// <summary> The PUT method replaces all current representations of the target resource with the request payload. </summary>
    [Description("The PUT method replaces all current representations of the target resource with the request payload.")]
    PUT,
    /// <summary> The DELETE method deletes the specified resource. </summary>
    [Description("The DELETE method deletes the specified resource.")]
    DELETE,
    /// <summary> The CONNECT method establishes a tunnel to the server identified by the target resource. </summary>
    [Description("The CONNECT method establishes a tunnel to the server identified by the target resource.")]
    CONNECT,
    /// <summary> The OPTIONS method describes the communication options for the target resource. </summary>
    [Description("The OPTIONS method describes the communication options for the target resource.")]
    OPTIONS,
    /// <summary> The TRACE method performs a message loop-back test along the path to the target resource. </summary>
    [Description("The TRACE method performs a message loop-back test along the path to the target resource.")]
    TRACE,
    /// <summary> The PATCH method applies partial modifications to a resource. </summary>
    [Description("The PATCH method applies partial modifications to a resource.")]
    PATCH,
}