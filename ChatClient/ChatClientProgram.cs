using System;
using System.Net.Sockets;
using System.Text;

namespace ChatClient
{
    class ChatClientProgram
    {
        TcpClient client = null;

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("==========클라이언트==========");
                Console.WriteLine("1.서버연결");
                Console.WriteLine("2.Message 보내기");
                Console.WriteLine("===============================");

                string key = Console.ReadLine();
                int order = 0;

                if (int.TryParse(key, out order))
                {
                    switch (order)
                    {
                        case 1:
                            {
                                if (client != null)
                                {
                                    Console.WriteLine("이미 연결되어있습니다.");
                                    Console.ReadKey();
                                }
                                else
                                {
                                    Connect();
                                }

                                break;

                            }
                        case 2:
                            {
                                if (client == null)
                                {
                                    Console.WriteLine("먼저 서버와 연결해주세요");
                                    Console.ReadKey();
                                }
                                else
                                {
                                    SendMessage();
                                }
                                break;
                            }
                    }
                }

                else
                {
                    Console.WriteLine("잘못 입력하셨습니다.");
                    Console.ReadKey();
                }
                Console.Clear();

            }
        }

        private void SendMessage()
        {
            Console.WriteLine("보낼 message를 입력해주세요");
            string message = Console.ReadLine();
            byte[] byteData = new byte[message.Length];
            byteData = Encoding.Default.GetBytes(message);

            client.GetStream().Write(byteData, 0, byteData.Length);
            // reference : https://docs.microsoft.com/ko-kr/dotnet/api/system.net.sockets.networkstream.write?view=net-6.0#System_Net_Sockets_NetworkStream_Write_System_Byte___System_Int32_System_Int32_

            Console.WriteLine("전송성공");
            Console.ReadKey();
        }


        private void Connect()
        {
            client = new TcpClient();
            client.Connect("127.0.0.1", 9999);
            //client.Connect("127.0.0.2", 9999);
            Console.WriteLine("서버연결 성공 이제 Message를 입력해주세요");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            ChatClientProgram ccp = new ChatClientProgram();
            ccp.Run();
        }
    }
}
