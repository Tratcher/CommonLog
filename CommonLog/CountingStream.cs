using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLog
{
    public class CountingStream : Stream
    {
        public CountingStream(Stream innerStream)
        {
            InnerStream = innerStream;
        }

        public Stream InnerStream { get; }

        public long BytesWritten { get; private set; }

        public override bool CanRead => InnerStream.CanRead;

        public override bool CanSeek => InnerStream.CanSeek;

        public override bool CanWrite => InnerStream.CanWrite;

        public override long Length => InnerStream.Length;

        public override long Position { get => InnerStream.Position; set => InnerStream.Position = value; }

        public override void Flush()
        {
            InnerStream.Flush();
        }

        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            return InnerStream.FlushAsync(cancellationToken);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return InnerStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return InnerStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            InnerStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            BytesWritten += count;
            InnerStream.Write(buffer, offset, count);
        }

        public override void Write(ReadOnlySpan<byte> buffer)
        {
            BytesWritten += buffer.Length;
            InnerStream.Write(buffer);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            BytesWritten += count;
            return InnerStream.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            InnerStream.EndWrite(asyncResult);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            BytesWritten += count;
            return InnerStream.WriteAsync(buffer, offset, count, cancellationToken);
        }

        public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
        {
            BytesWritten += buffer.Length;
            return InnerStream.WriteAsync(buffer, cancellationToken);
        }
    }
}
