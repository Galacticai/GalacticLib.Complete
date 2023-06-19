namespace GalacticLib.Networking;

public class HTTPResponse {
    public int Code { get; set; }
    public string? Content { get; set; }
    public string? ContentType { get; set; }

    public HTTPResponse(HTTPResponseCode code, string? content = null, string? contentType = null)
            : this((int)code, content, contentType) { }
    public HTTPResponse(int code, string? content = null, string? contentType = null) {
        Code = code;
        Content = content;
        ContentType = contentType;
    }

    /// <summary> Convert <see cref="Code"/> to <see cref="HTTPResponseCode"/> if it is a known code </summary>
    public HTTPResponseCode? OfficialResponseCode
        => Enum.IsDefined(typeof(HTTPResponseCode), Code)
        ? (HTTPResponseCode)Code
        : null;
}