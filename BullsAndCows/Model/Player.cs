namespace BullsAndCows.Model
{
    public abstract class Player
    {
        public Horneds Horneds { get; set; }
        abstract public void UpdateHorneds(Horneds horneds);
        abstract public string? GetNumbers();
        abstract public void Notify(string message, NotifyCode notifyCode);
    }
}
