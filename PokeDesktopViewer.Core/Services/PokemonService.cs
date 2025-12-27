using PokeApiNet;
using PokeDesktopViewer.Core.Entities;
using System.Collections.ObjectModel;

namespace PokeDesktopViewer.Core.Services;

public class PokemonService
{
    private readonly PokeApiClient _pokeApiClient;

    public PokemonService()
    {
        _pokeApiClient = new PokeApiClient();
    }

    public async Task<ObservableCollection<PokemonDto>> GetPokemons(int count)
    {
        var pokemons = new ObservableCollection<PokemonDto>();
        const int chunkSize = 25;

        var pokemonsPage = await _pokeApiClient.GetNamedResourcePageAsync<Pokemon>(count, 0);

        for (int i = 0; i < count; i += chunkSize)
        {
            var chunk = pokemonsPage.Results.Skip(i).Take(chunkSize);
            var batchResult = await GetChunkOfPokemons(chunk);

            if (batchResult != null && batchResult.Length > 0)
            {
                foreach (var item in batchResult)
                {
                    pokemons.Add(item);
                }
            }
        }

        return pokemons;
    }

    private async Task<PokemonDto[]?> GetChunkOfPokemons(IEnumerable<NamedApiResource<Pokemon>> chunk)
    {
        var semaphoreSlim = new SemaphoreSlim(8);

        var tasks = chunk.Select(async p =>
        {
            await semaphoreSlim.WaitAsync();

            try
            {
                var pokemon = await _pokeApiClient.GetResourceAsync(p);
                return new PokemonDto
                {
                    Name = pokemon.Name,
                    Sprite = pokemon.Sprites.FrontDefault
                };
            }
            finally
            {
                semaphoreSlim.Release();
            }
        });

        var results = await Task.WhenAll(tasks);

        return results;
    }
}
