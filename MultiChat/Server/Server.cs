using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;

namespace Server
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Connect();
        }

        private void Server_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            foreach (Socket item in clientList)
            {
                Send(item);
            }

            AddMessage(txbMessage.Text);

        }

        IPEndPoint IP;
        Socket server;
        List<Socket> clientList;

        void Connect()
        {
            clientList = new List<Socket>();
            IP = new IPEndPoint(IPAddress.Any, 9999);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            server.Bind(IP);
            Thread Listen = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        server.Listen(100);
                        Socket client = server.Accept();
                        clientList.Add(client);

                        // Nhận tên client từ client
                        byte[] nameBuffer = new byte[1024];
                        int nameBytes = client.Receive(nameBuffer);
                        string clientName = Encoding.UTF8.GetString(nameBuffer, 0, nameBytes);

                        // Thêm tên client vào ListBox
                        Invoke((MethodInvoker)(() =>
                        {
                            ListClient.Items.Add(clientName);
                        }));

                        Thread receive = new Thread(Receive);
                        receive.IsBackground = true;
                        receive.Start(client);
                    }
                }
                catch
                {
                    IP = new IPEndPoint(IPAddress.Parse("192.168.1.122"), 9999);
                    server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                }
            });
            Listen.IsBackground = true;
            Listen.Start();
        }


        


        void Close()
        {
            server.Close();
        }

        void Send(Socket client)
        {
            if (txbMessage.Text != string.Empty)
            {
                // Thêm tên client vào tin nhắn
                string message = $"SERVER: {txbMessage.Text}";
                client.Send(Serialize(message));
            }
            txbMessage.Clear();
        }


        void Receive(object obj)
        {
            Socket client = obj as Socket;
            string clientName = string.Empty;

            try
            {
                // Đọc tên client từ client (có thể bạn đã làm điều này ở nơi khác)
                byte[] nameBuffer = new byte[1024];
                int nameBytes = client.Receive(nameBuffer);
                clientName = Encoding.UTF8.GetString(nameBuffer, 0, nameBytes);

                while (true)
                {
                    byte[] data = new byte[1024 * 5000];
                    int bytesReceived = client.Receive(data);

                    if (bytesReceived == 0)
                    {
                        // Client đã ngắt kết nối
                        break;
                    }

                    string message = (string)Deserialize(data);
                    AddMessage(message);
                }
            }
            catch
            {
                // Xử lý lỗi ngắt kết nối không mong muốn
            }
            finally
            {
                // Xóa client khỏi danh sách và cập nhật giao diện người dùng
                clientList.Remove(client);
                UpdateClientList();

                client.Close();
            }
        }

        void UpdateClientList()
        {
            // Xóa tất cả các tên client hiện có trong ListBox
            ListClient.Items.Clear();

            // Thêm lại tất cả các tên client từ danh sách client
            foreach (Socket client in clientList)
            {
                // Đọc tên client từ client (có thể bạn đã làm điều này ở nơi khác)
                byte[] nameBuffer = new byte[1024];
                int nameBytes = client.Receive(nameBuffer);
                string clientName = Encoding.UTF8.GetString(nameBuffer, 0, nameBytes);

                ListClient.Items.Add(clientName);
            }
        }



        void AddMessage(string s)
        {
            lsvMessage.Items.Add(new ListViewItem() { Text = s });
        }

        byte[] Serialize(object obj)
        {
            string jsonString = JsonSerializer.Serialize(obj);
            return System.Text.Encoding.UTF8.GetBytes(jsonString);
        }

        object Deserialize(byte[] data)
        {
            string jsonString = System.Text.Encoding.UTF8.GetString(data).TrimEnd('\0');
            return JsonSerializer.Deserialize<string>(jsonString);
        }

        private void txbMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void ListClient_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}