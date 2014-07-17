using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            var ui = new UI();
            var app = new App();

            app.Spielstand_aktualisiert += ui.Spielstand_anzeigen;

            ui.Neues_Spiel_gewünscht += app.Neues_Spiel;
            ui.Spielstein_gesetzt += app.Zug_ausführen;

            app.Starten();

            Application.Run(ui);
        }
    }
}
