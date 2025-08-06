using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorEncomendasDrone
{
    public class Simulador
    {
        private LinkedList<Pedido> _pedidos;
        private LinkedList<Drone> _drones; 
        // coordenadas: de 1 a 9 e A a Z

        public Simulador()
        {
            _pedidos = new LinkedList<Pedido>();
            _drones = new LinkedList<Drone>();
        }
        
        /// <summary>
        /// Adiciona novo pedido no simulador.
        /// </summary>
        /// <param name="pedido">Pedido a ser adicionado</param>
        /// <exception cref="ArgumentNullException">Lançada se o objeto Pedido for nulo</exception>
        public void AdicionarPedido(Pedido pedido) 
        {
            if (pedido == null)
                throw new ArgumentNullException("O pedido não pode ser nulo.");
            _pedidos.AddLast(pedido);
        }

        /// <summary>
        /// Adiciona novo drone no simulador.
        /// </summary>
        /// <param name="drone">Drone a ser adicionado</param>
        /// <exception cref="ArgumentNullException">Lançada se o objeto Drone for nulo</exception>
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

        /// <summary>
        /// Ordena drones pela capacidade (da maior para a menor) e pelo alcance (menor para maior), pedidos pela prioridade (mais alta para mais baixa), peso (menor para maior) e distância entre sua localização e o ponto de origem dos drones (da menor para maior distância). Depois, para cada drone, filtra os pedidos que atendem às condições necessárias para que possam ser levados por este, que faz as entregas após não haver mais pedidos possíveis de serem alocados. Marca como entregues na lista de pedidos todos os que forem levados pelos drones.
        /// </summary>
        /// <returns>Retorna verdadeiro caso as alocações sejam feitas, e falso caso contrário (se não houver nenhum pedido ou nenhum drone cadastrado, ou todos os pedidos já foram entregues.)</returns>
        public bool AlocarPedidosNoDrone()
        {
            if (_drones.Count > 0 && _pedidos.Count > 0)
            {
                int contNaoEntregues = 0;
                foreach (Pedido p in _pedidos)
                {
                    if (!p.FoiEntregue())
                        contNaoEntregues++;
                }
                if(contNaoEntregues > 0)
                {
                    string origemDrones = _drones.First().GetLocalOrigem();
                    int contAlocacoes = 0;
                    IEnumerable<Drone> dronesOrdenados = _drones.OrderByDescending(d => d.GetCapacidade()).ThenBy(d => d.GetAlcance());

                    // ordenar pedidos por prioridade (alta - media - baixa), peso (menor-maior) e distância (menor - maior)
                    List<Pedido> pedidosOrdenados = _pedidos.OrderByDescending(p => p.GetPrioridade())
                        .ThenBy(p => p.GetPeso()).ThenBy(p => CalcularDistanciaEntre(p.GetLocalizacao(), origemDrones)).ToList();

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
                                contAlocacoes++;
                                Console.WriteLine($"\t-Drone #{d.GetID()} recebeu o pedido #{p.GetID()}. Sua capacidade restante é de {d.CapacidadeRestante()}kg.");
                            }
                        }

                        foreach (Pedido p in pedidosRecebidos)
                            pedidosOrdenados.Remove(p);

                        if (d.QuantosPedidosALevar() > 0)
                            d.Viajar();
                    }
                    if(contAlocacoes == 0)
                        Console.WriteLine("\n  Não há drones disponíveis para realizar as entregas pendentes no momento.\n\n");
                    return true;
                }
                return false;
            }
            return false;
        }
        
        /// <summary>
        /// Ordena os pedidos com base em algum critério passado por parâmetro (um Comparer<Pedido>). 
        /// </summary>
        /// <param name="comp">Comparador que especifique a característica dos pedidos a ser analisada para ordenação.</param>
        /// <param name="pedidos">IEnumerable de pedidos a ser ordenado.</param>
        /// <returns></returns>
        private List<Pedido> OrdenarPedidos(Comparer<Pedido> comp, IEnumerable<Pedido> pedidos)
        {
            List<Pedido> pedidosEmLista = pedidos.ToList();
            pedidosEmLista.Sort(comp);
            return pedidosEmLista;
        }
      
        /// <summary>
        /// Informa quais são os pedidos entregues e não entregues, se houver. Caso contrário, é exibida uma mensagem informando o usuário que não há pedidos.
        /// </summary>
        /// <returns>String para exibir relatório sobre os pedidos</returns>
        public string RelatorioPedidos()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\t=== RELATÓRIO - PEDIDOS ===\n");
            if (_pedidos.Count == 0)
                sb.AppendLine("Não há nenhum pedido ainda.\n");
            sb.AppendLine(" > PEDIDOS ENTREGUES");
            int contEntregues = 0;
            foreach (Pedido p in _pedidos)
            {
                if (p.FoiEntregue())
                { sb.AppendLine(p.ToString());  contEntregues++; }
            }
            if (contEntregues == 0)
                sb.AppendLine("Nenhum pedido foi entregue ainda.");
            sb.AppendLine(" > PEDIDOS NÃO ENTREGUES");
            int contNaoEntregues = 0;
            foreach (Pedido p in _pedidos)
            {
                if (!p.FoiEntregue())
                { sb.AppendLine(p.ToString()); contNaoEntregues++; }
            }
            if (contNaoEntregues == 0)
                sb.AppendLine("Todos os pedidos já foram entregues.");
            return sb.ToString();
        }

        /// <summary>
        /// Ordena os pedidos com base em um comparador e cria um relatório com os dados obtidos.
        /// </summary>
        /// <param name="comp">Comparador a ser utilizado para ordenação</param>
        /// <returns>String para exibição de relatório sobre os pedidos em ordem</returns>
        public string RelatorioOrdenadoPedidos(Comparer<Pedido> comp)
        {
            IEnumerable<Pedido> pedidosOrdenados = OrdenarPedidos(comp, _pedidos);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\t=== RELATÓRIO ORDENADO - PEDIDOS ===\n");
            foreach(Pedido p in pedidosOrdenados)
                { sb.AppendLine(p.ToString()); }
            return sb.ToString();
        }

        /// <summary>
        /// Exibe informações sobre os drones cadastrados, se houver. Caso contrário, informa que ainda não há nenhum.
        /// </summary>
        /// <returns>String para exibição de relatório dos drones</returns>
        public string RelatorioDrone()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\t=== RELATÓRIO - DRONES ===");
            if (_drones.Count == 0)
                sb.AppendLine("Não há nenhum drone cadastrado.\n");
            foreach(Drone d in _drones)
                sb.AppendLine(d.ToString());
            sb.AppendLine();
            return sb.ToString();
        }

        /// <summary>
        /// Calcula quantas entregas foram feitas por todos os drones do simulador e retorna relatório com os dados
        /// </summary>
        /// <returns>String para exibição de relatório</returns>
        public string QuantidadeEntregasFeitas()
        {
            StringBuilder sb = new StringBuilder();
            int quantidade = 0;
            foreach (Drone d in _drones)
                quantidade += d.PedidosEntregues();
            sb.AppendLine($"\n -Entregas realizadas: {quantidade}.\n");
            return sb.ToString();
        }

        /// <summary>
        /// Retorna a quantidade de entregas que foram realizadas.
        /// </summary>
        /// <returns>Inteiro que representa o número de pedidos entregues</returns>
        public int TotalEntregasFeitas()
        {
            int quant = 0;
            foreach (Drone d in _drones)
                quant += d.PedidosEntregues();
            return quant;
        }
        /// <summary>
        /// Verifica qual drone realizou a maior quantidade de entregas, caso já tenha sido realizada alguma, e retorna relatório com o resultado obtido.
        /// </summary>
        /// <returns>String com os dados obtidos</returns>
        public string DroneMaisEficiente()
        {
            StringBuilder sb = new StringBuilder();
            int contEntregas = 0;
            foreach(Pedido p in _pedidos)
            {
                if(p.FoiEntregue())
                    contEntregas++;
            }
            if (contEntregas == 0)
                sb.AppendLine("  Nenhum drone realizou entregas ainda!");
            else
            {
                Drone melhor = _drones.First();
                foreach (Drone d in _drones)
                {
                    if (d.PedidosEntregues() > melhor.PedidosEntregues())
                        melhor = d;
                }
                sb.AppendLine($"\n -Drone mais eficiente: #{melhor.GetID()} - Realizou {melhor.PedidosEntregues()} entregas.\n");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Ilustra no console, de forma simplificada, o mapa de coordenadas da cidade, mostrando o ponto de origem dos drones, pedidos ainda não enviados e os que já foram entregues.
        /// </summary>
        public void MapaCidade()
        {
            char[,] mapa = new char[9, 26];
            Console.WriteLine("\n\t\t\t=== Mapa da Cidade ===\n\t-Legenda:");
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(" P: ");
            Console.ResetColor();
            Console.WriteLine("Há um pedido nesta coordenada");
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(" E: ");
            Console.ResetColor();
            Console.WriteLine("Pedido foi entregue na coordenada");
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write(" D: ");
            Console.ResetColor();
            Console.WriteLine("Local de origem dos drones\n");
            Console.WriteLine("    A   B   C   D   E   F   G   H   I   J   K   L   M   N   O   P   Q   R   S   T   U   V   W   X   Y   Z\n");
            
            for(int i = 0; i<mapa.GetLength(0); i++)
            {
                for(int j = 0; j<mapa.GetLength(1); j++)
                {
                    mapa[i, j] = '.';
                }
            }
            string origemDrones = _drones.First().GetLocalOrigem();
            int xDrones = (int)char.GetNumericValue(origemDrones[0]);
            int yDrones = (char)origemDrones[1] - 'A' + 1;
            mapa[xDrones - 1, yDrones - 1] = 'D';
            foreach (Pedido p in _pedidos)
            {
                string local = p.GetLocalizacao();
                int xCoordenada = (int)char.GetNumericValue(local[0]);
                int yCoordenada = (char)local[1] - 'A' + 1;
                if (!p.FoiEntregue())
                    mapa[xCoordenada - 1, yCoordenada - 1] = 'P';
                else
                    mapa[xCoordenada - 1, yCoordenada - 1] = 'E';
                
            }
            for (int i = 0; i < mapa.GetLength(0); i++)
            {
                Console.Write($"{i+1}");
                for (int j = 0; j < mapa.GetLength(1); j++)
                {
                    Console.Write("   ");
                    if (mapa[i,j] == 'D')
                        Console.BackgroundColor = ConsoleColor.Green;
                    else if (mapa[i,j] == 'P')
                        Console.BackgroundColor = ConsoleColor.Red;
                    else if (mapa[i,j] == 'E')
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.Write($"{mapa[i, j]}");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }


        /// <summary>
        /// Calcula o tempo médio gasto por cada entrega.
        /// </summary>
        /// <returns>String para exibir um relatório com os dados.</returns>
        public string TempoMedioPorEntrega()
        {
            int contEntregas = 0; StringBuilder sb = new StringBuilder();
            foreach (Pedido p in _pedidos)
            {
                if(p.FoiEntregue())
                    contEntregas++;
            }
            if (contEntregas > 0)
            {
                double somaTempos = 0;
                double entregasFeitas = 0;
                foreach (Drone d in _drones)
                {
                    if (d.PedidosEntregues() > 0)
                    {
                        somaTempos += d.TempoTotalGasto();
                        entregasFeitas += d.PedidosEntregues();
                    }
                }
                double media = somaTempos / (double)entregasFeitas;
                double mediaMinutos = media * 60;
                // parte inteira da média
                int minutos = (int)Math.Floor(mediaMinutos);
                double parteDecimal = mediaMinutos - minutos;
                int segundos = (int)Math.Round(parteDecimal * 60);

                sb.AppendLine($"\n -Tempo médio por entrega: {media:N2}h, ou {minutos} minutos e {segundos} segundos.\n");
            }
            else
                sb.AppendLine("\n  Nenhuma entrega foi feita ainda!");

            return sb.ToString();
        }

        /// <summary>
        /// Retorna um relatório com o tempo total que foi gasto por todos os drones, em todas as suas entregas.
        /// </summary>
        /// <returns>String que informa o tempo total de entrega.</returns>
        public string TempoTotalGastoEntregas()
        {
            StringBuilder sb = new StringBuilder();
            double total = 0;
            foreach (Drone d in _drones)
            {
                if(d.PedidosEntregues() > 0)
                {
                    total += d.TempoTotalGasto();
                    TimeSpan tempoDrone = TimeSpan.FromHours(d.TempoTotalGasto());
                    int horas = tempoDrone.Hours; int minutos = tempoDrone.Minutes; int segundos = tempoDrone.Seconds;
                    sb.AppendLine($"\t-Tempo gasto pelo drone #{d.GetID()}: {horas:D2} horas, {minutos:D2} minutos e {segundos:D2} segundos. - {d.PedidosEntregues()} pedidos entregues.");
                }
            }
            TimeSpan tempo = TimeSpan.FromHours(total);
            int totalHoras = tempo.Hours; int totalMin = tempo.Minutes; int totalSeg = tempo.Seconds;
            sb.AppendLine($"\n -Tempo total gasto pelos drones: {totalHoras:D2}:{totalMin:D2}:{totalSeg:D2}.\n");
            
            return sb.ToString();
        }
    }
}
