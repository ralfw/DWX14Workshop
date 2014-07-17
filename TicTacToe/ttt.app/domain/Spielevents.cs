namespace ttt.app.domain
{
    public static class Spielevents
    {
        public const string EVENT_NEUES_SPIEL = "Neues Spiel begonnen"; // Spielstatus/Spieler
        public const string EVENT_SPIELSTEIN_GESETZT = "Spielstein gesetzt"; // Spielstatus/Spieler, Spielfeldindex
        public const string EVENT_SPIEL_GEWONNEN = "Spiel gewonnen"; // Spielstatus/Spieler
        public const string EVENT_SPIEL_UNENTSCHIEDEN = "Spiel unentschieden";
    }
}