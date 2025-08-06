using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorEncomendasDrone
{
    public class Pedido
    {
        private string _localizacao;
        private double _peso;
        private int _prioridade;
        private int _id;
        private static int _proxId = 1;
        private bool _entregue;

        public Pedido(string localizacao, double peso, string prioridade)
        {
            bool coordValida = Simulador.VerificarCoordenadaVálida(localizacao);
            if (!coordValida)
                throw new ArgumentException("A coordenada é inválida.");
           
            if (peso <= 0)
                throw new ArgumentException("O peso precisa ser maior que 0 kg.");

            _localizacao = localizacao.ToUpper();
            _peso = peso;
            _id = _proxId; _proxId++;
            if (prioridade.ToLower() == "baixa")
                _prioridade = 1;
            else if (prioridade.ToLower() == "media" || prioridade.ToLower() == "média")
                _prioridade = 2;
            else if (prioridade.ToLower() == "alta")
                _prioridade = 3;
            else
                throw new ArgumentException("Prioridade inválida. Deve ser 'baixa', 'média' ou 'alta'.");
            _entregue = false;
        }
        public int GetPrioridade()
        {
            return _prioridade;
        }
        public double GetPeso()
        {
            return _peso;
        }
        public string GetLocalizacao()
        {
            return _localizacao;
        }
        public int GetID()
        {
            return _id;
        }

        /// <summary>
        /// Verifica se um pedido já foi entregue.
        /// </summary>
        /// <returns>Verdadeiro se tiver sido entregue e falso caso não tenha sido ainda</returns>
        public bool FoiEntregue()
        {
            return _entregue;
        }

        /// <summary>
        /// Altera o atributo que determina se o pedido já foi entregue para "true"
        /// </summary>
        public void MarcarComoEntregue()
        {
            _entregue = true;
        }
        public override string ToString()
        {
            string entregue;
            if (FoiEntregue())
                entregue = "Sim.";
            else
                entregue = "Não.";
                string prioridade;
            if (_prioridade == 3)
                prioridade = "Alta";
            else if (_prioridade == 2)
                prioridade = "Média";
            else
                prioridade = "Baixa";
                return $"Pedido {_id}\n\tPrioridade: {prioridade}. Peso: {_peso}kg. Coordenadas: {_localizacao}.\nENTREGUE: {entregue}\n -----";
        }
    }
}
