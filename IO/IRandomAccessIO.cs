namespace jpeg2000_decoder.IO
{
    public interface IRandomAccessIO
    {
        long Position {get;}
        long Length {get;}
        void Seek(long offset);
        uint ReadInt();
        ushort ReadShort();
        ulong ReadLong();
    }
}