using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P2
{
    public class CarroRepository : ICarroRepository
    {
        private readonly ConexaoBanco conexaoBanco;

        public CarroRepository(ConexaoBanco conexaoBanco)
        {
            this.conexaoBanco = conexaoBanco;
        }

        public List<Carro> ObterTodos()
        {
            List<Carro> carros = new List<Carro>();

            try
            {
                MySqlConnection sqlConnection = conexaoBanco.AbrirConexao();
                string sql = "SELECT * FROM carros";
                MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Carro carro = new Carro
                        {
                            Id = reader.GetInt32(0),
                            Marca = reader.GetString(1),
                            Modelo = reader.GetString(2),
                            Ano = reader.GetInt32(3),
                            NumeroPortas = reader.GetInt32(4),
                            PrecoPorDia = reader.GetDecimal(5)
                        };
                        carros.Add(carro);
                    }
                }

                conexaoBanco.FecharConexao();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return carros;
        }

        public Carro ObterPorId(int id)
        {
            Carro carro = null;

            try
            {
                MySqlConnection sqlConnection = conexaoBanco.AbrirConexao();
                string sql = "SELECT * FROM carros WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        carro = new Carro
                        {
                            Id = reader.GetInt32(0),
                            Marca = reader.GetString(1),
                            Modelo = reader.GetString(2),
                            Ano = reader.GetInt32(3),
                            NumeroPortas = reader.GetInt32(4),
                            PrecoPorDia = reader.GetDecimal(5)
                        };
                    }
                }

                conexaoBanco.FecharConexao();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return carro;
        }

        public void Inserir(Carro carro)
        {
            try
            {
                ConexaoBanco conexao = new ConexaoBanco();
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conexao.AbrirConexao();

                    // Iniciar o insert no Banco
                    cmd.CommandText = "INSERT INTO carros (marca, modelo, ano, numero_portas, preco_por_dia) " +
                                     "VALUES " +
                                     "(@marca, @modelo, @ano, @numero_portas, @preco_por_dia)";

                    cmd.Parameters.Clear();

                    cmd.Parameters.AddWithValue("@marca", carro.Marca);
                    cmd.Parameters.AddWithValue("@modelo", carro.Modelo);
                    cmd.Parameters.AddWithValue("@ano", carro.Ano);
                    cmd.Parameters.AddWithValue("@numero_portas", carro.NumeroPortas);
                    cmd.Parameters.AddWithValue("@preco_por_dia", carro.PrecoPorDia);

                    cmd.ExecuteNonQuery(); // Retorna o número de linhas afetadas
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ocorreu: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void Atualizar(Carro carro)
        {
            try
            {
                ConexaoBanco conexao = new ConexaoBanco();
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conexao.AbrirConexao();

                    // Inicia o update no Banco
                    cmd.CommandText = "UPDATE carros SET marca=@marca, modelo=@modelo, ano=@ano, numero_portas=@numero_portas, preco_por_dia=@preco_por_dia "
                                    + "WHERE id=@id";

                    cmd.Parameters.Clear();

                    // Use as propriedades do objeto Carro para definir os parâmetros
                    cmd.Parameters.AddWithValue("@marca", carro.Marca);
                    cmd.Parameters.AddWithValue("@modelo", carro.Modelo);
                    cmd.Parameters.AddWithValue("@ano", carro.Ano);
                    cmd.Parameters.AddWithValue("@numero_portas", carro.NumeroPortas);
                    cmd.Parameters.AddWithValue("@preco_por_dia", carro.PrecoPorDia);
                    cmd.Parameters.AddWithValue("@id", carro.Id); // Use a propriedade Id do objeto Carro

                    cmd.ExecuteNonQuery(); // Retorna o número de linhas afetadas
                }
                conexao.FecharConexao();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Excluir(int id)
        {
            try
            {
                DialogResult conf = MessageBox.Show("Deseja excluir este registro?", "Ops, tem certeza?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (conf == DialogResult.Yes)
                {
                    ConexaoBanco conexao = new ConexaoBanco();
                    MySqlConnection sqlConnection = conexao.AbrirConexao();

                    string sql = "DELETE FROM carros WHERE id=@id";

                    MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
                    cmd.Parameters.AddWithValue("@id", id); // Usar o parâmetro 'id' em vez de 'idCarroSelecionado'

                    int rowsAffected = cmd.ExecuteNonQuery(); //retorna o número de linhas afetadas

                    conexao.FecharConexao();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }

}
