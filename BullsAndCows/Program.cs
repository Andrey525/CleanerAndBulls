using BullsAndCows.Model;
using BullsAndCows.Model.Players;
using Serilog;

namespace BullsAndCows
{
    public class Program
    {
        public static void Main()
        {
            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.File(@".\logs\log.txt", rollingInterval: RollingInterval.Hour)
                        .CreateLogger();

            Log.Information("Game started");

            var player = new ConsolePlayer();
            var bot = new Bot();
            var gameModel = new GameModel(player, bot);

            try
            {
                gameModel.Run();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return;
            }
        }
    }
}
