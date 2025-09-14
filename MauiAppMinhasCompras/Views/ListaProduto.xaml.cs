using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();
        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await CarregarProdutos();
        await CarregarCategorias();
    }

    private async Task CarregarProdutos(string categoria = null, string pesquisa = null)
    {
        try
        {
            lista.Clear();
            var produtos = await App.Db.GetAll();

            if (!string.IsNullOrEmpty(pesquisa))
                produtos = produtos.Where(p => p.Descricao.ToLower().Contains(pesquisa.ToLower())).ToList();

            if (!string.IsNullOrEmpty(categoria) && categoria != "Todas")
                produtos = produtos.Where(p => p.Categoria == categoria).ToList();

            produtos.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async Task CarregarCategorias()
    {
        var produtos = await App.Db.GetAll();
        var categorias = produtos.Select(p => p.Categoria).Distinct().OrderBy(c => c).ToList();
        categorias.Insert(0, "Todas"); // opção para mostrar todos

        pickerCategoria.ItemsSource = categorias;
        pickerCategoria.SelectedIndex = 0;
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        string pesquisa = e.NewTextValue;
        string categoria = pickerCategoria.SelectedItem?.ToString();
        await CarregarProdutos(categoria, pesquisa);
    }

    private async void pickerCategoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        string categoria = pickerCategoria.SelectedItem?.ToString();
        string pesquisa = txt_search.Text;
        await CarregarProdutos(categoria, pesquisa);
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NovoProduto());
    }

    private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);
        await DisplayAlert("Total dos Produtos", $"O total é {soma:C}", "OK");
    }

    private async void ToolbarItem_Clicked_2(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RelatorioCategoria());
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        if (sender is MenuItem selecionado && selecionado.BindingContext is Produto p)
        {
            bool confirm = await DisplayAlert("Confirmação",
                $"Deseja remover o produto {p.Descricao}?", "Sim", "Não");

            if (confirm)
            {
                lista.Remove(p);
                await App.Db.delete(p.Id);
            }
        }
    }

    private async void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Produto p)
        {
            await Navigation.PushAsync(new EditarProduto { BindingContext = p });
            lst_produtos.SelectedItem = null;
        }
    }

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        await CarregarProdutos(pickerCategoria.SelectedItem?.ToString(), txt_search.Text);
        lst_produtos.IsRefreshing = false;
    }
}