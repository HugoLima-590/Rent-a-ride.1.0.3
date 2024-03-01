using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2.Cadastro_de_Cliente
{
    public interface IClienteRepository
    {
        List<Cliente> ObterTodos();
        Cliente ObterPorId(int id);
        void Inserir(Cliente cliente);
        void Atualizar(Cliente cliente);
        void Excluir(int id);
    }

}
