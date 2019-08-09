using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokerServer.VisiualPart
{
    public partial class PlayerVisualizer : UserControl
    {
        public Client _client;
        public Client client
        {
            get
            {
                return _client;
            }
            set
            {
                if (value != null)
                {
                    _client = value;
                    masa = client.masa;
                }
            }
        }
        public Table masa;
        public PlayerVisualizer()//(Client _client,Table _masa)
        {
            //Enabled = false;
            InitializeComponent();
            // client = _client;
            // masa = _masa;
            VisibleChanged += PlayerVisualizer_EnabledChanged;
        }

        private void PlayerVisualizer_EnabledChanged(object sender, EventArgs e)
        {

            if (client == null) return;
            button4.Visible = !client.isActive;
            //if (masa.bigBet > masa.tableConfig.bigblind)
            trackBar1.Minimum = masa.bigBet + masa.tableConfig.bigblind;
            trackBar1.Maximum = (int)client.dlinq.Money;
            var call = masa.bigBet - client.onhandBet;
            if (call == 0)
            {
                button1.Visible = false;
                button2.Text = "CHECK";
            }
            else
            {
                button1.Visible = true;
                button2.Text = "CALL(" + (masa.bigBet - client.onhandBet) + ")";
            }

        }

        private void PlayerVisualizer_Load(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (client != null)
            client.Bet(trackBar1.Value-client.onhandBet);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(client !=null)
            client.Call();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(client !=null)
            client.FoldCards();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            button3.Text = "Bet("+ trackBar1.Value+")";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //client.ImBack();
        }
    }
}
