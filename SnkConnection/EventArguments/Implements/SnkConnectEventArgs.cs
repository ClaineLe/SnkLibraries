namespace SnkConnection
{
    namespace EventArguments
    {
        public class SnkConnectEventArgs : SnkConnectionEventArgs
        {
            public bool Result { get; }
            public bool Connecting { get; }
            public SnkConnectEventArgs(bool result, bool connecting)
            {
                this.Result = result;
                this.Connecting = connecting;
            }
        }
    }
}
