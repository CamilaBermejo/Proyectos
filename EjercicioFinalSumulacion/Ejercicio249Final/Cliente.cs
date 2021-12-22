using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio249Final
{
    internal class Cliente
    {
        private String nombre;
        private int numeroPedido;
        private String tipoPedido;
        private String estado;
        private double horaIngresoLocal;
        private double horaSalidaLocal;
        private bool seVa;

        public string Nombre { get => nombre; set => nombre = value; }
        public int NumeroPedido { get => numeroPedido; set => numeroPedido = value; }
        public string Estado { get => estado; set => estado = value; }
        public double HoraIngresoLocal { get => horaIngresoLocal; set => horaIngresoLocal = value; }
        public double HoraSalidaLocal { get => horaSalidaLocal; set => horaSalidaLocal = value; }
        public string TipoPedido { get => tipoPedido; set => tipoPedido = value; }
        public bool SeVa { get => seVa; set => seVa = value; }

        public Cliente(int cantClientes, double horaIngresoLocal)
        {
            this.Nombre = "Cliente " + cantClientes;
            this.horaIngresoLocal = horaIngresoLocal;
            this.seVa = false;
        }

        public void setParametros( double horaIngresoLocal)
        {
            this.horaIngresoLocal = horaIngresoLocal;
        }

        public void resetearCampos()
        {
            this.Estado = "";
            this.HoraIngresoLocal = 0;
            this.HoraSalidaLocal = 0;
            this.NumeroPedido = 0;
            this.TipoPedido = "";
        }
    }
}
