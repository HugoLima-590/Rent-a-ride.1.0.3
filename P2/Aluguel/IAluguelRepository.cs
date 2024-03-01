using System;
using System.Collections.Generic;
using static P2.CarrosAlugados;

namespace P2.Cadastro_de_Cliente
{
    public interface IAluguelRepository
    {
        void Inserir(AluguelCarro aluguel);
    }
}
