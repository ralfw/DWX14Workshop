using System;

namespace ttt.app
{
    public class App
    {
        public void Starten()
        {
            Neues_Spiel();
        }

        public void Neues_Spiel()
        {
            var spielstand = new Spielstand
            {
                Spielbrett = new Spielsteine[9],
                Spielstatus = Spielstatusse.XamZug
            };
            Spielstand_aktualisiert(spielstand);
        }
 
        public void Zug_ausführen(int spielfeldindex)
        {
            var spielstand = new Spielstand
            {
                Spielbrett = new Spielsteine[9],
                Spielstatus = Spielstatusse.XamZug
            };

            spielstand.Spielbrett[spielfeldindex] = Spielsteine.X;
            spielstand.Spielstatus = spielfeldindex%2 == 0 ? Spielstatusse.XamZug : Spielstatusse.OamZug;

            Spielstand_aktualisiert(spielstand);
        }

        
        public event Action<Spielstand> Spielstand_aktualisiert;
    }
}