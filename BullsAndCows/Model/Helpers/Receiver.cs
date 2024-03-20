using BullsAndCows.Model.Players;
using Serilog;

namespace BullsAndCows.Model.Helpers
{
    static public class Receiver
    {
        public static int[] GetSequenceFromPlayer(Player player, GameModel game)
        {
            int[] secret;

            while (true)
            {
                try
                {
                    if (!game.IsSecretsReceived)
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
    }
}
