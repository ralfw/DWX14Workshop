using System;

namespace ttt.app
{
    public class App
    {
        private readonly Spiel _spiel;
        private readonly Mapper _map;

        public App(Spiel spiel, Mapper map)
        {
            _spiel = spiel;
            _map = map;
        }


        public void Starten()
        {
            Neues_Spiel();
        }


        public void Neues_Spiel()
        {
            _spiel.Neues_Spiel_beginnen();
            var spieler = _spiel.Spieler_feststellen();
            var spielstand = _map.Spieldstand_generieren(spieler);
            Spielstand_aktualisiert(spielstand);
        }
 

        public void Zug_ausführen(int spielfeldindex)
        {
            var spieler = _spiel.Spieler_feststellen();
            _spiel.Spielstein_setzen(spieler, spielfeldindex);
            spieler = _spiel.Spieler_feststellen();
            var spielstand = _map.Spieldstand_generieren(spieler);
            Spielstand_aktualisiert(spielstand);
        }

        
        public event Action<Spielstand> Spielstand_aktualisiert;
    }
}