using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCompression.Compressor
{
    public class CompressionClient
    {
        private ICompressor compressor;

        public CompressionClient(ICompressor compressor)
        {
            this.compressor = compressor;
        }

        public bool Compress(byte[] inputData, out byte[] compressedData)
        {
            compressedData = null;
            if (inputData == null || inputData.Length == 0)
            {
                Console.WriteLine("No data to compress.");
                return false;
            }

            return compressor.Compress(inputData, ref compressedData);
        }

        public bool Decompress(byte[] inputData, out byte[] decompressedData)
        {
            decompressedData = null;
            if (inputData == null || inputData.Length == 0)
            {
                Console.WriteLine("No data to decompress.");
                return false;
            }

            return compressor.Decompress(inputData, ref decompressedData);
        }
    }
}
