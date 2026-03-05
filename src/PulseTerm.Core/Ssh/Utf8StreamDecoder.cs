using System.Text;

namespace PulseTerm.Core.Ssh;

public class Utf8StreamDecoder
{
    private readonly List<byte> _buffer = new();
    private readonly Decoder _decoder = Encoding.UTF8.GetDecoder();

    public string DecodeBytes(byte[] bytes)
    {
        if (bytes.Length == 0)
        {
            return string.Empty;
        }

        _buffer.AddRange(bytes);

        var charBuffer = new char[_buffer.Count * 2];
        var bytesUsed = 0;
        var charsUsed = 0;
        var completed = false;

        _decoder.Convert(
            _buffer.ToArray(),
            0,
            _buffer.Count,
            charBuffer,
            0,
            charBuffer.Length,
            flush: false,
            out bytesUsed,
            out charsUsed,
            out completed);

        _buffer.RemoveRange(0, bytesUsed);

        return new string(charBuffer, 0, charsUsed);
    }

    public void Reset()
    {
        _buffer.Clear();
        _decoder.Reset();
    }
}
