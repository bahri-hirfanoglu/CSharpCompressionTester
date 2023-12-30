using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Compressor;

namespace CSharpCompression.App.Compressor
{
    public class BrotliCompressor : ICompressor
    {
        public bool Compress(byte[] data, ref byte[] compressed)
        {
            try
            {
                using (var compressedStream = new MemoryStream())
                {
                    using (var brotliStream = new BrotliStream(compressedStream, CompressionMode.Compress))
                    {
                        brotliStream.Write(data, 0, data.Length);
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
                using (var brotliStream = new BrotliStream(compressedStream, CompressionMode.Decompress))
                using (var resultStream = new MemoryStream())
                {
                    brotliStream.CopyTo(resultStream);
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
