# Compression Performance Tester

## Overview
Compression Performance Tester is a C# application designed to evaluate and compare the performance of various compression algorithms. This tool allows users to test different aspects of compression algorithms such as time efficiency, compression ratio, memory usage, CPU load, and data integrity.

## Algorithms
This application currently supports the following compression algorithms:
- **Brotli**: A modern, lossless compression algorithm known for its efficiency in compressing web content. It provides high compression ratios and fast decompression speeds, making it ideal for web applications.
- **GZip**: A widely used compression method that balances compression ratio and speed. It is compatible with a broad range of platforms and applications, making it one of the most versatile compression algorithms.
- **Deflate**: An algorithm used in various compression formats including GZip and ZIP. Deflate offers a good balance between compression speed and efficiency, and it's widely supported across different platforms.

## Test Results

The table below compares various performance metrics for the Brotli, GZip, and Deflate algorithms.

|           | Deflate   | GZip     | Brotli    |
|-----------|-----------|----------|-----------|
| Compression Time (ms)       | 410,70    | 405,59   | 10584,17  |
| Decompression Time (ms)     | 12,67     | 14,30     | 13,7     |
| CPU Time for Compression (ms)| 97859,37   | 102109,37  | 39265,52 |
| CPU Time for Decompression (ms)| 15,62   | 15,62  | 15,62 |
| Memory Usage (bytes)        | 45177576  | 45177624  | 45173632  |
| Avg. Compression Time over 5 runs (ms) | 409,6 | 410,2 | 9487 |
| Avg. Decompression Time over 5 runs (ms) | 10,8 | 10,4 | 12,8 |

*Note: These values are provided as examples. Please use the actual results from your tests. All tests were conducted on 10 MB of data.*

## Features
- **Performance Testing**: Measure compression and decompression time, ratio, and CPU usage.
- **Data Integrity Checks**: Ensure that the decompressed data matches the original data.
- **Multiple Run Testing**: Perform tests over multiple iterations to ensure consistency and reliability.

## How to Use
1. Clone the repository to your local machine.
2. Ensure you have the necessary .NET environment to run C# applications.
3. Install required dependencies for the compression algorithms you wish to test.
4. Run the application and choose the compression algorithm and test parameters.
5. View and analyze the test results.

## Contributing
Contributions to add more features, tests, or support for additional compression algorithms are welcome. Please feel free to fork the repository and submit your pull requests.