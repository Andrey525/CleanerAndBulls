namespace BullsAndCows.Model
{
    public class ConsolePlayer : Player
    {
        public override void Handle(string? message, NotifyCode notifyCode)
        {
            switch (notifyCode)
            {
                case NotifyCode.YouWin:
                    Console.WriteLine($"You are winner! The hidden sequence was - {message}");
                    break;
                case NotifyCode.YouLose:
                    Console.WriteLine($"You are loser! The hidden sequence was - {message}");
                    break;
                case NotifyCode.OpponentSuggestedNumber:
                    Console.WriteLine($"Opponent suggested, that the hidden sequence is {message}");
                    break;
                case NotifyCode.MakeSecret:
                    Console.WriteLine($"Make a secret!");
                    break;
                case NotifyCode.YouStart:
                    Console.WriteLine($"You Started! :)");
                    break;
                case NotifyCode.OpponentStart:
                    Console.WriteLine($"Opponent Started! :(");
                    break;
                case NotifyCode.YouLastTry:
                    Console.WriteLine($"You have last try!");
                    break;
                case NotifyCode.OpponentLastTry:
                    Console.WriteLine($"Opponent has last try!");
                    break;
                case NotifyCode.Draw:
                    Console.WriteLine($"It is Draw!");
                    break;
                case NotifyCode.InvalidInputData:
                    Console.WriteLine($"Error: {message}");
                    break;
                case NotifyCode.InternalError:
                    Console.WriteLine("Error: Internal error!");
                    break;
                default:
                    break;
            }
        }

        public override string? GetNumbers()
        {
            Console.WriteLine($"Enter {GameModel.NUMBER_LENGTH}-digits number:");
            var numbersStr = Console.ReadLine();
            return numbersStr;
        }

        public override void UpdateHorneds(Horneds horneds)
        {
            Console.WriteLine($"Cows count: {horneds.CowsCount}; Bulls count: {horneds.BullsCount}");
        }
    }
}
