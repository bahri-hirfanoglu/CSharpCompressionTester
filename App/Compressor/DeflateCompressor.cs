using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCompression.App.Compressor
{
    public class DeflateCompressor : ICompressor
    {
        public bool Compress(byte[] data, ref byte[] compressed)
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress))
                {
                    deflateStream.Write(data, 0, data.Length);
                    deflateStream.Close();
                    compressed = memoryStream.ToArray();
                    return true;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Compression failed: " + ex.Message);
                return false;
            }
        }

        public bool Decompress(byte[] data, ref byte[] decompressed)
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream(data))
                using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress))
                using (MemoryStream decompressedStream = new MemoryStream())
                {
                    deflateStream.CopyTo(decompressedStream);
                    decompressed = decompressedStream.ToArray();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Decompression failed: " + ex.Message);
                return false;
            }
        }
    }
}
