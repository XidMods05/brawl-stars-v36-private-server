using System.Net.Sockets;
using VeloBrawl.Supercell.Titan.CommonUtils;
using VeloBrawl.Titan.Graphic;

namespace VeloBrawl.Proxy.Networking;

public class ProxyTransportConnection
{
    private Socket? _listener;
    private ProxyTransportMessaging? _proxyTransportMessaging;
    private Socket? _userListener;

    public byte[]? ByteBuffer { get; set; } = new byte[32768];
    public MemoryStream? MemoryStream { get; set; } = new();

    public Socket? GetListener()
    {
        return _listener;
    }

    public Socket? SetListener(Socket? socket)
    {
        return _listener = socket;
    }

    public ProtocolType GetInterfaceName()
    {
        return _listener!.ProtocolType;
    }

    public Socket? GetUserListener()
    {
        return _userListener;
    }

    public Socket? SetUserListener(Socket? socket)
    {
        return _userListener = socket;
    }

    public ProxyTransportMessaging? GetProxyTransportMessaging()
    {
        return _proxyTransportMessaging;
    }

    public ProxyTransportMessaging SetProxyTransportMessaging(ProxyTransportMessaging messaging)
    {
        return _proxyTransportMessaging = messaging;
    }

    public void Close()
    {
        try
        {
            ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Tcp,
                "New user-transport-disconnected! Information: " +
                $"server address: {Helper.GetIpFromDomain(GetListener()!.LocalEndPoint!.ToString()!)}; " +
                $"server port: {Helper.GetPortFromDomain(GetListener()!.LocalEndPoint!.ToString()!)}; " +
                $"client address: {Helper.GetIpBySocket(GetUserListener()!)}; " +
                $"client port: {Helper.GetPortBySocket(GetUserListener()!)}.");

            _userListener?.Close();
        }
        catch (Exception)
        {
            // ignored.
        }
    }
}