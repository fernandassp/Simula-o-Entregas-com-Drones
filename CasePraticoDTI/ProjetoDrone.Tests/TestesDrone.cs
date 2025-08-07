using Xunit;
using SimuladorEncomendasDrone;

namespace ProjetoDrone.Tests
{
    public class TestesDrone
    {
        [Fact]
        public void TesteCabeMaisPedidos()
        {
            Drone drone = new Drone(30, 30, "3A", 20);
            Assert.True(drone.CapacidadeRestante() > 0);

            drone.ReceberPedido(new Pedido("2B", 15, "alta"));
            Assert.True(drone.CapacidadeRestante() > 0);

            drone.ReceberPedido(new Pedido("4A", 15, "alta"));
            Assert.False(drone.CapacidadeRestante() > 0);
        }

        [Fact]
        public void TestePodeReceberPedidoInvalido()
        {
            Drone drone = new Drone(10,15, "2A",25);
            Pedido pedido = new Pedido("3B", 15, "alta"); 
            Assert.False(drone.PodeReceberPedido(pedido));
        }

        [Fact]
        public void TestePodeViajarAteCoordenada()
        {
            Drone drone = new Drone(10, 2, "2A", 20);
            string coordenada = "9M";
            Assert.False(drone.PodeViajarAte(coordenada));
        }

        [Fact]
        public void TesteViagemFeitaCorretamente()
        {
            Drone drone = new Drone(30, 25, "2A", 15);
            Pedido pedido = new Pedido("3B", 5, "alta");
            int teste = drone.ReceberPedido(pedido);
            Assert.True(teste == 1 && drone.QuantosPedidosALevar() == 1);
            drone.Viajar();
            Assert.True(drone.PedidosEntregues() == 1 && drone.TempoTotalGasto() > 0);
        }

        [Fact]
        public void TesteLancaExcecao()
        {
            Drone drone = new Drone(20, 25, "4G", 10);
            Assert.Throws<InvalidOperationException>( () => drone.Viajar()); // testa se lança exceção quando o drone não recebeu nenhum pedido e tenta viajar
        }
    }
}
