using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P2
{
    public class ConexaoBanco
    {
        private MySqlConnection conexao;
        private string data_source = "datasource=localhost;username=root;password=;database=test"; //Colocar aqui o endereço do banco de dados

        public ConexaoBanco()
        {
            conexao = new MySqlConnection(data_source);
        }

        public MySqlConnection AbrirConexao()
        {
            if (conexao.State == System.Data.ConnectionState.Closed)
            {
                conexao.Open();
            }
            return conexao;
        }

        public void FecharConexao()
        {
            if (conexao.State == System.Data.ConnectionState.Open)
            {
                conexao.Close();
            }
        }

        public int ExecuteQuery(string query, MySqlParameter[] parameters)
        {
            try
            {
                if (conexao.State == System.Data.ConnectionState.Closed)
                {
                    conexao.Open();
                }

                MySqlCommand cmd = new MySqlCommand(query, conexao);

                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ocorreu: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1; // Retorna um valor indicando erro
            }
            finally
            {
                if (conexao.State == System.Data.ConnectionState.Open)
                {
                    conexao.Close();
                }
            }
        }
    }
}
