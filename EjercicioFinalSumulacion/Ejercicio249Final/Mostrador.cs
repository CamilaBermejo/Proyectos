using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio249Final
{
    internal class Mostrador
    {
        private String nombre;
        private String estado;
        private double proximoFin;
        private Cliente clienteAtendido;
        
        public string Nombre { get => nombre; set => nombre = value; }
        public string Estado { get => estado; set => estado = value; }
        public double ProximoFin { get => proximoFin; set => proximoFin = value; }
        internal Cliente ClienteAtendido { get => clienteAtendido; set => clienteAtendido = value; }

        public Mostrador(string nombre)
        {
            this.Nombre = nombre;
            this.Estado = "libre";
        }
    }
}
