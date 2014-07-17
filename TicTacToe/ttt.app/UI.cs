using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ttt.app
{
    public partial class UI : Form
    {
        private List<Button> _spielfelder;

        public UI()
        {
            InitializeComponent();

            _spielfelder = new List<Button>
                {
                    button1,
                    button2,
                    button3,
                    button4,
                    button5,
                    button6,
                    button7,
                    button8,
                    button9
                };
        }


        private void button10_Click(object sender, EventArgs e)
        {
            Neues_Spiel_gewünscht();
        }

        private void btnSpielfeld_click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            Spielstein_gesetzt(int.Parse(btn.Tag.ToString()));
        }


        public void Spielstand_anzeigen(Spielstand spielstand)
        {
            for (var i = 0; i < spielstand.Spielbrett.Length; i++)
            {
                _spielfelder[i].Text = Spielstein_mappen(spielstand.Spielbrett[i]);
            }

            label1.Text = spielstand.Spielstatus == Spielstatusse.XamZug ? "X am Zug" : "O am Zug";
        }

        private string Spielstein_mappen(Spielsteine spielstein)
        {
            if (spielstein == Spielsteine.Keiner) return " ";
            return spielstein == Spielsteine.X ? "X" : "O";
        }


        public event Action Neues_Spiel_gewünscht;
        public event Action<int> Spielstein_gesetzt;
    }
}
