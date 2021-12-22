using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio249Final
{
    internal class Caja
    {
        private String estado;
        private Queue<Cliente> colaCaja;
        private Cliente cAtendido;

        public string Estado { get => estado; set => estado = value; }
        internal Queue<Cliente> ColaCaja { get => colaCaja; set => colaCaja = value; }
        internal Cliente CAtendido { get => cAtendido; set => cAtendido = value; }

        public Caja()
        {
            this.estado = "libre";
            this.ColaCaja = new Queue<Cliente>();
        }
    }
}
