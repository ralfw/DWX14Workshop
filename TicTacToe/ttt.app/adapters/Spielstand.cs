namespace ttt.app.adapters
{
    public class Spielstand
    {
        public Spielsteine[] Spielbrett;
        public Spielstatusse Spielstatus;
    }

    public enum Spielsteine
    {
        Keiner,
        X,
        O
    }

    public enum Spielstatusse
    {
        XamZug,
        OamZug,
        Xgewonnen,
        Ogewonnen,
        Unentschieden,
        UngültigerZug
    }
}