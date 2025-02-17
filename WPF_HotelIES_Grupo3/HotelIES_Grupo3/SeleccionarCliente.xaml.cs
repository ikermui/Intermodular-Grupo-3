using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
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
    /// Interaction logic for SeleccionarCliente.xaml
    /// </summary>
    public partial class SeleccionarCliente : Window
    {
        ListaReservas lr;
        CrearReservas cr;
        EditarReserva er;
        List<CliLista> listaObjetos;
        public SeleccionarCliente(ListaReservas lr, CrearReservas cr, EditarReserva er)
        {
            this.lr = lr;
            this.cr = cr;
            this.er = er;
            listaObjetos = new List<CliLista>();
            InitializeComponent();
            rellenaLista();       
        }

        private async void rellenaLista()
        {
            List<Usuarios> clientes = await Usuarios.ObtenerTodosAsync();
            foreach (Usuarios cli in clientes)
            {
                listaObjetos.Add(new CliLista(MainWindow.convertirImagen(cli.imagen), cli.dni + " - " + cli.nombre + " " + cli.apellido));
            }
            seleccionCliente.ItemsSource = listaObjetos;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (seleccionCliente.SelectedIndex == -1)
            {
                MessageBox.Show("Tienes que seleccionar un cliente", "ERROR");
            } else
            {
                String dniCli = listaObjetos[seleccionCliente.SelectedIndex].Name.Split("-")[0];

                Usuarios cli = await Usuarios.ObtenerUnoAsync(dniCli.Trim());
                if (cr != null)
                {
                    cr.actualizaUsuario(cli);
                    this.Close();
                }
                if(lr != null)
                {
                    lr.actualizaUsuario(cli);
                    this.Close();
                }
                if(er != null)
                {
                    er.actualizaUsuario(cli);
                    this.Close();
                }
            }
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
    }

    public class CliLista
    {
        public CliLista(BitmapImage image, string name)
        {
            Image = image;
            Name = name;
        }

        public BitmapImage Image { get; set; }

        public string Name { get; set; }
    }

}
