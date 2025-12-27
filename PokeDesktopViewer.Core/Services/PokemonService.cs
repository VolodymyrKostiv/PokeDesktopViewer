using PokeApiNet;
using PokeDesktopViewer.Core.Entities;

namespace PokeDesktopViewer.Core.Services;

public class PokemonService
{
    private readonly PokeApiClient _pokeApiClient;
    private readonly SemaphoreSlim _semaphoreSlim = new(8);

    public PokemonService()
    {
        _pokeApiClient = new PokeApiClient();
    }

    public async IAsyncEnumerable<List<PokemonDto>> GetPokemons(int count)
    {
        const int chunkSize = 25;

        var pokemonsPage = await _pokeApiClient.GetNamedResourcePageAsync<Pokemon>(count, 0);
        int pokemonsPageCount = pokemonsPage.Results.Count;

        for (int i = 0; i < pokemonsPageCount; i += chunkSize)
        {
            var chunk = pokemonsPage.Results.Skip(i).Take(chunkSize);
            var batchResult = await GetChunkOfPokemons(chunk);

            yield return batchResult;
        }
    }

    private async Task<List<PokemonDto>> GetChunkOfPokemons(IEnumerable<NamedApiResource<Pokemon>> chunk)
    {
        var tasks = chunk.Select(async p =>
        {
            await _semaphoreSlim.WaitAsync();

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
                _semaphoreSlim.Release();
            }
        });

        var results = await Task.WhenAll(tasks);

        return [.. results];
    }
}
