using Agenda1Semestre3.Models;
using SQLite;

namespace Agenda1Semestre3.Helpers
{
    public class SQLiteDatabaseHelper
    {
        // Faz a conexão uma única vez e deixa conectado
        readonly SQLiteAsyncConnection _conn;
        //Construtor: recebe o caminho do arquivo do banco de dados e inicializa a conexão.
        public SQLiteDatabaseHelper(string path) 
        {
            // cria uma nova conexão com o banco de dados assícrona com o banco de dados
            _conn = new SQLiteAsyncConnection(path);
            // Criando a tabela de produtos o Wait faz aguarda terminar antes de seguir o código.
            _conn.CreateTableAsync<Produto>().Wait();
        }

        // Task<int> mostra quantas linhas foi afetada e também permite o código continuar sendo executado enquanto a operação está sendo executada
        public Task<int> Insert(Produto p)
        {
            //Só acontece quando for chamado através do "await"
            return _conn.InsertAsync(p);
        }
        // Mais uma Task, pega a lista de Produtos e edita uma única linha
        public Task<List<Produto>> Update(Produto p)
        {
            // encapsula o procedimento de UPDATE na variavel sql 
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";

            // chama a conexão e executa o update com as informações passadas pela classe modelo.
            return _conn.QueryAsync<Produto>
                (sql, p.Descricao, p.Quantidade, p.Preco, p.Id );
        }

        //Mais uma Task que retorna o numero de linhas deletadas 
        public Task<int> Delete(int Id)
        { 
            // Usa Lanbda para pegar os itens da tabela (i) escolhe a coluna Id(i.Id) e deleta oque for igual (==) ao Id fornecido
           return _conn.Table<Produto>().DeleteAsync(i => i.Id == Id);
        }
        
        public Task<List<Produto>> GetAll()
        {
            // quando chamado vai retornar a lista de produtos
            return _conn.Table<Produto>().ToListAsync();
        }

        public Task<List<Produto>> Search(string q)
        {
            // encapsula o procedimento de pesquisa na variavel sql aonde usa o % para pesquisar qualquer valor que contenha o conteudo na variavel q 
            string sql = "SELECT * Produto WHERE Descricao LIKE '%"+ q +"%'";

            // chama a conexão e executa a pesquisa com o filtro passado pelo q.
            return _conn.QueryAsync<Produto>(sql);
        }
    }
}
