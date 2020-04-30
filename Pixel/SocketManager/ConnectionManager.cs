using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Pixel.SocketManager
{
    public class ConnectionManager
    {
        private ConcurrentDictionary<string, WebSocket> _connection = new ConcurrentDictionary<string, WebSocket>();

        public WebSocket GetSocketById(string id)
        {
            return _connection.FirstOrDefault(x => x.Key == id).Value;
        }

        public ConcurrentDictionary<string, WebSocket> GetAllConnection()
        {
            return _connection;
        }

        public string GetId(WebSocket socket)
        {
            return _connection.FirstOrDefault(x => x.Value == socket).Key;
        }

        public async Task RemoveSocketAsync(string id)
        {
            _connection.TryRemove(id, out var socket);
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "socket connection closed", CancellationToken.None);
        }
        public void AddSocket(WebSocket socket)
        {
            _connection.TryAdd(GetConnectionId(), socket);
        }
        private string GetConnectionId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
