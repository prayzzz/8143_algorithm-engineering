package icosoku;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by Patrick on 22.10.2014.
 */
public class IcoSokuSolver2
{
    public static void Solve(IcoSoku ico, int flaeche)
    {
        if (ico.pruefeAlles())
        {
            return;
        }

        flaeche++;
        List<Plaettchen> pieces = GetAvailablePieces(ico);

        while (pieces.size() > 0)
        {
            Plaettchen plaettchen = pieces.get(0);
            // pieces.remove(0);
            EntfernePlaettchen(pieces, plaettchen);

            if (!SetzePlaettchen(ico, flaeche, plaettchen.getPlaettchen(), plaettchen.getOrient()))
            {
                continue;
            }

            Solve(ico, flaeche);

            if (ico.pruefeAlles())
            {
                return;
            }
        }

        ico.entfernePlaettchen(flaeche);
    }

    /**
     * Doppelte Plätchen werden entfernt
     *
     * @param pieces
     * @param pl
     */
    private static void EntfernePlaettchen(List<Plaettchen> pieces, Plaettchen pl)
    {
        int plaettchen = pl.getPlaettchen();
        int orient = pl.getOrient();

        if (plaettchen == 5 || plaettchen == 6 || plaettchen == 7)
        {
            pieces.removeIf(x -> x.getPlaettchen() == 5 && x.getOrient() == orient);
            pieces.removeIf(x -> x.getPlaettchen() == 6 && x.getOrient() == orient);
            pieces.removeIf(x -> x.getPlaettchen() == 7 && x.getOrient() == orient);
            return;
        }

        if (plaettchen == 8 || plaettchen == 9 || plaettchen == 10)
        {
            pieces.removeIf(x -> x.getPlaettchen() == 8 && x.getOrient() == orient);
            pieces.removeIf(x -> x.getPlaettchen() == 9 && x.getOrient() == orient);
            pieces.removeIf(x -> x.getPlaettchen() == 10 && x.getOrient() == orient);

            return;
        }

        if (plaettchen == 14 || plaettchen == 15)
        {
            pieces.removeIf(x -> x.getPlaettchen() == 14 && x.getOrient() == orient);
            pieces.removeIf(x -> x.getPlaettchen() == 15 && x.getOrient() == orient);

            return;
        }

        if (plaettchen == 16 || plaettchen == 17)
        {
            pieces.removeIf(x -> x.getPlaettchen() == 16 && x.getOrient() == orient);
            pieces.removeIf(x -> x.getPlaettchen() == 17 && x.getOrient() == orient);
            return;
        }

        pieces.remove(pl);
    }

    private static boolean SetzePlaettchen(IcoSoku ico, int flaeche, int plaettchen, int orient)
    {
        ico.entfernePlaettchen(flaeche);
        ico.setzePlaettchen(flaeche, plaettchen, orient);

        if (!ico.pruefeFlaeche(flaeche))
        {
            ico.entfernePlaettchen(flaeche);
            return false;
        }

        return true;
    }

    private static List<Plaettchen> GetAvailablePieces(IcoSoku ico)
    {
        List<Plaettchen> pieces = new ArrayList<Plaettchen>();

        for (int plaettchen = 0; plaettchen < 20; plaettchen++)
        {
            if (!ico.istPlaettchenVerfuegbar(plaettchen))
            {
                continue;
            }

            // Drehen bringt nix bei 0,0,0 usw.
            if (ico.istPlaettchenSymmetrisch(plaettchen))
            {

                pieces.add(new Plaettchen(plaettchen, 0));
                continue;
            }

            // alle drei Orientierungen hinzufügen
            for (int i = 0; i < 3; i++)
            {
                pieces.add(new Plaettchen(plaettchen, i));
            }
        }

        return pieces;
    }

    private static final class Plaettchen
    {
        private int plaettchen;

        private int orient;

        public Plaettchen(int plaettchen, int orient)
        {
            this.plaettchen = plaettchen;
            this.orient = orient;
        }

        public int getPlaettchen()
        {
            return plaettchen;
        }

        public int getOrient()
        {
            return orient;
        }
    }
}
