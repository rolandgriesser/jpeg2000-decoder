
using System;
using System.IO;
using jpeg2000_decoder.Exceptions;

namespace jpeg2000_decoder.IO
{

    public class RandomAccessFile : IRandomAccessIO
    {
        private readonly Stream _stream;
        private byte[] _buffer;
        private int _maxBytes; // current buffered bytes count
        private long _pos; // current position in buffer
        private long _offset;
        private bool _isEOFInBuffer;

        public long Position => _offset + _pos;
        public long Length
        {
            get
            {
                var len = _stream.Length;

                // If the position in the buffer is not past the end of the file,
                // the length of theFile is the length of the stream
                if ((_offset + _maxBytes) <= len)
                {
                    return len;
                }
                else
                { // If not, the file is extended due to the buffering
                    return (_offset + _maxBytes);
                }

            }
        }

        public RandomAccessFile(Stream stream, int bufferSize = 1024)
        {
            _stream = stream;
            _buffer = new byte[bufferSize];
            ReadNewBuffer(0);
        }

        public void Seek(long offset)
        {
            if (offset >= _offset && offset < _offset + _buffer.Length)
            {
                if (_isEOFInBuffer && offset > _offset + _maxBytes)
                {
                    throw new EndOfFileException();
                }
                _pos = offset - _offset;
            }
            else
                ReadNewBuffer(offset);
        }
        //todo test
        public ulong ReadLong()
        {
            return (ulong)((Read() << 56) | (Read() << 48) | (Read() << 40) |
                (Read() << 32) | (Read() << 24) | (Read() << 16) |
                (Read() << 8) | (Read()));
        }

        public uint ReadInt()
        {
            return (uint)(Read() << 24 | Read() << 16 | Read() << 8 | Read());
        }

        public ushort ReadShort()
        {
            return (ushort)(Read() << 8 | Read());
        }

        public int Read()
        {
            if (_pos < _maxBytes)
            {
                return _buffer[_pos++];
            }
            else if (_isEOFInBuffer)
            {
                _pos = _maxBytes + 1;
                throw new EndOfFileException();
            }
            else
            {
                ReadNewBuffer(_offset + _pos);
                return Read();
            }
        }

        private void ReadNewBuffer(long offset)
        {
            _offset = offset;
            _stream.Seek(_offset, SeekOrigin.Begin);
            _maxBytes = _stream.Read(_buffer, 0, _buffer.Length);
            _pos = 0;
            _isEOFInBuffer = _maxBytes < _buffer.Length;
            if (_maxBytes == -1)
                _maxBytes = 0;
        }

        public void ReadFully(byte[] buffer, long offset, long length)
        {
            long clen; // current length to read
            while (length > 0)
            {
                // There still is some data to read
                if (_pos < _maxBytes)
                { // We can read some data from buffer
                    clen = _maxBytes - Position;
                    if (clen > length) clen = length;
                    Array.Copy(_buffer, _pos, buffer, offset, clen);
                    _pos += clen;
                    offset += clen;
                    length -= clen;
                }
                else if (_isEOFInBuffer)
                {
                    _pos = _maxBytes + 1; // Set position to EOF
                    throw new EndOfFileException();
                }
                else
                { // Buffer empty => get more data
                    ReadNewBuffer(_offset + _pos);
                }
            }
        }
    }
}