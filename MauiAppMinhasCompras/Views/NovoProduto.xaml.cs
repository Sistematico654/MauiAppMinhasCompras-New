using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views
{
    public partial class NovoProduto : ContentPage
    {
        public NovoProduto()
        {
            InitializeComponent();
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_categoria.Text) ||
                    string.IsNullOrWhiteSpace(txt_descricao.Text) ||
                    string.IsNullOrWhiteSpace(txt_quantidade.Text) ||
                    string.IsNullOrWhiteSpace(txt_preco.Text))
                {
                    await DisplayAlert("Atenção", "Preencha todos os campos!", "OK");
                    return;
                }

                if (!double.TryParse(txt_quantidade.Text, out double quantidade))
                {
                    await DisplayAlert("Erro", "Quantidade inválida", "OK");
                    return;
                }

                if (!double.TryParse(txt_preco.Text, out double preco))
                {
                    await DisplayAlert("Erro", "Preço inválido", "OK");
                    return;
                }

                Produto produtoNovo = new Produto
                {
                    Categoria = txt_categoria.Text,
                    Descricao = txt_descricao.Text,
                    Quantidade = quantidade,
                    Preco = preco
                };

                await App.Db.Insert(produtoNovo);
                await DisplayAlert("Sucesso!", "Registro inserido com sucesso", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }
}
