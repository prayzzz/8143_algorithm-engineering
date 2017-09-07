package icosoku;

import java.util.LinkedList;

/**
 * Created by Patrick on 22.10.2014.
 */
public class IcoSokuSolver
{
    public static boolean Solve(IcoSoku ico, int flaeche)
    {
        // Rätsel gelöst
        if (flaeche > 19)
        {
            return ico.pruefeAlles();
        }

        LinkedList<Integer> pieces = GetAvailablePieces(ico);

        // Solange Plättchen noch nicht angeschaut wurden
        while (pieces.size() > 0)
        {
            int plaettchen = pieces.get(0);

            for (int orient = 0; orient < 3; orient++)
            {
                ico.setzePlaettchen(flaeche, plaettchen, orient);
                if (ico.pruefeFlaeche(flaeche))
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
                ico.entfernePlaettchen(flaeche);

                // rotieren bei gleichen Plättchen-Zahlen abbrechen
                if (ico.istPlaettchenSymmetrisch(plaettchen))
                {
                    break;
                }
            }

            // Plättchen passt mit keiner Orientierung, entferne es und alle mit den gleichen Werten
            EntfernePlaettchen(pieces, plaettchen);
        }

        return false;
    }

    private static void EntfernePlaettchen(LinkedList<Integer> pieces, int plaettchen)
    {
        if (plaettchen == 5 || plaettchen == 6 || plaettchen == 7)
        {
            pieces.remove((Object) 5);
            pieces.remove((Object) 6);
            pieces.remove((Object) 7);

            return;
        }

        if (plaettchen == 8 || plaettchen == 9 || plaettchen == 10)
        {
            pieces.remove((Object) 8);
            pieces.remove((Object) 9);
            pieces.remove((Object) 10);

            return;
        }

        if (plaettchen == 14 || plaettchen == 15)
        {
            pieces.remove((Object) 14);
            pieces.remove((Object) 15);

            return;
        }

        if (plaettchen == 16 || plaettchen == 17)
        {
            pieces.remove((Object) 16);
            pieces.remove((Object) 17);

            return;
        }

        pieces.remove((Object) plaettchen);
    }

    private static LinkedList<Integer> GetAvailablePieces(IcoSoku ico)
    {
        LinkedList<Integer> pieces = new LinkedList<>();

        for (int plaettchen = 0; plaettchen < 20; plaettchen++)
        {
            if (ico.istPlaettchenVerfuegbar(plaettchen))
            {
                pieces.add(plaettchen);
            }
        }

        return pieces;
    }
}
