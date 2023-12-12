using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Sem_4
{
    internal class Loging
    {

        static List<Message> receiveMessages = new List<Message>();
        static List<Message> sendMessages = new List<Message>();

        
        public static void Log(Message message, string type)
        {
           
            if (type.Equals("receive"))
            {
                
                receiveMessages.Add(message);
            }
            if (type.Equals("send"))
            {
                
                sendMessages.Add(message);
            }
        }

        public static void PrintReceiveLog()
        {
            foreach (var message in receiveMessages)
            {
                Console.WriteLine($"{message.DateTime}: Принято: {message}");
            }
        }
        public static void PrintSendLog()
        {
            foreach (var message in sendMessages)
            {
                if (string.IsNullOrEmpty(message.NicknameTo))
                {
                    message.NicknameTo = "ToAll";
                }
                Console.WriteLine($"{message.DateTime}: Отправленно для: {message.NicknameTo} {message.Text}");
            }
        }
    }
}
