using CSharpCompression.App.Helpers;
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

        // Constructor: Initializes the compressor with the given ICompressor implementation, test data, and compressor name.
        public TestCompressor(ICompressor compressor, byte[] testData, string compressorName)
        {
            this.compressor = compressor;
            this.testData = testData;
            this.compressorName = compressorName;
        }

        // Runs all the tests for the specified compressor, allows specifying the number of runs for multiple run tests.
        public void RunTests(int numberOfRuns = 5)
        {
            Console.WriteLine($"Testing {compressorName}...");

            TestCompressionDecompressionTime();
            TestCompressionRatio();
            TestMemoryUsage();
            TestPeakMemoryUsage();
            TestCPULoad();
            TestDataSizeReduction();
            TestThroughputRate();
            TestIntegrity();
            TestMultipleRuns(numberOfRuns);
            TestStress();

            Console.WriteLine($"Completed tests for {compressorName}.\n");
        }

        // Measures the time taken to compress and decompress the test data, providing insight into the algorithm's speed.
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
                PrintHelper.TableLine("Compression Time (ms)", $"{compressionTime.TotalMilliseconds} ms", ConsoleColor.Green);
                PrintHelper.TableLine("Decompression Time (ms)", $"{decompressionTime.TotalMilliseconds} ms", ConsoleColor.Green);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Compression/Decompression failed.");
                Console.ResetColor();
            }
        }

        // Calculates the compression ratio to understand how effectively the algorithm reduces data size.
        private void TestCompressionRatio()
        {
            byte[] compressedData = null;
            compressor.Compress(testData, ref compressedData);
            double ratio = (double)compressedData.Length / testData.Length;
            PrintHelper.TableLine("Compression Ratio", $"{ratio:P2} (smaller is better)", ConsoleColor.Yellow);
        }

        // Measures the CPU time used for compression and decompression to gauge the algorithm's CPU usage.
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

            PrintHelper.TableLine("CPU Time for Compression (ms)", $"{compressionCpuUsage} ms");
            PrintHelper.TableLine("CPU Time for Decompression (ms)", $"{decompressionCpuUsage} ms");
        }

        // Measures the memory usage before and after compression and decompression to understand the algorithm's memory footprint.
        private void TestMemoryUsage()
        {
            long initialMemory = GC.GetTotalMemory(true);
            byte[] compressedData = null;
            compressor.Compress(testData, ref compressedData);
            byte[] decompressedData = null;
            compressor.Decompress(compressedData, ref decompressedData);
            long finalMemory = GC.GetTotalMemory(false);

            PrintHelper.TableLine("Memory Usage (bytes)", $"{finalMemory - initialMemory}", ConsoleColor.Magenta);
        }

        // Checks if the decompressed data is identical to the original data to ensure the algorithm's reliability.
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

        // Repeats all tests a specified number of times to ensure consistency and reliability of the results.
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

            PrintHelper.TableLine("Average Compression Time (ms)", $"{totalCompressionTime / numberOfRuns} ms");
            PrintHelper.TableLine("Average Decompression Time (ms)", $"{totalDecompressionTime / numberOfRuns} ms");
            PrintHelper.TableLine("Average Memory Usage (bytes)", $"{totalMemoryUsed / numberOfRuns}");
        }

        // Calculates the percentage reduction in data size, providing another perspective on the algorithm's effectiveness.
        private void TestDataSizeReduction()
        {
            byte[] compressedData = null;
            compressor.Compress(testData, ref compressedData);

            long originalSize = testData.Length;
            long compressedSize = compressedData.Length;

            double reductionPercentage = 100 * (1 - (double)compressedSize / originalSize);

            PrintHelper.TableLine("Data Size Reduction", $"{reductionPercentage:F2}% (higher is better)");
        }

        // Measures the throughput rate in MB/s for both compression and decompression, indicating the algorithm's speed in processing data.
        private void TestThroughputRate()
        {
            Stopwatch stopwatch = new Stopwatch();

            byte[] compressedData = null;
            stopwatch.Start();
            compressor.Compress(testData, ref compressedData);
            stopwatch.Stop();
            double compressionThroughput = (testData.Length / 1024.0 / 1024.0) / (stopwatch.ElapsedMilliseconds / 1000.0); // MB/s

            byte[] decompressedData = null;
            stopwatch.Restart();
            compressor.Decompress(compressedData, ref decompressedData);
            stopwatch.Stop();
            double decompressionThroughput = (compressedData.Length / 1024.0 / 1024.0) / (stopwatch.ElapsedMilliseconds / 1000.0); // MB/s

            PrintHelper.TableLine("Compression Throughput (MB/s)", $"{compressionThroughput:F2} MB/s (higher is better)");
            PrintHelper.TableLine("Decompression Throughput (MB/s)", $"{decompressionThroughput:F2} MB/s (higher is better)");
        }

        // Measures the peak memory usage during compression and decompression to identify the maximum memory requirement.
        private void TestPeakMemoryUsage()
        {
            long initialMemory = GC.GetTotalMemory(true);
            byte[] compressedData = null;

            compressor.Compress(testData, ref compressedData);
            long peakMemoryDuringCompression = GC.GetTotalMemory(true) - initialMemory;

            byte[] decompressedData = null;
            initialMemory = GC.GetTotalMemory(true);

            compressor.Decompress(compressedData, ref decompressedData);
            long peakMemoryDuringDecompression = GC.GetTotalMemory(true) - initialMemory;

            PrintHelper.TableLine("Peak Memory Usage during Compression (bytes)", $"{peakMemoryDuringCompression}", ConsoleColor.Blue);
            PrintHelper.TableLine("Peak Memory Usage during Decompression (bytes)", $"{peakMemoryDuringDecompression}", ConsoleColor.Blue);
        }

        private void TestStress()
        {
            int length = testData.Length * 10;
            Console.WriteLine($"Starting Stress Test with {length / 1024 / 1024} MB");

            Stopwatch stopwatchTotal = new Stopwatch();
            stopwatchTotal.Start();

            int largeDataSetSize = length;
            byte[] largeTestData = new byte[largeDataSetSize];
            new Random().NextBytes(largeTestData);

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            byte[] compressedData = null;
            compressor.Compress(largeTestData, ref compressedData);
            stopwatch.Stop();
            PrintHelper.TableLine("Large Data Compression Time (ms)", $"{stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();
            byte[] decompressedData = null;
            compressor.Decompress(compressedData, ref decompressedData);
            stopwatch.Stop();
            PrintHelper.TableLine("Large Data Decompression Time (ms)", $"{stopwatch.ElapsedMilliseconds} ms");

            stopwatchTotal.Stop();
            PrintHelper.TableLine("Total Stress Test Time (ms)", $"{stopwatchTotal.ElapsedMilliseconds} ms");
        }
    }
}
