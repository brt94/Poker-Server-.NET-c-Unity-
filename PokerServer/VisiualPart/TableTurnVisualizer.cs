using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientServerCommonLibrary;
using Color = ClientServerCommonLibrary.Color;

namespace PokerServer
{
    public partial class TableTurnVisualizer : Form
    {
        Table masa;
        public TableTurnVisualizer(Table table)
        {
            masa = table;
            InitializeComponent();
            //timer1.
            timer1.Start();
        }

        public System.Drawing.Color GetColor(Color c)
        {
            switch (c){
                case Color.InimaNeagra:
                    {
                        return System.Drawing.Color.Black;
                       // break;
                    }
                case Color.InimaRosie:
                    {
                        return System.Drawing.Color.Red;
                    }
                case Color.Romb:
                    {
                        return System.Drawing.Color.Blue;
                    }
                case Color.Trefla:
                    {
                        return System.Drawing.Color.Green;
                    }                   
            }
            return System.Drawing.Color.Black;
        }

        private void TableTurnVisualizer_Load(object sender, EventArgs e)
        {
            playerVisualizer1.client = masa.Clienti[0];
            playerVisualizer2.client = masa.Clienti[1];
            playerVisualizer3.client = masa.Clienti[2];
            playerVisualizer4.client = masa.Clienti[3];
            playerVisualizer5.client = masa.Clienti[4];
            playerVisualizer6.client = masa.Clienti[5];
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label6.Text = masa.bigBet.ToString();
            label2.Text = masa.round.ToString();
            label4.Text = masa.Pot.ToString();
            if (masa.Clienti[0] != null)
            {
                panel1.Visible = masa.Clienti[0].isTurn;
                p1c1.ForeColor = GetColor(masa.Clienti[0].card1.Culoare);
                p1c2.ForeColor = GetColor(masa.Clienti[0].card2.Culoare);
                p1c1.Text = masa.Clienti[0].card1.Numar.ToString();
                p1c2.Text = masa.Clienti[0].card2.Numar.ToString();
                p0bet.Text = masa.Clienti[0].onhandBet.ToString();
            }
            //
            if (masa.Clienti[1] != null)
            {
                panel2.Visible = masa.Clienti[1].isTurn;
                p2c1.ForeColor = GetColor(masa.Clienti[1].card1.Culoare);
                p2c2.ForeColor = GetColor(masa.Clienti[1].card2.Culoare);
                p2c1.Text = masa.Clienti[1].card1.Numar.ToString();
                p2c2.Text = masa.Clienti[1].card2.Numar.ToString();
                p1bet.Text = masa.Clienti[1].onhandBet.ToString();
            }
            //
            if (masa.Clienti[2] != null)
            {
                panel3.Visible = masa.Clienti[2].isTurn;
                p3c1.ForeColor = GetColor(masa.Clienti[2].card1.Culoare);
                p3c2.ForeColor = GetColor(masa.Clienti[2].card2.Culoare);
                p3c1.Text = masa.Clienti[2].card1.Numar.ToString();
                p3c2.Text = masa.Clienti[2].card2.Numar.ToString();
                p2bet.Text = masa.Clienti[2].onhandBet.ToString();
            }
            //
            if (masa.Clienti[3] != null)
            {
                panel4.Visible = masa.Clienti[3].isTurn;
                p4c1.ForeColor = GetColor(masa.Clienti[3].card1.Culoare);
                p4c2.ForeColor = GetColor(masa.Clienti[3].card2.Culoare);
                p4c1.Text = masa.Clienti[3].card1.Numar.ToString();
                p4c2.Text = masa.Clienti[3].card2.Numar.ToString();
                p3bet.Text = masa.Clienti[3].onhandBet.ToString();
            }
            //
            if (masa.Clienti[4] != null)
            {
                panel5.Visible = masa.Clienti[4].isTurn;
                p5c1.ForeColor = GetColor(masa.Clienti[4].card1.Culoare);
                p5c2.ForeColor = GetColor(masa.Clienti[4].card2.Culoare);
                p5c1.Text = masa.Clienti[4].card1.Numar.ToString();
                p5c2.Text = masa.Clienti[4].card2.Numar.ToString();
                p4bet.Text = masa.Clienti[4].onhandBet.ToString();
            }
            //
            if (masa.Clienti[5] != null)
            {
                panel6.Visible = masa.Clienti[5].isTurn;
                p6c1.ForeColor = GetColor(masa.Clienti[5].card1.Culoare);
                p6c2.ForeColor = GetColor(masa.Clienti[5].card2.Culoare);
                p6c1.Text = masa.Clienti[5].card1.Numar.ToString();
                p6c2.Text = masa.Clienti[5].card2.Numar.ToString();
                p5bet.Text = masa.Clienti[5].onhandBet.ToString();
            }
            try
            {
                playerVisualizer1.Visible = masa.Clienti[0].isTurn;
                playerVisualizer2.Visible = masa.Clienti[1].isTurn;
                playerVisualizer3.Visible = masa.Clienti[2].isTurn;
                playerVisualizer4.Visible = masa.Clienti[3].isTurn;
                playerVisualizer5.Visible = masa.Clienti[4].isTurn;
                playerVisualizer6.Visible = masa.Clienti[5].isTurn;
            }
            catch
            {

            }
            try
            {
                f1.ForeColor = GetColor(masa.TableCards[0].Culoare);
                f1.Text = masa.TableCards[0].Numar.ToString();
                //
                f2.ForeColor = GetColor(masa.TableCards[1].Culoare);
                f2.Text = masa.TableCards[1].Numar.ToString();
                //
                f3.ForeColor = GetColor(masa.TableCards[2].Culoare);
                f3.Text = masa.TableCards[2].Numar.ToString();
                //
                r.ForeColor = GetColor(masa.TableCards[3].Culoare);
                r.Text = masa.TableCards[3].Numar.ToString();
                //
                t.ForeColor = GetColor(masa.TableCards[4].Culoare);
                t.Text = masa.TableCards[4].Numar.ToString();
            }
            catch
            {

            }
        }

        private void playerVisualizer1_Load(object sender, EventArgs e)
        {

        }
    }
}
