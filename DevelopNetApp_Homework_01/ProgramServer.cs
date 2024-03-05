
using System.Net;
using System.Net.Sockets;
using System.Text;

class ProgramServer
{
    static void Main()
    {
        using (Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
            var localEndPoint = new IPEndPoint(IPAddress.Parse("192.168.50.100"), 12345);

            listener.Blocking = true;

            listener.Bind(localEndPoint); //связывает объект Socket с локальной конечной точкой

            listener.Listen(100); //начинает прослушивание входящих запросов

            Console.WriteLine("Waiting for connection...");

            var socket = listener.Accept(); //создает новый объект Socket для обработки входящего подключения

            Console.WriteLine("Connected!");

            byte[] buffer = new byte[255];

            int count = socket.Receive(buffer); //получает данные

            if (count > 0)
            {
                string message = Encoding.UTF8.GetString(buffer);
                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine("nothing");
            }

            try
            {
                socket.Shutdown(SocketShutdown.Both); //При работе с протоколами, ориентированными на установку соединения,
                                                      //например, TCP, Microsoft рекомендует перед методом Close вызывать метод Shutdown,
                                                      //который блокирует примем и отправку данных для сокета и гарантирует,
                                                      //что все данные будут получены и отправлены перед закрытием сокета.

                        //В качестве параметра в метод Shutdown передается значение из перечисления SocketShutdown:
        //Send: блокируется отправка данных
        //Receive: блокируется получение данных
        //Both: блокируются отправка и получение данных
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                socket.Close(); //После завершения работы с сокетом рекомендуется его закрыть.
                                //Для этого можно использовать метод Close().
                                //Он закрывает подключение с удаленным хостом и освобождает все управляемые и неуправляемые ресурсы,
                                //связанные с сокетом. После этого свойство Connected будет равно false
            }
        }
    }
}