using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class RelatorioCategoria : ContentPage
{
    ObservableCollection<CategoriaTotal> lista = new ObservableCollection<CategoriaTotal>();

    public RelatorioCategoria()
    {
        InitializeComponent();
        lst_relatorio.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            lista.Clear();

            // Pega todos os produtos
            var produtos = await App.Db.GetAll();

            // Agrupa por categoria e soma o total
            var agrupados = produtos
                            .GroupBy(p => p.Categoria)
                            .Select(g => new CategoriaTotal
                            {
                                Categoria = g.Key,
                                Total = g.Sum(x => x.Total)
                            })
                            .OrderByDescending(x => x.Total);

            foreach (var item in agrupados)
            {
                lista.Add(item);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}

// Classe auxiliar para o relatório
public class CategoriaTotal
{
    public string Categoria { get; set; }
    public double Total { get; set; }
}
