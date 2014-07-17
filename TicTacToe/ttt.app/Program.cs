using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using dwx14.eventstore;
using ttt.app.adapters;
using ttt.app.domain;

namespace ttt.app
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var es = new EventStore();
            var ui = new UI();
            var sp = new Spiel(es);
            var map = new Mapper(es);
            var app = new App(sp, map);

            app.Spielstand_aktualisiert += ui.Spielstand_anzeigen;

            ui.Neues_Spiel_gewünscht += app.Neues_Spiel;
            ui.Spielstein_gesetzt += app.Zug_ausführen;

            app.Starten();

            Application.Run(ui);
        }
    }
}
