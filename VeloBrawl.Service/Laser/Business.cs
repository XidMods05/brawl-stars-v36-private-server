using VeloBrawl.General.Network;
using VeloBrawl.Proxy.Network;

namespace VeloBrawl.Service.Laser;

public static class Business
{
    public static HttpLaserSocketListener? HttpLaserSocketListener;
    public static readonly Dictionary<int, TcpLaserSocketListener?> TcpLaserSocketListeners = new();
    public static readonly Dictionary<int, UdpLaserSocketListener?> UdpLaserSocketListeners = new();
    public static ProxySocketTransportListener? ProxySocketTransportListener;
}