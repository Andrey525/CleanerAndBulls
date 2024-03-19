namespace BullsAndCows.Model.Helpers
{
    public static class MessageParser
    {
        public static int[] ParseMessage(string? numberStr)
        {
            if (numberStr == null ||
                !int.TryParse(numberStr, out _) ||
                numberStr.Length != GameModel.NUMBER_LENGTH ||
                numberStr.Distinct().Count() != GameModel.NUMBER_LENGTH)
            {
                throw new ArgumentException($"Move - {GameModel.NUMBER_LENGTH} unique digits");
            }

            int[] numbers = numberStr.ToCharArray().Select(t => (int)char.GetNumericValue(t)).ToArray();

            return numbers;
        }
    }
}
