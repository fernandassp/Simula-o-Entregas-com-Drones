using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorEncomendasDrone
{
    internal class Drone
    {
        private double _capacidade;
        private double _pesoAtual;
        private double _distanciaMaxima;
        private int _cargaBateria; // pensar depois
        private int _id;
        private static int _proxID = 1;
        private string _localOrigem;
        private string _localizacao;
        private LinkedList<Pedido> _pedidosALevar;
        private int _quantPedidosLevados;
        public Drone(double capacidade, double distanciaMaxima)
        {
            if (capacidade <= 0)
                throw new ArgumentException("A capacidade do drone deve ser maior que 0 kg.");
            if (distanciaMaxima <= 0)
                throw new ArgumentException("A distância máxima que o drone alcança deve ser maior que 0 km.");
            _capacidade = capacidade;
            _pesoAtual = 0;
            _distanciaMaxima = distanciaMaxima;
            _cargaBateria = 100;
            _id = _proxID;
            _proxID++;
            _quantPedidosLevados = 0;
            _pedidosALevar = new LinkedList<Pedido>();
        }
        public Drone(double capacidade, double distanciaMaxima, string localOrigem)
        {
            if (capacidade <= 0)
                throw new ArgumentException("A capacidade do drone deve ser maior que 0 kg.");
            if (distanciaMaxima <= 0)
                throw new ArgumentException("A distância máxima que o drone alcança deve ser maior que 0 km.");
            bool coordValida = Simulador.VerificarCoordenadaVálida(localOrigem);
            if (!coordValida)
                throw new ArgumentException("A coordenada é inválida.");

            _capacidade = capacidade; _pesoAtual= 0;
            _distanciaMaxima = distanciaMaxima;
            _cargaBateria = 100;
            _id = _proxID;
            _proxID++;
            _localizacao = _localOrigem = localOrigem;
            _quantPedidosLevados= 0;
            _pedidosALevar = new LinkedList<Pedido>();
        }

        private bool PodeViajarAte(string coordenada)
        {
            double distancia = Simulador.CalcularDistanciaEntre(coordenada, _localOrigem);
            if (distancia <= _distanciaMaxima)
                return true;
            return false;
        }

        public bool PodeReceberPedido(Pedido p)
        {
            return (!p.FoiEntregue()) && PodeViajarAte(p.GetLocalizacao()) && CapacidadeRestante() >= p.GetPeso();
        }

        public double CapacidadeRestante()
        {
            return _capacidade - _pesoAtual;
        }
        

        /// <summary>
        /// Aloca um pedido a um drone, alterando seu peso atual e adicionando-o à lista de pedidos do drone.
        /// </summary>
        /// <param name="p">Pedido a ser alocado</param>
        /// <returns>Retorna -1 se a operação não foi conclída (peso máximo excedido).</returns>
        public int ReceberPedido(Pedido p)
        {
            if (_pesoAtual + p.GetPeso() <= _capacidade)
            {
                _pesoAtual += p.GetPeso();
                _pedidosALevar.AddLast(p);
                return 1;
            }
            return -1;
        }
        public void Viajar()
        {
            if (_pedidosALevar == null)
                throw new Exception("Não há nenhum pedido para ser levado por este drone.");

            while (_pedidosALevar.Count > 0)
            {
                var pedido = _pedidosALevar.First();
                _localizacao = pedido.GetLocalizacao();
                _pesoAtual -= pedido.GetPeso();
                _pedidosALevar.RemoveFirst();
                _quantPedidosLevados++;
            }
            _localizacao = _localOrigem;

            Console.WriteLine($"\t**Drone #{GetID()} viajou!**");
        }

        public override string ToString()
        {
            return $"Drone de ID {_id}\n\tCapacidade: {_capacidade}kg. Distancia max.: {_distanciaMaxima}km. Localização: {_localizacao}\n";
        }
        public string GetLocalOrigem()
        {
            return _localOrigem;
        }
        public double GetAlcance()
        {
            return _distanciaMaxima;
        }

        public int GetID()
        {
            return _id;
        }
        public double GetCapacidade()
        {
            return _capacidade;
        }
        public int QuantosPedidosALevar()
        {
            return _pedidosALevar.Count;
        }
       
        // O sistema precisa alocar os pacotes nos drones com o menor número de viagens possível
        // Buscar combinações de pacotes por viagem que maximizem o uso do drone (capacidade + alcance)

        // saber quais pedidos tem a menor distancia em relação ao drone (tbm olhar prioridade e peso) +pri, -dist, -peso
        // escolher pedidos que serão levados pelo drone; os entregues, remover da lista 
        // escolher qual drone levará os pedidos?

        // tempo total de entrega
        // fila de entrega por prioridade + tempo de chegada
        // Relatórios, dashboard
    }
}
