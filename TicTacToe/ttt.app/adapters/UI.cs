using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ttt.app.adapters
{
    public partial class UI : Form
    {
        private readonly List<Button> _spielfelder;

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

            switch (spielstand.Spielstatus)
            {
                case Spielstatusse.XamZug:
                    label1.Text = "X am Zug!";
                    break;
                case Spielstatusse.OamZug:
                    label1.Text = "O am Zug";
                    break;
                case Spielstatusse.UngültigerZug:
                    label1.Text = "Ungültiger Zug!";
                    break;
                case Spielstatusse.Unentschieden:
                    label1.Text = "Unentschieden! Das Spiel ist zuende.";
                    break;
                case Spielstatusse.Xgewonnen:
                    label1.Text = "Gewonnen: X!";
                    break;
                case Spielstatusse.Ogewonnen:
                    label1.Text = "Gewonnen: O!";
                    break;
                default:
                    label1.Text = spielstand.Spielstatus.ToString();
                    break;
            }
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
