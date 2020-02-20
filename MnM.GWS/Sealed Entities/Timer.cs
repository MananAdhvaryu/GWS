using System;
using System.Diagnostics;
using System.Threading.Tasks;

using static MnM.GWS.Implementation;

namespace MnM.GWS
{
#if AllHidden
    partial class GwsFactory
    {
#else
        public
# endif
        sealed class Timer : ITimer
        {
            #region VARIABLES
            int interval = 5;
            bool running = false;
            Stopwatch watch;
            bool enabled;
            static readonly EventArgs empty = new EventArgs();
            EventHandler<IEventArgs> tick;
            #endregion

            #region CONSTRUCTORS
            public Timer(int interval = 5)
            {
                ID = Factory.NewID("Timer");
                Factory.Add(this, ObjType.Timer);
                watch = new Stopwatch();

                if (interval < 5)
                    interval = 5;
                this.interval = interval;
            }
            #endregion

            #region PROPERTIES
            public bool Enabled
            {
                get => enabled;
                set
                {
                    if (value)
                        Start();
                    else
                        Stop();
                }
            }
            public int Interval
            {
                get => interval;
                set
                {
                    if (interval < 5)
                        return;
                    interval = value;
                }
            }
            public bool Due => enabled && watch.ElapsedMilliseconds >= interval && tick != null;
            public string ID { get; private set; }
            #endregion

            #region METHODS
            public void Start()
            {
                enabled = true;
                watch.Restart();
                running = false;
            }
            public void Stop()
            {
                enabled = false;
                watch.Stop();
                watch.Reset();
                running = false;
            }
            public void Reset()
            {
                var _enabled = enabled;
                enabled = false;
                watch.Reset();
                enabled = _enabled;
                running = false;
            }
            public void Restart()
            {
                Reset();
                Start();
            }
            public void FireEvent()
            {
                if (!Enabled)
                    return;
                if (running)
                    return;
                if (Due)
                {
                    running = true;
                    new Task(() =>
                    {
                        tick?.Invoke(this, empty);
                        watch.Restart();
                    }
                    ).RunSynchronously();
                    running = false;
                }
                running = false;
            }

            public void Dispose()
            {
                watch = null;
                tick = null;
                Factory.Remove(this, ObjType.Timer);
            }
            #endregion

            #region EVENTS
            public event EventHandler<IEventArgs> Tick
            {
                add
                {
                    if (tick == null)
                        tick += value;
                }
                remove
                {
                    tick -= value;
                }
            }
            #endregion

            void IStoreable.AssignIDIfNone()
            {
                if (ID == null)
                    ID = Factory.NewID(this);
            }
        }
#if AllHidden
    }
#endif
}
