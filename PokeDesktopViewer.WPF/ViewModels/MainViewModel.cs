using CommunityToolkit.Mvvm.ComponentModel;
using PokeDesktopViewer.Core.Entities;
using PokeDesktopViewer.Core.Services;
using System.Collections.ObjectModel;

namespace PokeDesktopViewer.WPF.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<PokemonDto> _pokemons = [];

    public MainViewModel()
    {
        Task.Run(InitializePokemonCards);
    }

    public async Task InitializePokemonCards()
    {
        var pokemonService = new PokemonService();
        await foreach (var batch in pokemonService.GetPokemons(150))
        {
            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                foreach (var pokemon in batch)
                {
                    Pokemons.Add(pokemon);
                }
            });
        }
    }
}
