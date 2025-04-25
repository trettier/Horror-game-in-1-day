using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using UnityEngine;

public class WebSocketTest : MonoBehaviour
{
    private ClientWebSocket webSocket;

    async void Start()
    {
        string uri = "wss://40379255d1a8.pr.edgegap.net:30658/"; // Заменить, если надо

        Debug.Log("🧪 Попытка подключения к: " + uri);
        webSocket = new ClientWebSocket();

        try
        {
            await webSocket.ConnectAsync(new Uri(uri), CancellationToken.None);
            Debug.Log("✅ Подключение успешно");

            // Пример: отправка простого сообщения (можешь закомментировать)
            string message = "Hello WebSocket Server!";
            ArraySegment<byte> bytesToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
            await webSocket.SendAsync(bytesToSend, WebSocketMessageType.Text, true, CancellationToken.None);
            Debug.Log("📨 Сообщение отправлено: " + message);

            // Пример: ожидание ответа
            var buffer = new ArraySegment<byte>(new byte[1024]);
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
            string receivedMessage = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
            Debug.Log("📥 Ответ от сервера: " + receivedMessage);
        }
        catch (Exception e)
        {
            Debug.LogError("❌ Ошибка подключения: " + e.Message);
        }
        finally
        {
            if (webSocket != null)
                webSocket.Dispose();
        }
    }
}
