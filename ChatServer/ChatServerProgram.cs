using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatServer
{
    class ChatServerProgram
    {

        static void Main(string[] args)
        {
            MyServer a = new MyServer();
        
        }
    }
    class MyServer
    {
        public MyServer()
        {
            AsyncServerStart();
        }

        private void AsyncServerStart()
        {
            TcpListener listener = new TcpListener(new IPEndPoint(IPAddress.Any, 9999));
            listener.Start();
            Console.WriteLine("서버를 시작합니다. 클라이언트의 접속을 기다립니다.");


            
            

            while (true)
            {
                TcpClient acceptClient = listener.AcceptTcpClient();
                ClientData clientData = new ClientData(acceptClient);
                clientData.client.GetStream().BeginRead(clientData.readByteData, 0, clientData.readByteData.Length, new AsyncCallback(DataReceived), clientData);
                //BeginInvoke, BeginRead, BeginWrite 등 앞에 Begin이 붙은 메서드들은 비동기입니다.
            }
        }

        private void DataReceived(IAsyncResult ar)
        {
            //콜백 메서드란 다른 함수에 인수로 전달되는 함수이며, 이벤트 후에 실행되는 것

            ClientData callbackClient = ar.AsyncState as ClientData;

            int bytesRead = callbackClient.client.GetStream().EndRead(ar);

            string readString = Encoding.Default.GetString(callbackClient.readByteData, 0, bytesRead);

            Console.WriteLine("{0}번 사용자 : {1}", callbackClient.clientNumber, readString);

            //중요
            callbackClient.client.GetStream().BeginRead(callbackClient.readByteData, 0, callbackClient.readByteData.Length, new AsyncCallback(DataReceived), callbackClient);


        }
    }

    class ClientData
    { 
        public TcpClient client { get; set; }
        public byte[] readByteData { get; set; }

        public int clientNumber;

        public ClientData(TcpClient client)
        {
            this.client = client;
            this.readByteData = new byte[1024];

            string clientEndPoint = client.Client.LocalEndPoint.ToString();
            char[] point = { '.', ':' };
            string[] splitedData = clientEndPoint.Split(point);
            this.clientNumber = int.Parse(splitedData[3]);
            Console.WriteLine("{0}번 사용자 접속성공", clientNumber);

        }
    
    }
}
