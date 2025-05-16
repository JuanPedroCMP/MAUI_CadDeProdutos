using System.Xml.Linq;
using Microsoft.Maui.Controls;

namespace MauiAppCadProduto;

public partial class ListaProdutosPage : ContentPage
{
    public ListaProdutosPage()
    {
        InitializeComponent();

        produtosCollectionView.ItemsSource = MainPage.Produtos;     
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        filtroPicker.SelectedItem = "Todos";
        produtosCollectionView.ItemsSource = MainPage.Produtos.OrderBy(p => p.DataValiReal).ToList();
    
        VerificarValidade();
        AtualizarResumo();
    }

    public void VerificarValidade()
    {
        DateTime dataAt = DateTime.Now.Date;

        List<Produto> Produtos = new();

        int numProdVencidos = 0;
        int numQuaseProdVencidos = 0;

        foreach (Produto produto in MainPage.Produtos)
        {
            Produto produtoOrigi = produto;
            if (DateTime.Now.Date < produto.DataValiReal.Date)
            {
                produto.color = "LightGreen";
            }
            else if (DateTime.Now.Date > produto.DataValiReal.Date)
            {
                numProdVencidos++;
                produto.color = "Red";
            }
            else
            {
                numQuaseProdVencidos++;
                produto.color = "Yellow";
            }
            Produtos.Add(produtoOrigi);
        }
        MainPage.Produtos = Produtos;

        if (numProdVencidos > 0 || numQuaseProdVencidos > 0)
        {
            alertaLabel.Text = $"⚠️⚠️⚠️ Atenção: {numProdVencidos} vencido(s),{numQuaseProdVencidos}  prestes a vencer! ⚠️⚠️⚠️";
        }

    }

    private void filtroPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        string categoriaSelecionada = filtroPicker.SelectedItem?.ToString() ?? "Todos";

        if (categoriaSelecionada == "Todos")
        {
            produtosCollectionView.ItemsSource = MainPage.Produtos.OrderBy(p => p.DataValiReal).ToList();
        }
        else
        {
            var produtosFiltrados = MainPage.Produtos 
                .Where(p => p.Categoria == categoriaSelecionada)
                .OrderBy(p => p.DataValiReal)
                .ToList();
            produtosCollectionView.ItemsSource = produtosFiltrados;
        }
    }

    private async void BotaoRemover_Clicked(object sender, EventArgs e)
    {
        if (sender is Button botao && botao.BindingContext is Produto produto)
        {
            bool confirmar = await DisplayAlert("Remover Produto",
                $"Deseja remover o produto \"{produto.Nome}\"?", "Sim", "Não");

            if (confirmar)
            {
                MainPage.Produtos.Remove(produto);
                ProdutoStorage.SalvarProdutos(MainPage.Produtos);
                produtosCollectionView.ItemsSource = MainPage.Produtos.OrderBy(p => p.DataValiReal).ToList();

            }
        }
    }

    private void AtualizarResumo()
    {
        var produtos = produtosCollectionView.ItemsSource as List<Produto>;
        int quantidade = produtos?.Count ?? 0;
        double total = produtos?.Sum(p => p.Preco) ?? 0;
        resumoLabel.Text = $"Total: {quantidade} produto(s) - Valor: R$ {total:F2}";
    }
}