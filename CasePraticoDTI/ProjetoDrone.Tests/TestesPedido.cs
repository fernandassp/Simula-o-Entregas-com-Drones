using Xunit;
using SimuladorEncomendasDrone;

namespace ProjetoDrone.Tests
{
    public class TestesPedido
    {
        [Fact]
        public void PedidoMarcadoComoEntregueApósEntrega()
        {
            Simulador simulador = new Simulador();
            Pedido p1 = new Pedido("5A", 10, "alta");
            simulador.AdicionarPedido(p1);
            simulador.AdicionarPedido(new Pedido("2B", 20, "baixa"));
            simulador.AdicionarPedido(new Pedido("4C", 15, "media"));

            simulador.AdicionarDrone(new Drone(30, 20, "3C", 15));
            simulador.AdicionarDrone(new Drone(15, 15, "3C", 20));
            simulador.AdicionarDrone(new Drone(20, 25, "3C", 10));

            simulador.AlocarPedidosNoDrone();
            Assert.True(p1.FoiEntregue());
        }
    }
}