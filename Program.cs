using System;


namespace ThompsonSampling
{
    class Program
    {
        static void Main(string[] args)
        {
            /*  
             *  Given any number of slot machines, each with a different "win rate,"
             *  and starting with a given amount, try to win as much money as possible.
             *  A loss costs $1, but a win nets us $2. Try to find the best machine to
             *  play as quickly as possible, and use that machine.
             */
            
            double[] testMachines = { 0.3, 0.5, 0.7 }; // Our machines and their win rate
            FindBestOption(testMachines, 100); // Calling the method with 100 plays
        }

        static void FindBestOption(double[] rates, int trials)
        {
            // TSMath class has our method for beta sampling.
            TSMath tsm = new TSMath();
            Random rand = new Random();
            int N = rates.Length;
            int[] positiveResult = new int[N];
            int[] negativeResult = new int[N];
            // Array for storing our beta samples
            double[] samples = new double[N];
            // currently selected machine for playing
            int selected = 0;
            // Keep track of best sample
            double selectedSample;
            int money = trials; // Lets play with money. Loss -= $1, Win += $2

            for (int i = 0; i < trials; i++)
            {
                selectedSample = 0.0;
                
                for (int j = 0; j < N; j++)
                {
                    // Get our beta samples of the machines
                    double sample = tsm.Sample(positiveResult[j] + 1, negativeResult[j] + 1);
                    if (sample > selectedSample)
                    {
                        // Play the one that returned the highest value
                        selectedSample = sample;
                        selected = j;
                    }
                }

                if (rand.NextDouble() <= rates[selected])
                {
                    positiveResult[selected]++;
                    money += 2;
                }
                else
                {
                    negativeResult[selected]++;
                    money--;
                }
                
            }
            selectedSample = 0.0;
            Console.WriteLine("Number of times machines played:");
            for (int j = 0; j < N; j++)
            {
                int timesPlayed = positiveResult[j] + negativeResult[j];
                
                samples[j] = (positiveResult[j] * 1.0) / (positiveResult[j] + negativeResult[j]);
                Console.WriteLine($"Machine #{j + 1}: {timesPlayed} Mean: {samples[j]} ");
                if (samples[j] > selectedSample)
                {
                    selected = j;
                    selectedSample = samples[j];
                }
            }
            Console.WriteLine($"Selected machine is #{selected + 1}");
            Console.WriteLine($"Money: ${money}");
        }
    }
}
