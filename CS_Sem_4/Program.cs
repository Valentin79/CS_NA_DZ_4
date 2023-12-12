namespace CS_Sem_4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            Manager.CreateUsers(); // несколько юзеров для теста рассылки
            server.UdpServer();
            Console.WriteLine("log\n");
            Loging.PrintReceiveLog();
            Loging.PrintSendLog();
        }
    }
}
