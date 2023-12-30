using CSharpCompression.App;
using CSharpCompression.App.Compressor;
using CSharpCompression.Compressor;
using System.Reflection;

namespace CSharpCompression
{
    internal class Program
    {
        static void Main(string[] args)
        {
            byte[] testData = new byte[10000000];
            new Random().NextBytes(testData);

            var typesInAssembly = Assembly.GetExecutingAssembly().GetTypes();

            var compressorTypes = typesInAssembly.Where(type => typeof(ICompressor).IsAssignableFrom(type) && !type.IsAbstract).ToList();

            foreach (var type in compressorTypes)
            {
                var compressorInstance = (ICompressor)Activator.CreateInstance(type);
                new TestCompressor(compressorInstance, testData, type.Name).RunTests();
            }

            Console.ReadKey();
        }
    }
}