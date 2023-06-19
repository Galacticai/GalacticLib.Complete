/// —————————————————————————————————————————————
//?
//!? 📜 HTTPServer.cs
//!? 🖋️ Galacticai 📅 2022 - 2023
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies: No special dependencies
//?
/// —————————————————————————————————————————————


using System.Net;
using System.Text;

namespace GalacticLib.Networking;

/// <summary> Listen to a URI prefix and respond with a <see cref="string"/> by running <see cref="ResponseMethod"/>
/// <br/> ⚠️ Warning: Returning <see langword="null"/> means the request will be ignored </summary>
public class HTTPServer : IDisposable {
    #region this object

    protected HttpListener Listener { get; }

    /// <summary> Response method called when a request is received </summary>
    protected ResponseMethodHandler ResponseMethod { get; }
    public delegate Task ResponseMethodHandler(HTTPServer server, HttpListenerContext context);

    public event ResponseEventHandler? OnRequestReceived;
    public delegate void ResponseEventHandler(HTTPServer server, HttpListenerContext request);

    public HTTPServer(ResponseMethodHandler method, params string[] uriPrefixes) {
        if (!IsSupported)
            throw new NotSupportedException("Unsupported OS");
        if (uriPrefixes == null || uriPrefixes.Length == 0)
            throw new ArgumentException("At least 1 URI prefix is required");

        ResponseMethod = method;
        Listener = new() {
            AuthenticationSchemes = AuthenticationSchemes.Negotiate
        };
        foreach (string uriPrefix in uriPrefixes)
            Listener.Prefixes.Add(uriPrefix);
    }

    public static HTTPServer CreateAndStart(ResponseMethodHandler method, params string[] uriPrefixes) {
        HTTPServer server = new(method, uriPrefixes);
        server.Start();
        return server;
    }

    #endregion
    #region Shortcuts

    public static bool IsSupported => HttpListener.IsSupported;
    public bool IsListening => Listener.IsListening;
    public HttpListenerPrefixCollection Prefixes => Listener.Prefixes;

    #endregion
    #region Methods

    /// <summary> Close the <see cref="Listener"/>
    /// <br/>
    ///  • Note: <paramref name="confirm"/> value must be "confirm" to proceed </summary>
    /// <param name="confirm"></param>
    /// <returns> true if <see cref="HttpListener.Close"/> was called </returns>
    public bool Kill(string confirm) {
        if (!IsListening) return true;
        if (confirm != "confirm") return false;
        Listener.Close();
        return true;
    }
    public void Pause() {
        if (!IsListening) return;
        Listener.Stop();
    }
    public void Start() {
        if (IsListening) return;
        Listener.Start();

        Task.Run(async () => {
            Console.WriteLine("Starting server at...");
            try {
                while (IsListening) {
                    HttpListenerContext context = await Listener.GetContextAsync();
                    await Task.Run(async () => {
                        try {
                            await ResponseMethod(this, context);
                            OnRequestReceived?.Invoke(this, context);
                        } catch (Exception ex) {
                            Console.WriteLine(
                                $"(!) An error has occurred while responding to {context.Request.RemoteEndPoint}."
                                + $"{Environment.NewLine} (X) {ex.GetType().Name}: {ex.Message}"
                            );
                        } finally {
                            context.Response.OutputStream.Close();
                        }
                    });
                }
            } catch (Exception ex) {
                Console.WriteLine(
                    "Server has encountered an error!"
                    + $"{Environment.NewLine} (X) {ex.GetType().Name}: {ex.Message}"
                );
            }
        }).Start();
    }

    /// <summary> Just a shortcut
    /// <br/>
    /// <br/> • Sets <paramref name="code"/> as <see cref="HttpListenerResponse.StatusCode"/>
    /// <br/> • Encodes <paramref name="content"/> as UTF8 <see cref="byte"/>s and writes it to <see cref="HttpListenerResponse.OutputStream"/>
    /// <br/> • Sets <paramref name="contentType"/> as <see cref="HttpListenerResponse.ContentType"/> if <paramref name="content"/> is present
    /// <br/> • Sets <paramref name="headers"/> as <see cref="HttpListenerResponse.Headers"/> </summary>
    public static void SetContext(
            HttpListenerContext context,
            HTTPResponseCode code,
            string? content = null,
            string? contentType = null,
            params (string name, string value)[] headers) {
        context.Response.StatusCode = (int)code;
        if (content is not null) {
            var responseBuffer = Encoding.UTF8.GetBytes(content);
            if (contentType is not null)
                context.Response.ContentType = contentType;
            context.Response.ContentLength64 = responseBuffer.Length;
            context.Response.OutputStream.Write(responseBuffer, 0, responseBuffer.Length);
        }
        foreach (var (name, value) in headers) {
            context.Response.Headers[name] = value;
        }
    }

    public void Dispose() {
        Listener.Close();
        OnRequestReceived = null;
        GC.SuppressFinalize(this);
    }
    #endregion
}
