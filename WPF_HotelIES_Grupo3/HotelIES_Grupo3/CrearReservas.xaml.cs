using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
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
    /// Interaction logic for CrearReservas.xaml
    /// </summary>
    public partial class CrearReservas : Window
    {
        // Datos Iniciales

        public Habitaciones hab { get; set; }
        public Usuarios cli { get; set; }
        int diasTotal;

        public CrearReservas(Habitaciones hab, Usuarios cli, String fechadeInicio, String fechadeFin, int huespedesNum)
        {
            InitializeComponent();
            fecha_inicio.DisplayDateStart = DateTime.Today;
            fecha_final.DisplayDateStart = DateTime.Today;
            fecha_inicio.DisplayDateEnd = DateTime.Today.AddYears(1);
            fecha_final.DisplayDateEnd = DateTime.Today.AddYears(1);
            fecha_inicio.Text = fechadeInicio;
            fecha_final.Text = fechadeFin;
            if(fecha_inicio.Text != "" && fecha_final.Text != "" && hab != null)
            {
                diasTotal = Reservas.calcularPrecioTotal(fechadeInicio, fechadeFin, hab.Oferta);
            } else
            {
                diasTotal = 1;
            }
            this.hab = hab;
            if (hab != null)
            {
                actualizarCampos(hab);
                bloquearFechas();
                if(huespedesNum > 0)
                {
                    comboHuespedes.SelectedIndex = huespedesNum - 1;
                }  
            }
            this.cli = cli;
            if (cli != null)
            {
                actualizaUsuario(cli);
            }
            
        }
        public async void bloquearFechas()
        {
                HashSet<DateTime> fechasOcupadas = await listaOcupadas();
                fecha_inicio.BlackoutDates.Clear();
                fecha_final.BlackoutDates.Clear();
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

        public void actualizaHuespedes(int numHuespedes)
        {
            comboHuespedes.Items.Clear();
            for (int i = 1; i < numHuespedes + 1; i++)
            {  
                comboHuespedes.Items.Add(i);
            }
        }

        public void actualizarCampos(Habitaciones h)
        {
            hab = h;
            idHabitacion.Text = hab.Id_hab;
            nomHab.Text = hab.Nombre;
            imagenHab.Source = MainWindow.convertirImagen(hab.Imagen);
            precioTxt.Text = Math.Round(hab.Oferta * diasTotal, 2) + " €";
            actualizaHuespedes(hab.Huespedes);
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
                if(hab != null)
                {
                    diasTotal = Reservas.calcularPrecioTotal(fecha_inicio.Text, fecha_final.Text, hab.Oferta);
                }
                if(diasTotal == -1)
                {
                    MessageBox.Show("La fecha final no puede ser anterior a la fecha inicial", "Marca otra fecha");
                    if(hab != null)
                    {
                        precioTxt.Text = Math.Round(hab.Oferta, 2) + " €";
                    } else
                    {
                        precioTxt.Text = "N/A €";
                    }
                } else
                {
                    if(hab != null)
                    {
                        precioTxt.Text = Math.Round(diasTotal * hab.Oferta, 2) + " €";
                    }
                }
                
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(cli == null || hab == null || fecha_inicio.Text == "" || fecha_final.Text == "" || Convert.ToInt32(comboHuespedes.SelectedItem) == 0) {
                MessageBox.Show("Tienes que rellenar todos los campos", "ERROR");
            } else
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
                    } else
                    {
                        int numHuespedes = Convert.ToInt32(comboHuespedes.SelectedItem);
                        Reservas reserva = new Reservas("R", cli.dni, hab.Id_hab, fecha_ini, fecha_fin, numHuespedes);
                        Reservas.anadirReserva(reserva);
                        MessageBox.Show("Se ha añadido la reserva correctamente", "EXITO");
                        this.Close();
                    }
                } else
                {
                    MessageBox.Show("La fecha final no puede ser anterior a la fecha inicial");
                }
            } 
        }


        public async Task<HashSet<DateTime>> listaOcupadas()
        {

                List<Reservas> reservas = await Reservas.GetReservaBuscador("", "", hab.Id_hab, "", "", 0);
                HashSet<DateTime> fechasOcupadas = new HashSet<DateTime>();
                if(reservas == null)
                {
                    reservas = new List<Reservas>();
                }
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
                            fechasOcupadas.Add(fecha);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
                return fechasOcupadas; 
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SelecionarHabitacion sh = new SelecionarHabitacion(this, null, null);
            sh.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SeleccionarCliente sc = new SeleccionarCliente(null, this, null);
            sc.Show();
        }
    }
}
