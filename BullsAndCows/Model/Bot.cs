using Serilog;
namespace BullsAndCows.Model
{
    public class Bot : Player
    {
        Random _random = new Random();
        public override string? GetNumbers()
        {
            /* Generate sequence */
            int[] numbers = new int[GameModel.NUMBER_LENGTH];
            for (int i = 0; i < GameModel.NUMBER_LENGTH; i++)
            {
                int digit;
                do
                {
                    digit = _random.Next(0, 10);
                } while (numbers.Contains(digit));
                numbers[i] = digit;
            }

            return string.Join("", numbers);
        }

        public override void Handle(string message, NotifyCode notifyCode)
        {
            Log.Information($"Code: {notifyCode}; Message: {message};");
            return;
        }

        public override void UpdateHorneds(Horneds horneds)
        {
            Log.Information($"Cows count: {horneds.CowsCount}; Bulls count: {horneds.BullsCount}");
        }
    }
}
