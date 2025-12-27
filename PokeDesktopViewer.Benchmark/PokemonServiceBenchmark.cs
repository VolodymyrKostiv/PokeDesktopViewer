using BenchmarkDotNet.Attributes;
using PokeDesktopViewer.Core.Services;

namespace PokeDesktopViewer.Benchmark;

[MemoryDiagnoser]
[MarkdownExporter]
[HtmlExporter]
public class PokemonServiceBenchmark
{
    [Params(1, 10, 20, 50, 100, 200)]
    public int ItemCount;

    [Benchmark]
    public async Task GetPokemonsBenchmark()
    {
        var pokemonService = new PokemonService();

        await pokemonService.GetPokemons(ItemCount);
    }
}
