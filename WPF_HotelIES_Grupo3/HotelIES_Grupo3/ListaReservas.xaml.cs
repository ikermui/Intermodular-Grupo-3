using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public partial class ListaReservas : Window
    {

        List<Reservas> listaReservas;
        Usuarios cliUsu;
        public ListaReservas(Usuarios cliUsu)
        {
            this.cliUsu = cliUsu;
            listaReservas = new List<Reservas>();
            InitializeComponent();
            obtenerReservas();
        }

        private async void obtenerReservas()
        {
            List<ReservaItem> reservaItems = new List<ReservaItem>();
            try
            {
                listaReservas = await Reservas.GetAllReservas();
                foreach (Reservas res in listaReservas)
                {
                    try
                    {
                        Usuarios c = await Usuarios.ObtenerUnoAsync(res.Dni);
                        Habitaciones h = await Habitaciones.ObtenerUnoAsync(res.Id_hab);
                        String[] fechasIni = res.Fecha_ini.Split("T")[0].Split("-");
                        String fechaIniConvertida = fechasIni[2] + "/" + fechasIni[1] + "/" + fechasIni[0];
                        String[] fechasFin = res.Fecha_fin.Split("T")[0].Split("-");
                        String fechaFinConvertida = fechasFin[2] + "/" + fechasFin[1] + "/" + fechasFin[0];
                        reservaItems.Add(new ReservaItem(res.ID, MainWindow.convertirImagen(c.imagen), c.dni, c.nombre + " " + c.apellido, MainWindow.convertirImagen(h.Imagen), h.Id_hab, h.Nombre, fechaIniConvertida, fechaFinConvertida, res.NumHuespedes));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Se ha saltado la reserva: " + res.ID + "\nMotivo: " + ex.Message);
                    }
                }
                reservasList.ItemsSource = reservaItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron cargar los datos " + ex.Message);
            }
            
        }

        private async void obtenerReservasFiltro(String id, String dni, String id_hab, String fecha_ini, String fecha_fin, int numHuespedes)
        {
            List<ReservaItem> reservaItems = new List<ReservaItem>();
            try
            {
                listaReservas = await Reservas.GetReservaFiltroDate(id, dni, id_hab, fecha_ini, fecha_fin, numHuespedes);
                foreach (Reservas res in listaReservas)
                {
                    try
                    {
                        Usuarios c = await Usuarios.ObtenerUnoAsync(res.Dni);
                        Habitaciones h = await Habitaciones.ObtenerUnoAsync(res.Id_hab);
                        String[] fechasIni = res.Fecha_ini.Split("T")[0].Split("-");
                        String fechaIniConvertida = fechasIni[2] + "/" + fechasIni[1] + "/" + fechasIni[0];
                        String[] fechasFin = res.Fecha_fin.Split("T")[0].Split("-");
                        String fechaFinConvertida = fechasFin[2] + "/" + fechasFin[1] + "/" + fechasFin[0];
                        reservaItems.Add(new ReservaItem(res.ID, MainWindow.convertirImagen(c.imagen), c.dni, c.nombre + " " + c.apellido, MainWindow.convertirImagen(h.Imagen), h.Id_hab, h.Nombre, fechaIniConvertida, fechaFinConvertida, res.NumHuespedes));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Se ha saltado la reserva: " + res.ID + "\nMotivo: " + ex.Message);
                    }
                }
                reservasList.ItemsSource = reservaItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No hay reservas que cumplan esos filtros", "No hay reservas", MessageBoxButton.OK, MessageBoxImage.Warning);
                reservasList.ItemsSource = new List<ReservaItem>();
            }

        }

        public void actualizaUsuario(Usuarios cli)
        {
            if (cli != null)
            {
                cliDatos.Text = cli.dni + " - " + cli.nombre + " " + cli.apellido;
            }
        }
        public void actualizaHabitacion(Habitaciones hab)
        {
            if (hab != null)
            {
                habDatos.Text = hab.Id_hab + " - " + hab.Nombre;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CrearReservas cr = new CrearReservas(null, null, "", "", 0);
            cr.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SeleccionarCliente sc = new SeleccionarCliente(this, null, null);
            sc.Show();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (scrollViewer != null)
            {
                if (e.Delta > 0)
                    scrollViewer.LineUp();
                else
                    scrollViewer.LineDown();

                e.Handled = true;
            }
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if(reservasList.SelectedIndex == -1)
            {
                MessageBox.Show("No hay ninguna reserva seleccionada", "ERROR");
            } else
            {
                try
                {
                    ReservaItem ri = reservasList.SelectedItem as ReservaItem;
                    EditarReserva er = new EditarReserva(await Reservas.GetReserva(ri.ID));
                    er.Show();
                } catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if(reservasList.SelectedIndex == -1)
            {
                MessageBox.Show("No hay ninguna reserva seleccionada", "ERROR");
            } else
            {
                MessageBoxResult resultado = MessageBox.Show("¿Seguro que quieres eliminar la reserva?", "Eliminar Reserva", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (resultado == MessageBoxResult.Yes)
                {
                    ReservaItem ri = reservasList.SelectedItem as ReservaItem;
                    await Reservas.deleteReserva(ri.ID);
                    MessageBox.Show("Se ha eliminado la reserva", "Reserva Eliminada");
                    obtenerReservas();
                }
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            SelecionarHabitacion sh = new SelecionarHabitacion(null, this, null);
            sh.Show();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            finalText.Text = "";
            inicioText.Text = "";
            cliDatos.Text = "Ningun cliente seleccionado";
            habDatos.Text = "Ninguna habitacion seleccionada";
            huespedesText.SelectedIndex = -1;
            obtenerReservas();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            // String id, String dni, String id_hab, String fecha_ini, String fecha_fin, int numHuespedes
            String dni;
            String id_hab;
            String fecha_ini;
            String fecha_fin;
            int numHuespedes;

            String[] datosCliente = cliDatos.Text.Replace(" ", "").Split("-");
            String[] datosHabitacion = habDatos.Text.Replace(" ", "").Split("-");
            if (datosCliente.Length != 2)
            {
                dni = "";
            } else
            {
                dni = datosCliente[0];
            }
            if (datosHabitacion.Length != 2)
            {
                id_hab = "";
            }
            else
            {
                id_hab = datosHabitacion[0];
            }

            fecha_ini = inicioText.Text;
            fecha_fin = finalText.Text;

            if (huespedesText.SelectedIndex == -1)
            {
                numHuespedes = 0;
            } else
            {
                numHuespedes = huespedesText.SelectedIndex + 1;
            }
            MessageBox.Show(id_hab);
            obtenerReservasFiltro("", dni, id_hab, fecha_ini, fecha_fin, numHuespedes);
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            BuscarReserva br = new BuscarReserva(cliUsu);
            br.Show();
            this.Close();
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            BuscadorUsuarios bu = new BuscadorUsuarios(cliUsu);
            bu.Show();
            this.Close();
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            ListaReservas lr = new ListaReservas(cliUsu);
            lr.Show();
            this.Close();
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            LoginView login = new LoginView();
            login.Show();
            foreach (Window win in Application.Current.Windows)
            {
                if (win != login)
                {
                    win.Close();
                }
            }
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            BuscarHabitacion bh = new BuscarHabitacion(cliUsu);
            bh.Show();
            this.Close();
        }
    }

    class ReservaItem
    {
        public String ID { get; set; }
        public BitmapImage Usuario { get; set; }
        public String Dni { get; set; }
        public String NombreCliente { get; set; }
        public BitmapImage Habitacion { get; set; }
        public String IDHab { get; set; }
        public String NombreHabitacion { get; set; }
        public String FechaInicio { get; set; }
        public String FechaFin { get; set; }
        public int Huespedes { get; set; }

        public String toString()
        {
            return Dni + " " + NombreCliente + " " + ID + " " + FechaInicio + " " + FechaFin + " " + Huespedes;
        }

        public ReservaItem(String id, BitmapImage imgCli, string dni, string nombreCliente, BitmapImage imgHab, string idHab, string nombreHab, string fechaInicio, string fechaFinal, int numHuespedes)
        {
            this.ID = id;
            this.Usuario = imgCli;
            this.Dni = dni;
            this.NombreCliente = nombreCliente;
            this.Habitacion = imgHab;
            this.IDHab = idHab;
            this.NombreHabitacion = nombreHab;
            this.FechaInicio = fechaInicio;
            this.FechaFin = fechaFinal;
            this.Huespedes = numHuespedes;
        }
    }
}
