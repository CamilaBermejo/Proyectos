using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio249Final
{
    internal class VectorEstado
    {
        private String evento;
        private double reloj;
        private int linea;
        private double rndLlegadaCleinte;
        private double tiempoEntreLlegadas;
        private double proximaLlegada;
        private double rndCaja;
        private double tiempoEntreCaja;
        private double proximaCaja;
        private Caja caja;
        private double rndTipoPedido;
        private String tipoPedido;
        private double rndMostrador;
        private double tiempoEntrePedidos;
        private List<Mostrador> mostradores;
        private Queue<Cliente> colaMostrador;
        private double rndTipoSalon;
        private String tipoSalon;
        private double rndSR;
        private double tiempoSalidaClienteSR;
        private double rndSA;
        private double tiempoSalidaClienteSA;
        private Salon salonRojo;
        private Salon salonAzul;
        private List<Cliente> clientes;
        private int cantClientes;
        private int cantClientesSCaja;
        private double tiempoTotalColaCaja;
        private double tiempoPromedioColaCaja;
        private double tiempoTotalPermanencia;
        private double tiempoPromedioPermanecia;

        public string Evento { get => evento; set => evento = value; }
        public double Reloj { get => reloj; set => reloj = value; }
        public int Linea { get => linea; set => linea = value; }
        public double RndLlegadaCleinte { get => rndLlegadaCleinte; set => rndLlegadaCleinte = value; }
        public double TiempoEntreLlegadas { get => tiempoEntreLlegadas; set => tiempoEntreLlegadas = value; }
        public double ProximaLlegada { get => proximaLlegada; set => proximaLlegada = value; }
        public double RndCaja { get => rndCaja; set => rndCaja = value; }
        public double TiempoEntreCaja { get => tiempoEntreCaja; set => tiempoEntreCaja = value; }
        public double ProximaCaja { get => proximaCaja; set => proximaCaja = value; }
        public double RndTipoPedido { get => rndTipoPedido; set => rndTipoPedido = value; }
        public string TipoPedido { get => tipoPedido; set => tipoPedido = value; }
        public double RndMostrador { get => rndMostrador; set => rndMostrador = value; }
        public double TiempoEntrePedidos { get => tiempoEntrePedidos; set => tiempoEntrePedidos = value; }
        public double RndTipoSalon { get => rndTipoSalon; set => rndTipoSalon = value; }
        public String TipoSalon { get => tipoSalon; set => tipoSalon = value; }
        public double RndSR { get => rndSR; set => rndSR = value; }
        public double TiempoSalidaClienteSR { get => tiempoSalidaClienteSR; set => tiempoSalidaClienteSR = value; }
        public double RndSA { get => rndSA; set => rndSA = value; }
        public double TiempoSalidaClienteSA { get => tiempoSalidaClienteSA; set => tiempoSalidaClienteSA = value; }
        public int CantClientes { get => cantClientes; set => cantClientes = value; }
        public double TiempoTotalColaCaja { get => tiempoTotalColaCaja; set => tiempoTotalColaCaja = value; }
        public double TiempoPromedioColaCaja { get => tiempoPromedioColaCaja; set => tiempoPromedioColaCaja = value; }
        public double TiempoTotalPermanencia { get => tiempoTotalPermanencia; set => tiempoTotalPermanencia = value; }
        public double TiempoPromedioPermanecia { get => tiempoPromedioPermanecia; set => tiempoPromedioPermanecia = value; }
        public int CantClientesSCaja { get => cantClientesSCaja; set => cantClientesSCaja = value; }
        internal Caja Caja { get => caja; set => caja = value; }
        internal List<Mostrador> Mostradores { get => mostradores; set => mostradores = value; }
        internal Queue<Cliente> ColaMostrador { get => colaMostrador; set => colaMostrador = value; }
        internal Salon SalonRojo { get => salonRojo; set => salonRojo = value; }
        internal Salon SalonAzul { get => salonAzul; set => salonAzul = value; }
        internal List<Cliente> Clientes { get => clientes; set => clientes = value; }
        
        public VectorEstado(int cR, int cA)
        {
            this.Caja = new Caja();
            this.Mostradores = new List<Mostrador>();
            this.ColaMostrador = new Queue<Cliente>();
            this.SalonAzul = new Salon(cA);
            this.SalonRojo = new Salon(cR);
            this.Clientes = new List<Cliente>();
            this.Reloj = 0;
            this.Evento = "inicio_simulacion";
            this.Mostradores.Add(new Mostrador("Mostrador 1"));
            this.Mostradores.Add(new Mostrador("Mostrador 2"));
            this.Mostradores.Add(new Mostrador("Mostrador 3"));
        }

        public VectorEstado copiaVector()
        {
            return (VectorEstado)this.MemberwiseClone();
        }

        public void resetearCamposLleagada()
        {
            this.RndLlegadaCleinte = 0;
            this.TiempoEntreLlegadas = 0;
            this.RndCaja = 0;
            this.tiempoEntreCaja = 0;
            this.RndTipoPedido = 0;
            this.TipoPedido = "";
            this.rndMostrador = 0;
            this.tiempoEntrePedidos = 0;
            this.rndTipoSalon = 0;
            this.TipoSalon = "";
            this.rndSR = 0;
            this.tiempoSalidaClienteSR = 0;
            this.rndSA = 0;
            this.tiempoSalidaClienteSA = 0;
            
        }

        public int buscarClienteVacio()
        {
            int res = -1;
            for(int i = 0; i < clientes.Count; i++)
            {
                //Tomo el cliente de la posicion i
                Cliente c = clientes[i];
                if(c.Estado == "")
                {
                    res = i;
                    break;
                }
            }
            return res;
        }

        public int buscarlugarMostrador()
        {
            int res = -1;
            for (int i = 0; i < mostradores.Count; i++)
            {
                //Tomo el cliente de la posicion i
                Mostrador m = mostradores[i];
                if (m.Estado == "libre")
                {
                    res = i;
                    break;
                }
            }
            return res;
        }


    }
}
