using BullsAndCows.Model.Helpers;
using Serilog;
namespace BullsAndCows.Model
{
    public class GameModel
    {
        public const int NUMBER_LENGTH = 4;
        public const int PLAYERS_COUNT = 2;

        readonly List<Player> _players;
        List<int[]> _playersSecrets;

        private Random _random = new Random();

        private int WhoStarted { get; init; }
        private int WhoseTurn { get; set; }
        private bool IsDraw { get; set; } = false;
        private bool IsSecretsReceived { get; set; }

        public GameModel(Player player1, Player player2)
        {
            _players = new List<Player> { player1, player2 };
            _playersSecrets = new List<int[]>() { new int[NUMBER_LENGTH], new int[NUMBER_LENGTH] };
            WhoStarted = WhoseTurn = _random.Next(0, PLAYERS_COUNT);
        }
        public void Run()
        {
            /* Request secrets from each */
            for (int i = 0; i < PLAYERS_COUNT; i++)
            {
                _playersSecrets[i] = GetSequenceFromPlayer(_players[i]);

                if (i == WhoStarted)
                    _players[i].Handle(string.Empty, NotifyCode.YouStart);
                else
                    _players[i].Handle(string.Empty, NotifyCode.OpponentStart);
            }

            IsSecretsReceived = true;

            /* Game loop */
            while (true)
            {
                int[] numbers = GetSequenceFromPlayer(_players[WhoseTurn]);
                var horneds = HornedsCounter.CountHorneds(numbers, _playersSecrets[WhoseTurn ^ 1]);

                for (int i = 0; i < PLAYERS_COUNT; i++)
                {
                    if (i == WhoseTurn)
                        _players[i].UpdateHorneds(horneds);
                    else
                        _players[i].Handle(string.Join("", numbers), NotifyCode.OpponentSuggestedNumber);
                }

                WhoseTurn ^= 1;

                if (horneds.BullsCount == NUMBER_LENGTH)
                    break;
            }

            /* Let's give the second player one last chance */
            if (WhoStarted != WhoseTurn)
            {
                for (int i = 0; i < PLAYERS_COUNT; i++)
                {
                    if (i == WhoseTurn)
                        _players[i].Handle(string.Empty, NotifyCode.YouLastTry);
                    else
                        _players[i].Handle(string.Empty, NotifyCode.OpponentLastTry);
                }

                while (true)
                {
                    var horneds = MoveIteration();

                    if (horneds.BullsCount == NUMBER_LENGTH)
                        IsDraw = true;

                    break;
                }
            }

            /* Handle each other, who win/lose or may be draw. And send opponent's secret */
            for (int i = 0; i < PLAYERS_COUNT; i++)
            {
                if (IsDraw)
                {
                    _players[i].Handle(string.Join("", _playersSecrets[i ^ 1]), NotifyCode.Draw);
                }
                else if (i == WhoseTurn)
                    _players[i].Handle(string.Join("", _playersSecrets[i ^ 1]), NotifyCode.YouLose);
                else
                    _players[i].Handle(string.Join("", _playersSecrets[i ^ 1]), NotifyCode.YouWin);
            }

            /*Game over*/
        }

        private int[] GetSequenceFromPlayer(Player player)
        {
            int[] secret;

            while (true)
            {
                try
                {
                    if (!IsSecretsReceived)
                        player.Handle(string.Empty, NotifyCode.MakeSecret);

                    var secretStr1 = player.GetNumbers();
                    secret = MessageParser.ParseMessage(secretStr1);
                    break;
                }
                catch (ArgumentException e)
                {
                    player.Handle(e.Message, NotifyCode.InvalidInputData);
                }
                catch (Exception e)
                {
                    Log.Error(e, $"GetSequenceFromPlayer: {e.Message}");
                    throw new Exception(e.Message);
                }
            }

            return secret;
        }

        private Horneds MoveIteration()
        {
            int[] numbers = GetSequenceFromPlayer(_players[WhoseTurn]);

            var horneds = HornedsCounter.CountHorneds(numbers, _playersSecrets[WhoseTurn ^ 1]);

            for (int i = 0; i < PLAYERS_COUNT; i++)
            {
                if (i == WhoseTurn)
                    _players[i].UpdateHorneds(horneds);
                else
                    _players[i].Handle(string.Join("", numbers), NotifyCode.OpponentSuggestedNumber);
            }

            return horneds;
        }
    }
}
