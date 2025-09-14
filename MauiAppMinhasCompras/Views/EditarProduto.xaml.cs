using MauiAppMinhasCompras.Models;
using System.Globalization;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
    public EditarProduto()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is Produto produto)
        {
            txt_categoria.Text = produto.Categoria;
            txt_descricao.Text = produto.Descricao;
            txt_quantidade.Text = produto.Quantidade.ToString(CultureInfo.CurrentCulture);
            txt_preco.Text = produto.Preco.ToString(CultureInfo.CurrentCulture);
        }
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (BindingContext is Produto produto_anexado)
            {
                // conversões seguras
                double quantidade = double.TryParse(txt_quantidade.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double q) ? q : 0;
                double preco = double.TryParse(txt_preco.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double p) ? p : 0;

                Produto produtoAtualizado = new Produto
                {
                    Id = produto_anexado.Id,
                    Categoria = txt_categoria.Text,
                    Descricao = txt_descricao.Text,
                    Quantidade = quantidade,
                    Preco = preco
                };

                await App.Db.Update(produtoAtualizado);
                await DisplayAlert("Sucesso!", "Registro Atualizado", "OK");
                await Navigation.PopAsync();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}