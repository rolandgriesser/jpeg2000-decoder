namespace jpeg2000_decoder.IO
{
    public interface IRandomAccessIO
    {
        long Position {get;}
        long Length {get;}
        void Seek(long offset);
        uint ReadInt();
        ushort ReadShort();
        ushort ReadUnsignedShort();
        ulong ReadLong();
        void ReadFully(byte[] buffer, long offset, long length);
    }
}