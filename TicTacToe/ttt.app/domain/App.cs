using System;
using System.Linq;
using ttt.app.adapters;
using ttt.app.domain;

namespace ttt.app
{
    public class App
    {
        private readonly Spiel _spiel;
        private readonly Schiedsrichter _schiri;
        private readonly Mapper _map;

        public App(Spiel spiel, Schiedsrichter schiri, Mapper map)
        {
            _spiel = spiel;
            _schiri = schiri;
            _map = map;
        }


        public void Starten()
        {
            _spiel.Laufendes_Spiel_zusichern();
            Spiel_fortsetzen();
        }


        public void Neues_Spiel()
        {
            _spiel.Neues_Spiel_beginnen();
            Spiel_fortsetzen();
        }

        private void Spiel_fortsetzen()
        {
            _schiri.Spielende_schon_erreicht(
                () => {
                    var spieler = _spiel.Spieler_feststellen();
                    Spielstand_generieren(spieler);
                },
                Spielstand_generieren);
        }
 

        public void Zug_ausführen(int spielfeldindex)
        {
            _schiri.Spielende_schon_erreicht(
                () => _spiel.Zug_validieren(spielfeldindex,
                        () => {
                                var spieler = _spiel.Spieler_feststellen();
                                _spiel.Spielstein_setzen(spieler, spielfeldindex);
                                  var spielzüge = _spiel.Spielzüge_des_aktuellen_Spiels_ermittteln().ToArray();
                                _schiri.Prüfe_auf_Spielende(spielzüge,
                                    () => {
                                        spieler = _spiel.Spieler_feststellen();
                                        Spielstand_generieren(spieler);
                                    },
                                    Spielstand_generieren);
                            },
                        () => Spielstand_generieren(Spielstatusse.UngültigerZug)),
                Spielstand_generieren);
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