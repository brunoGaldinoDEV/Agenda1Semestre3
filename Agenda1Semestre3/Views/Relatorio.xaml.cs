using Agenda1Semestre3.Helpers;
using static Agenda1Semestre3.Helpers.SQLiteDatabaseHelper;

namespace Agenda1Semestre3.Views
{
    public partial class Relatorio : ContentPage
    {
        public Relatorio()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                var relatorio = await App.Db.GetTotalPorCategoria();
                lst_relatorio.ItemsSource = relatorio;

                double soma = relatorio.Sum(i => i.Total);
                lblTotalGeral.Text = $"Total Geral: {soma:C}";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

    }
}
