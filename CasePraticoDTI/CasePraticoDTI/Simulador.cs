using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorEncomendasDrone
{
    internal class Simulador
    {
        private LinkedList<Pedido> _pedidos;
        private LinkedList<Drone> _drones; 
        // coordenadas: de 1 a 9 e A a Z

        public Simulador()
        {
            _pedidos = new LinkedList<Pedido>();
            _drones = new LinkedList<Drone>();
        }
     
        public void AdicionarPedido(Pedido pedido) 
        {
            if (pedido == null)
                throw new ArgumentNullException("O pedido não pode ser nulo.");
            _pedidos.AddLast(pedido);
        }

        public void AdicionarDrone(Drone drone)
        {
            if (drone == null)
                throw new ArgumentNullException("O drone não pode ser nulo.");
            _drones.AddLast(drone);
        }

        /// <summary>
        /// Calcula a menor distância possível entre duas coordenadas utilizando fórmula da aplicação do teorema de Pitágoras.
        /// </summary>
        /// <param name="p1">Primeiro ponto/coordenada</param>
        /// <param name="p2">Segundo ponto/coordenada</param>
        /// <returns>Retorna a menor distância entre os dois pontos.</returns>
        public static double CalcularDistanciaEntre(string p1, string p2)
        {
            // ver possibilidade de fazer com coordenadas que envolvem >=10 (10B, 12C...)

            bool p1Valido = VerificarCoordenadaVálida(p1); bool p2Valido = VerificarCoordenadaVálida(p2);
            if (!p1Valido || !p2Valido)
                throw new ArgumentException("A coordenada fornecida é inválida.");

            p1 = p1.ToUpper(); p2 = p2.ToUpper();
            // converter string p1 (ex.: 2A) em duas variaveis x1 e y1; p2 em x2 e y2
            int x1 = (int)char.GetNumericValue(p1[0]); int x2 = (int)char.GetNumericValue(p2[0]);
            int y1 = (char)p1[1] - 'A' + 1; int y2 = (char)p2[1] - 'A' + 1;

            double distancia = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
            return distancia;
        }

        /// <summary>
        /// Verifica se a coordenada (X, Y) passada como parâmetro é válida: seu X deve ser um inteiro de 1 a 9 e seu Y, uma letra de A a Z.
        /// </summary>
        /// <param name="localizacao"></param>
        /// <returns>Retorna true se a coordenada for válida e false caso contrário.</returns>
        public static bool VerificarCoordenadaVálida(string localizacao)
        {
            if (localizacao == null)
                return false;
            localizacao = localizacao.ToUpper();
            int xCoordenada = (int)char.GetNumericValue(localizacao[0]);
            int yCoordenada = (char)localizacao[1] - 'A' + 1;
            if (xCoordenada == -1 || yCoordenada < 1 || yCoordenada >= 27) // x: garantir que seja um número de 1 a 9; y: letra de A a Z (código ASCII)
                return false;
            return true;
        }

        public void AlocarPedidosNoDrone()
        {
            string origemDrones = _drones.First().GetLocalOrigem();
            IEnumerable<Drone> dronesOrdenados = _drones.OrderByDescending(d => d.GetCapacidade()).ThenBy(d => d.GetAlcance());

            // ordenar pedidos por prioridade (alta - media - baixa), peso (maior - menor) e distância (menor - maior)
            List<Pedido> pedidosOrdenados = _pedidos.OrderByDescending(p => p.GetPrioridade())
                .ThenByDescending(p => p.GetPeso()).ThenBy(p => CalcularDistanciaEntre(p.GetLocalizacao(), origemDrones)).ToList();   
             
                foreach (Drone d in dronesOrdenados)
                {
                    IEnumerable<Pedido> pedidosParaODrone = pedidosOrdenados.Where(p => d.PodeReceberPedido(p));
                    List<Pedido> pedidosRecebidos = new List<Pedido>();

                     foreach (Pedido p in pedidosParaODrone)
                     {
                        int recebeu = d.ReceberPedido(p);
                        if (recebeu == 1)
                        {
                            pedidosRecebidos.Add(p);
                            p.MarcarComoEntregue();
                            Console.WriteLine($"\t-Drone #{d.GetID()} recebeu o pedido #{p.GetID()}. Sua capacidade restante é de {d.CapacidadeRestante()}kg.");
                        }
                     }

                    foreach (Pedido p in pedidosRecebidos)
                        pedidosOrdenados.Remove(p);
                    
                    if (d.QuantosPedidosALevar() > 0)
                        d.Viajar();
                }
        }

        private List<Pedido> OrdenarPedidos(Comparer<Pedido> comp, IEnumerable<Pedido> pedidos)
        {
            List<Pedido> pedidosEmLista = pedidos.ToList();
            pedidosEmLista.Sort(comp);
            return pedidosEmLista;
        }
      
        public string RelatorioPedidos()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\t=== RELATÓRIO - PEDIDOS ===\n");
            foreach (Pedido p in _pedidos)
            {
                sb.AppendLine(p.ToString());
            }
            return sb.ToString();
        }

        public string RelatorioOrdenadoPedidos(Comparer<Pedido> comp)
        {
            IEnumerable<Pedido> pedidosOrdenados = OrdenarPedidos(comp, _pedidos);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\t=== RELATÓRIO ORDENADO - PEDIDOS ===\n");
            foreach(Pedido p in pedidosOrdenados)
                { sb.AppendLine(p.ToString()); }
            return sb.ToString();
        }

        public string RelatorioDrone()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\t=== RELATÓRIO - DRONES ===");
            foreach(Drone d in _drones)
                sb.AppendLine(d.ToString());
            sb.AppendLine();
            return sb.ToString();
        }

        public string QuantidadeEntregasFeitas()
        {
            StringBuilder sb = new StringBuilder();
            int quantidade = 0;
            foreach (Drone d in _drones)
                quantidade += d.PedidosEntregues();
            sb.AppendLine($"\n -Entregas realizadas: {quantidade}.\n");
            return sb.ToString();
        }
        public string DroneMaisEficiente()
        {
            StringBuilder sb = new StringBuilder();
            Drone melhor = _drones.First();
            foreach(Drone d in _drones)
            {
                if (d.PedidosEntregues() > melhor.PedidosEntregues())
                    melhor = d;
            }
            sb.AppendLine($"\n -Drone mais eficiente: #{melhor.GetID()} - Realizou {melhor.PedidosEntregues()} entregas.\n");
            return sb.ToString();
        }

        public string MapaCidade() //??
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("    A   B   C   D   E   F   G   H   I   J   K   L   M   N   O   P   Q   R   S   T   U   V   W   X   Y   Z");
            
            for (int i = 0; i < 9; i++)
            {

                sb.AppendLine($"{i+1}\n");
            }

            return sb.ToString();
        }

        public string TempoMedioPorEntrega() // conversão pra minutos
        {
            StringBuilder sb = new StringBuilder();
            double somaTempos = 0;
            double entregasFeitas = 0;
            foreach (Drone d in _drones)
            { 
                if(d.PedidosEntregues() > 0)
                {
                    somaTempos += d.TempoTotalGasto();
                    entregasFeitas += d.PedidosEntregues();
                }
            }
            
            double media = somaTempos / (double)entregasFeitas;
            sb.AppendLine($"\n -Tempo médio por entrega: {media:N2}h.\n");
            return sb.ToString();

        }
        //mapa das entregas; tempo médio por entrega
        // tempo total
        // bateria
        // testes
        // documentação
    }
}
