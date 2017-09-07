package icosoku;

import java.util.Arrays;
import java.util.Random;

/**
 * Created by Patrick on 17.10.2014.
 */
public class Program
{
    private static int[][] eckenAnordnungen =
            {{1, 3, 10, 7, 5, 4, 11, 6, 12, 8, 9, 2},
                    {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12},
                    {6, 12, 9, 10, 7, 2, 8, 1, 3, 5, 4, 11},
                    {12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1}};

    // {10, 4, 8, 7, 5, 9, 3, 2, 11, 6, 1, 12} schlechte Performance

    public static void main(String[] args)
    {
        int[] anordnung = HandleArgument(args);

        System.out.println("Teste Ecken: " + Arrays.toString(anordnung));

        IcoSoku icoSoku = new IcoSoku(anordnung);

        long startTime = System.currentTimeMillis();
        IcoSokuSolver.Solve(icoSoku, 0);
        long stopTime = System.currentTimeMillis();

        long elapsedTimeInSeconds = (stopTime - startTime);

        System.out.println();
        System.out.println("Lösung gefunden: " + icoSoku.pruefeAlles());

        if (icoSoku.pruefeAlles())
        {
            icoSoku.ZeigeLoesung();
        }

        System.out.println("In " + elapsedTimeInSeconds + " Millisekunden");
    }

    private static int[] HandleArgument(String[] args)
    {
        int[] anordnung = eckenAnordnungen[1];

        // too less or too many arguments
        if (args.length != 1)
        {
            return anordnung;
        }

        // random
        if (args[0].equals("r"))
        {
            anordnung = eckenAnordnungen[1];
            ShuffleArray(anordnung);
            return anordnung;
        }

        // no integer
        if (!args[0].matches("\\d+"))
        {
            return anordnung;
        }
        // specific testobject
        int index = Integer.parseInt(args[0]);
        if (index >= 0 && index <= 3)
        {
            return eckenAnordnungen[index];
        }

        return anordnung;
    }

    // Fisher-Yates-Shuffle
    private static void ShuffleArray(int[] array)
    {
        int index;
        Random random = new Random();
        for (int i = array.length - 1; i > 0; i--)
        {
            index = random.nextInt(i + 1);
            if (index != i)
            {
                array[index] ^= array[i];
                array[i] ^= array[index];
                array[index] ^= array[i];
            }
        }
    }
}
