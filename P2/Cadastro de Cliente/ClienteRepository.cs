using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P2.Cadastro_de_Cliente
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ConexaoBanco conexaoBanco;

        public ClienteRepository(ConexaoBanco conexaoBanco)
        {
            this.conexaoBanco = conexaoBanco;
        }

        public List<Cliente> ObterTodos()
        {
            List<Cliente> clientes = new List<Cliente>();
            try
            {
                ConexaoBanco conexao = new ConexaoBanco();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao.AbrirConexao();

                cmd.CommandText = "SELECT * FROM clientes ORDER BY id DESC";
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Cliente cliente = new Cliente
                    {
                        Id = reader.GetInt32(0),
                        Nome = reader.GetString(1),
                        Telefone = reader.GetString(2),
                        CPF = reader.GetString(3),
                        CNH = reader.GetString(4)
                    };

                    clientes.Add(cliente);
                }

                return clientes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return clientes; // Tratar adequadamente os erros na aplicação
            }
        }

        public Cliente ObterPorId(int id)
        {
            Cliente cliente = null;

            try
            {
                MySqlConnection sqlConnection = conexaoBanco.AbrirConexao();
                string sql = "SELECT * FROM clientes WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cliente = new Cliente
                        {
                            Id = reader.GetInt32(0),
                            Nome = reader.GetString(1),
                            Telefone = reader.GetString(2),
                            CPF = reader.GetString(3),
                            CNH = reader.GetString(4)
                        };
                    }
                }

                conexaoBanco.FecharConexao();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return cliente;
        }

        public void Inserir(Cliente cliente)
        {
            try
            {
                ConexaoBanco conexao = new ConexaoBanco();
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conexao.AbrirConexao();

                    // Inicia o insert no Banco
                    cmd.CommandText = "INSERT INTO clientes (nome, telefone, cpf, cnh) " +
                                     "VALUES " +
                                     "(@nome, @telefone, @cpf, @cnh)";

                    cmd.Parameters.Clear();

                    cmd.Parameters.AddWithValue("@nome", cliente.Nome);
                    cmd.Parameters.AddWithValue("@telefone", cliente.Telefone);
                    cmd.Parameters.AddWithValue("@cpf", cliente.CPF);
                    cmd.Parameters.AddWithValue("@cnh", cliente.CNH);

                    cmd.ExecuteNonQuery(); // Retorna o número de linhas afetadas
                }
                conexao.FecharConexao();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ocorreu: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Atualizar(Cliente cliente)
        {
            try
            {
                ConexaoBanco conexao = new ConexaoBanco();
                {
                    MySqlCommand cmd = new MySqlCommand();

                    cmd.Connection = conexao.AbrirConexao();

                    // Inicia o update no Banco
                    cmd.CommandText = "UPDATE clientes SET nome=@nome, telefone=@telefone, cpf=@cpf, cnh=@cnh WHERE id=@id";

                    cmd.Parameters.AddWithValue("@nome", cliente.Nome);
                    cmd.Parameters.AddWithValue("@telefone", cliente.Telefone);
                    cmd.Parameters.AddWithValue("@cpf", cliente.CPF);
                    cmd.Parameters.AddWithValue("@cnh", cliente.CNH);
                    cmd.Parameters.AddWithValue("@id", cliente.Id);

                    cmd.ExecuteNonQuery(); // Retorna o número de linhas afetadas

                    MessageBox.Show("Cliente Atualizado com sucesso", "Sucesso", MessageBoxButtons.OK);
                }
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
                ConexaoBanco conexao = new ConexaoBanco();
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conexao.AbrirConexao();

                    cmd.CommandText = "DELETE FROM clientes WHERE id=@id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery(); // Retorna o número de linhas afetadas
                }  
            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
