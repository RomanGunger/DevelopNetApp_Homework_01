using System;
namespace DevelopNetApp_Homework_01
{
	public class Program
	{
		static async Task Main()
		{
            ServerObject server = new ServerObject();
			await server.ListenAsync();
        }
	}
}

