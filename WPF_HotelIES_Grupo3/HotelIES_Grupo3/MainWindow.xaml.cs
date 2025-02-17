using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotelIES_Grupo3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();
        public static List<Usuarios> listaClientes = new List<Usuarios>();
        public static List<Habitaciones> listaHabitaciones = new List<Habitaciones>();
        public static List<Reservas> listaReservas =  new List<Reservas>();
        public MainWindow()
        {
            InitializeComponent();
            LoginView lw = new LoginView();
            lw.Show();
            this.Close();
        }

        public static BitmapImage convertirImagen(string base64String)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                using (var ms = new MemoryStream(imageBytes))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                    bitmap.Freeze();
                    return bitmap;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR Al cargar imagen");
                return null;
            }
        }

        public static Usuarios encuentraCli(String dni)
        {
            foreach (Usuarios cli in listaClientes)
            {
                if(cli.dni == dni)
                {
                    return cli;
                }
            }
            return null;
        }

        public static Habitaciones encuentraHab(String id)
        {
            foreach (Habitaciones hab in listaHabitaciones)
            {
                if (hab.Id_hab == id)
                {
                    return hab;
                }
            }
            return null;
        }

        public static Boolean compruebaFecha(String fecha1, String fecha2) {
        DateTime f1, f2;
            bool esValida1 = DateTime.TryParseExact(fecha1, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out f1);
            bool esValida2 = DateTime.TryParseExact(fecha2, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out f2);

            if (!esValida1 || !esValida2)
            {
                MessageBox.Show("Las fechas no estan en un formato correcto");
                return false;
            }

            return f1 <= f2;
        }

    private async void Button_Click(object sender, RoutedEventArgs e)
        {
            CrearReservas cr = new CrearReservas(await Habitaciones.ObtenerUnoAsync("H002"), await Usuarios.ObtenerUnoAsync("12345678B"), "", "", 0);
            cr.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ListaReservas lr = new ListaReservas(null);
            lr.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            BuscarReserva br = new BuscarReserva(null);
            br.Show();
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Reservas reservas = await Reservas.GetReserva(_idTextBox.Text);
            if (reservas != null) {
                listaReservas.Add(reservas);
            } else
            {
                MessageBox.Show("No se pudo obtener la reserva, inténtalo otra vez", "ERROR");
            }
           String ca = "";
            foreach (Reservas res in listaReservas) { 
                ca = ca + "\n" + res.toString();
            }
            MessageBox.Show(ca);
        }

        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            int numHuespedes = 0;

            if(numHuespedesTextBox.Text != "")
            {
                numHuespedes = Convert.ToInt32(numHuespedesTextBox.Text);
            }

            List<Reservas> reservas = await Reservas.GetReservaFiltro(_idTextBox.Text, dniTextBox.Text, idTextBox.Text, fecha_iniTextBox.Text, fecha_finTextBox.Text, numHuespedes);
            String devolver = "";
            if (reservas == null)
            {
                MessageBox.Show("No se encontro ninguna reserva con esos filtros", "No encontrado");
            }
            else
            {
                foreach (Reservas res in reservas)
                {
                    devolver += "\n" + res.toString() + "\n";
                }
                MessageBox.Show(devolver);
            }
        }

        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            List<Reservas> reservas = await Reservas.GetAllReservas();

            String devolver = "";
            if (reservas == null)
            {
                MessageBox.Show("No se encontro ninguna reserva con esos filtros", "No encontrado");
            }
            else
            {
                foreach (Reservas res in reservas)
                {
                    devolver += "\n" + res.toString() + "\n";
                }
                MessageBox.Show(devolver);
            }
        }

        private async void Button_Click_6(object sender, RoutedEventArgs e)
        {
            int numHuespedes = 0;
            if (numHuespedesTextBox.Text != "")
            {
                numHuespedes = Convert.ToInt32(numHuespedesTextBox.Text);
            }
            Boolean update = await Reservas.updateReserva(_idTextBox.Text, dniTextBox.Text, idTextBox.Text, fecha_iniTextBox.Text, fecha_finTextBox.Text, numHuespedes);
            String devolver = "";
            
            if (update)
            {
                 MessageBox.Show("Se ha actualizado correctamente la reserva", "EXITO");
            } else
            {
                 MessageBox.Show("No se pudo realizar la actualizacion", "ERROR");
            }
        }

        private async void Button_Click_7(object sender, RoutedEventArgs e)
        {
            Boolean delete = await Reservas.deleteReserva(_idTextBox.Text);
            if (delete)
            {
                MessageBox.Show("Se ha eliminado correctamente la reserva", "EXITO");
            } else
            {
                MessageBox.Show("No se pudo eliminar la reserva", "ERROR");
            }
        }
    }
}