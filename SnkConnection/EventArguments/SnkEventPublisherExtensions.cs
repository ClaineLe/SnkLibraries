using System;

namespace SnkConnection
{
    namespace EventArguments
    {
        public static class SnkEventPublisherExtensions
        {
            public static void PublishException(this SnkEventPublisher publisher, string message, Exception exception)
            {
                publisher.Publish(new SnkExceptionEventArgs(message, exception));
            }

            public static void PublishConnect(this SnkEventPublisher publisher, bool result, bool processing)
            {
                publisher.Publish(new SnkConnectEventArgs(result, processing));
            }
        }
    }
}
