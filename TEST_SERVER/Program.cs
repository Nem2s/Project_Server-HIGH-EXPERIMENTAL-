using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Drawing;         //For Icon
using System.Reflection;      //For Assembly
using Helper;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

//// State object for reading client data asynchronously
//public class StateObject
//{
//    // Client  socket.
//    public Socket workSocket = null;
//    // Size of receive buffer.
//    public const int BufferSize = 1024;
//    // Receive buffer.
//    public byte[] buffer = new byte[BufferSize];
//    // Received data string.
//    public StringBuilder sb = new StringBuilder();
//}

//public class AsynchronousSocketListener
//{
//    // Thread signal.
//    public static ManualResetEvent allDone = new ManualResetEvent(false);
//    public List<string> list = new List<string>();

//    public AsynchronousSocketListener()
//    {
//    }

//    public static void StartListening()
//    {
//        // Data buffer for incoming data.
//        byte[] bytes = new Byte[1024];

//        // Establish the local endpoint for the socket.
//        // The DNS name of the computer
//        // running the listener is "host.contoso.com".
//        IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
//        IPAddress ipAddress = ipHostInfo.AddressList[0];
//        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

//        // Create a TCP/IP socket.
//        Socket listener = new Socket(AddressFamily.InterNetwork,
//            SocketType.Stream, ProtocolType.Tcp);

//        // Bind the socket to the local endpoint and listen for incoming connections.
//        try
//        {
//            listener.Bind(localEndPoint);
//            listener.Listen(100);

//            while (true)
//            {
//                // Set the event to nonsignaled state.
//                allDone.Reset();

//                // Start an asynchronous socket to listen for connections.
//                Console.WriteLine("Waiting for a connection on " + ipAddress.ToString() + ":" + "11000");
//                listener.BeginAccept(
//                    new AsyncCallback(AcceptCallback),
//                    listener);

//                // Wait until a connection is made before continuing.
//                allDone.WaitOne();
//            }

//        }
//        catch (Exception e)
//        {
//            Console.WriteLine(e.ToString());
//        }

//        Console.WriteLine("\nPress ENTER to continue...");
//        Console.Read();

//    }

//    public static void AcceptCallback(IAsyncResult ar)
//    {
//        // Signal the main thread to continue.
//        allDone.Set();
//        List<string> lop = new List<string>();
//        foreach (var p in Process.GetProcesses())
//        {
//            lop.Add(p.ProcessName);
//        }

//        // Get the socket that handles the client request.
//        Socket listener = (Socket)ar.AsyncState;
//        Socket handler = listener.EndAccept(ar);

//        // Create the state object.
//        StateObject state = new StateObject();
//        state.workSocket = handler;
//        //handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
//        //new AsyncCallback(ReadCallback), state);
//        SendProcessesList(handler, lop);
//    }

//    private static void SendProcessesList(Socket handler, List<string> lop)
//    {
//        byte[] byteData = new byte[10000000];
//        foreach (string p in lop)
//        {
//            byteData = Encoding.ASCII.GetBytes(p);

//            // Begin sending the data to the remote device.
//        }

//        handler.BeginSend(byteData, 0, byteData.Length, 0,
//                new AsyncCallback(SendCallback), handler);
//    }

//    private static void SendProcessesListCallback(IAsyncResult ar)
//    {
//        try
//        {
//            // Retrieve the socket from the state object.
//            Socket handler = (Socket)ar.AsyncState;

//            // Complete sending the data to the remote device.
//            int bytesSent = handler.EndSend(ar);
//            //Console.WriteLine("Sent {0} bytes to client.", bytesSent);
//            Debug.WriteLine("Sent {0} bytes", bytesSent);
//            handler.Shutdown(SocketShutdown.Both);
//            handler.Close();

//        }
//        catch (Exception e)
//        {
//            Console.WriteLine(e.ToString());
//        }
//    }

//    public static void ReadCallback(IAsyncResult ar)
//    {
//        String content = String.Empty;

//        // Retrieve the state object and the handler socket
//        // from the asynchronous state object.
//        StateObject state = (StateObject)ar.AsyncState;
//        Socket handler = state.workSocket;

//        // Read data from the client socket. 
//        int bytesRead = handler.EndReceive(ar);

//        if (bytesRead > 0)
//        {
//            // There  might be more data, so store the data received so far.
//            state.sb.Append(Encoding.ASCII.GetString(
//                state.buffer, 0, bytesRead));

//            // Check for end-of-file tag. If it is not there, read 
//            // more data.
//            content = state.sb.ToString();
//            if (content.IndexOf("<EOF>") > -1)
//            {
//                // All the data has been read from the 
//                // client. Display it on the console.
//                Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
//                    content.Length, content);
//                // Echo the data back to the client.
//                Send(handler, content);
//            }
//            else
//            {
//                // Not all data received. Get more.
//                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
//                new AsyncCallback(ReadCallback), state);
//            }
//        }
//    }

//    private static void Send(Socket handler, String data)
//    {
//        // Convert the string data to byte data using ASCII encoding.
//        byte[] byteData = Encoding.ASCII.GetBytes(data);

//        // Begin sending the data to the remote device.
//        handler.BeginSend(byteData, 0, byteData.Length, 0,
//            new AsyncCallback(SendCallback), handler);
//    }

//    private static void SendCallback(IAsyncResult ar)
//    {
//        try
//        {
//            // Retrieve the socket from the state object.
//            Socket handler = (Socket)ar.AsyncState;

//            // Complete sending the data to the remote device.
//            int bytesSent = handler.EndSend(ar);
//            Console.WriteLine("Sent {0} bytes to client.", bytesSent);

//            handler.Shutdown(SocketShutdown.Both);
//            handler.Close();

//        }
//        catch (Exception e)
//        {
//            Console.WriteLine(e.ToString());
//        }
//    }

//    // Convert an object to a byte array
//    private static byte[] ObjectToByteArray(Object obj)
//    {
//        if (obj == null)
//            return null;

//        BinaryFormatter bf = new BinaryFormatter();
//        MemoryStream ms = new MemoryStream();
//        bf.Serialize(ms, obj);

//        return ms.ToArray();
//    }

//    // Convert a byte array to an Object
//    private static Object ByteArrayToObject(byte[] arrBytes)
//    {
//        MemoryStream memStream = new MemoryStream();
//        BinaryFormatter binForm = new BinaryFormatter();
//        memStream.Write(arrBytes, 0, arrBytes.Length);
//        memStream.Seek(0, SeekOrigin.Begin);
//        Object obj = (Object)binForm.Deserialize(memStream);

//        return obj;
//    }
//}

namespace AsyncEchoServer
{
    public class AsyncEchoServer
    {
        private int _listeningPort;
        public AsyncEchoServer(int port)
        {
            _listeningPort = port;
        }
        ///
//<summary>
        /// Start listening for connection
        /// </summary>
        public async void Start()
        {
            IPAddress ipAddre = IPAddress.Loopback;
            TcpListener listener = new TcpListener(ipAddre, _listeningPort);
            listener.Start();
            LogMessage("Server is running");
            LogMessage("Listening on " + ipAddre.ToString() + ":" + _listeningPort);

            while (true)
            {
                LogMessage("Waiting for connections...");
                try
                {
                    var tcpClient = await listener.AcceptTcpClientAsync();
                    HandleConnectionAsync(tcpClient);
                }
                catch (Exception exp)
                {
                    LogMessage(exp.ToString());
                }

            }

        }
        ///
//<summary>
        /// Process Individual client
        /// </summary>
        ///
        ///
        private async void HandleConnectionAsync(TcpClient tcpClient)
        {
            string clientInfo = tcpClient.Client.RemoteEndPoint.ToString();
            LogMessage(string.Format("Got connection request from {0}", clientInfo));
            int i = 0;
                    await Task.Run(() =>
                    {
                        Process[] arr = Process.GetProcesses();
                        IFormatter formatter = new BinaryFormatter(); // the formatter that will serialize my object on my stream
                        using (var networkStream = tcpClient.GetStream())
                        {
                            formatter.Serialize(networkStream, Environment.MachineName);
                            formatter.Serialize(networkStream, arr.Length);
                            foreach (Process p in arr)
                            {
                                ProcessDataHelper pd;
                                try
                                {
                                    Icon icon = Icon.ExtractAssociatedIcon(p.MainModule.FileName);
                                    BitmapImage bi = HelpMePlease.ToBitmapImage(icon.ToBitmap());
                                    pd = new ProcessDataHelper(p.ProcessName, HelpMePlease.GetBitmapImageByteArray(bi));
                                }
                                catch
                                {
                                    LogMessage("Can't extract icon for " + p.ProcessName);
                                    BitmapImage bi = HelpMePlease.ToBitmapImage(Bitmap.FromHicon(SystemIcons.Application.Handle));
                                    pd = new ProcessDataHelper(p.ProcessName, HelpMePlease.GetBitmapImageByteArray(bi));
                                }


                                formatter.Serialize(networkStream, pd); // the serialization process
                                i++;
                                LogMessage("Sent " + pd.Str);
                            }
                            networkStream.Flush();
                        }
                        LogMessage("Sent " + i + " processes");
                        LogMessage(string.Format("Closing the client connection - {0}",
                                    clientInfo));
                        tcpClient.Close();
                    });
                        
                
                //var dataFromServer = await reader.ReadLineAsync();
                //if (string.IsNullOrEmpty(dataFromServer))
                //{
                //    break;
                //}
                //LogMessage(dataFromServer);
                //await writer.WriteLineAsync("FromServer-" + dataFromServer);
                
        }
        private void LogMessage(string message,
                                [CallerMemberName]string callername = "")
        {
            System.Console.WriteLine("[{0}] - Thread-{1}- {2}",
                    callername, Thread.CurrentThread.ManagedThreadId, message);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            AsyncEchoServer async = new AsyncEchoServer(11000);
            async.Start();
            Console.ReadLine();
        }
    }
}