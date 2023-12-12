using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CS_Sem_4
{
    public class Manager
    {
        public static Server _server;
        public Manager(Server server) => _server = server;

        public static void CreateUsers()
        {
            _server.Users = new Dictionary<string, IPEndPoint>();
            _server.Users.Add("Bob", new IPEndPoint(IPAddress.Parse("120.0.0.3"), 12345));
            _server.Users.Add("Jon", new IPEndPoint(IPAddress.Parse("121.0.0.3"), 12345));
            _server.Users.Add("Mary", new IPEndPoint(IPAddress.Parse("122.0.0.3"), 12345));
            /*foreach (var user in _server.Users )
            {
                Console.WriteLine($"{user.Key}, {user.Value}");
            }*/
        }

        public void Register(string user, IPEndPoint iPEndPoint)
        {
            if (_server.Users == null)
            {
                _server.Users = new Dictionary<string, IPEndPoint>();
            }
            if (Search(user))
            {
                _server.Users.Add(user, iPEndPoint);
            }
        }

        public void Delete(string user)
        {
            _server.Users.Remove(user);
            Console.WriteLine($"Пользователь {user} удален");
        }

        public void Execute(Message message, IPEndPoint iPEndPoint)
        {
            if (message.Text.ToLower().Equals("delete"))
            {
                _server.sendAnsverToClient($"Пользователь {message.NicknameFrom} удален");
                Delete(message.NicknameFrom);
            }
            else if (message.Text.ToLower().Equals("register"))
            {
                _server.sendAnsverToClient($"Пользователь {message.NicknameFrom} создан");
                Register(message.NicknameFrom, iPEndPoint);
            }
            else if (message.Text.ToLower().Equals("showusers"))
            {
                string answer = "";
                foreach (var user in _server.Users)
                {
                    answer += $"{user.Key}\n";
                }
                _server.sendAnsverToClient(answer);
            }
            else
            {
                _server.sendAnsverToClient($"Команды {message.Text} нет. Используйте 'delete', 'register', 'showusers' ");
            }
        }

        public TypeSend SendTo(Message message)
        {
            if (string.IsNullOrEmpty(message.NicknameTo))
            {
                return TypeSend.ToAll;
            }
            else if (message.NicknameTo.ToLower().Equals("server"))
            {
                return TypeSend.ToServer;
            }
            else return TypeSend.ToOne;
        }

        public bool Search(string user)
        {
            foreach (var u in _server.Users.ToList())
            {
                if (u.Key.Equals(user))
                {
                    Console.WriteLine($"Юзер {user} уже создан");
                    return false;
                }
            } 
            return true;
        }
    }
}
