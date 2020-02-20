namespace MnM.GWS
{
    public class EventInfo : IEventInfo
    {
        public static readonly EventInfo Empty = new EventInfo();
        internal EventInfo() { }
        public EventInfo(string sender, IEventArgs args, GwsEvent type)
        {
            Sender = sender;
            Args = args;
            Type = type;
        }
        public string Sender { get; internal set; }
        public IEventArgs Args { get; internal set; }
        public GwsEvent Type { get; internal set; }
    }
}
