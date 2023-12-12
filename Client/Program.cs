using CS_Sem_4;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new Thread(() => SendMessage( "Alex", "127.0.0.1")).Start();
        }


        public static void SendMessage(string From, string ip, string to = "")
        {

            bool flag = true;
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);
            using (UdpClient udpClient = new UdpClient())
            {


                while (flag)
                {
                    string messageText;
                    do
                    {
                        Console.WriteLine("Кому отправить сообщение?\n(пустая строка - для всех");
                        to = Console.ReadLine();
                        Console.WriteLine("Введите сообщение \n(\"exit\" для выхода)");
                        messageText = Console.ReadLine();
                        if (messageText.ToLower().Equals("exit")) { flag = false; }

                    }
                    while (string.IsNullOrEmpty(messageText));

                    if (flag)
                    {
                        Message message = new Message() 
                        { Text = messageText, NicknameFrom = From, NicknameTo = to, DateTime = DateTime.Now };
                        string json = message.SerializeMessageToJson();

                        byte[] data = Encoding.UTF8.GetBytes(json);
                        udpClient.Send(data, data.Length, iPEndPoint);

                        byte[] buffer = udpClient.Receive(ref iPEndPoint);
                        var answer = Encoding.UTF8.GetString(buffer);
                        Console.WriteLine(answer);
                    }
                }
            }
        }
    }
}
