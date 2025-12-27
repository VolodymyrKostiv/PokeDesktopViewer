using BenchmarkDotNet.Attributes;
using PokeDesktopViewer.Core.Entities;
using PokeDesktopViewer.Core.Services;
using System.Collections.ObjectModel;

namespace PokeDesktopViewer.Benchmark;

[MemoryDiagnoser]
[MarkdownExporter]
[HtmlExporter]
public class PokemonServiceBenchmark
{
    [Params(1, 10, 25, 50, 100, 250, 500)]
    public int ItemCount;

    [Benchmark]
    public async Task GetPokemonsBenchmark()
    {
        var pokemonService = new PokemonService();
        var pokemons = new ObservableCollection<PokemonDto>();
        await foreach (var batch in pokemonService.GetPokemons(ItemCount))
        {
            foreach (var pokemon in batch)
            {
                pokemons.Add(pokemon);
            }
        }
    }
}
