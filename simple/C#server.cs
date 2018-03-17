using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Collections;

public class serv
{
    protected bool receivedEnd = false;
    public static Hashtable clientsList = new Hashtable();
    
    public static void Main()
    {
        while (true)
        { 
            string receivedData = "";
            byte[] b = new byte[100];

            try
            {
                IPAddress ipAd = IPAddress.Parse("192.168.0.107");
                TcpListener myList = new TcpListener(ipAd, 4444);

                myList.Start();

                Console.WriteLine("The server is running at port 8001...");
                Console.WriteLine("The local End point is  :" + myList.LocalEndpoint);
                Console.WriteLine("Waiting for a connection.....");

                Socket s = myList.AcceptSocket();
                Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);

                /*int k = s.Receive(b);
                for (int i = 0; i < k; i++)
                    receivedData += (Convert.ToChar(b[i])).ToString();
                clientsList.Add(receivedData, s);*/

                Client client = new Client();
                client.startClient(s);
                
            }
            catch (Exception e)
            {
                Console.WriteLine("main connecting: Error..... " + e.StackTrace);
            }
        }
    }

    public void broadcast()
    {

    }
}

class Client{
    Socket s;
    public void startClient(Socket socket)
    {
        Console.WriteLine("Already in startClient");
        this.s = socket;
        Thread threadChat = new Thread(doChat);
        threadChat.Start();
    }

    public void doChat()
    {
        Console.WriteLine("In doChat!");
        while (true)
        {
            Console.WriteLine("In doChat: while");
            try
            {
                string receivedData = null;
                byte[] b = new byte[100];
                int k = s.Receive(b);

                Console.WriteLine("Recieved...");
                for (int i = 0; i < k; i++)
                    receivedData += (Convert.ToChar(b[i])).ToString();
                Console.WriteLine(receivedData);

                ASCIIEncoding asen = new ASCIIEncoding();
                s.Send(asen.GetBytes("The string was recieved by the server."));
            }
            catch(Exception e)
            {
                Console.WriteLine("doChat: Error..... " + e.StackTrace);
            }
        }
        //s.Close();
        //myList.Stop();
    }
}