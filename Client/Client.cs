using System.Net.Sockets;

namespace DevelopNetApp_Homework_01_Client
{
    public class Client
    {


        static async Task Main()
        {
            TcpClient client = new TcpClient();

            string? userName = string.Empty;
            StreamReader? Reader = null;
            StreamWriter? Writer = null;

            Console.Write("Введите свое имя: ");
            userName = Console.ReadLine();
            Console.WriteLine($"Добро пожаловать, {userName}");
            try
            {
                client.Connect("127.0.0.1", 12345);
                Reader = new StreamReader(client.GetStream());
                Writer = new StreamWriter(client.GetStream());
                if (Writer is null || Reader is null) return;
                Task.Run(() => ReceiveMessageAsync(Reader));
                await SendMessageAsync(Writer, userName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static async Task ReceiveMessageAsync(StreamReader reader)
        {
            while (true)
            {
                try
                {
                    // считываем ответ в виде строки
                    string? message = await reader.ReadLineAsync();
                    // если пустой ответ, ничего не выводим на консоль
                    if (string.IsNullOrEmpty(message)) continue;
                    Print(message);//вывод сообщения
                }
                catch
                {
                    break;
                }
            }
        }

        static async Task SendMessageAsync(StreamWriter writer, string username)
        {
            // сначала отправляем имя
            await writer.WriteLineAsync(username);
            await writer.FlushAsync();
            Console.WriteLine("Для отправки сообщений введите сообщение и нажмите Enter");

            while (true)
            {
                string? message = Console.ReadLine();
                await writer.WriteLineAsync(message);
                await writer.FlushAsync();
            }
        }

        static void Print(string message)
        {
            if (OperatingSystem.IsWindows())    // если ОС Windows
            {
                var position = Console.GetCursorPosition(); // получаем текущую позицию курсора
                int left = position.Left;   // смещение в символах относительно левого края
                int top = position.Top;     // смещение в строках относительно верха
                                            // копируем ранее введенные символы в строке на следующую строку
                Console.MoveBufferArea(0, top, left, 1, 0, top + 1);
                // устанавливаем курсор в начало текущей строки
                Console.SetCursorPosition(0, top);
                // в текущей строке выводит полученное сообщение
                Console.WriteLine(message);
                // переносим курсор на следующую строку
                // и пользователь продолжает ввод уже на следующей строке
                Console.SetCursorPosition(left, top + 1);
            }
            else Console.WriteLine(message);
        }
    }
}

