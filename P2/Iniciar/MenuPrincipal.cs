using P2.Cadastro_de_Cliente;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P2
{
    public partial class MenuPrincipal : Form
    {
        private IClienteRepository clienteRepository;
        private ICarroRepository carroRepository;
        private IAluguelRepository aluguelRepository;

        public MenuPrincipal()
        {
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            InitializeComponent();
        }

        private void clienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IClienteRepository clienteRepository = new ClienteRepository(new ConexaoBanco());
            Form1 cliente = new Form1(clienteRepository);
            cliente.ShowDialog();

        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void veículoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 veiculo = new Form2();
            veiculo.ShowDialog();
        }
        private void MenuPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        private void botaoCliente_Click(object sender, EventArgs e)
        {
            IClienteRepository clienteRepository = new ClienteRepository(new ConexaoBanco());
            Form1 cliente = new Form1(clienteRepository);
            cliente.ShowDialog();

        }
        private void button2_Click(object sender, EventArgs e)
        {
            Form2 cliente = new Form2();
            cliente.ShowDialog();
        }
        private void btnAlugel_Click(object sender, EventArgs e)
        {
            Aluguel aluguel = new Aluguel(clienteRepository, carroRepository, aluguelRepository);
            aluguel.ShowDialog();
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
        private void btnCarroAlugado_Click(object sender, EventArgs e)
        {
            CarrosAlugados aluguel = new CarrosAlugados();
            aluguel.ShowDialog();
        }
    }
}
