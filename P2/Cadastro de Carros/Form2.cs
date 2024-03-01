using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using P2.Cadastro_de_Cliente;

namespace P2
{
    public partial class Form2 : Form
    {
        /*
         * 
         * 
         * 
         * form 2 = Cadastro de Veículos
         * 
         * 
         * 
         * 
         */
        private int? idCarroSelecionado = null; //quando o '?' fica antes da variavel int ela vira um tipo anulavel
        private IClienteRepository clienteRepository;
        private ICarroRepository carroRepository;
        private IAluguelRepository aluguelRepository;


        public Form2()
        {

            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            InitializeComponent();
            this.carroRepository = new CarroRepository(new ConexaoBanco());
            carregar_carros();


            listCarrosCadastrados.View = View.Details;
            listCarrosCadastrados.LabelEdit = false;
            listCarrosCadastrados.AllowColumnReorder = true;
            listCarrosCadastrados.FullRowSelect = true;
            listCarrosCadastrados.GridLines = true;

            listCarrosCadastrados.Columns.Add("ID", 30, HorizontalAlignment.Left);
            listCarrosCadastrados.Columns.Add("Marca", 150, HorizontalAlignment.Left);
            listCarrosCadastrados.Columns.Add("Modelo", 150, HorizontalAlignment.Left);
            listCarrosCadastrados.Columns.Add("Ano", 150, HorizontalAlignment.Left);
            listCarrosCadastrados.Columns.Add("N° Portas", 150, HorizontalAlignment.Left);
            listCarrosCadastrados.Columns.Add("Preço/Dia", 150, HorizontalAlignment.Left);
        }

        private void buttonCadastrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMarca.Text) || string.IsNullOrEmpty(txtModelo.Text) ||
                string.IsNullOrEmpty(txtAno.Text) || string.IsNullOrEmpty(txtNDP.Text) || string.IsNullOrEmpty(txtPPD.Text))
            {
                MessageBox.Show("Todos os campos devem ser preenchidos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Carro novoCarro = new Carro
                {
                    Marca = txtMarca.Text,
                    Modelo = txtModelo.Text,
                    Ano = Convert.ToInt32(txtAno.Text),
                    NumeroPortas = Convert.ToInt32(txtNDP.Text),
                    PrecoPorDia = Convert.ToDecimal(txtPPD.Text)
                };

                carroRepository.Inserir(novoCarro);

                carregar_carros();
                reset_carros();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ocorreu: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void carregar_carros()
        {
            try
            {
                ConexaoBanco conexao = new ConexaoBanco();
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conexao.AbrirConexao();

                    //Inicia a consulta no Banco
                    cmd.CommandText = "SELECT * FROM carros ORDER BY id DESC";

                    cmd.Parameters.Clear();

                    MySqlDataReader reader = cmd.ExecuteReader();

                    listCarrosCadastrados.Items.Clear();

                    while (reader.Read())
                    {
                        string[] row =
                        {
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                };

                        var linha_listview = new ListViewItem(row);

                        listCarrosCadastrados.Items.Add(linha_listview);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void AtualizarCarro_Click(object sender, EventArgs e)
        {
            try
            {
                if (idCarroSelecionado.HasValue)
                {
                    Carro carroAtualizado = new Carro
                    {
                        Id = idCarroSelecionado.Value,
                        Marca = txtMarca.Text,
                        Modelo = txtModelo.Text,
                        Ano = Convert.ToInt32(txtAno.Text),
                        NumeroPortas = Convert.ToInt32(txtNDP.Text),
                        PrecoPorDia = Convert.ToDecimal(txtPPD.Text)
                    };

                    carroRepository.Atualizar(carroAtualizado);

                    MessageBox.Show("Carro Atualizado com sucesso", "Sucesso", MessageBoxButtons.OK);
                    carregar_carros();
                }
                else
                {
                    MessageBox.Show("Selecione um carro para atualizar.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnBuscarCarro_Click(object sender, EventArgs e)
        {
            try
            {
                string q = "%" + txtBuscarCarro.Text + "%";

                ConexaoBanco conexao = new ConexaoBanco();
                MySqlConnection sqlConnection = conexao.AbrirConexao();

                string sql = "SELECT * " +
                             "FROM carros " +
                             "WHERE marca LIKE @q OR modelo LIKE @q";

                MySqlCommand comando = new MySqlCommand(sql, sqlConnection);
                comando.Parameters.AddWithValue("@q", q);

                MySqlDataReader reader = comando.ExecuteReader();

                listCarrosCadastrados.Items.Clear();

                while (reader.Read())
                {
                    string[] row =
                    {
                reader.GetString(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetString(4),
                reader.GetString(5),
            };

                    var linha_listview = new ListViewItem(row);

                    listCarrosCadastrados.Items.Add(linha_listview);
                }

                conexao.FecharConexao();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                if (idCarroSelecionado.HasValue)
                {
                    carroRepository.Excluir(idCarroSelecionado.Value);

                    MessageBox.Show("Carro excluído com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    carregar_carros();
                }
                else
                {
                    MessageBox.Show("Selecione um carro para excluir.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void listCarrosCadastrados_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection itens_selecionados = listCarrosCadastrados.SelectedItems;

            foreach (ListViewItem item in itens_selecionados)
            {


                idCarroSelecionado = Convert.ToInt32(item.SubItems[0].Text);

                txtMarca.Text = item.SubItems[1].Text;
                txtModelo.Text = item.SubItems[2].Text;
                txtAno.Text = item.SubItems[3].Text;
                txtNDP.Text = item.SubItems[4].Text;
                txtPPD.Text = item.SubItems[5].Text;

            }
        }

        private void reset_carros()
        {
            txtMarca.Clear();
            txtNDP.Clear();
            txtAno.Clear();
            txtModelo.Clear();
            txtPPD.Clear();
        }

        private void veículoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Form2 veiculo = new Form2();
            veiculo.ShowDialog();
        }

        private void clienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IClienteRepository clienteRepository = new ClienteRepository(new ConexaoBanco());
            Form1 cliente = new Form1(clienteRepository);
            cliente.ShowDialog();

        }

        private void sairToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void carrosAlugadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CarrosAlugados aluguel = new CarrosAlugados();
            aluguel.ShowDialog();
        }



        private void aluguelToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Aluguel aluguel = new Aluguel(clienteRepository, carroRepository, aluguelRepository);
            aluguel.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            reset_carros();
        }

       
    }
}
