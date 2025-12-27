using CommunityToolkit.Mvvm.ComponentModel;
using PokeDesktopViewer.Core.Entities;
using PokeDesktopViewer.Core.Services;
using System.Collections.ObjectModel;

namespace PokeDesktopViewer.WPF.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<PokemonDto> _pokemons;

    public MainViewModel()
    {
        Get();
    }

    public async Task Get()
    {
        var pokemonService = new PokemonService();
        Pokemons = await pokemonService.GetPokemons(50);
    }
}
