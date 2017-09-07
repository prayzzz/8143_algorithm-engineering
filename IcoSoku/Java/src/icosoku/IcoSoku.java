package icosoku;

public class IcoSoku
{
    private static int[][] eckeToFlaechen =
            {{0, 1, 2, 3, 4}, // Ecke 0
                    {4, 13, 14, 5, 0},
                    {1, 0, 5, 6, 7},
                    {1, 2, 7, 8, 9},
                    {2, 3, 9, 10, 11}, // Ecke 4
                    {3, 4, 11, 12, 13},
                    {5, 6, 14, 15, 19},
                    {6, 7, 8, 15, 16},
                    {8, 9, 10, 16, 17}, // Ecke 8
                    {10, 11, 12, 17, 18},
                    {12, 13, 14, 18, 19},
                    {15, 16, 17, 18, 19}};

    private static int[][] flaecheToEcken =
            {{0, 1, 2}, // 0
                    {0, 2, 3},
                    {0, 3, 4},
                    {0, 4, 5},
                    {0, 5, 1}, // 4
                    {1, 2, 6},
                    {2, 6, 7},
                    {2, 3, 7},
                    {3, 7, 8}, // 8
                    {3, 4, 8},
                    {4, 8, 9},
                    {4, 5, 9},
                    {5, 9, 10}, // 12
                    {1, 5, 10},
                    {1, 6, 10},
                    {6, 7, 11},
                    {7, 8, 11}, // 16
                    {8, 9, 11},
                    {9, 10, 11},
                    {6, 10, 11}};

    private static int[][] ecke2flaecheorient =
            {{0, 0, 0, 0, 0}, // Ecke 0
                    {2, 2, 0, 0, 1},
                    {1, 2, 2, 0, 0},
                    {2, 1, 2, 0, 0},
                    {2, 1, 2, 0, 0}, // Ecke 4
                    {2, 1, 2, 0, 0},
                    {1, 1, 2, 0, 2},
                    {2, 1, 1, 2, 0},
                    {2, 1, 1, 2, 0}, // Ecke 8
                    {2, 1, 1, 2, 0},
                    {2, 1, 1, 2, 0},
                    {1, 1, 1, 1, 1}};

    /**
     * Do not change!
     */
    private static int[][] plaettchenZahlen =
            {{0, 0, 0},// 0
                    {0, 0, 1},
                    {0, 0, 2},
                    {0, 0, 3},
                    {0, 1, 1},// 4
                    {0, 1, 2},
                    {0, 1, 2},
                    {0, 1, 2},
                    {0, 2, 1},//8
                    {0, 2, 1},
                    {0, 2, 1},
                    {0, 2, 2},
                    {0, 3, 3},//12
                    {1, 1, 1},
                    {1, 2, 3},
                    {1, 2, 3},
                    {1, 3, 2},//16
                    {1, 3, 2},
                    {2, 2, 2},
                    {3, 3, 3}};

    private int[] eckenzahlen = new int[12];
    private int[] flaecheHatPlaettchen = new int[20];
    private int[] flaecheHatPlaettchenOrient = new int[20];

    private boolean[] flaecheIstBelegt = new boolean[20];
    private boolean[] plaettchenIstVerfuegbar = new boolean[20];

    public IcoSoku(int[] zahlen)
    {
        if (zahlen.length != 12)
        {
            throw new IllegalArgumentException();
        }

        boolean[] zahltest = new boolean[12];
        for (int i = 0; i < 12; i++)
        {
            zahltest[i] = false;
        }

        for (int i = 0; i < 12; i++)
        {
            if (zahlen[i] < 1 || zahlen[i] > 12)
            {
                throw new IllegalArgumentException();
            }
            eckenzahlen[i] = zahlen[i];
            zahltest[zahlen[i] - 1] = true;
        }

        for (int i = 0; i < 12; i++)
        {
            if (!zahltest[i])
            {
                throw new IllegalArgumentException();
            }
        }

        for (int i = 0; i < 20; i++)
        {
            flaecheIstBelegt[i] = false;
            plaettchenIstVerfuegbar[i] = true;
        }
    }

    public boolean istPlaettchenSymmetrisch(int plaettchen)
    {
        return plaettchenZahlen[plaettchen][0] == plaettchenZahlen[plaettchen][1] && plaettchenZahlen[plaettchen][1] == plaettchenZahlen[plaettchen][2];
    }

    public boolean istFlaecheBelegt(int flaeche)
    {
        if (flaeche < 0 || flaeche > 19)
        {
            throw new IllegalArgumentException();
        }

        return flaecheIstBelegt[flaeche];
    }

    public boolean istPlaettchenVerfuegbar(int plaettchen)
    {
        if (plaettchen < 0 || plaettchen > 19)
        {
            throw new IllegalArgumentException();
        }

        return plaettchenIstVerfuegbar[plaettchen];
    }

    public int zahlAmPlaettchen(int plaettchen, int orient)
    {
        if (plaettchen < 0 || plaettchen > 19 || orient < 0 || orient > 2)
        {
            throw new IllegalArgumentException();
        }

        return plaettchenZahlen[plaettchen][orient];
    }

    public boolean setzePlaettchen(int flaeche, int plaettchen, int orient)
    {
        if (flaeche < 0 || flaeche > 19 || plaettchen < 0 || plaettchen > 19 || orient < 0 || orient > 2)
        {
            throw new IllegalArgumentException();
        }

        if (flaecheIstBelegt[flaeche] || !plaettchenIstVerfuegbar[plaettchen])
        {
            return false;
        }
        else
        {
            flaecheHatPlaettchen[flaeche] = plaettchen;
            flaecheHatPlaettchenOrient[flaeche] = orient;
            flaecheIstBelegt[flaeche] = true;
            plaettchenIstVerfuegbar[plaettchen] = false;
            return true;
        }
    }

    public boolean entfernePlaettchen(int flaeche)
    {
        if (flaeche < 0 || flaeche > 19)
        {
            throw new IllegalArgumentException();
        }

        if (!flaecheIstBelegt[flaeche])
        {
            return false;
        }
        else
        {
            flaecheIstBelegt[flaeche] = false;
            plaettchenIstVerfuegbar[flaecheHatPlaettchen[flaeche]] = true;
            flaecheHatPlaettchen[flaeche] = -1;
            return true;
        }
    }

    public void entferneAllePlaettchen()
    {
        for (int i = 0; i < 20; i++)
        {
            flaecheIstBelegt[i] = false;
            plaettchenIstVerfuegbar[i] = true;
        }
    }

    public int sollWertAnEcke(int ecke)
    {
        if (ecke < 0 || ecke > 11)
        {
            throw new IllegalArgumentException();
        }

        return eckenzahlen[ecke];
    }

    public int istWertAnEcke(int ecke)
    {
        if (ecke < 0 || ecke > 11)
        {
            throw new IllegalArgumentException();
        }

        int summe = 0;
        for (int i = 0; i < 5; i++)
        {
            int flaeche = eckeToFlaechen[ecke][i];
            int florient = ecke2flaecheorient[ecke][i];

            if (flaecheIstBelegt[flaeche])
            {
                int plaettchen = flaecheHatPlaettchen[flaeche];
                int orient = flaecheHatPlaettchenOrient[flaeche];
                int plaettindex = (florient + orient) % 3;
                summe += plaettchenZahlen[plaettchen][plaettindex];
            }
        }

        return summe;
    }

    public int anzahlPlaettchenAnEcke(int ecke)
    {
        if (ecke < 0 || ecke > 11)
        {
            throw new IllegalArgumentException();
        }

        int zaehler = 0;
        for (int i = 0; i < 5; i++)
        {
            if (flaecheIstBelegt[eckeToFlaechen[ecke][i]])
            {
                zaehler++;
            }
        }

        return zaehler;
    }

    /**
     * Prüft für jede anliegende Ecke, ob die anliegenden Flächen gefüllt sind
     * Sind nicht alle Flächen belegt, darf der IstWert nicht größer als der SollWert sein.
     * Sind alle Flächen belegt, darf der IstWert sich nicht vom SollWert unterscheiden
     */
    public boolean pruefeFlaeche(int flaeche)
    {
        for (int ecke : flaecheToEcken[flaeche])
        {
            int belegteFlaechen = anzahlPlaettchenAnEcke(ecke);
            int istWertAnEcke = istWertAnEcke(ecke);
            int sollWertAnEcke = sollWertAnEcke(ecke);

            if (belegteFlaechen != 5)
            {
                // Wert darf nicht überschritten sein
                if (istWertAnEcke > sollWertAnEcke)
                {
                    return false;
                }

                // Der noch benötigte Wert darf nicht größer sein als ein vielfaches (freieFlächen) von 3 (maximal Punktzahl)
                if (sollWertAnEcke - istWertAnEcke > (5 - belegteFlaechen) * 3)
                {
                    return false;
                }

                continue;
            }

            // alle Flächen sind belegt
            if (istWertAnEcke != sollWertAnEcke)
            {
                return false;
            }
        }

        return true;
    }

    public boolean pruefeAlles()
    {
        for (int ecke = 0; ecke < 12; ecke++)
        {
            if (anzahlPlaettchenAnEcke(ecke) < 5)
            {
                return false;
            }

            if (istWertAnEcke(ecke) != sollWertAnEcke(ecke))
            {
                return false;
            }
        }

        return true;
    }

    public void ZeigeLoesung()
    {
        for (int i = 0; i < 20; i++)
        {
            System.out.println("\tFläche: " + String.format("%2d", i) + " mit Plättchen " + String.format("%2d", flaecheHatPlaettchen[i]) + " und Orientierung " + flaecheHatPlaettchenOrient[i]);
        }
    }
}