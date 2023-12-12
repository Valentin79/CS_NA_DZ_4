﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CS_Sem_4

    
{
    
    public class Message
    {
        public Commands command { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public string NicknameFrom { get; set; }
        public string NicknameTo { get; set; }

        public string SerializeMessageToJson()
        {
            return JsonSerializer.Serialize(this);
        }
        // Метод сверху можно сделать так же. => - вместо ретурн.
        public static Message? DeserializeFromJson(string message) => JsonSerializer.Deserialize<Message>(message);

        public void Print()
        {
            Console.WriteLine(ToString());
        }

        public override string ToString()
        {
            return $"{this.DateTime} получено сообщение {this.Text} от {this.NicknameFrom}";
        }
    }
}
