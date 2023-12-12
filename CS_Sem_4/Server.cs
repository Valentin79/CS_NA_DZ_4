using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace CS_Sem_4
{
    public enum Commands
    {
        Register,
        Delete,
        Default
    }

    public enum TypeSend
    {
        ToAll,
        ToOne,
        ToServer,
        Default
    }

    public class Server
    {
        private readonly UdpClient _udpClient;
        private  IPEndPoint _iPEndPoint;
        private Manager _manager;

        public Server() 
        {
            _udpClient = new UdpClient(12345);
            _iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            _manager = new Manager(this);
        }


        public string Name = "Server";
        public Dictionary<string, IPEndPoint> Users { get; set; }
        

        static private CancellationTokenSource cts = new CancellationTokenSource();
        static private CancellationToken token = cts.Token;

        public Message Listen()
        {
            byte[] buffer = _udpClient.Receive(ref _iPEndPoint);
            var messageText = Encoding.UTF8.GetString(buffer);
            Message message = Message.DeserializeFromJson(messageText);
            Console.WriteLine($"Пришло сообщение {message} \n");
            Loging.Log(message, "receive");
            return message;
        }

        public void ServerSend(TypeSend type, Message message)
        {
            //Console.WriteLine($"Отправляю сообщение {message.Text}, {type}\n");
            Loging.Log(message, "send"); 
            string json = message.SerializeMessageToJson();
            byte[] data = Encoding.UTF8.GetBytes(json);
            switch (type)
            {
                case TypeSend.ToAll:
                    foreach (var ip in Users.Values)
                    {
                        _udpClient.Send(data, data.Length, ip);
                        sendAnsverToClient($"Сообщение отправлено для всех");
                        //Console.WriteLine($"Сообщение отправлено {ip}, {message.NicknameTo}");
                    }
                    break;
                case TypeSend.ToOne:
                    if(Users.TryGetValue(message.NicknameTo, out IPEndPoint ep))
                    {
                        _udpClient.Send(data, data.Length, ep);
                        sendAnsverToClient($"Сообщение отправлено для {message.NicknameTo}");
                        //Console.WriteLine($"Сообщение отправлено {ep}, {message.NicknameTo}");
                    }
                    break;
                case TypeSend.ToServer:
                    _manager.Execute(message, _iPEndPoint);
                    break;

            }
        }

        public void sendAnsverToClient(string message)
        {
            
            ThreadPool.QueueUserWorkItem(obj =>
            {
               
                byte[] answer = Encoding.UTF8.GetBytes(message);
                _udpClient.Send(answer, answer.Length, _iPEndPoint);
            });
        }


        public void UdpServer()
        {
            Console.WriteLine("Сервер ждет сообщение от клиента");

            ThreadPool.QueueUserWorkItem(obj =>
            {
                Console.ReadKey(true);
                cts.Cancel();
            });

            while (!token.IsCancellationRequested)
            {
                ThreadPool.QueueUserWorkItem(obj =>
                {
                    var msg = Listen();
                    var typesend = _manager.SendTo(msg);

                    ServerSend(typesend, msg);

                }, token);
            }
        }
    }
}
