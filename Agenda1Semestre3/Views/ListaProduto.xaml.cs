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
	// Sempre é chamado quando uma tela aparece e está em outro processo
    protected async override void OnAppearing()
    {
		try
		{
			//Criando a lista de produtos aguardavel 
			List<Produto> tmp = await App.Db.GetAll();
			// Colhendo as informações da lista generica para uma lista de objetos/ForEach
			tmp.ForEach(i => lista.Add(i));
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

			//usado para limpar a lista
			lista.Clear();

			//Criando a lista de preucura aguardavel 
			List<Produto> tmp = await App.Db.Search(q);
			// Colhendo as informações da lista generica para uma lista de objetos/ForEach
			tmp.ForEach(i => lista.Add(i));
		}
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

    private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
		try
		{
			double soma = lista.Sum(i => i.Total);

			string msg = $"O total é {soma:C}";
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
		bool confirm = await DisplayAlert("Tem Certeza?", $"Remover {p.Descricao}", "Sim", "Não");

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
}