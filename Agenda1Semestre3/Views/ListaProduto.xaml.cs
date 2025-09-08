using Agenda1Semestre3.Models;
using System.Collections.ObjectModel;

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
		//Criando a lista de produtos aguardavel 
		List<Produto> tmp = await App.Db.GetAll();
		// Colhendo as informações da lista generica para uma lista de objetos/ForEach
		tmp.ForEach( i => lista.Add(i));
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
		string q = e.NewTextValue;
	
		//usado para limpar a lista
		lista.Clear();

        //Criando a lista de preucura aguardavel 
        List<Produto> tmp = await App.Db.Search(q);
        // Colhendo as informações da lista generica para uma lista de objetos/ForEach
        tmp.ForEach(i => lista.Add(i));
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
		double soma = lista.Sum(i => i.Total);

		string msg = $"O total é {soma:C}";
		DisplayAlert("Total de produtos", msg, "Ok");
    }

    private void MenuItem_Clicked(object sender, EventArgs e)
    {

    }
}