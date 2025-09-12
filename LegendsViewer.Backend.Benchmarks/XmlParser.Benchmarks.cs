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
        // Use absolute paths for test data files
        var absXmlFile = @"C:\Users\Thomas Svoboda\source\repos\LegendsViewer-Next\LegendsViewer.Backend.Tests\TestData\Xah_Atho-00005-01-01-legends.xml";
        var absXmlPlusFile = @"C:\Users\Thomas Svoboda\source\repos\LegendsViewer-Next\LegendsViewer.Backend.Tests\TestData\Xah_Atho-00005-01-01-legends_plus.xml";
        _xmlFile = absXmlFile;
        _xmlPlusFile = absXmlPlusFile;
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