using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCompression.App.Compressor
{
    public class GZipCompressor : ICompressor
    {
        public bool Compress(byte[] data, ref byte[] compressed)
        {
            try
            {
                using (var compressedStream = new MemoryStream())
                {
                    using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                    {
                        zipStream.Write(data, 0, data.Length);
                    }
                    compressed = compressedStream.ToArray();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Compression failed: " + ex.Message);
                return false;
            }
        }

        public bool Decompress(byte[] data, ref byte[] decompressed)
        {
            try
            {
                using (var compressedStream = new MemoryStream(data))
                using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                using (var resultStream = new MemoryStream())
                {
                    zipStream.CopyTo(resultStream);
                    decompressed = resultStream.ToArray();
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