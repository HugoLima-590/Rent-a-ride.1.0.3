using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using static P2.CarrosAlugados;

namespace P2.Cadastro_de_Cliente
{
    public class AluguelRepository : IAluguelRepository
    {
        private readonly ConexaoBanco conexaoBanco;

        public AluguelRepository(ConexaoBanco conexaoBanco)
        {
            this.conexaoBanco = conexaoBanco;
        }

        public void Inserir(AluguelCarro aluguel)
        {
            try
            {
                MySqlConnection sqlConnection = conexaoBanco.AbrirConexao();

                string query = "INSERT INTO alugueis (carro, nome, cnh, total) VALUES (@carro, @nome, @cnh, @total)";

                MySqlCommand cmd = new MySqlCommand(query, sqlConnection);
                cmd.Parameters.AddWithValue("@carro", aluguel.CarroAlugado);
                cmd.Parameters.AddWithValue("@nome", aluguel.NomePessoa);
                cmd.Parameters.AddWithValue("@cnh", aluguel.CNHPessoa);
                cmd.Parameters.AddWithValue("@total", aluguel.Total);

                cmd.ExecuteNonQuery();

                conexaoBanco.FecharConexao();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao salvar o aluguel: " + ex.Message);
            }
        }
    }
}
