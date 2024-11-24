//using System;
//using System.Net.Sockets;
//using System.Threading.Tasks;

//namespace SnkConnection
//{
//    namespace IO
//    {
//        public class SnkNetworkStream : SnkDisposable
//        {
//            private readonly Socket _socket;
//            private SnkByteBuffer _byteBuffer;

//            private byte[] _receiveBuffer;
//            private byte[] _sendBuffer;

//            private NetworkStream _networkStream;
//            public SnkNetworkStream(Socket socket)
//            {
//                _socket = socket;
//                _networkStream = new NetworkStream(socket);
//                _networkStream.
//                _receiveBuffer = new byte[socket.ReceiveBufferSize * 2];
//                _sendBuffer = new byte[socket.SendBufferSize * 2];
//            }

//            public void Flush() 
//            {
//            }

//            public async Task<int> Read(byte[] buffer, int offset, int count) 
//            {
//                var segment = new ArraySegment<byte>(buffer, offset, count);
//                return await this._socket.ReceiveAsync(segment, SocketFlags.None).ConfigureAwait(false);
//            }

//            public async Task<int> Write(byte[] buffer, int offset, int count)
//            {
//                var segment = new ArraySegment<byte>(buffer, offset, count);
//                return await this._socket.SendAsync(segment, SocketFlags.None).ConfigureAwait(false);
//            }

//            public async Task FlushAsync() 
//            {
//                var segment = new ArraySegment<byte>();
//                await _socket.SendAsync(segment, SocketFlags.None).ConfigureAwait(false);
//            }

//            public void Close() { }

//            protected override void OnDispose()
//            {

//            }
//        }
//    }
//}
