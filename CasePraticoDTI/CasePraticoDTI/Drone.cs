using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorEncomendasDrone
{
    public class Drone
    {
        private double _capacidade;
        private double _pesoAtual;
        private double _distanciaMaxima;
        private int _cargaBateria;
        private int _id;
        private static int _proxID = 1;
        private string _localOrigem;
        private string _localizacao;
        private LinkedList<Pedido> _pedidosALevar;
        private int _quantPedidosLevados;
        private double _velocidadeMedia;
        private double _tempoTotalGasto;
        
        public Drone(double capacidade, double distanciaMaxima, string localOrigem, double velocidadeMedia)
        {
            if (capacidade <= 0)
                throw new ArgumentException("A capacidade do drone deve ser maior que 0 kg.");
            if (distanciaMaxima <= 0)
                throw new ArgumentException("A distância máxima que o drone alcança deve ser maior que 0 km.");
            bool coordValida = Simulador.VerificarCoordenadaVálida(localOrigem);
            if (!coordValida)
                throw new ArgumentException("A coordenada é inválida.");
            if(velocidadeMedia <= 0)
                throw new ArgumentException("A velocidade média do drone deve ser maior que 0 km/h.");

            _capacidade = capacidade; _pesoAtual= 0;
            _distanciaMaxima = distanciaMaxima;
            _cargaBateria = 100;
            _id = _proxID;
            _proxID++;
            _localizacao = _localOrigem = localOrigem;
            _quantPedidosLevados= 0;
            _pedidosALevar = new LinkedList<Pedido>();
            _velocidadeMedia = velocidadeMedia;
            _tempoTotalGasto = 0;
        }

        /// <summary>
        /// Verifica se a distância entre o ponto de origem do drone e determinada localização pode ser percorrida, com base no alcance do drone.
        /// </summary>
        /// <param name="coordenada">Localização que se deseja calcular a distância em relação à origem do drone</param>
        /// <returns>Verdadeiro caso a distância seja adequada e falso caso contrário.</returns>
        public bool PodeViajarAte(string coordenada)
        {
            double distancia = Simulador.CalcularDistanciaEntre(coordenada, _localOrigem);
            if (distancia <= _distanciaMaxima)
                return true;
            return false;
        }

        /// <summary>
        /// Verifica se o drone pode receber um pedido para entregar, com base em sua capacidade, distância do pedido e carga da bateria.
        /// </summary>
        /// <param name="p">Pedido a ser recebido</param>
        /// <returns>Verdadeiro se o drone estiver apto a receber o pedido e falso caso não esteja.</returns>
        public bool PodeReceberPedido(Pedido p)
        {
            return (!p.FoiEntregue()) && PodeViajarAte(p.GetLocalizacao()) && CapacidadeRestante() >= p.GetPeso() && _cargaBateria > 30;
        }

        public double CapacidadeRestante()
        {
            return _capacidade - _pesoAtual;
        }
        

        /// <summary>
        /// Aloca um pedido a um drone, alterando seu peso atual e adicionando-o à lista de pedidos do drone.
        /// </summary>
        /// <param name="p">Pedido a ser alocado</param>
        /// <returns>Retorna -1 se a operação não foi concluída (drone não cumpria os requisitos necessários para levar o pedido).</returns>
        public int ReceberPedido(Pedido p)
        {
            if (PodeReceberPedido(p))
            {
                _pesoAtual += p.GetPeso();
                _pedidosALevar.AddLast(p);
                return 1;
            }
            return -1;
        }
        /// <summary>
        /// Simula a entrega dos pedidos, alterando a localização do drone, calculando a distância percorrida, a redução da bateria, a quantidade de pedidos levados e o tempo gasto, com base em sua velocidade média.
        /// </summary>
        /// <exception cref="InvalidOperationException">Lançada quando não há pedidos para o drone realizar uma viagem.</exception>
        public void Viajar()
        {
            if (_pedidosALevar == null)
                throw new InvalidOperationException("Não há nenhum pedido para ser levado por este drone.");
            string localAnterior = _localOrigem;
            while (_pedidosALevar.Count > 0)
            {
                Pedido pedido = _pedidosALevar.First();
                _tempoTotalGasto += Simulador.CalcularDistanciaEntre(pedido.GetLocalizacao(), _localizacao) / _velocidadeMedia;
                _localizacao = pedido.GetLocalizacao();
                _pesoAtual -= pedido.GetPeso();
                _pedidosALevar.RemoveFirst();
                _quantPedidosLevados++;
                if (Simulador.CalcularDistanciaEntre(localAnterior, _localizacao) >= 5)
                    _cargaBateria -= 10;
                
                if (_cargaBateria <= 30)
                    Recarregar();
                localAnterior = _localizacao;

            }
            _localizacao = _localOrigem;
            
            Console.WriteLine($"\t**Drone #{GetID()} viajou!** - bateria: {_cargaBateria}%");
            Recarregar();
        }

        /// <summary>
        /// Recarrega a bateria do drone novamente para 100% após uma viagem
        /// </summary>
        public void Recarregar()
        {
            _localizacao = _localOrigem;
            _cargaBateria = 100;
        }
        public override string ToString()
        {
            return $"Drone de ID {_id}\n\tCapacidade: {_capacidade}kg. Distancia max.: {_distanciaMaxima}km. Localização: {_localizacao}. Vel. média: {_velocidadeMedia}km/h.\n";
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

        /// <summary>
        /// Retorna quantidade de pedidos a serem levados pelo drone.
        /// </summary>
        /// <returns>Quantidade de itens na lista que contém os pedidos pendentes para entrega do drone</returns>
        public int QuantosPedidosALevar()
        {
            return _pedidosALevar.Count;
        }

        /// <summary>
        /// Retorna quantos pedidos um drone entregou.
        /// </summary>
        /// <returns>Quantidade de pedidos entregues.</returns>
        public int PedidosEntregues()
        {
            return _quantPedidosLevados;
        }
       
        /// <summary>
        /// Retorna o valor referente a quanto tempo o drone gastou realizando todas as suas entregas.
        /// </summary>
        /// <returns>Quanto tempo (em horas) foi gasto.</returns>
        public double TempoTotalGasto()
        {
            return _tempoTotalGasto;
        }
       
    }
}
