namespace SnkConnection
{
    public struct SnkConnectorName
    {
        public string Host;
        public int Port;
        public string ProtocolType;

        public override string ToString() => $"[{ProtocolType}]{Host}:{Port}";
    }
}
