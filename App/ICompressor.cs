using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCompression.Compressor
{
    public interface ICompressor
    {
        bool Compress(byte[] data, ref byte[] compressed);
        bool Decompress(byte[] data, ref byte[] decompressed);
    }

}
