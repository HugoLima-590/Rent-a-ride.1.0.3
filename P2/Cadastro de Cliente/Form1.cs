using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using P2.Cadastro_de_Cliente;
using static Org.BouncyCastle.Crypto.Digests.SkeinEngine;

namespace P2
{
        /*
         * 
         * 
         * 
         * form 1 = Cadastro de Cliente
         * 
         * 
         * 
         * 
         */
    public partial class Form1 : Form
    {     
        private int ?idClienteSelecionado = null; //quando o '?' fica antes da variavel int ela vira um tipo anulavel
        private IClienteRepository clienteRepository;
        private ICarroRepository carroRepository;
        private IAluguelRepository aluguelRepository;

        public Form1(IClienteRepository clienteRepository)
        {   //janela nao maximiza
            MaximizeBox = false;
            //janela não pode ser redimensionada 
            FormBorderStyle = FormBorderStyle.FixedSingle;

            InitializeComponent();

            this.clienteRepository = clienteRepository;
            list_Busca.View = View.Details;
            list_Busca.LabelEdit = false;
            list_Busca.AllowColumnReorder = true;
            list_Busca.FullRowSelect = true;
            list_Busca.GridLines = true;

            list_Busca.Columns.Add("ID", 30, HorizontalAlignment.Left);
            list_Busca.Columns.Add("Nome", 150, HorizontalAlignment.Left);
            list_Busca.Columns.Add("Telefone", 150, HorizontalAlignment.Left);
            list_Busca.Columns.Add("CPF", 150, HorizontalAlignment.Left);
            list_Busca.Columns.Add("CNH", 150, HorizontalAlignment.Left);

            carregar_clientes();

        }

        private void carregar_clientes()
        {
            List<Cliente> clientes = clienteRepository.ObterTodos();
            list_Busca.Items.Clear();

            foreach (var cliente in clientes)
            {
                string[] row =
                {
                    cliente.Id.ToString(),
                    cliente.Nome,
                    cliente.Telefone,
                    cliente.CPF,
                    cliente.CNH
                };

                var linha_listview = new ListViewItem(row);
                list_Busca.Items.Add(linha_listview);
            }
        }

        private void buttonCadastrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNome.Text) || string.IsNullOrEmpty(txtTelefone.Text) || string.IsNullOrEmpty(txtCPF.Text) || string.IsNullOrEmpty(txtCNH.Text))
            {
                MessageBox.Show("Todos os campos devem ser preenchidos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Cliente novoCliente = new Cliente
            {
                Nome = txtNome.Text,
                Telefone = txtTelefone.Text,
                CPF = txtCPF.Text,
                CNH = txtCNH.Text
            };

            clienteRepository.Inserir(novoCliente);

            MessageBox.Show("Cliente cadastrado com sucesso.", "Sucesso", MessageBoxButtons.OK);
            carregar_clientes();

            txtNome.Clear();
            txtCNH.Clear();
            txtCPF.Clear();
            txtTelefone.Clear();
        }

        private void buttonListaCompleta_Click(object sender, EventArgs e)
        {
            // Verificar se algum campo está vazio
            if (string.IsNullOrEmpty(txtNome.Text) || string.IsNullOrEmpty(txtTelefone.Text) ||
                string.IsNullOrEmpty(txtCPF.Text) || string.IsNullOrEmpty(txtCNH.Text))
            {
                MessageBox.Show("Todos os campos devem ser preenchidos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Encerrar a execução do método
            }

            ConexaoBanco conexao = new ConexaoBanco();
            string query = "INSERT INTO clientes (nome, telefone, cpf, cnh) VALUES (@nome, @telefone, @cpf, @cnh)";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@nome", txtNome.Text),
                new MySqlParameter("@telefone", txtTelefone.Text),
                new MySqlParameter("@cpf", txtCPF.Text),
                new MySqlParameter("@cnh", txtCNH.Text)
            };

            int rowsAffected = conexao.ExecuteQuery(query, parameters);

            if (rowsAffected > 0)
            {
                MessageBox.Show("Cliente cadastrado com sucesso.", "Sucesso", MessageBoxButtons.OK);
                carregar_clientes(); 
            }

            txtNome.Clear();
            txtCNH.Clear();
            txtCPF.Clear();
            txtTelefone.Clear();
        }

        private void botaoFiltrarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                string q = "'%" + txtBuscarCliente.Text + "%'";

                ConexaoBanco conexao = new ConexaoBanco();
                {
                    string sql = "SELECT * FROM clientes " +
                                 "WHERE cpf LIKE " + q + " OR nome LIKE " + q;

                    MySqlCommand comando = new MySqlCommand(sql, conexao.AbrirConexao());

                    MySqlDataReader reader = comando.ExecuteReader();

                    list_Busca.Items.Clear();

                    while (reader.Read())
                    {
                        string[] row =
                        {
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                };

                        var linha_listview = new ListViewItem(row);

                        list_Busca.Items.Add(linha_listview);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idClienteSelecionado.HasValue)
            {
                Cliente cliente = clienteRepository.ObterPorId(idClienteSelecionado.Value);

                if (cliente != null)
                {
                    cliente.Nome = txtNome.Text;
                    cliente.Telefone = txtTelefone.Text;
                    cliente.CPF = txtCPF.Text;
                    cliente.CNH = txtCNH.Text;

                    clienteRepository.Atualizar(cliente);
                    MessageBox.Show("Cliente atualizado com sucesso", "Sucesso", MessageBoxButtons.OK);
                    carregar_clientes();
                }
            }

            txtNome.Clear();
            txtCNH.Clear();
            txtCPF.Clear();
            txtTelefone.Clear();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (idClienteSelecionado.HasValue)
            {
                DialogResult conf = MessageBox.Show("Deseja excluir este registro?", "Ops, tem certeza?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (conf == DialogResult.Yes)
                {
                    clienteRepository.Excluir(idClienteSelecionado.Value);
                    MessageBox.Show("Cliente excluído com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            carregar_clientes();
            txtNome.Clear();
            txtCNH.Clear();
            txtCPF.Clear();
            txtTelefone.Clear();
        }

        private void list_Busca_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListView.SelectedListViewItemCollection itens_selecionados = list_Busca.SelectedItems;

            foreach (ListViewItem item in itens_selecionados)
            {
                idClienteSelecionado = Convert.ToInt32(item.SubItems[0].Text);
                txtNome.Text = item.SubItems[1].Text;
                txtTelefone.Text = item.SubItems[2].Text;
                txtCPF.Text = item.SubItems[3].Text;
                txtCNH.Text = item.SubItems[4].Text;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            clienteRepository.Excluir(idClienteSelecionado.Value);
        }

        private void veículoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 veiculo = new Form2();
            veiculo.ShowDialog();
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aluguelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Aluguel aluguel = new Aluguel(clienteRepository, carroRepository, aluguelRepository);
            aluguel.ShowDialog();
        }

        private void carrosAlugadosToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            CarrosAlugados aluguel = new CarrosAlugados();
            aluguel.ShowDialog();
        }

        private void btnCarroAlugado_Click(object sender, EventArgs e)
        {
            CarrosAlugados aluguel = new CarrosAlugados();
            aluguel.ShowDialog();
        }


        private void btnNovo_Click(object sender, EventArgs e)
        {
            txtNome.Clear();
            txtCNH.Clear();
            txtCPF.Clear();
            txtTelefone.Clear();
        }

        private void aluguelToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Aluguel aluguel = new Aluguel(clienteRepository, carroRepository, aluguelRepository);
            aluguel.ShowDialog();
        }
    }

    }



