using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimuladorEncomendasDrone
{
    internal class Program
    {
        static void GerarPedidos(Simulador s)
        {
            for(int i = 0; i < 100; i++)
            {
                Random r = new Random();
                //localização: 1 a 9, A a Z;
                string x = r.Next(1, 10).ToString();
                char y = (char)('A' + r.Next(0, 26));
                string coord = x + y;
                double peso = r.Next(1, 25);
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
        static void GerarDrones(Simulador s)
        {
            Random localOrigem = new Random(); // todos drones com mesmo ponto de origem
            string x = localOrigem.Next(1, 10).ToString(); // char ao invés de string?
            char y = (char)('A' + localOrigem.Next(0, 26));
            string coord = x + y;
            for (int i = 0; i<15; i++)
            {
                Random r = new Random();
                double capac = r.Next(1, 15);
                double distMax = r.Next(3, 11);
                double velMedia = r.Next(30, 51);
                s.AdicionarDrone(new Drone(capac, distMax, coord, velMedia));
            }
        }
        static void Main(string[] args)
        {
            Simulador s = new Simulador();
            GerarPedidos(s);
            GerarDrones(s);

            
            Console.WriteLine("\n" + s.RelatorioPedidos());
            Console.WriteLine();
            Console.WriteLine(s.RelatorioDrone());

            s.AlocarPedidosNoDrone();
            Console.WriteLine(s.QuantidadeEntregasFeitas());


            Console.WriteLine(s.TempoMedioPorEntrega());
        }
    }
}
