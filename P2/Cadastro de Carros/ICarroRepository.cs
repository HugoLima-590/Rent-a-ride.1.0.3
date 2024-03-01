using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2
{
    public interface ICarroRepository
    {
        List<Carro> ObterTodos();
        Carro ObterPorId(int id);
        void Inserir(Carro carro);
        void Atualizar(Carro carro);
        void Excluir(int id);
    }
}
