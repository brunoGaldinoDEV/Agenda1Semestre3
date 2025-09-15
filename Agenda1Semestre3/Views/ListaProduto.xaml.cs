using Agenda1Semestre3.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Agenda1Semestre3.Views;

public partial class ListaProduto : ContentPage
{
	// Usado para deixa a lista atualizando automaticamente 
	ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
	public ListaProduto()
	{
		InitializeComponent();

		lst_produtos.ItemsSource = lista;
	}
	// Sempre � chamado quando uma tela aparece e est� em outro processo
    protected async override void OnAppearing()
    {
		try
		{
			lista.Clear();
			//Criando a lista de produtos aguardavel 
			List<Produto> tmp = await App.Db.GetAll();
			// Colhendo as informa��es da lista generica para uma lista de objetos/ForEach
			tmp.ForEach(i => lista.Add(i));
		}
		catch (Exception ex)
		{
            await DisplayAlert("Ops", ex.Message, "Ok");
        }

        try
        {
            lista.Clear();
            List<Produto> tmp = await App.Db.GetAll();
            tmp.ForEach(i => lista.Add(i));

            // Pegando categorias �nicas do banco
            var categorias = tmp.Select(p => p.Categoria).Distinct().ToList();
            categorias.Insert(0, "Todas"); // Op��o para voltar a mostrar tudo

            pickerCategoria.ItemsSource = categorias;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
			Navigation.PushAsync( new Views.NovoProduto());
		}
		catch (Exception ex)
		{
			DisplayAlert("Ops", ex.Message, "Ok");
		}
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
		try
		{
			string q = e.NewTextValue;

			lst_produtos.IsRefreshing = true;
			//usado para limpar a lista
			lista.Clear();

			//Criando a lista de preucura aguardavel 
			List<Produto> tmp = await App.Db.Search(q);
			// Colhendo as informa��es da lista generica para uma lista de objetos/ForEach
			tmp.ForEach(i => lista.Add(i));
		}
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
		finally
		{
			lst_produtos.IsRefreshing = false;
		}
    }

    private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
		try
		{
			double soma = lista.Sum(i => i.Total);

			string msg = $"O total � {soma:C}";
			await DisplayAlert("Total de produtos", msg, "Ok");
		}
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
		MenuItem selecinado = sender as MenuItem;
		Produto p = selecinado.BindingContext as Produto;
		bool confirm = await DisplayAlert("Tem Certeza?", $"Remover {p.Descricao}", "Sim", "N�o");

		if (confirm)
		{
			await App.Db.Delete(p.Id);
			lista.Remove(p);
		}

    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
		try
		{
			Produto p = e.SelectedItem as Produto;

			Navigation.PushAsync(new Views.EditarProdutos
			{
				BindingContext = p,
			});
		}
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        try
        {

            lista.Clear();

			List<Produto> tmp = await App.Db.GetAll();

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
		finally
		{
			lst_produtos.IsRefreshing = false;
		}
    }

    private async void pickerCategoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string categoriaSelecionada = pickerCategoria.SelectedItem.ToString();
            lista.Clear();

            List<Produto> tmp;

            if (categoriaSelecionada == "Todas")
            {
                // Mostra tudo
                tmp = await App.Db.GetAll();
            }
            else
            {
                // Filtra pela categoria
                tmp = await App.Db.GetByCategoria(categoriaSelecionada);
            }

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

    private void ToolbarItem_Clicked_2(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.Relatorio());
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "Ok");
        }
    }
}