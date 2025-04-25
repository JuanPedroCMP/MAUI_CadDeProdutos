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
    }

    public void VerificarValidade()
    {
        DateTime dataAt = DateTime.Now.Date;

        List<Produto> Produtos = new();

        foreach (Produto produto in MainPage.Produtos)
        {
            Produto produtoOrigi = produto;
            if (DateTime.Now.Date < produto.DataValiReal.Date)
            {
                produto.color = "LightGreen";
            }
            else if (DateTime.Now.Date > produto.DataValiReal.Date)
            {
                produto.color = "Red";
            }
            else
            {
                produto.color = "Yellow";
            }
            Produtos.Add(produtoOrigi);
        }
        MainPage.Produtos = Produtos;
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
}