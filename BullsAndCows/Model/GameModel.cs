namespace BullsAndCows.Model
{
    public class GameModel
    {
        public const int NUMBER_LENGTH = 4;
        readonly Player _player1;
        readonly Player _player2;
        private int[] _secretNumbersOfPlayer1;
        private int[] _secretNumbersOfPlayer2;
        private Random _random = new Random();
        private bool IsFirstPlayerStarted { get; init; }
        private bool IsFirstPlayerTurn { get; set; }
        private bool IsDraw { get; set; } = false;

        public GameModel(Player player1, Player player2)
        {
            _player1 = player1;
            _player2 = player2;

            if (_random.Next(0, 2) == 0)
                IsFirstPlayerTurn = IsFirstPlayerStarted = true;
            else
                IsFirstPlayerTurn = IsFirstPlayerStarted = false;
        }
        public void Run()
        {

            if (!GetSecretFromPlayer(_player1, ref _secretNumbersOfPlayer1))
                return;

            if (!GetSecretFromPlayer(_player2, ref _secretNumbersOfPlayer2))
                return;

            if (IsFirstPlayerTurn)
            {
                _player1.Notify(string.Empty, NotifyCode.YouStart);
                _player2.Notify(string.Empty, NotifyCode.OpponentStart);
            }
            else
            {
                _player1.Notify(string.Empty, NotifyCode.OpponentStart);
                _player2.Notify(string.Empty, NotifyCode.YouStart);
            }


            while (true)
            {
                try
                {
                    var numberStr = IsFirstPlayerTurn ? _player1.GetNumbers()
                                                      : _player2.GetNumbers();

                    var numbers = ParseMessage(numberStr);

                    var horneds = IsFirstPlayerTurn ? CountHorneds(numbers, _secretNumbersOfPlayer2)
                                                    : CountHorneds(numbers, _secretNumbersOfPlayer1);

                    if (IsFirstPlayerTurn)
                    {
                        _player2.Notify(numberStr, NotifyCode.OpponentSuggestedNumber);
                        _player1.UpdateHorneds(horneds);
                    }
                    else
                    {
                        _player1.Notify(numberStr, NotifyCode.OpponentSuggestedNumber);
                        _player2.UpdateHorneds(horneds);
                    }

                    IsFirstPlayerTurn = !IsFirstPlayerTurn;

                    if (horneds.BullsCount == NUMBER_LENGTH)
                        break;

                }
                catch (ArgumentException e)
                {
                    _player1.Notify(e.Message, NotifyCode.InvalidInputData);
                }
                catch (Exception e)
                {
                    _player1.Notify(e.Message, NotifyCode.InternalError);
                    return;
                }
            }

            /* Let's give the second player one last chance */
            if (IsFirstPlayerStarted != IsFirstPlayerTurn)
            {
                if (IsFirstPlayerTurn) // first player can lose or make draw
                {
                    _player1.Notify(string.Join("", _secretNumbersOfPlayer2), NotifyCode.YouLastTry);
                    _player2.Notify(string.Join("", _secretNumbersOfPlayer1), NotifyCode.OpponentLastTry);
                }
                else // second player can lose or make draw
                {
                    _player1.Notify(string.Join("", _secretNumbersOfPlayer2), NotifyCode.OpponentLastTry);
                    _player2.Notify(string.Join("", _secretNumbersOfPlayer1), NotifyCode.YouLastTry);
                }
                while (true)
                {
                    try
                    {
                        var numberStr = IsFirstPlayerTurn ? _player1.GetNumbers()
                                                          : _player2.GetNumbers();

                        var numbers = ParseMessage(numberStr);

                        var horneds = IsFirstPlayerTurn ? CountHorneds(numbers, _secretNumbersOfPlayer2)
                                                        : CountHorneds(numbers, _secretNumbersOfPlayer1);

                        if (IsFirstPlayerTurn)
                        {
                            _player2.Notify(numberStr, NotifyCode.OpponentSuggestedNumber);
                            _player1.UpdateHorneds(horneds);
                        }
                        else
                        {
                            _player1.Notify(numberStr, NotifyCode.OpponentSuggestedNumber);
                            _player2.UpdateHorneds(horneds);
                        }

                        if (horneds.BullsCount == NUMBER_LENGTH)
                            IsDraw = true;

                        break;
                    }
                    catch (ArgumentException e)
                    {
                        _player1.Notify(e.Message, NotifyCode.InvalidInputData);
                    }
                    catch (Exception e)
                    {
                        _player1.Notify(e.Message, NotifyCode.InternalError);
                        return;
                    }
                }
            }

            /* We have winner. Notify each other, who win/lose. And send opponent's secret */
            if (IsDraw)
            {
                _player1.Notify(string.Join("", _secretNumbersOfPlayer2), NotifyCode.Draw);
                _player2.Notify(string.Join("", _secretNumbersOfPlayer1), NotifyCode.Draw);
            }
            else if (IsFirstPlayerTurn) // second player won, because last move was by second player
            {
                _player1.Notify(string.Join("", _secretNumbersOfPlayer2), NotifyCode.YouLose);
                _player2.Notify(string.Join("", _secretNumbersOfPlayer1), NotifyCode.YouWin);
            }
            else // first player won
            {
                _player1.Notify(string.Join("", _secretNumbersOfPlayer2), NotifyCode.YouWin);
                _player2.Notify(string.Join("", _secretNumbersOfPlayer1), NotifyCode.YouLose);
            }

        }

        private int[] ParseMessage(string? numberStr)
        {
            if (numberStr == null ||
                !int.TryParse(numberStr, out _) ||
                numberStr.Length != NUMBER_LENGTH ||
                numberStr.Distinct().Count() != NUMBER_LENGTH)
            {
                throw new ArgumentException($"Move - {NUMBER_LENGTH} unique digits");
            }

            int[] numbers = numberStr.ToCharArray().Select(t => (int)char.GetNumericValue(t)).ToArray();

            return numbers;
        }

        private Horneds CountHorneds(int[] numbers, int[] secretNumbers)
        {
            var horneds = new Horneds();

            for (int i = 0; i < NUMBER_LENGTH; i++)
            {
                if (numbers[i] == secretNumbers[i])
                    horneds.BullsCount++;
                else if (secretNumbers.Contains(numbers[i]))
                    horneds.CowsCount++;
            }

            return horneds;
        }

        private bool GetSecretFromPlayer(Player player, ref int[] secret)
        {
            do
            {
                try
                {
                    player.Notify(string.Empty, NotifyCode.MakeSecret);
                    var secretStr1 = player.GetNumbers();
                    secret = ParseMessage(secretStr1);
                }
                catch (ArgumentException e)
                {
                    player.Notify(e.Message, NotifyCode.InvalidInputData);
                }
                catch (Exception e)
                {
                    _player1.Notify(e.Message, NotifyCode.InternalError);
                    _player2.Notify(e.Message, NotifyCode.InternalError);
                    return false;
                }
            } while (secret == null);

            return true;
        }
    }
}
