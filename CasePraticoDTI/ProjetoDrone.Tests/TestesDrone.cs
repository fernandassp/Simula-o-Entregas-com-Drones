using Xunit;
using SimuladorEncomendasDrone;

namespace ProjetoDrone.Tests
{
    public class TestesDrone
    {
        [Fact]
        public void DroneCabeMaisPedidos()
        {
            Drone drone = new Drone(30, 30, "3A", 20);
            Assert.True(drone.CapacidadeRestante() > 0);

            drone.ReceberPedido(new Pedido("2B", 15, "alta"));
            Assert.True(drone.CapacidadeRestante() > 0);

            drone.ReceberPedido(new Pedido("4A", 15, "alta"));
            Assert.False(drone.CapacidadeRestante() > 0);
        }
    }
}
