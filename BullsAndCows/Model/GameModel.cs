using BullsAndCows.Model.Helpers;
using BullsAndCows.Model.Players;

namespace BullsAndCows.Model
{
    public class GameModel
    {
        public const int NUMBER_LENGTH = 4;
        public const int PLAYERS_COUNT = 2;

        readonly List<Player> _players;
        readonly List<int[]> _playersSecrets;

        private Random _random = new Random();

        public int WhoStarted { get; init; }
        public int WhoseTurn { get; private set; }
        public bool IsDraw { get; private set; }
        public bool IsSecretsReceived { get; private set; }

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
                _playersSecrets[i] = Receiver.GetSequenceFromPlayer(_players[i], this);

                if (i == WhoStarted)
                    _players[i].Handle(string.Empty, NotifyCode.YouStart);
                else
                    _players[i].Handle(string.Empty, NotifyCode.OpponentStart);
            }

            IsSecretsReceived = true;

            /* Game loop */
            while (true)
            {
                var horneds = GetHornedsFromCurrentPlayer();

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

                var horneds = GetHornedsFromCurrentPlayer();

                if (horneds.BullsCount == NUMBER_LENGTH)
                    IsDraw = true;
            }

            /* Notify each other, who win/lose or may be draw. And send opponent's secret */
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

        private Horneds GetHornedsFromCurrentPlayer()
        {
            int[] numbers = Receiver.GetSequenceFromPlayer(_players[WhoseTurn], this);

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
