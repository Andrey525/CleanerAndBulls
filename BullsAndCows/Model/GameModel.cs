namespace BullsAndCows.Model
{
    public class GameModel : IDisposable
    {
        public delegate string? GetNumbersCallback();
        public event GetNumbersCallback? GetNumbers;

        public delegate void GetResponseCallback(int cowsCount, int bullsCount);
        public event GetResponseCallback? GetResponse;

        public delegate void ErrorOccuredCallback(string message);
        public event ErrorOccuredCallback? ErrorOccured;


        private int[] _secretNumbers;
        private Random _random;
        public const int NUMBER_LENGTH = 4;
        public bool GameIsOver { get; private set; }

        public GameModel()
        {
            _random = new Random();
            _secretNumbers = new int[NUMBER_LENGTH];
            GameIsOver = false;

            /* Generate secret value */
            for (int i = 0; i < NUMBER_LENGTH; i++)
            {
                int digit;
                do
                {
                    digit = _random.Next(0, 10);
                } while (_secretNumbers.Contains(digit));
                _secretNumbers[i] = digit;
            }

            _secretNumbers.ToList().ForEach(t => Console.Write(t));
            Console.WriteLine();
        }
        public void Run()
        {
            while (!GameIsOver)
            {
                try
                {
                    var numberStr = GetNumbers?.Invoke();
                    var numbers = ParseMessage(numberStr);
                    var horneds = CountHorneds(numbers);

                    if (horneds.BullsCount == NUMBER_LENGTH)
                        GameIsOver = true;

                    GetResponse?.Invoke(horneds.CowsCount, horneds.BullsCount);

                }
                catch (Exception e)
                {
                    ErrorOccured?.Invoke(e.Message);
                }
            }
        }

        private int[] ParseMessage(string? numberStr)
        {
            if (numberStr == null ||
                !int.TryParse(numberStr, out _) ||
                numberStr.Distinct().Count() != NUMBER_LENGTH)
            {
                throw new Exception($"Move - {NUMBER_LENGTH} unique digits");
            }

            int[] numbers = numberStr.ToCharArray().Select(t => (int)Char.GetNumericValue(t)).ToArray();

            return numbers;
        }

        private Horneds CountHorneds(int[] numbers)
        {
            var horneds = new Horneds();

            for (int i = 0; i < NUMBER_LENGTH; i++)
            {
                if (numbers[i] == _secretNumbers[i])
                    horneds.BullsCount++;
                else if (_secretNumbers.Contains(numbers[i]))
                    horneds.CowsCount++;
            }

            return horneds;
        }

        public void Dispose()
        {
            GetNumbers = null;
            GetResponse = null;
        }
    }
}
