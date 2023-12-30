using CSharpCompression.Compressor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCompression.App
{
    public class TestCompressor
    {
        private ICompressor compressor;
        private string compressorName;
        private byte[] testData;

        public TestCompressor(ICompressor compressor, byte[] testData, string compressorName)
        {
            this.compressor = compressor;
            this.testData = testData;
            this.compressorName = compressorName;
        }

        public void RunTests(int numberOfRuns = 5)
        {
            Console.WriteLine($"Testing {compressorName}...");

            TestCompressionDecompressionTime();
            TestCompressionRatio();
            TestMemoryUsage();
            TestCPULoad();
            TestIntegrity();
            TestMultipleRuns(numberOfRuns);

            Console.WriteLine($"Completed tests for {compressorName}.\n");
        }

        private void TestCompressionDecompressionTime()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            byte[] compressedData = null;
            bool compressed = compressor.Compress(testData, ref compressedData);

            stopwatch.Stop();
            TimeSpan compressionTime = stopwatch.Elapsed;

            stopwatch.Restart();
            byte[] decompressedData = null;
            bool decompressed = compressor.Decompress(compressedData, ref decompressedData);

            stopwatch.Stop();
            TimeSpan decompressionTime = stopwatch.Elapsed;

            if (compressed && decompressed)
            {
                Console.WriteLine($"Compression Time: {compressionTime.TotalMilliseconds} ms");
                Console.WriteLine($"Decompression Time: {decompressionTime.TotalMilliseconds} ms");
            }
            else
            {
                Console.WriteLine("Compression/Decompression failed.");
            }
        }

        private void TestCompressionRatio()
        {
            byte[] compressedData = null;
            compressor.Compress(testData, ref compressedData);
            double ratio = (double)compressedData.Length / testData.Length;
            Console.WriteLine($"Compression Ratio: {ratio:P2} (smaller is better)");
        }

        private void TestCPULoad()
        {
            var process = Process.GetCurrentProcess();

            byte[] compressedData = null;
            compressor.Compress(testData, ref compressedData);

            double compressionCpuUsage = process.TotalProcessorTime.TotalMilliseconds;

            byte[] decompressedData = null;
            compressor.Decompress(compressedData, ref decompressedData);

            double totalCpuUsage = process.TotalProcessorTime.TotalMilliseconds;
            double decompressionCpuUsage = totalCpuUsage - compressionCpuUsage;

            Console.WriteLine($"CPU Time for Compression: {compressionCpuUsage} ms");
            Console.WriteLine($"CPU Time for Decompression: {decompressionCpuUsage} ms");
        }
        private void TestMemoryUsage()
        {
            long initialMemory = GC.GetTotalMemory(true);
            byte[] compressedData = null;
            compressor.Compress(testData, ref compressedData);
            byte[] decompressedData = null;
            compressor.Decompress(compressedData, ref decompressedData);
            long finalMemory = GC.GetTotalMemory(false);

            Console.WriteLine($"Memory Usage: {finalMemory - initialMemory} bytes");
        }
        private void TestIntegrity()
        {
            byte[] compressedData = null;
            compressor.Compress(testData, ref compressedData);
            byte[] decompressedData = null;
            compressor.Decompress(compressedData, ref decompressedData);

            if (testData.SequenceEqual(decompressedData))
            {
                Console.WriteLine("Integrity Test Passed: Decompressed data matches the original.");
            }
            else
            {
                Console.WriteLine("Integrity Test Failed: Decompressed data does not match the original.");
            }
        }

        private void TestMultipleRuns(int numberOfRuns)
        {
            Stopwatch stopwatch = new Stopwatch();
            double totalCompressionTime = 0;
            double totalDecompressionTime = 0;
            long totalMemoryUsed = 0;

            for (int i = 0; i < numberOfRuns; i++)
            {
                byte[] compressedData = null;
                byte[] decompressedData = null;

                long initialMemory = GC.GetTotalMemory(true);

                stopwatch.Restart();
                compressor.Compress(testData, ref compressedData);
                stopwatch.Stop();
                totalCompressionTime += stopwatch.ElapsedMilliseconds;

                stopwatch.Restart();
                compressor.Decompress(compressedData, ref decompressedData);
                stopwatch.Stop();
                totalDecompressionTime += stopwatch.ElapsedMilliseconds;

                long finalMemory = GC.GetTotalMemory(false);
                totalMemoryUsed += finalMemory - initialMemory;

                if (!testData.SequenceEqual(decompressedData))
                {
                    Console.WriteLine($"Integrity failed on run {i + 1}");
                    break;
                }
            }

            Console.WriteLine($"Average Compression Time over {numberOfRuns} runs: {totalCompressionTime / numberOfRuns} ms");
            Console.WriteLine($"Average Decompression Time over {numberOfRuns} runs: {totalDecompressionTime / numberOfRuns} ms");
            Console.WriteLine($"Average Memory Usage over {numberOfRuns} runs: {totalMemoryUsed / numberOfRuns} bytes");
        }
    }
}
