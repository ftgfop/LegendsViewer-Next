namespace LegendsViewer.Backend.Legends.Parser;

public class FilteredStream : Stream
{
    private readonly Stream _baseStream;
    private bool _disposed = false;

    public FilteredStream(Stream baseStream)
    {
        _baseStream = baseStream ?? throw new ArgumentNullException(nameof(baseStream));
    }

    public override bool CanRead => _baseStream.CanRead;

    public override bool CanSeek => _baseStream.CanSeek;

    public override bool CanWrite => _baseStream.CanWrite;

    public override long Length => _baseStream.Length;

    public override long Position { get => _baseStream.Position; set => _baseStream.Position = value; }

    public override void Flush()
    {
        _baseStream.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        int bytesRead = _baseStream.Read(buffer, offset, count);
        if (bytesRead == 0) return 0;

        for (int i = 0; i < bytesRead; i++)
        {
            if (buffer[offset + i] < 32)
            {
                buffer[offset + i] = (byte)' ';
            }
        }
        return bytesRead;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return _baseStream.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        _baseStream.SetLength(value);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        _baseStream.Write(buffer, offset, count);
    }

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _baseStream.Dispose();
            }
            _disposed = true;
            base.Dispose(disposing);
        }
    }
}