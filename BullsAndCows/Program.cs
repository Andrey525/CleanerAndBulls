using BullsAndCows.Model;

namespace BullsAndCows
{
    public class Program
    {
        static string? _numberStr;
        public static void Main()
        {
            Console.WriteLine("Wellcome to the game 'Bulls And Cows'");

            var gameModel = new GameModel();
            gameModel.GetNumbers += UserInteractionHandler;
            gameModel.GetResponse += GetResponse;
            gameModel.ErrorOccured += PrintErrorMessage;

            try
            {
                gameModel.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            Console.WriteLine($"You are winner! The hidden sequence was - {_numberStr}");
        }

        private static string? UserInteractionHandler()
        {
            Console.WriteLine($"Enter {GameModel.NUMBER_LENGTH}-digits number:");
            _numberStr = Console.ReadLine();
            return _numberStr;
        }

        private static void GetResponse(int cowsCount, int bullsCount)
        {
            Console.WriteLine($"Cows count: {cowsCount}; Bulls count: {bullsCount}");
        }

        private static void PrintErrorMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
