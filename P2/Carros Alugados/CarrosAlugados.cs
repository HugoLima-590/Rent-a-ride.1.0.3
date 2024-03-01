using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using P2.Cadastro_de_Cliente;
using P2.Carros_Alugados;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace P2
{
    public partial class CarrosAlugados : Form, IObserver
    {
        private AluguelSubject aluguelSubject = new AluguelSubject();
        private IClienteRepository clienteRepository;
        private ICarroRepository carroRepository;
        private IAluguelRepository aluguelRepository;

        public CarrosAlugados()
        {
            InitializeComponent();
            carregar_aluguel();

            listViewCarrosAlugados.View = View.Details;
            listViewCarrosAlugados.LabelEdit = false;
            listViewCarrosAlugados.AllowColumnReorder = true;
            listViewCarrosAlugados.FullRowSelect = true;
            listViewCarrosAlugados.GridLines = true;

            aluguelSubject.AdicionarObserver(this);

            listViewCarrosAlugados.Columns.Add("ID", 30, HorizontalAlignment.Left);
            listViewCarrosAlugados.Columns.Add("Carro Alugado", 100, HorizontalAlignment.Left);
            listViewCarrosAlugados.Columns.Add("Nome Pessoa", 100, HorizontalAlignment.Left);
            listViewCarrosAlugados.Columns.Add("CNH", 100, HorizontalAlignment.Left);
            listViewCarrosAlugados.Columns.Add("Total", 100, HorizontalAlignment.Left);
        }

        public void Update(AluguelCarro novoAluguel)
        {
            // Criar um ListViewItem com as informações do novo aluguel
            ListViewItem item = new ListViewItem(novoAluguel.CarroAlugado);
            item.SubItems.Add(novoAluguel.NomePessoa);
            item.SubItems.Add(novoAluguel.CNHPessoa);

            // Adicionar o item à ListView
            listViewCarrosAlugados.Items.Add(item);
        }

        public class AluguelCarro
        {
            public string CarroAlugado { get; set; }
            public string NomePessoa { get; set; }
            public string CNHPessoa { get; set; }
            public string Total { get;  set; }
        }

        public void ReceberAluguel(AluguelCarro aluguel)
        {
            // Criar um ListViewItem com as informações do aluguel
            ListViewItem item = new ListViewItem(aluguel.CarroAlugado);
            item.SubItems.Add(aluguel.NomePessoa);
            item.SubItems.Add(aluguel.CNHPessoa);

            // Adicionar o item à ListView
            listViewCarrosAlugados.Items.Add(item);
            aluguelSubject.NotificarObservers(aluguel);

        }

        public void carregar_aluguel()
        {
            try
            {
                ConexaoBanco conexao = new ConexaoBanco();
                MySqlConnection sqlConnection = conexao.AbrirConexao();

                string sql = "SELECT * FROM alugueis";

                MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);

                MySqlDataReader reader = cmd.ExecuteReader();

                listViewCarrosAlugados.Items.Clear();

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

                    listViewCarrosAlugados.Items.Add(linha_listview);
                }

                conexao.FecharConexao();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void clienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IClienteRepository clienteRepository = new ClienteRepository(new ConexaoBanco());
            Form1 cliente = new Form1(clienteRepository);
            cliente.ShowDialog();

        }

        private void veículoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 veiculo = new Form2();
            veiculo.ShowDialog();
        }

        private void aluguelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Aluguel aluguel = new Aluguel(clienteRepository, carroRepository, aluguelRepository);
            aluguel.ShowDialog();
        }

        private void carrosAlugadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CarrosAlugados aluguel = new CarrosAlugados();
            aluguel.ShowDialog();
        }
    }
}
