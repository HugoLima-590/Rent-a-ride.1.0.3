using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static P2.CarrosAlugados;

namespace P2.Carros_Alugados
{
    public interface IObserver
    {
        void Update(AluguelCarro novoAluguel);
    }
}
