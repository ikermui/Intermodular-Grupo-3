using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelIES_Grupo3
{
    /// <summary>
    /// Interaction logic for EditarReserva.xaml
    /// </summary>
    public partial class EditarReserva : Window
    {
        String idOriginal;
        Habitaciones hab;
        Usuarios cli;
        Reservas reserva;
        int diasTotal;
        public EditarReserva(Reservas reserva)
        {
            InitializeComponent();
            this.reserva = reserva;
            idOriginal = reserva.Id_hab;
            fecha_inicio.DisplayDateStart = DateTime.Today;
            fecha_final.DisplayDateStart = DateTime.Today;
            fecha_inicio.DisplayDateEnd = DateTime.Today.AddYears(1);
            fecha_final.DisplayDateEnd = DateTime.Today.AddYears(1);
            idRes.Text = reserva.ID;
            rellenaDatos();
        }

        public async void rellenaDatos()
        {
            Habitaciones hab = await Habitaciones.ObtenerUnoAsync(reserva.Id_hab);
            this.hab = hab;
            String[] fechasIni = reserva.Fecha_ini.Split("T")[0].Split("-");
            String fechaIniConvertida = fechasIni[2] + "/" + fechasIni[1] + "/" + fechasIni[0];
            String[] fechasFin = reserva.Fecha_fin.Split("T")[0].Split("-");
            String fechaFinConvertida = fechasFin[2] + "/" + fechasFin[1] + "/" + fechasFin[0];
            fecha_inicio.Text = fechaIniConvertida;
            fecha_final.Text = fechaFinConvertida;
            actualizarCampos(hab);
            Usuarios cli = await Usuarios.ObtenerUnoAsync(reserva.Dni);
            this.cli = cli;
            actualizaUsuario(cli);
        }

        public void actualizaHuespedes(int numHuespedes)
        {
            comboHuespedes.Items.Clear();
            for (int i = 1; i < numHuespedes + 1; i++)
            {
                comboHuespedes.Items.Add(i);
            }
        }

        public async void bloquearFechas()
        {
            HashSet<DateTime> fechasOcupadas = await listaOcupadas();
            fecha_inicio.BlackoutDates.Clear();
            fecha_final.BlackoutDates.Clear();

            String[] fechasIni = reserva.Fecha_ini.Split("T")[0].Split("-");
            String fechaIniConvertida = fechasIni[2] + "/" + fechasIni[1] + "/" + fechasIni[0];
            String[] fechasFin = reserva.Fecha_fin.Split("T")[0].Split("-");
            String fechaFinConvertida = fechasFin[2] + "/" + fechasFin[1] + "/" + fechasFin[0];

            /*DateTime fInicio = DateTime.Parse(fechaIniConvertida);
            DateTime fFinal = DateTime.Parse(fechaFinConvertida);
            List<DateTime> diasReservadosEditar = new List<DateTime>();
            for(DateTime fecha = fInicio; fecha <= fFinal; fecha = fecha.AddDays(1))
            {
                try
                {
                    diasReservadosEditar.Add(fecha);
                } catch (Exception ex)
                {
                    MessageBox.Show("se");
                }
                
            }*/
            foreach (DateTime fecha in fechasOcupadas)
            {
                try
                {
                        fecha_inicio.BlackoutDates.Add(new CalendarDateRange(fecha));
                }
                catch (Exception ex)
                {
                    try
                    {
                        fecha_inicio.Text = "";
                        MessageBox.Show("Las fecha de inicio no es valida porque interfiere con una fecha ocupada", "Se ha quitado la fecha de inicio", MessageBoxButton.OK, MessageBoxImage.Information);
                        fecha_inicio.BlackoutDates.Add(new CalendarDateRange(fecha));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Ha habido un error con la fecha: " + fecha.ToString() + ": " + e.Message);
                    }
                }
                try
                {
                        fecha_final.BlackoutDates.Add(new CalendarDateRange(fecha));   
                }
                catch (Exception ex)
                {
                    try
                    {
                        fecha_final.Text = "";
                        MessageBox.Show("Las fecha de final no es valida porque interfiere con una fecha ocupada", "Se ha quitado la fecha de final", MessageBoxButton.OK, MessageBoxImage.Information);
                        fecha_final.BlackoutDates.Add(new CalendarDateRange(fecha));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Ha habido un error con la fecha: " + fecha.ToString() + ": " + e.Message);
                    }

                }

            }
        }
        public void actualizarCampos(Habitaciones h)
        {
            this.hab = h;
            
            comboHuespedes.SelectedItem = reserva.NumHuespedes;
            
            idHabitacion.Text = h.Id_hab;
            nomHab.Text = h.Nombre;
            imagenHab.Source = MainWindow.convertirImagen(h.Imagen);
            precioTxt.Text = Math.Round(h.Oferta * diasTotal, 2) + " €";
            actualizaHuespedes(h.Huespedes);
            bloquearFechas();
        }

        public void actualizaUsuario(Usuarios cli)
        {
            this.cli = cli;
            datosUsu.Text = cli.nombre + " " + cli.apellido + " - " + cli.email + " - " + cli.dni;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void fechaSelectedDateChanged(object sender, RoutedEventArgs e)
        {
            if (fecha_inicio.Text != "" && fecha_final.Text != "")
            {
                diasTotal = Reservas.calcularPrecioTotal(fecha_inicio.Text, fecha_final.Text, hab.Oferta);
                if (diasTotal == -1)
                {
                    MessageBox.Show("La fecha final no puede ser anterior a la fecha inicial", "Marca otra fecha");
                    precioTxt.Text = Math.Round(hab.Oferta, 2) + " €";
                }
                else
                {
                    precioTxt.Text = Math.Round(diasTotal * hab.Oferta, 2) + " €";
                }

            }
        }

        public async Task<HashSet<DateTime>> listaOcupadas()
        {
            List<Reservas> reservas = await Reservas.GetReservaBuscador("", "", hab.Id_hab, "", "", 0);
            MessageBox.Show(this.hab.Id_hab + " | " + idOriginal);
            List<DateTime> diasReservadosEditar = new List<DateTime>();
            if (reservas == null)
            {
                reservas = new List<Reservas>();
            }
            if (idOriginal == hab.Id_hab)
            {
                String[] fechasIni = reserva.Fecha_ini.Split("T")[0].Split("-");
                String fechaIniConvertida = fechasIni[2] + "/" + fechasIni[1] + "/" + fechasIni[0];
                String[] fechasFin = reserva.Fecha_fin.Split("T")[0].Split("-");
                String fechaFinConvertida = fechasFin[2] + "/" + fechasFin[1] + "/" + fechasFin[0];

                DateTime fInicio = DateTime.Parse(fechaIniConvertida);
                DateTime fFinal = DateTime.Parse(fechaFinConvertida);
                
                for(DateTime fecha = fInicio; fecha <= fFinal; fecha = fecha.AddDays(1))
                {
                    try
                    {
                        diasReservadosEditar.Add(fecha);
                    } catch (Exception ex)
                    {
                        Console.WriteLine("ERROR con la fecha: " + fecha.ToString());
                    }

                }
            }
            HashSet<DateTime> fechasOcupadas = new HashSet<DateTime>();
            foreach (Reservas reserva in reservas)
            {
                try
                {
                    String[] formatoInicio = reserva.Fecha_ini.Split("T")[0].Split("-");
                    String fechadeinicio = formatoInicio[2] + "/" + formatoInicio[1] + "/" + formatoInicio[0];
                    String[] formatoFinal = reserva.Fecha_fin.Split("T")[0].Split("-");
                    String fechadefinal = formatoFinal[2] + "/" + formatoFinal[1] + "/" + formatoFinal[0];
                    DateTime fechaInicio = DateTime.Parse(fechadeinicio);
                    DateTime fechaFin = DateTime.Parse(fechadefinal);
                    for (DateTime fecha = fechaInicio; fecha <= fechaFin; fecha = fecha.AddDays(1))
                    {
                        if (!diasReservadosEditar.Contains(fecha))
                        {
                            fechasOcupadas.Add(fecha);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            return fechasOcupadas;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (cli == null || hab == null || fecha_inicio.Text == "" || fecha_final.Text == "" || Convert.ToInt32(comboHuespedes.SelectedItem) == 0)
            {
                MessageBox.Show("Tienes que rellenar todos los campos", "ERROR");
            }
            else
            {
                Boolean ocupado = false;
                String fecha_ini = fecha_inicio.Text;
                String fecha_fin = fecha_final.Text;
                if (MainWindow.compruebaFecha(fecha_ini, fecha_fin))
                {
                    DateTime fInicio = DateTime.Parse(fecha_ini);
                    DateTime fFin = DateTime.Parse(fecha_fin);
                    HashSet<DateTime> diasOcupados = await listaOcupadas();
                    for (DateTime fecha = fInicio; fecha <= fFin; fecha = fecha.AddDays(1))
                    {
                        if (diasOcupados.Contains(fecha))
                        {
                            ocupado = true;
                        }
                    }
                    if (ocupado)
                    {
                        MessageBox.Show("Las fechas interfieren con otra reserva", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        String id_hab = idHabitacion.Text;
                        String dni = cli.dni;
                        int numHuespedes = Convert.ToInt32(comboHuespedes.SelectedItem);
                        await Reservas.updateReserva(reserva.ID, dni, id_hab, fecha_ini, fecha_fin, numHuespedes);
                        MessageBox.Show("Se ha editado la reserva correctamente", "EXITO");
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("La fecha final no puede ser anterior a la fecha inicial");
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SelecionarHabitacion sh = new SelecionarHabitacion(null, null, this);
            sh.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SeleccionarCliente sc = new SeleccionarCliente(null, null, this);
            sc.Show();
        }
    }
}
