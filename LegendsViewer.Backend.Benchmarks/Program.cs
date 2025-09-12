using System;
using LegendsViewer.Backend.Benchmarks;

namespace LegendsViewer.Backend.Benchmarks;

public class Program
{
    public static void Main(string[] args)
    {
        // Register code page provider for legacy encodings (e.g., CP437)
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        BenchmarkDotNet.Running.BenchmarkRunner.Run<XmlParserBenchmarks>();
    }
}
