using System;
using System.Collections.Generic;

namespace IcoSoku
{
    public class IcoSoku
    {
        private static readonly int[][] EckeToFlaechen =
        { 
            new[] { 0, 1, 2, 3, 4 }, // Ecke 0
            new[] { 4, 13, 14, 5, 0 }, 
            new[] { 1, 0, 5, 6, 7 }, 
            new[] { 1, 2, 7, 8, 9 }, 
            new[] { 2, 3, 9, 10, 11 }, // Ecke 4
            new[] { 3, 4, 11, 12, 13 }, 
            new[] { 5, 6, 14, 15, 19 }, 
            new[] { 6, 7, 8, 15, 16 }, 
            new[] { 8, 9, 10, 16, 17 }, // Ecke 8
            new[] { 10, 11, 12, 17, 18 }, 
            new[] { 12, 13, 14, 18, 19 }, 
            new[] { 15, 16, 17, 18, 19 }
         };

        private static readonly int[][] FlaecheToEcken =
        { 
            new[] { 0, 1, 2 }, // 0
            new[] { 0, 2, 3 }, 
            new[] { 0, 3, 4 },
            new[] { 0, 4, 5 },
            new[] { 0, 5, 1 }, // 4
            new[] { 1, 2, 6 },
            new[] { 2, 6, 7 },
            new[] { 2, 3, 7 }, 
            new[] { 3, 7, 8 }, // 8
            new[] { 3, 4, 8 }, 
            new[] { 4, 8, 9 },
            new[] { 4, 5, 9 }, 
            new[] { 5, 9, 10 }, // 12
            new[] { 1, 5, 10 }, 
            new[] { 1, 6, 10 }, 
            new[] { 6, 7, 11 }, 
            new[] { 7, 8, 11 }, // 16
            new[] { 8, 9, 11 }, 
            new[] { 9, 10, 11 }, 
            new[] { 6, 10, 11 }
         };

        private static readonly int[][] Ecke2Flaecheorient =
        { 
            new[] { 0, 0, 0, 0, 0 }, // Ecke 0
            new[] { 2, 2, 0, 0, 1 },
            new[] { 1, 2, 2, 0, 0 }, 
            new[] { 2, 1, 2, 0, 0 }, 
            new[] { 2, 1, 2, 0, 0 }, // Ecke 4
            new[] { 2, 1, 2, 0, 0 },
            new[] { 1, 1, 2, 0, 2 },
            new[] { 2, 1, 1, 2, 0 }, 
            new[] { 2, 1, 1, 2, 0 }, // Ecke 8
            new[] { 2, 1, 1, 2, 0 }, 
            new[] { 2, 1, 1, 2, 0 },
            new[] { 1, 1, 1, 1, 1 }
         };

        /**
         * Do not change!
         */
        private static readonly int[][] PlaettchenZahlen =
        { 
            new[] { 0, 0, 0 }, // 0
            new[] { 0, 0, 1 },
            new[] { 0, 0, 2 }, 
            new[] { 0, 0, 3 }, 
            new[] { 0, 1, 1 }, // 4
            new[] { 0, 1, 2 }, 
            new[] { 0, 1, 2 }, 
            new[] { 0, 1, 2 }, 
            new[] { 0, 2, 1 }, // 8
            new[] { 0, 2, 1 }, 
            new[] { 0, 2, 1 }, 
            new[] { 0, 2, 2 },
            new[] { 0, 3, 3 }, // 12
            new[] { 1, 1, 1 }, 
            new[] { 1, 2, 3 },
            new[] { 1, 2, 3 },
            new[] { 1, 3, 2 }, // 16
            new[] { 1, 3, 2 },
            new[] { 2, 2, 2 }, 
            new[] { 3, 3, 3 }
         };

        private static readonly int[] Eckenzahlen = new int[12];
        private static readonly int[] FlaecheHatPlaettchen = new int[20];
        private static readonly int[] FlaecheHatPlaettchenOrient = new int[20];

        private static readonly bool[] FlaecheIstBelegt = new bool[20];
        private static readonly bool[] PlaettchenIstVerfuegbar = new bool[20];

        public IcoSoku(IList<int> zahlen)
        {
            if (zahlen.Count != 12)
            {
                throw new ArgumentException();
            }

            var zahltest = new bool[12];
            for (var i = 0; i < 12; i++)
            {
                zahltest[i] = false;
            }

            for (var i = 0; i < 12; i++)
            {
                if (zahlen[i] < 1 || zahlen[i] > 12)
                {
                    throw new ArgumentException();
                }

                Eckenzahlen[i] = zahlen[i];
                zahltest[zahlen[i] - 1] = true;
            }

            for (var i = 0; i < 12; i++)
            {
                if (!zahltest[i])
                {
                    throw new ArgumentException();
                }
            }

            for (var i = 0; i < 20; i++)
            {
                FlaecheIstBelegt[i] = false;
                PlaettchenIstVerfuegbar[i] = true;
            }
        }

        public bool IstPlaettchenSymmetrisch(int plaettchen)
        {
            return PlaettchenZahlen[plaettchen][0] == PlaettchenZahlen[plaettchen][1] &&
                   PlaettchenZahlen[plaettchen][1] == PlaettchenZahlen[plaettchen][2];
        }

        public bool IstFlaecheBelegt(int flaeche)
        {
            if (flaeche < 0 || flaeche > 19)
            {
                throw new ArgumentException();
            }

            return FlaecheIstBelegt[flaeche];
        }

        public bool IstPlaettchenVerfuegbar(int plaettchen)
        {
            if (plaettchen < 0 || plaettchen > 19)
            {
                throw new ArgumentException();
            }

            return PlaettchenIstVerfuegbar[plaettchen];
        }

        public int ZahlAmPlaettchen(int plaettchen, int orient)
        {
            if (plaettchen < 0 || plaettchen > 19 || orient < 0 || orient > 2)
            {
                throw new ArgumentException();
            }

            return PlaettchenZahlen[plaettchen][orient];
        }

        public bool SetzePlaettchen(int flaeche, int plaettchen, int orient)
        {
            if (flaeche < 0 || flaeche > 19 || plaettchen < 0 || plaettchen > 19 || orient < 0 || orient > 2)
            {
                throw new ArgumentException();
            }

            if (FlaecheIstBelegt[flaeche] || !PlaettchenIstVerfuegbar[plaettchen])
            {
                return false;
            }

            FlaecheHatPlaettchen[flaeche] = plaettchen;
            FlaecheHatPlaettchenOrient[flaeche] = orient;
            FlaecheIstBelegt[flaeche] = true;
            PlaettchenIstVerfuegbar[plaettchen] = false;

            return true;
        }

        public bool EntfernePlaettchen(int flaeche)
        {
            if (flaeche < 0 || flaeche > 19)
            {
                throw new ArgumentException();
            }

            if (!FlaecheIstBelegt[flaeche])
            {
                return false;
            }

            FlaecheIstBelegt[flaeche] = false;
            PlaettchenIstVerfuegbar[FlaecheHatPlaettchen[flaeche]] = true;
            FlaecheHatPlaettchen[flaeche] = -1;

            return true;
        }

        public void EntferneAllePlaettchen()
        {
            for (var i = 0; i < 20; i++)
            {
                FlaecheIstBelegt[i] = false;
                PlaettchenIstVerfuegbar[i] = true;
            }
        }

        public int SollWertAnEcke(int ecke)
        {
            if (ecke < 0 || ecke > 11)
            {
                throw new ArgumentException();
            }

            return Eckenzahlen[ecke];
        }

        public int IstWertAnEcke(int ecke)
        {
            if (ecke < 0 || ecke > 11)
            {
                throw new ArgumentException();
            }

            var summe = 0;
            for (var i = 0; i < 5; i++)
            {
                var flaeche = EckeToFlaechen[ecke][i];
                var florient = Ecke2Flaecheorient[ecke][i];

                if (FlaecheIstBelegt[flaeche])
                {
                    var plaettchen = FlaecheHatPlaettchen[flaeche];
                    var orient = FlaecheHatPlaettchenOrient[flaeche];
                    var plaettindex = (florient + orient) % 3;
                    summe += PlaettchenZahlen[plaettchen][plaettindex];
                }
            }

            return summe;
        }

        public int AnzahlPlaettchenAnEcke(int ecke)
        {
            if (ecke < 0 || ecke > 11)
            {
                throw new ArgumentException();
            }

            var zaehler = 0;
            for (var i = 0; i < 5; i++)
            {
                if (FlaecheIstBelegt[EckeToFlaechen[ecke][i]])
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

        public bool PruefeFlaeche(int flaeche)
        {
            foreach (var ecke in FlaecheToEcken[flaeche])
            {
                var istWertAnEcke = this.IstWertAnEcke(ecke);
                var sollWertAnEcke = this.SollWertAnEcke(ecke);
                var belegteFlaechen = this.AnzahlPlaettchenAnEcke(ecke);

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

        public bool PruefeAlles()
        {
            for (var ecke = 0; ecke < 12; ecke++)
            {
                if (this.AnzahlPlaettchenAnEcke(ecke) < 5)
                {
                    return false;
                }

                if (this.IstWertAnEcke(ecke) != this.SollWertAnEcke(ecke))
                {
                    return false;
                }
            }

            return true;
        }

        public void ZeigeLoesung()
        {
            for (var i = 0; i < 20; i++)
            {
                Console.WriteLine("\tFläche: " + string.Format("{0,2}", i) + " mit Plättchen " + string.Format("{0,2}", FlaecheHatPlaettchen[i]) + " und Orientierung " + FlaecheHatPlaettchenOrient[i]);
            }
        }
    }
}