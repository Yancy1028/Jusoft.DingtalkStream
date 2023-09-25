using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace SimulationStreamService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            // 创建一个WebSocket监听
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:12345/");
            listener.Start();
            Console.WriteLine("Listening...");

            while (!stoppingToken.IsCancellationRequested)
            {
                var listenerContext = await listener.GetContextAsync();

                if (listenerContext.Request.IsWebSocketRequest)
                {
                    var webSocketContext = await listenerContext.AcceptWebSocketAsync(subProtocol: null);

                    _ = Task.Run(async () =>
                    {
                        while (webSocketContext.WebSocket.State == WebSocketState.Open)
                        {
                            await Task.Delay(10000);
                            using var stream = new MemoryStream();
                            using var writer = new Utf8JsonWriter(stream);

                            writer.WriteStartObject();

                            writer.WriteString("specVersion", "1.0");
                            writer.WriteString("type", "SYSTEM");
                            writer.WriteString("data", "{\"data\":\"asdfa123\"}");
                            writer.WritePropertyName("headers");
                            writer.WriteStartObject();
                            writer.WriteString("messageId", Guid.NewGuid());
                            writer.WriteString("topic", "disconnect");

                            writer.WriteEndObject();

                            writer.WriteEndObject();
                            await writer.FlushAsync();
                            stream.Seek(0, SeekOrigin.Begin);

                            await webSocketContext.WebSocket.SendAsync(stream.ToArray(), WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                    });

                    await HandleWebSocketRequestAsync(webSocketContext.WebSocket);
                }
                else
                {
                    listenerContext.Response.StatusCode = 400;
                    listenerContext.Response.Close();
                }

            }


            async Task HandleWebSocketRequestAsync(WebSocket webSocket)
            {
                var buffer = new byte[1024];
                var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), stoppingToken);

                while (!receiveResult.CloseStatus.HasValue)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
                    Console.WriteLine($"Received message: {message}");

                    //var response = Encoding.UTF8.GetBytes($"[Echo] {message}");
                    //await webSocket.SendAsync(new ArraySegment<byte>(response, 0, response.Length), WebSocketMessageType.Text, true, CancellationToken.None);

                    receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), stoppingToken);
                }

                await webSocket.CloseAsync(receiveResult.CloseStatus.Value, receiveResult.CloseStatusDescription, CancellationToken.None);

            }
        }
    }
}