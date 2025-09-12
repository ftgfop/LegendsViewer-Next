using System;
using System.IO;
using System.Threading.Tasks;
using LegendsViewer.Backend.Legends.Parser;
using BenchmarkDotNet.Attributes;
using LegendsViewer.Backend.Legends;

namespace LegendsViewer.Backend.Benchmarks;
[MemoryDiagnoser]
public class XmlParserBenchmarks
{
    private XmlParser? _xmlParser;
    private string _xmlFile = "Xah_Atho-00005-01-01-legends.xml";
    private string? _xmlPlusFile = "Xah_Atho-00005-01-01-legends_plus.xml";
    private World? _world;

    [GlobalSetup]
    public void GlobalSetup()
    {
        // Register code page provider for legacy encodings (e.g., CP437)
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        // Find the solution root by traversing up from the current directory
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, "LegendsViewer.sln")))
        {
            dir = dir.Parent;
        }
        if (dir == null)
            throw new DirectoryNotFoundException("Could not find solution root (LegendsViewer-Next.sln)");
        var testDataDir = Path.Combine(dir.FullName, "LegendsViewer.Backend.Tests", "TestData");
        _xmlFile = Path.Combine(testDataDir, "Xah_Atho-00005-01-01-legends.xml");
        _xmlPlusFile = Path.Combine(testDataDir, "Xah_Atho-00005-01-01-legends_plus.xml");
    }

    [IterationSetup]
    public void IterationSetup()
    {
        // Recreate world and parser for each iteration
        _world = new World();
        _xmlParser = new XmlParser(_world, _xmlFile, _xmlPlusFile);
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        _xmlParser?.Dispose();
        _xmlParser = null;
        _world?.Dispose();
        _world = null;
    }

    [Benchmark]
    public async Task ParseAsync_Benchmark()
    {
        await _xmlParser.ParseAsync();
    }
}