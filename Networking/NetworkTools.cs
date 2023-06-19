using System.Net;
using System.Net.NetworkInformation;

namespace GalacticLib.Networking;
public static class NetworkTools {
    public static async Task<PingReply> PingAsync(this IPAddress address) {
        using Ping ping = new();
        var taskCompletionSource = new TaskCompletionSource<PingReply>();
        ping.PingCompleted += (sender, ev) => {
            if (ev.Error != null) {
                taskCompletionSource.SetException(ev.Error);
            } else if (ev.Cancelled) {
                taskCompletionSource.SetCanceled();
            } else {
                taskCompletionSource.SetResult(ev.Reply!);
            }
        };
        ping.SendAsync(address, null);
        return await taskCompletionSource.Task;
    }

    public static IEnumerable<IPAddress> GetLANHosts() => GetLANHosts(NetworkInterface.GetAllNetworkInterfaces());
    public static IEnumerable<IPAddress> GetLANHosts(params NetworkInterface[] interfaces) {
        foreach (NetworkInterface ni in interfaces)
            foreach (UnicastIPAddressInformation info in ni.GetIPProperties().UnicastAddresses)
                yield return info.Address;
    }
}
