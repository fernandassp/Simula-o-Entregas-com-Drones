using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimuladorEncomendasDrone
{
    internal class Program
    {
        /// <summary>
        /// Gera pedidos aleatórios para o Simulador, com prioridades que variam entre "alta", "média" e "baixa", coordenadas (x, y) (X: de 1 a 9, Y: de A a Z) e pesos que vão de 1kg a 20kg.
        /// </summary>
        /// <param name="s">Simulador no qual os pedidos serão cadastrados.</param>
        /// <param name="localProibido">Coordenada em que não se pode alocar pedidos: é o ponto de origem dos drones.</param>
        static void GerarPedidos(Simulador s, string localProibido)
        {
            for(int i = 0; i < 100; i++)
            {
                Random r = new Random();
                //localização: 1 a 9, A a Z;
                string coord;
                do
                {
                    string x = r.Next(1, 10).ToString();
                    char y = (char)('A' + r.Next(0, 26));
                    coord = x + y;
                } while (coord == localProibido);
                double peso = r.Next(1, 21);
                int prioridade = r.Next(1,4);
                if (prioridade == 1)
                {
                    s.AdicionarPedido(new Pedido(coord, peso, "baixa"));
                }
                else if (prioridade == 2)
                {
                    s.AdicionarPedido(new Pedido(coord, peso, "media"));
                }
                else
                    s.AdicionarPedido(new Pedido(coord, peso, "alta"));
            }
            
        }
        /// <summary>
        /// Gera drones para o simulador, todos com o mesmo local de origem e com capacidades, alcances máximos e velocidades médias aleatórios. Retorna o local de origem dos drones, para que o usuário possa cadastrar outros com a mesma origem dos gerados aleatoriamente.
        /// </summary>
        /// <param name="s">Simulador no qual os drones serão cadastrados.</param>
        /// <returns>String com as coordenadas do ponto de origem dos drones.</returns>
        static string GerarDrones(Simulador s)
        {
            Random localOrigem = new Random(); // todos drones com mesmo ponto de origem
            string x = localOrigem.Next(1, 10).ToString();
            char y = (char)('A' + localOrigem.Next(0, 26));
            string coord = x + y;
            for (int i = 0; i<15; i++)
            {
                Random r = new Random();
                double capac = r.Next(10, 30);
                double distMax = r.Next(10, 20);
                double velMedia = r.Next(30, 51);
                s.AdicionarDrone(new Drone(capac, distMax, coord, velMedia));
            }
            return coord;
        }
        /// <summary>
        /// Exibe menu de opções para o usuário
        /// </summary>
        static void MenuOpcoes()
        {
            Console.WriteLine("\t\t\t== Simulador de Entregas com Drones ==\n-Digite a opção (de 1 a 5):\n1-Cadastrar um drone\n2-Cadastrar um pedido\n3-Ver drones cadastrados\n4-Ver pedidos cadastrados\n5-Ver relatórios\n6-Alocar pedidos nos drones\n7-Sair\n");
        }
        /// <summary>
        /// Exibe o menu com as opções de relatórios para o usuário
        /// </summary>
        static void MenuRelatorios()
        {
            Console.WriteLine("\n\t\t\t == Relatórios ==\n-Digite a opção (de 1 a 5) referente ao relatório que deseja ver:\n1-Quantidade de entregas feitas \n2-Tempo médio gasto por entrega\n3-Tempo total gasto com entregas\n4-Drone mais eficiente\n5-Mapa da cidade\n6-Relatório ordenado dos pedidos");
        }
        static void Main(string[] args)
        {
            Simulador s = new Simulador();
            string origemDrones = GerarDrones(s);
            GerarPedidos(s, origemDrones);
            int opc;
            do
            {
                MenuOpcoes();
                opc = int.Parse(Console.ReadLine());

                switch (opc)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("-> Informe a capacidade (em kg) do drone:");
                        double capac = double.Parse(Console.ReadLine());
                        while(capac <= 0)
                        {
                            Console.WriteLine("A capacidade deve ser maior que 0 kg.");
                            capac = double.Parse(Console.ReadLine());
                        }
                        Console.WriteLine("-> Informe o alcance máximo (em km) do drone:");
                        double alcance = double.Parse(Console.ReadLine());
                        while (alcance <= 0)
                        {
                            Console.WriteLine("O alcance máximo deve ser maior que 0 km.");
                            alcance = double.Parse(Console.ReadLine());
                        }
                        Console.WriteLine("->Informe a velocidade média (km/h) do drone:");
                        double vel = double.Parse(Console.ReadLine());
                        while (vel <= 0)
                        {
                            Console.WriteLine("A velocidade média deve ser maior que 0 km/h.");
                            vel = double.Parse(Console.ReadLine());
                        }
                        Drone novo = new Drone(capac, alcance, origemDrones, vel);
                        s.AdicionarDrone(novo);
                        Console.WriteLine($" ** Drone registrado com sucesso! ID: #{novo.GetID()}\n");
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("-> Informe a localização do pedido (coordenada (x,y) sendo X um inteiro de 1 a 9 e Y uma letra de A a Z):");
                        string coord = Console.ReadLine();
                        while (!Simulador.VerificarCoordenadaVálida(coord))
                        {
                            Console.WriteLine("A coordenada é inválida. Digite outra:");
                        }
                        Console.WriteLine("-> Informe a prioridade do pedido (alta, média ou baixa):");
                        string prioridade = Console.ReadLine(); prioridade = prioridade.ToLower();
                        while(prioridade != "baixa" && prioridade != "alta" && prioridade != "media" && prioridade != "média")
                        {
                            Console.WriteLine("Prioridade inválida. Escolha novamente:");
                            prioridade = Console.ReadLine();
                        }
                        Console.WriteLine("-> Informe o peso (em kg) do pedido.");
                        double peso = double.Parse(Console.ReadLine());
                        while (peso <= 0)
                        {
                            Console.WriteLine("Informe um peso maior que 0 kg.");
                            peso = double.Parse(Console.ReadLine());
                        }
                        Pedido novoPedido = new Pedido(coord, peso, prioridade);
                        s.AdicionarPedido(novoPedido);
                        Console.WriteLine($" ** Pedido registrado com sucesso! ID: #{novoPedido.GetID()}\n.");
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine(s.RelatorioDrone());
                        break;
                    case 4:
                        Console.Clear();
                        Console.WriteLine(s.RelatorioPedidos());
                        break;
                    case 5:
                        Console.Clear();
                        MenuRelatorios();
                        int relatorio = int.Parse(Console.ReadLine());
                        while (!(relatorio>= 1 && relatorio<=6))
                        {
                            Console.WriteLine("Escolha inválida. Escolha um relatório, de 1 a 6:");
                            relatorio = int.Parse(Console.ReadLine());
                        }
                        switch (relatorio)
                        {
                            case 1:
                                Console.Clear();
                                Console.WriteLine(s.QuantidadeEntregasFeitas());
                                break;
                            case 2:
                                Console.Clear();
                                Console.WriteLine(s.TempoMedioPorEntrega());
                                break;
                            case 3:
                                Console.Clear();
                                Console.WriteLine(s.TempoTotalGastoEntregas());
                                break;
                            case 4:
                                Console.Clear();
                                Console.WriteLine(s.DroneMaisEficiente());
                                break;
                            case 5:
                                Console.Clear();
                                s.MapaCidade();
                                break;
                            case 6:
                                Console.Clear();
                                int resp;
                                do
                                {
                                    Console.WriteLine("Ordenar os pedidos por:\n1- Prioridade (decrescente)\n2-Peso (crescente)\n3-Distância entre sua localização e a origem dos drones (crescente)\n(Escolha de 1 a 3)");
                                    resp = int.Parse(Console.ReadLine());
                                } while (!(resp>= 1 && resp<=3));

                                if(resp == 1)
                                {
                                    Comparer<Pedido> comp = Comparer<Pedido>.Create((p1,p2) =>
                                    {
                                        if (p1.GetPrioridade() > p2.GetPrioridade()) return -1;
                                        if (p1.GetPrioridade() < p2.GetPrioridade()) return 1;
                                        return 0;
                                    });
                                    Console.WriteLine(s.RelatorioOrdenadoPedidos(comp));
                                }
                                else if(resp == 2)
                                {
                                    Comparer<Pedido> comp = Comparer<Pedido>.Create((p1, p2) =>
                                    {
                                        if (p1.GetPeso() > p2.GetPeso()) return 1;
                                        if (p1.GetPeso() < p2.GetPeso()) return -1;
                                        return 0;
                                    });
                                    Console.WriteLine(s.RelatorioOrdenadoPedidos(comp));
                                }
                                else
                                {
                                    Comparer<Pedido> comp = Comparer<Pedido>.Create((p1, p2) =>
                                    {
                                        if (Simulador.CalcularDistanciaEntre(origemDrones, p1.GetLocalizacao()) > Simulador.CalcularDistanciaEntre(origemDrones, p2.GetLocalizacao())) return -1;
                                        if (Simulador.CalcularDistanciaEntre(origemDrones, p1.GetLocalizacao()) < Simulador.CalcularDistanciaEntre(origemDrones, p2.GetLocalizacao())) return 1;
                                        return 0;
                                    });
                                    Console.WriteLine(s.RelatorioOrdenadoPedidos(comp));
                                }
                                    break;
                        }
                        break;
                    case 6:
                        Console.Clear();
                        bool teste = s.AlocarPedidosNoDrone();
                        if (!teste)
                            Console.WriteLine("Não há pedidos/drones cadastrados para que seja possível realizar entregas, ou todos os pedidos já foram entregues.");
                        break;
                    case 7:
                        Console.Clear();
                        Console.WriteLine("\nO programa será encerrado.");
                        break;
                    default:
                        while (!(opc>=1 && opc<=7))
                        {
                            Console.WriteLine("Opção inválida. Escolha de 1 a 7.");
                            opc = int.Parse(Console.ReadLine());
                        }
                        break;
                }
            }
            while (opc != 7);
        }
    }
}
