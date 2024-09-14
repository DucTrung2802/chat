using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Text;
using System.Text.Json;


namespace Client
{
    public partial class Client : Form
    {
        private string clientName;
        public Client()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            clientName = Microsoft.VisualBasic.Interaction.InputBox("Nhập tên của bạn:", "Tên Client", "Client");
            Connect();
            client.Send(Serialize(clientName));
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Send();
            AddMessage(txbMessage.Text);
        }

        IPEndPoint IP;
        Socket client;

        void Connect()
        {
            IP = new IPEndPoint(IPAddress.Parse("192.168.1.53"), 9999);
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            try
            {
                client.Connect(IP);
                client.Send(Serialize(clientName));

            }
            catch
            {
                MessageBox.Show("khong the ket noi", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Thread listen = new Thread(Receive);
            listen.IsBackground = true;
            listen.Start();
        }

        

        void Close()
        {
            client.Close();
        }

        void Send()
        {
            if (txbMessage.Text != string.Empty)
            {
                // Thêm tên client vào tin nhắn
                string message = $"{clientName}: {txbMessage.Text}";
                client.Send(Serialize(message));
            }
        }

    

        void Receive()
        {
            try
            {
                byte[] data = new byte[1024]; // Sử dụng kích thước bộ đệm hợp lý hơn

                while (true)
                {
                    int bytesRead = client.Receive(data);
                    if (bytesRead == 0)
                    {
                        // Kết nối đã bị đóng
                        break;
                    }

                    byte[] receivedData = new byte[bytesRead];
                    Array.Copy(data, receivedData, bytesRead);

                    string message = (string)Deserialize(receivedData);

                    AddMessage(message);
                }
            }
            catch (SocketException ex)
            {
                // Xử lý các ngoại lệ liên quan đến socket
                Console.WriteLine($"SocketException: {ex.Message}");
            }
            catch (IOException ex)
            {
                // Xử lý các ngoại lệ IO
                Console.WriteLine($"IOException: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ khác
                Console.WriteLine($"Exception: {ex.Message}");
            }
            finally
            {
                Close();
            }
        }


        void AddMessage(string s)
        {
            lsvMessage.Items.Add(new ListViewItem() { Text = s });
            txbMessage.Clear();
        }

        byte[] Serialize(object obj)
        {
            string jsonString = JsonSerializer.Serialize(obj);
            return Encoding.UTF8.GetBytes(jsonString);
        }

        object Deserialize(byte[] data)
        {
            string jsonString = System.Text.Encoding.UTF8.GetString(data);
            return JsonSerializer.Deserialize<string>(jsonString);
        }

        private void Client_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }

    }
}