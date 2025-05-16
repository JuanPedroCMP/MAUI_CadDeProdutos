using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace MauiAppCadProduto
{
    public partial class MainPage : ContentPage
    {
        private string caminhoImagemSelecionada;
        public static List<Produto> Produtos { get; set; } = ProdutoStorage.CarregarProdutos();
        // public static List<String> Categorias { get; set; } = new();
        // ToDo Adicionar função de criar novas categorias 

        public MainPage()
        {
            InitializeComponent();

            // TODO descomentar para testes
            ////// Apenas para testes
            //Produtos.Add(new Produto("Arroz", 5.99, "Pacote de 1kg de arroz branco", "10/12/2025", "Alimentos", DateTime.Parse("10/12/2025")));
            //Produtos.Add(new Produto("Feijão", 7.49, "Pacote de 1kg de feijão carioca", "15/11/2025", "Alimentos", DateTime.Parse("15/11/2025")));
            //Produtos.Add(new Produto("Macarrão", 3.89, "Pacote de 500g de macarrão espaguete", "20/10/2025", "Alimentos", DateTime.Parse("20/10/2025")));
            //Produtos.Add(new Produto("Refrigerante", 6.50, "Garrafa de 2L de refrigerante cola", "01/01/2026", "Bebidas", DateTime.Parse("01/01/2026")));
            //Produtos.Add(new Produto("Notebook", 3500.00, "Notebook com 8GB RAM e 256GB SSD", "N/A", "Eletrônicos", DateTime.Parse("10/12/2999")));
            //Produtos.Add(new Produto("Camiseta", 49.90, "Camiseta de algodão tamanho M", "N/A", "Roupas", DateTime.Parse("10/12/2999")));
            //Produtos.Add(new Produto("Bolo", 60.89, "Sobor de chocolate, bem recheado", "20/01/2025", "Alimentos", DateTime.Parse("20/01/2025")));
            //Produtos.Add(new Produto("Pão de batata", 3.89, "Feito com batata", DateTime.Now.Date.ToShortDateString(), "Alimentos", DateTime.Now.Date));
            //ProdutoStorage.SalvarProdutos(Produtos);
            //////

        }

        private void AdicionarProduto_Clicked(object sender, EventArgs e)
        {
            string nome = nomeEntry.Text;
            string descricao = descricaoEntry.Text;
            string categoria = categoriaPicker.SelectedItem?.ToString() ?? "Não selecionado";
            DateTime dataValiReal = validadeDatePicker.Date;
            string dataVali = categoria != "Eletrônicos" && categoria != "Roupas" ? dataValiReal.Date.ToString("dd/MM/yyyy") : "N/A";

            if (dataVali == "N/A")
            {
                dataValiReal = DateTime.Parse("10/12/2999");
            }

            if (double.TryParse(precoEntry.Text, out double preco) && !string.IsNullOrWhiteSpace(nome))
            {
                Produtos.Add(new Produto(nome, preco, descricao, dataVali, categoria, dataValiReal, caminhoImagemSelecionada));
                ProdutoStorage.SalvarProdutos(Produtos);

                mensagemLabel.Text = "Produto Cadastrado com Sucesso!";
                nomeEntry.Text = string.Empty;
                precoEntry.Text = string.Empty;
                descricaoEntry.Text = string.Empty;
                validadeDatePicker.Date = DateTime.Now;
                categoriaPicker.SelectedItem = null;

            }
            else
            {
                mensagemLabel.Text = "Preencha os campos corretamente!";
            }
        }

        private async void IrParaLista_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaProdutosPage());
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var resultado = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Selecione uma imagem",
                FileTypes = FilePickerFileType.Images
            }
            );
            if (resultado != null)
            {
                caminhoImagemSelecionada = resultado.FullPath;
                previewImagem.Source =
                ImageSource.FromFile(caminhoImagemSelecionada);
            }
        }
    }
    


    public class Produto
    {
        public string Nome { get; set; }
        public double Preco { get; set; }
        public string Descricao { get; set; }
        public DateTime DataValiReal { get; set; }
        public string DataVali { get; set; }
        public string Categoria { get; set; }
        public string color { get; set; }
        public string? CaminhoImagem { get; set; }
        public string PrecoFormatado => $"R$ {Preco:F2}";
        public Produto(string nome, double preco, string descricao, string dataVali, string categoria, DateTime dataValiReal, string? CaminhoImagem)
        {
            Nome = nome ?? throw new ArgumentNullException(nameof(nome), "O nome não pode ser nulo");
            Preco = preco;
            Descricao = descricao;
            DataVali = dataVali;
            Categoria = categoria;
            DataValiReal = dataValiReal;
        this.CaminhoImagem = CaminhoImagem;

            DateTime dataAt = DateTime.Now.Date;

            if (DateTime.Now.Date < dataValiReal.Date)
            {
                color = "LightGreen";
            }
            else if (DateTime.Now.Date > dataValiReal.Date)
            {
                color = "Red";
            }
            else
            {
                color = "Yellow";
            }
        }

        public Produto() { }
    }
}
