using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static P2.CarrosAlugados;

namespace P2.Carros_Alugados
{
    public class AluguelSubject
    {
        private List<IObserver> observers = new List<IObserver>();

        public void AdicionarObserver(IObserver observer)
        {
            observers.Add(observer);
        }

        public void NotificarObservers(AluguelCarro novoAluguel)
        {
            foreach (var observer in observers)
            {
                observer.Update(novoAluguel);
            }
        }
    }

}
