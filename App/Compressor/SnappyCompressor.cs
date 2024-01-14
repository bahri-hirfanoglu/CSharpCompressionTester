using CSharpCompression.Compressor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snappy.Sharp;

namespace CSharpCompression.App.Compressor
{
    public class SnappyCompressor : ICompressor
    {
        public bool Compress(byte[] data, ref byte[] compressed)
        {
            try
            {
                var snappyCompressor = new Snappy.Sharp.SnappyCompressor();
                int maxCompressedLength = snappyCompressor.MaxCompressedLength(data.Length);
                compressed = new byte[maxCompressedLength];
                int compressedLength = snappyCompressor.Compress(data, 0, data.Length, compressed);
                Array.Resize(ref compressed, compressedLength);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Compression failed: " + ex.Message);
                return false;
            }
        }

        public bool Decompress(byte[] compressed, ref byte[] decompressed)
        {
            try
            {
                var snappyDecompressor = new Snappy.Sharp.SnappyDecompressor();
                decompressed = snappyDecompressor.Decompress(compressed, 0, compressed.Length);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Decompression failed: " + ex.Message);
                return false;
            }
        }
    }

}
