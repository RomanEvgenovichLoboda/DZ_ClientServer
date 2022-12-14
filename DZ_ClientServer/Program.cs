using System.Net;
using System.Net.Sockets;
using System.Text;

const int PORT = 8088;
const string IP = "127.0.0.1";
Console.WriteLine("Server start...");
IPEndPoint iPEnd = new IPEndPoint(IPAddress.Parse(IP), PORT);
Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
try
{
    serverSocket.Bind(iPEnd);
    serverSocket.Listen(10);
    Socket clientSocket = serverSocket.Accept();
    do
    {
        Console.WriteLine($"Server listen on {IP}:{PORT}");
        int bytes = 0;
        byte[] buffer = new byte[1024];
        StringBuilder builder = new StringBuilder();
        do
        {
            bytes = clientSocket.Receive(buffer);
            builder.Append(Encoding.Unicode.GetString(buffer, 0, bytes));
        } while (clientSocket.Available > 0);
        Console.WriteLine($"Equation:\t{builder.ToString()}");
        try
        {
            string number1 = "";
            string number2 = "";
            string symbol = "";
            double result = 0;
            foreach (var item in builder.ToString())
            { 
                if (item.ToString() == "*" || item.ToString() == "/" || item.ToString() == "+" || item.ToString() == "-") { symbol = item.ToString(); }
                else if (symbol == "") { number1 += item.ToString(); }
                else if (symbol != "" && number1 != "") { number2 += item.ToString(); }
            }
            switch (symbol)
            {
                case "*":
                    {
                        result = double.Parse(number1) * double.Parse(number2);
                        break;
                    }
                case "/":
                    {
                        result = double.Parse(number1) / double.Parse(number2);
                        break;
                    }
                case "+":
                    {
                        result = double.Parse(number1) + double.Parse(number2);
                        break;
                    }
                case "-":
                    {
                        result = double.Parse(number1) - double.Parse(number2);
                        break;
                    }
                default:
                    break;
            }
            byte[] data = Encoding.Unicode.GetBytes(result.ToString());
            clientSocket.Send(data);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            byte[] data = Encoding.Unicode.GetBytes("Boroda");
            clientSocket.Send(data);
        }
    } while (true);
    clientSocket.Close();
    serverSocket.Shutdown(SocketShutdown.Both);
    serverSocket.Close();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
Console.WriteLine("Server end...");
Console.ReadLine();