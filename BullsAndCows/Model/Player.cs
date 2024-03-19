namespace BullsAndCows.Model
{
    public abstract class Player
    {
        /// <summary>
        /// You can do what you want with Horneds data. I just print it
        /// </summary>
        abstract public void UpdateHorneds(Horneds horneds);

        /// <summary>
        /// Return string which contains player's answer for GameModel
        /// </summary>
        abstract public string? GetNumbers();

        /// <summary>
        /// Player can handle some sorts of "events"
        /// </summary>
        abstract public void Handle(string message, NotifyCode notifyCode);
    }
}
