using System.Collections.Generic;
using System.Linq;

namespace IcoSoku
{
    public class IcoSokuSolver
    {
        public static bool Solve(IcoSoku ico, int flaeche)
        {
            // Rätsel gelöst
            if (flaeche > 19)
            {
                return ico.PruefeAlles();
            }

            var pieces = GetAvailablePieces(ico);

            // Solange Plättchen noch nicht angeschaut wurden

            while (pieces.Count > 0)
            {
                var plaettchen = pieces.First();

                for (var orient = 0; orient < 3; orient++)
                {
                    ico.SetzePlaettchen(flaeche, plaettchen, orient);
                    if (ico.PruefeFlaeche(flaeche))
                    {
                        // Plättchen korrekt platziert, gehe zu nächster Fläche
                        if (Solve(ico, flaeche + 1))
                        {
                            // Wenn Rätsel gelöst, verlasse Rekursion
                            return true;
                        }
                    }

                    // Falsch platziert oder Rätsel noch nicht gelöst
                    // Entferne platziertes Plättchen
                    ico.EntfernePlaettchen(flaeche);

                    // rotieren bei gleichen Plättchen-Zahlen abbrechen
                    if (ico.IstPlaettchenSymmetrisch(plaettchen))
                    {
                        break;
                    }
                }

                // Plättchen passt mit keiner Orientierung, entferne es und alle mit den gleichen Werten
                EntfernePlaettchen(pieces, plaettchen);
            }

            return false;
        }

        private static void EntfernePlaettchen(LinkedList<int> pieces, int plaettchen)
        {
            if (plaettchen == 5 || plaettchen == 6 || plaettchen == 7)
            {
                pieces.Remove(5);
                pieces.Remove(6);
                pieces.Remove(7);

                return;
            }

            if (plaettchen == 8 || plaettchen == 9 || plaettchen == 10)
            {
                pieces.Remove(8);
                pieces.Remove(9);
                pieces.Remove(10);

                return;
            }

            if (plaettchen == 14 || plaettchen == 15)
            {
                pieces.Remove(14);
                pieces.Remove(15);

                return;
            }

            if (plaettchen == 16 || plaettchen == 17)
            {
                pieces.Remove(16);
                pieces.Remove(17);

                return;
            }

            pieces.Remove(plaettchen);
        }

        private static LinkedList<int> GetAvailablePieces(IcoSoku ico)
        {
            var pieces = new LinkedList<int>();

            for (var plaettchen = 0; plaettchen < 20; plaettchen++)
            {
                if (ico.IstPlaettchenVerfuegbar(plaettchen))
                {
                    pieces.AddLast(plaettchen);
                }
            }

            return pieces;
        }
    }
}