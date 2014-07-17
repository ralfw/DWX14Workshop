using System;
using ttt.app.adapters;
using ttt.app.domain;

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
            Spielstand_generieren(spieler);
        }
 

        public void Zug_ausführen(int spielfeldindex)
        {
            _spiel.Zug_validieren(spielfeldindex, 
                () => {
                    var spieler = _spiel.Spieler_feststellen();
                    _spiel.Spielstein_setzen(spieler, spielfeldindex);
                    spieler = _spiel.Spieler_feststellen();
                    Spielstand_generieren(spieler);
                },
                () => Spielstand_generieren(Spielstatusse.UngültigerZug));

        }

        private void Spielstand_generieren(Spielstatusse spieler)
        {
            var spielzüge = _spiel.Spielzüge_des_aktuellen_Spiels_ermittteln();
            var spielstand = _map.Spieldstand_generieren(spielzüge, spieler);
            Spielstand_aktualisiert(spielstand);
        }


        public event Action<Spielstand> Spielstand_aktualisiert;
    }
}