namespace BullsAndCows.Model.Helpers
{
    public static class HornedsCounter
    {
        public static Horneds CountHorneds(int[] numbers, int[] secretNumbers)
        {
            var horneds = new Horneds();

            for (int i = 0; i < GameModel.NUMBER_LENGTH; i++)
            {
                if (numbers[i] == secretNumbers[i])
                    horneds.BullsCount++;
                else if (secretNumbers.Contains(numbers[i]))
                    horneds.CowsCount++;
            }

            return horneds;
        }
    }
}
