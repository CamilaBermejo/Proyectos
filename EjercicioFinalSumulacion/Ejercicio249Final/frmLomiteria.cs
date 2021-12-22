using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ejercicio249Final
{
    public partial class frmLomiteria : Form
    {
        int tLlegadaDesde, tLlegadaHasta, tAtencionDesde, tAtencionHasta, probParaLlevar, probParaLocal, tPrepLlevarDesde, tPrepLlevarHasta, tPrepLocalDesde, tPrepLocalHasta,
            capacidadSR, probSR, capacidadSA, probSA;
        VectorEstado anterior;
        VectorEstado actual;
        Random random = new Random();
        bool inicioSimulacion;
        Cliente proximoClienteSalir;
        Mostrador proximoMostrador;
        bool enSR = false;
        bool enSA = false;
        int cantPedidos;



        public frmLomiteria()
        {
            InitializeComponent();
        }

        private void dgvSimulacion_Scroll(object sender, ScrollEventArgs e)
        {
            this.dgvClientes.FirstDisplayedScrollingRowIndex = this.dgvMetricas.FirstDisplayedScrollingRowIndex = this.dgvSimulacion.FirstDisplayedScrollingRowIndex;
        }

        private void dgvMetricas_Scroll(object sender, ScrollEventArgs e)
        {
            this.dgvClientes.FirstDisplayedScrollingRowIndex = this.dgvSimulacion.FirstDisplayedScrollingRowIndex = this.dgvMetricas.FirstDisplayedScrollingRowIndex;
        }

        private void dgvClientes_Scroll(object sender, ScrollEventArgs e)
        {
            this.dgvMetricas.FirstDisplayedScrollingRowIndex = this.dgvSimulacion.FirstDisplayedScrollingRowIndex = this.dgvClientes.FirstDisplayedScrollingRowIndex;
        }

        private void dgvSimulacion_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvSimulacion.SelectedRows.Count > 0 && dgvClientes.Rows.Count > dgvSimulacion.SelectedRows[0].Index)
            {
                dgvClientes.Rows[dgvSimulacion.SelectedRows[0].Index].Selected = true;
                dgvMetricas.Rows[dgvSimulacion.SelectedRows[0].Index].Selected = true;
            }
        }

        private void dgvClientes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count > 0 && dgvMetricas.Rows.Count > dgvClientes.SelectedRows[0].Index)
            {
                dgvSimulacion.Rows[dgvClientes.SelectedRows[0].Index].Selected = true;
                dgvMetricas.Rows[dgvClientes.SelectedRows[0].Index].Selected = true;
            }
        }

        private void dgvMetricas_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMetricas.SelectedRows.Count > 0 && dgvSimulacion.Rows.Count > dgvClientes.SelectedRows[0].Index)
            {
                dgvSimulacion.Rows[dgvMetricas.SelectedRows[0].Index].Selected = true;
                dgvClientes.Rows[dgvMetricas.SelectedRows[0].Index].Selected = true;
            }
        }


        //Validacion de los campos 
        private bool validarCampos()
        {
            //rango de tiempo de llegada
            if (!(int.TryParse(txtALlegada.Text, out tLlegadaDesde) && tLlegadaDesde > 0))
            {
                MessageBox.Show("El valor mínimo del rango de llegada al local debe ser un número entero positivo mayor a 0", "Rango de llegada al Local", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!(int.TryParse(txtBLlegada.Text, out tLlegadaHasta) && tLlegadaHasta > tLlegadaDesde))
            {
                MessageBox.Show("El valor máximo del rango de llegada al local debe ser un número entero positivo mayor al mínimo", "Rango de llegada al Local", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            //rango de tiempo de atencion
            if (!(int.TryParse(txtAAtencion.Text, out tAtencionDesde) && tAtencionDesde > 0))
            {
                MessageBox.Show("El valor mínimo del rango de Atención de la caja debe ser un número entero positivo mayor a 0", "Rango de Tiempo de atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!(int.TryParse(txtBAtencion.Text, out tAtencionHasta) && tAtencionHasta > tAtencionDesde))
            {
                MessageBox.Show("El valor máximo del rango de Atención de la caja debe ser un número entero positivo mayor al mínimo", "Rango de Tiempo de atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            //Probabilidad para llevar
            if(!(int.TryParse(txtPorcLlevar.Text, out probParaLlevar) && probParaLlevar >= 0))
            {
                MessageBox.Show("El valor de la probabilidad de un pedido para llevar debe ser un número entero positivo mayor a 0", "Probabilidad de un pedido para llevar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            //Probabilidad para comer en el local
            if (!(int.TryParse(txtPorcLocal.Text, out probParaLocal) && probParaLocal >= 0))
            {
                MessageBox.Show("El valor de la probabilidad de un pedido para local debe ser un número entero positivo mayor a 0", "Probabilidad de un pedido para local", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            //suma de probabilidades
            if(probParaLocal+probParaLlevar != 100)
            {
                MessageBox.Show("La suma de las probabilidades debe ser 100%", "Suma de probabilidades eleccion de pedido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            //rango tiempo de preparacion para llevar
            if (!(int.TryParse(txtAParaLlevar.Text, out tPrepLlevarDesde) && tPrepLlevarDesde > 0))
            {
                MessageBox.Show("El valor mínimo del rango de tiempo de preparación para llevar debe ser un número entero positivo mayor a 0", "Rango de preparación para llevar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!(int.TryParse(txtBParaLlevar.Text, out tPrepLlevarHasta) && tPrepLlevarHasta > tPrepLlevarDesde))
            {
                MessageBox.Show("El valor máximo del rango de tiempo de preparación para llevar debe ser un número entero positivo mayor al mínimo", "Rango de preparación para llevar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            //rango tiempo de preparacion para comer en el local
            if (!(int.TryParse(txtALocal.Text, out tPrepLocalDesde) && tPrepLocalDesde > 0))
            {
                MessageBox.Show("El valor mínimo del rango de tiempo de preparación para comer en el local debe ser un número entero positivo mayor a 0", "Rango de preparación para comer en el local", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!(int.TryParse(txtBLocal.Text, out tPrepLocalHasta) && tPrepLocalHasta > tPrepLocalDesde))
            {
                MessageBox.Show("El valor máximo del rango de tiempo de preparación para comer en el local debe ser un número entero positivo mayor al mínimo", "Rango de preparación para comer en el local", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            //Capacidad Salon Rojo
            if (!(int.TryParse(txtCapSR.Text, out capacidadSR) && capacidadSR > 0))
            {
                MessageBox.Show("El valor de la Capacidad del Salón Rojo debe ser un número entero positivo mayor a 0", "Capacidad del Salón Rojo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            //Capacidad Salon Azul
            if (!(int.TryParse(txtCapSA.Text, out capacidadSA) && capacidadSA > 0))
            {
                MessageBox.Show("El valor de la Capacidad del Salón Azul debe ser un número entero positivo mayor a 0", "Capacidad del Salón Azul", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            //Probabilidad Salon Azul
            if (!(int.TryParse(txtProbSA.Text, out probSA) && probSA >= 0))
            {
                MessageBox.Show("El valor de la probabilidad para elegir el salón azul debe ser un número entero positivo mayor a 0", "Probabilidad para elegir salón", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            //Probabilidad Salon Rojo
            if (!(int.TryParse(txtProbSR.Text, out probSR) && probSR >= 0))
            {
                MessageBox.Show("El valor de la probabilidad para elegir el salón Rojo debe ser un número entero positivo mayor a 0", "Probabilidad para elegir salón", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            //suma de probabilidades
            if (probSA + probSR != 100)
            {
                MessageBox.Show("La suma de las probabilidades debe ser 100%", "Suma de probabilidades eleccion de salón", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btnSimular_Click(object sender, EventArgs e)
        {
            dgvSimulacion.Rows.Clear();
            dgvClientes.Rows.Clear();
            dgvMetricas.Rows.Clear();
            limpiarClientes();

            if (validarCampos())
            {
                anterior = new VectorEstado(capacidadSR, capacidadSA);
                actual = new VectorEstado(capacidadSR, capacidadSA);
                inicioSimulacion = false;
                //MessageBox.Show("Salio todo bien", "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cantPedidos = 0;

                while (actual.Reloj < 14400)
                {
                    actual.Linea++;

                    anterior = actual.copiaVector();
                    if (inicioSimulacion) proximoEvento();

                    switch (actual.Evento)
                    {
                        case "inicio_simulacion":
                            //Calcula el primer proximo evento, proxima llegada para la llegada del primer cliente.
                            actual.RndLlegadaCleinte = random.NextDouble();
                            actual.TiempoEntreLlegadas = aleatorioInt(tLlegadaDesde, tLlegadaHasta, actual.RndLlegadaCleinte);
                            actual.ProximaLlegada = actual.Reloj + actual.TiempoEntreLlegadas;

                            inicioSimulacion = true;
                            break;
                        case "llegada_cliente":
                            //Reseteamos los campos que no queremos mostrar
                            actual.resetearCamposLleagada();

                            //Calculamos la siguiente proxima llegada
                            actual.RndLlegadaCleinte = random.NextDouble();
                            actual.TiempoEntreLlegadas = aleatorioInt(tLlegadaDesde, tLlegadaHasta, actual.RndLlegadaCleinte);
                            actual.ProximaLlegada = actual.Reloj + actual.TiempoEntreLlegadas;

                            //creamos el cliente
                            Cliente nuevoCliente;
                            int posCliente = actual.buscarClienteVacio();
                            if(posCliente != -1)
                            {
                                nuevoCliente = actual.Clientes[posCliente];
                                nuevoCliente.setParametros(actual.Reloj);
                            }
                            else
                            {
                                nuevoCliente = new Cliente(actual.Clientes.Count, actual.Reloj);
                                actual.Clientes.Add(nuevoCliente);
                            }
                            //asigna al cliente en una caja
                            if(!(actual.Caja.Estado == "ocupado"))
                            {
                                actual.RndCaja = random.NextDouble();
                                actual.TiempoEntreCaja = aleatorioInt(tAtencionDesde, tAtencionHasta, actual.RndCaja);
                                actual.ProximaCaja = actual.Reloj + actual.TiempoEntreCaja;
                                actual.Caja.Estado = "ocupado";
                                nuevoCliente.Estado = "en Caja";
                                actual.Caja.CAtendido = nuevoCliente;
                            }
                            //si la caja esta ocupada, lo mete a la cola
                            else
                            {
                                actual.Caja.ColaCaja.Enqueue(nuevoCliente);
                                nuevoCliente.Estado = "en cola Caja";
                            }
                            break;
                        case "fin_Caja":
                            //Resetea los campos que no queremos mostrar
                            actual.resetearCamposLleagada();
                            //verificamos que cliente se va 
                            Cliente clienteSale = actual.Caja.CAtendido;
                            clienteSale.NumeroPedido = ++cantPedidos;
                            //busca el lugar del mostrador y de acuerdo a eso lo mete al mostrador o a la cola.
                            int resultado = actual.buscarlugarMostrador();
                            if ( resultado != -1)
                            {
                                actual.RndTipoPedido = random.NextDouble();
                                actual.TipoPedido = probTipoPedido(actual.RndTipoPedido);
                                clienteSale.TipoPedido = actual.TipoPedido;
                                actual.RndMostrador = random.NextDouble();
                                actual.TiempoEntrePedidos = calcularTiempoEntrePedidos(actual.RndMostrador);
                                actual.Mostradores[resultado].ProximoFin = actual.Reloj + actual.TiempoEntrePedidos;
                                clienteSale.Estado = "En mostrador";
                                actual.Mostradores[resultado].Estado = "ocupado";
                                actual.Mostradores[resultado].ClienteAtendido = clienteSale;
                            }
                            else
                            {
                                actual.ColaMostrador.Enqueue(clienteSale);
                                clienteSale.Estado = "en cola mostrador";

                            }
                            actual.CantClientesSCaja += 1;
                            //si la cola de caja no esta vacia, saca el primero cliente y lo asigna a la caja.
                            if (actual.Caja.ColaCaja.Count > 0)
                            {
                                Cliente clienteEntra;
                                clienteEntra = actual.Caja.ColaCaja.Dequeue();
                                actual.RndCaja = random.NextDouble();
                                actual.TiempoEntreCaja = aleatorioInt(tAtencionDesde, tAtencionHasta, actual.RndCaja);
                                actual.ProximaCaja = actual.Reloj + actual.TiempoEntreCaja;
                                actual.Caja.Estado = "ocupado";
                                clienteEntra.Estado = "en Caja";
                                actual.Caja.CAtendido = clienteEntra;
                                actual.TiempoTotalColaCaja += (actual.Reloj - clienteEntra.HoraIngresoLocal);
                            }
                            else
                            {
                                actual.Caja.Estado = "libre";
                                actual.ProximaCaja = 0;
                                actual.Caja.CAtendido = null;
                            }
                            break;
                        case "fin_pedido":
                            //Resetea los campos que no queremos mostrar
                            actual.resetearCamposLleagada();
                            //asigna el cliente que se esta por ir
                            Cliente clienteSeVa = proximoMostrador.ClienteAtendido;
                            //si el pedido no es "para llevar", le asigna un salon y lo guardamos en la lista.
                            if(clienteSeVa.TipoPedido != "Para llevar")
                            {
                                //define a cual salo va a ir el cliente.
                                actual.RndTipoSalon = random.NextDouble();
                                actual.TipoSalon = probTipoSalon(actual.RndTipoSalon);

                                //si hay capacidad lo mete en el Salón, si no, lo mete en su cola
                                if (actual.TipoSalon == "Salón Rojo")
                                {
                                    if (actual.SalonRojo.Capacidad > 0)
                                    {
                                        actual.RndSR = random.NextDouble();
                                        actual.TiempoSalidaClienteSR = timepoPermanenciaSR(actual.RndSR);
                                        actual.SalonRojo.Capacidad -= 1;
                                        if (actual.SalonRojo.OcupMaxima < actual.SalonRojo.ClientesEnSalon.Count) actual.SalonRojo.OcupMaxima = actual.SalonRojo.ClientesEnSalon.Count;
                                        clienteSeVa.Estado = "en Salón Rojo";
                                        clienteSeVa.HoraSalidaLocal = actual.Reloj + actual.TiempoSalidaClienteSR;
                                        actual.SalonRojo.ClientesEnSalon.Add(clienteSeVa);
                                    }
                                    else
                                    {
                                        actual.SalonRojo.ColaSalon.Enqueue(clienteSeVa);
                                        clienteSeVa.Estado = "en cola Salón Rojo";
                                    }
                                }
                                //si hay capacidad lo mete en el Salón, si no, lo mete en su cola
                                if (actual.TipoSalon == "Salón Azul")
                                {
                                    if (actual.SalonAzul.Capacidad > 0)
                                    {
                                        actual.RndSA = random.NextDouble();
                                        actual.TiempoSalidaClienteSA = timepoPermanenciaSA(actual.RndSR);
                                        actual.SalonAzul.Capacidad -= 1;
                                        if (actual.SalonAzul.OcupMaxima < actual.SalonAzul.ClientesEnSalon.Count) actual.SalonAzul.OcupMaxima = actual.SalonAzul.ClientesEnSalon.Count;
                                        clienteSeVa.Estado = "en Salón Azul";
                                        clienteSeVa.HoraSalidaLocal = actual.Reloj + actual.TiempoSalidaClienteSA;
                                        actual.SalonAzul.ClientesEnSalon.Add(clienteSeVa);
                                    }
                                    else
                                    {
                                        actual.SalonAzul.ColaSalon.Enqueue(clienteSeVa);
                                        clienteSeVa.Estado = "en cola Salón Azul";
                                    }
                                }
                            }
                            else
                            {
                                //Cuenta el cliente
                                actual.CantClientes += 1;
                                actual.TiempoTotalPermanencia += (actual.Reloj - clienteSeVa.HoraIngresoLocal);
                                clienteSeVa.SeVa = true;
                                clienteSeVa.resetearCampos();
                            }
                            if (actual.ColaMostrador.Count > 0)
                            {
                                Cliente clienteEntra;
                                clienteEntra = actual.ColaMostrador.Dequeue();
                                actual.RndTipoPedido = random.NextDouble();
                                actual.TipoPedido = probTipoPedido(actual.RndTipoPedido);
                                clienteEntra.TipoPedido = actual.TipoPedido;
                                actual.RndMostrador = random.NextDouble();
                                actual.TiempoEntrePedidos = calcularTiempoEntrePedidos(actual.RndMostrador);
                                proximoMostrador.ProximoFin = actual.Reloj + actual.TiempoEntrePedidos;
                                clienteEntra.Estado = "En mostrador";
                                proximoMostrador.Estado = "ocupado";
                                proximoMostrador.ClienteAtendido = clienteEntra;
                            }
                            else
                            {
                                proximoMostrador.Estado = "libre";
                                proximoMostrador.ProximoFin = 0;
                                proximoMostrador.ClienteAtendido = null;
                            }
                            break;
                        case "salida_cliente":
                            //Resetea los campos que no queremos mostrar
                            actual.resetearCamposLleagada();
                            actual.CantClientes += 1;
                            actual.TiempoTotalPermanencia += (proximoClienteSalir.HoraSalidaLocal - proximoClienteSalir.HoraIngresoLocal);
                            proximoClienteSalir.SeVa = true;
                            //Saca el cliente del salon correspondiente
                            if (enSR)
                            {
                                actual.SalonRojo.ClientesEnSalon.Remove(proximoClienteSalir);
                                proximoClienteSalir.resetearCampos();
                                //Aumenta la capacidad
                                actual.SalonRojo.Capacidad += 1;
                                //resetea el espacio de cliente en la lista de clientes ene el negocio
                                if (actual.SalonRojo.ColaSalon.Count > 0)
                                {
                                    Cliente clienteEntra;
                                    clienteEntra = actual.SalonRojo.ColaSalon.Dequeue();

                                    actual.RndSR = random.NextDouble();
                                    actual.TiempoSalidaClienteSR = timepoPermanenciaSR(actual.RndSR);
                                    actual.SalonRojo.Capacidad -= 1;
                                    clienteEntra.Estado = "en Salón Rojo";
                                    clienteEntra.HoraSalidaLocal = actual.Reloj + actual.TiempoSalidaClienteSR;
                                    actual.SalonRojo.ClientesEnSalon.Add(clienteEntra);
                                }
                            }
                            if (enSA)
                            {
                                actual.SalonAzul.ClientesEnSalon.Remove(proximoClienteSalir);
                                proximoClienteSalir.resetearCampos();

                                actual.SalonAzul.Capacidad += 1;
                                if (actual.SalonAzul.ColaSalon.Count > 0)
                                {
                                    Cliente clienteEntra;
                                    clienteEntra = actual.SalonAzul.ColaSalon.Dequeue();

                                    actual.RndSA = random.NextDouble();
                                    actual.TiempoSalidaClienteSA = timepoPermanenciaSR(actual.RndSR);
                                    actual.SalonAzul.Capacidad -= 1;
                                    clienteEntra.Estado = "en Salón Azul";
                                    clienteEntra.HoraSalidaLocal = actual.Reloj + actual.TiempoSalidaClienteSA;
                                    actual.SalonAzul.ClientesEnSalon.Add(clienteEntra);
                                }
                            }

                            break;
                        case "fin_simulacion":
                            break;
                    }

                    cargarDatosSim();
                    cargarDatosCliente();
                    cargarDatosMetricas();
                } 
            }
        }

        private void proximoEvento()
        {
            string evento = "fin_simulacion";
            double minimo = 14400;
            enSR = false;
            enSA = false;

            if (minimo > anterior.ProximaLlegada && anterior.ProximaLlegada > 0)
            {
                evento = "llegada_cliente";
                minimo = anterior.ProximaLlegada;
            }

            if(minimo > anterior.ProximaCaja && anterior.ProximaCaja > 0)
            {
                evento = "fin_Caja";
                minimo = anterior.ProximaCaja;
            }

            foreach(Mostrador mostrador in anterior.Mostradores)
            {
                if(minimo > mostrador.ProximoFin && mostrador.ProximoFin > 0)
                {
                    evento = "fin_pedido";
                    minimo = mostrador.ProximoFin;
                    proximoMostrador = mostrador;
                }
            }

            foreach(Cliente cliente in anterior.SalonRojo.ClientesEnSalon)
            {
                if(minimo > cliente.HoraSalidaLocal && cliente.HoraSalidaLocal > 0)
                {
                    evento = "salida_cliente";
                    minimo = cliente.HoraSalidaLocal;
                    proximoClienteSalir = cliente;
                    enSR = true;
                    enSA = false;
                }
            }

            foreach (Cliente cliente in anterior.SalonAzul.ClientesEnSalon)
            {
                if (minimo > cliente.HoraSalidaLocal && cliente.HoraSalidaLocal > 0)
                {
                    evento = "salida_cliente";
                    minimo = cliente.HoraSalidaLocal;
                    proximoClienteSalir = cliente;
                    enSA = true;
                    enSR = false;
                }
            }

            actual.Evento = evento;
            actual.Reloj = minimo;
        }

        //borra las columnas de cliente
        private void limpiarClientes()
        {
            for(int i = 1; i < dgvClientes.Columns.Count;)
            {
                dgvClientes.Columns.RemoveAt(i);
            }
        }

        //Es el metodo que genera una variable aleatoria entera con el metodo Uniforme.
        private int aleatorioInt(int desde, int hasta, double aleatorio)
        {
            return (int)(desde + (aleatorio * 1000) % (hasta - desde + 1));
        }

        private string probTipoPedido(double aleatorio)
        {
            if (aleatorio * 100 < probParaLlevar) return "Para llevar";
            return "Para consumir en local";
        }

        private int calcularTiempoEntrePedidos(double aleatorio)
        {
            if (actual.TipoPedido == "Para llevar") return aleatorioInt(tPrepLlevarDesde, tPrepLlevarHasta, aleatorio);
            return aleatorioInt(tPrepLocalDesde, tPrepLocalHasta, aleatorio);
        }

        private string probTipoSalon(double aleatorio)
        {
            if (aleatorio * 100 < probSR) return "Salón Rojo";
            return "Salón Azul";
        }

        private int timepoPermanenciaSR(double aleatorio)
        {
            if (actual.Reloj < 3600) return aleatorioInt(300, 2100, aleatorio);
            if (actual.Reloj < 7200) return aleatorioInt(900, 2700, aleatorio);
            if(actual.Reloj < 10800) return aleatorioInt(1200, 3000, aleatorio);
            return aleatorioInt(300, 2100, aleatorio);
        }

        private int timepoPermanenciaSA(double aleatorio)
        {
            if (actual.Reloj < 3600) return aleatorioInt(900, 2700, aleatorio);
            if (actual.Reloj < 7200) return aleatorioInt(1500, 3300, aleatorio);
            if (actual.Reloj < 10800) return aleatorioInt(2100, 3300, aleatorio);
            return aleatorioInt(1200, 3000, aleatorio);
        }

        private string relojToString(double tiempo)
        {
            double horas, minutos, segundos;
            //suma al reloj las 11 en segundos
            tiempo +=  39600;
            horas = (int)tiempo / 3600;
            minutos = (int)(tiempo % 3600) / 60;
            segundos = (int)(tiempo % 60);

            string m = (minutos < 10) ? "0" + minutos : minutos.ToString();
            string s = (segundos < 10) ? "0" + segundos : segundos.ToString();
            return horas + ":" + m + ":" + s; 
        }

        private void cargarDatosSim()
        {
            int linea = actual.Linea;
            string evento = actual.Evento;
            string reloj = relojToString(actual.Reloj);
            string rndLlegadaCliente = (actual.RndLlegadaCleinte > 0) ? Math.Round(actual.RndLlegadaCleinte, 2).ToString() : "";
            string tiempoEntreLleadas = (actual.TiempoEntreLlegadas > 0) ? Math.Round(actual.TiempoEntreLlegadas, 2).ToString() : "";
            string proximaLlegadaCliente = (actual.ProximaLlegada > 0) ? relojToString(actual.ProximaLlegada) : "";
            string rndCaja = (actual.RndCaja > 0) ? Math.Round(actual.RndCaja, 2).ToString() : "";
            string tiempoentreCajas = (actual.TiempoEntreCaja > 0) ? Math.Round(actual.TiempoEntreCaja, 2).ToString() : "";
            string proximaCaja = (actual.ProximaCaja > 0) ? relojToString(actual.ProximaCaja) : "";
            string estadoCaja = actual.Caja.Estado;
            string colaCaja = actual.Caja.ColaCaja.Count.ToString();
            string rndTipopedido = (actual.RndTipoPedido > 0) ? Math.Round(actual.RndTipoPedido, 2).ToString() : "";
            string tipoPedido = actual.TipoPedido;
            string rndMostrador = (actual.RndMostrador > 0) ? Math.Round(actual.RndMostrador, 2).ToString() : "";
            string tiempoEntrePedido = (actual.TiempoEntrePedidos > 0) ? Math.Round(actual.TiempoEntrePedidos, 2).ToString() : "";
            string proximoFinPedido1 = (actual.Mostradores[0].ProximoFin > 0) ? relojToString(actual.Mostradores[0].ProximoFin) : "";
            string proximoFinPedido2 = (actual.Mostradores[1].ProximoFin > 0) ? relojToString(actual.Mostradores[1].ProximoFin) : "";
            string proximoFinPedido3 = (actual.Mostradores[2].ProximoFin > 0) ? relojToString(actual.Mostradores[2].ProximoFin) : "";
            string pedidoP1 = (actual.Mostradores[0].ClienteAtendido != null && actual.Mostradores[0].ClienteAtendido.NumeroPedido > 0) ? " | " + actual.Mostradores[0].ClienteAtendido.NumeroPedido.ToString() : "";
            string estadoP1 = actual.Mostradores[0].Estado + pedidoP1;
            string pedidoP2 = (actual.Mostradores[1].ClienteAtendido != null && actual.Mostradores[1].ClienteAtendido.NumeroPedido > 0) ? " | " + actual.Mostradores[1].ClienteAtendido.NumeroPedido.ToString() : "";
            string estadoP2 = actual.Mostradores[1].Estado + pedidoP2;
            string pedidoP3 = (actual.Mostradores[2].ClienteAtendido != null && actual.Mostradores[2].ClienteAtendido.NumeroPedido > 0) ? " | " + actual.Mostradores[2].ClienteAtendido.NumeroPedido.ToString() : "";
            string estadoP3 = actual.Mostradores[2].Estado + pedidoP3;
            string colaMostrador = actual.ColaMostrador.Count.ToString();
            string rndTipoSalon = (actual.RndTipoSalon > 0) ? Math.Round(actual.RndTipoSalon, 2).ToString() : "";
            string tipoSalon = actual.TipoSalon;
            string rndSalonRojo = (actual.RndSR > 0) ? Math.Round(actual.RndSR, 2).ToString() : "";
            string tiemposalidaSR = (actual.TiempoSalidaClienteSR > 0) ? Math.Round(actual.TiempoSalidaClienteSR, 2).ToString() : "";
            string capacidadSR = actual.SalonRojo.Capacidad.ToString();
            string rndSalonAzul = (actual.RndSA > 0) ? Math.Round(actual.RndSA, 2).ToString() : "";
            string tiemposalidaSA = (actual.TiempoSalidaClienteSA > 0) ? Math.Round(actual.TiempoSalidaClienteSA, 2).ToString() : "";
            string capacidadSA = actual.SalonAzul.Capacidad.ToString();

            dgvSimulacion.Rows.Add(linea, evento, reloj, rndLlegadaCliente, tiempoEntreLleadas, proximaLlegadaCliente, rndCaja, tiempoentreCajas, proximaCaja,
                                    estadoCaja, colaCaja, rndTipopedido, tipoPedido, rndMostrador, tiempoEntrePedido, proximoFinPedido1, proximoFinPedido2,
                                    proximoFinPedido3, estadoP1, estadoP2, estadoP3, colaMostrador, rndTipoSalon, tipoSalon, rndSalonRojo, tiemposalidaSR,
                                    capacidadSR, rndSalonAzul, tiemposalidaSA, capacidadSA);
        }

        private void cargarDatosCliente()
        {
            dgvClientes.Rows.Add(1);
            dgvClientes.Rows[dgvClientes.Rows.Count - 1].Cells[0].Value = actual.Linea;

            for(int i=0; i< actual.Clientes.Count; i++)
            {
                Cliente cliente = actual.Clientes[i];

                if(cliente.Estado != "")
                {
                    string horaSalida = (cliente.HoraSalidaLocal > 0) ? relojToString(cliente.HoraSalidaLocal) : "";
                    string datosCliente = cliente.Estado + " | " + cliente.NumeroPedido + " | "
                        + relojToString(cliente.HoraIngresoLocal) + " | " + horaSalida;

                    //para verificar que el cliente esta agregado a la columna
                    if (i+1 <= dgvClientes.Columns.Count - 1)
                    {
                        dgvClientes.Rows[dgvClientes.Rows.Count - 1].Cells[i+1].Value = datosCliente;
                    }
                    else
                    {
                        //Se agrega la columna ESTADO a la tabla
                        DataGridViewTextBoxColumn columna = new DataGridViewTextBoxColumn();
                        columna.Name = cliente.Nombre;
                        columna.HeaderText = cliente.Nombre;
                        columna.Width = 300;
                        dgvClientes.Columns.Add(columna);
                        dgvClientes.Rows[dgvClientes.Rows.Count - 1].Cells[dgvClientes.Columns.Count - 1].Value = datosCliente;
                    }
                }
                else
                {
                    if (cliente.SeVa && actual.Evento != "fin_simulación")
                    {
                        dgvClientes.Rows[dgvClientes.Rows.Count - 1].Cells[i + 1].Style.BackColor = Color.FromArgb(56, 176, 0);
                        cliente.SeVa = false;
                        continue;
                    }
                    continue;
                }
            }
        }

        private void cargarDatosMetricas()
        {
            int linea = actual.Linea;
            int cantClientesLocal = actual.CantClientes;
            int cantClientesCaja = actual.CantClientesSCaja;
            double tiempoTotalColaCaja = actual.TiempoTotalColaCaja;
            double tiempoPromedioColaCaja = cantClientesCaja > 0 ? Math.Round(tiempoTotalColaCaja / cantClientesCaja,2) : 0;
            double tiempoTotalPermanencia = actual.TiempoTotalPermanencia;
            double tiempoPromedioPermanencia = cantClientesLocal > 0 ? Math.Round(tiempoTotalPermanencia / cantClientesLocal,2) : 0;
            int ocupMaxSR = actual.SalonRojo.OcupMaxima;
            int ocupMaxSA = actual.SalonAzul.OcupMaxima;

            dgvMetricas.Rows.Add(linea, cantClientesLocal, cantClientesCaja, tiempoTotalColaCaja, tiempoPromedioColaCaja,
                                tiempoTotalPermanencia, tiempoPromedioPermanencia, ocupMaxSR, ocupMaxSA);
        }
    }
}
