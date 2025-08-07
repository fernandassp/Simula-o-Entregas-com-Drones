using Xunit;
using SimuladorEncomendasDrone;

namespace ProjetoDrone.Tests
{
    public class TestesSimulador
    {
        [Fact]
        public void TesteEntregaPedidosCorretamente()
        {
            Simulador simulador = new Simulador();
            simulador.AdicionarPedido(new Pedido("5A", 10, "alta"));
            simulador.AdicionarPedido(new Pedido("2B", 20, "baixa"));
            simulador.AdicionarPedido(new Pedido("4C", 15, "media"));

            simulador.AdicionarDrone(new Drone(30, 20, "3C", 15));
            simulador.AdicionarDrone(new Drone(15, 15, "3C", 20));
            simulador.AdicionarDrone(new Drone(20, 25, "3C", 10));

            simulador.AlocarPedidosNoDrone();

            Assert.True(simulador.TotalEntregasFeitas() == 3);

            simulador.AlocarPedidosNoDrone();
        }

        [Fact]
        public void TesteVerificaCoordenadaCorretamente()
        {
            string coordenada1 = "32";
            string coordenada2 = "aa";
            string coordenada3 = "#1";
            Assert.False(Simulador.VerificarCoordenadaVálida(coordenada1));
            Assert.False(Simulador.VerificarCoordenadaVálida(coordenada2));
            Assert.False(Simulador.VerificarCoordenadaVálida(coordenada3));
        }

       
    }
}
