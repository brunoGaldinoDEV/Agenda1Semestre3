using Agenda1Semestre3.Helpers;
namespace Agenda1Semestre3
{
    public partial class App : Application
    {
        //Só pode ser acessesado dentro da classe e cria a conexão apenas uma vez com o Banco de Dados
        static SQLiteDatabaseHelper _db;

        //Publico para possibilita o acesso ao banco de dados uma única vez
        public static SQLiteDatabaseHelper Db
        {

            //Sempre que Db é chamado esse bloco é executado
            get 
            {
                //verefica se a instância ainda não foi criada
                if(_db == null)
                {
                    //cria uma string para receber o caminho do banco de dados
                    // usa o Path.Combine para achar o caminho independente do SO
                    string path = Path.Combine
                        (
                            Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData),
                            // Nomeia o banco de dados
                            "banco_sqlite_compras.db3"
                        );
                    // Cria a instância do SQLiteDatabaseHelper com o caminho definido
                    _db = new SQLiteDatabaseHelper(path);
                }
                // Retorna a instância do banco de dados
                return _db; 
            }
        }
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Views.ListaProduto());
        }
    }
}
