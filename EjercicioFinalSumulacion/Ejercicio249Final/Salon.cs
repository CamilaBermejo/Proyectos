using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio249Final
{
    internal class Salon
    {
        private String nombre;
        private int capacidad;
        private List<Cliente> clientesEnSalon;
        private Queue<Cliente> colaSalon;
        private int ocupMaxima;

        public string Nombre { get => nombre; set => nombre = value; }
        public int Capacidad { get => capacidad; set => capacidad = value; }
        public int OcupMaxima { get => ocupMaxima; set => ocupMaxima = value; }
        internal List<Cliente> ClientesEnSalon { get => clientesEnSalon; set => clientesEnSalon = value; }
        internal Queue<Cliente> ColaSalon { get => colaSalon; set => colaSalon = value; }

        public Salon(int capacidad)
        {
            this.ClientesEnSalon = new List<Cliente>();
            this.ColaSalon = new Queue<Cliente>();
            this.capacidad = capacidad;
            this.ocupMaxima = 0;
        }
    }
}
