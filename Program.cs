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
            Console.Title = "Compression Performance Tester";

            int sizeInMB;
            string input;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter the size of the byte array in MB:");
                input = Console.ReadLine();
            }
            while (!int.TryParse(input, out sizeInMB) || sizeInMB <= 0);

            byte[] testData = new byte[sizeInMB * 1024 * 1024];
            new Random().NextBytes(testData);

            Console.Clear();

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