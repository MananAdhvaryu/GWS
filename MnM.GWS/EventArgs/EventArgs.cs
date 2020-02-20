namespace MnM.GWS
{
    public class EventArgs: System.EventArgs, IEventArgs
    {
        public static readonly new EventArgs Empty = new EventArgs();
        public bool Handled { get; set; }
    }
}
