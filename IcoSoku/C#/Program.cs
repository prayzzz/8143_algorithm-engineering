using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace IcoSoku
{
    public class Program
    {
        private static readonly int[][] EckenAnordnungen =
        {
            new[] { 1, 3, 10, 7, 5, 4, 11, 6, 12, 8, 9, 2 }, 
            new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, 
            new[] { 6, 12, 9, 10, 7, 2, 8, 1, 3, 5, 4, 11 }, 
            new[] { 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 }
        };

        //// {10, 4, 8, 7, 5, 9, 3, 2, 11, 6, 1, 12} schlechte Performance

        public static void Main(string[] args)
        {
            var anordnung = HandleArgument(args);

            Console.WriteLine("Teste Ecken: " + string.Join(", ", anordnung));

            var icoSoku = new IcoSoku(anordnung);

            var watch = new Stopwatch();
            watch.Start();
            IcoSokuSolver.Solve(icoSoku, 0);
            watch.Stop();

            var elapsedTimeInSeconds = watch.ElapsedMilliseconds;

            Console.WriteLine();
            Console.WriteLine("Lösung gefunden: " + icoSoku.PruefeAlles());

            if (icoSoku.PruefeAlles())
            {
                icoSoku.ZeigeLoesung();
            }

            Console.WriteLine("In " + elapsedTimeInSeconds + " Millisekunden");
        }

        private static int[] HandleArgument(IList<string> args)
        {
            var anordnung = EckenAnordnungen[1];

            // too less or too many arguments
            if (args.Count != 1)
            {
                return anordnung;
            }

            // random
            if (args[0].Equals("r"))
            {
                anordnung = EckenAnordnungen[1];
                ShuffleArray(anordnung);
                return anordnung;
            }

            // no integer
            if (!Regex.IsMatch(args[0], "\\d+"))
            {
                return anordnung;
            }

            // specific testobject
            var index = int.Parse(args[0]);
            if (index >= 0 && index <= 3)
            {
                return EckenAnordnungen[index];
            }

            return anordnung;
        }

        // Fisher-Yates-Shuffle
        private static void ShuffleArray(IList<int> array)
        {
            var random = new Random();
            for (var i = array.Count - 1; i > 0; i--)
            {
                var index = random.Next(i + 1);
                if (index != i)
                {
                    array[index] ^= array[i];
                    array[i] ^= array[index];
                    array[index] ^= array[i];
                }
            }
        }
    }
}