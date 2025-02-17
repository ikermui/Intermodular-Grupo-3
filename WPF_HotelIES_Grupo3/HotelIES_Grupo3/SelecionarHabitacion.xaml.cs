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
    /// Interaction logic for SelecionarHabitacion.xaml
    /// </summary>
    public partial class SelecionarHabitacion : Window
    {
        List<HabLista> listaObjetos;
        CrearReservas cr;
        ListaReservas lr;
        EditarReserva er;
        public SelecionarHabitacion(CrearReservas cr, ListaReservas lr, EditarReserva er)
        {
            InitializeComponent();
            obtenerHabitaciones();
            this.cr = cr;
            this.lr = lr;
            this.er = er;
        }

        public async void obtenerHabitaciones()
        {
            List<Habitaciones> habitaciones = await Habitaciones.ObtenerHabitacionesAsync();
            listaObjetos = new List<HabLista>();
            foreach (Habitaciones hab in habitaciones)
            {
                listaObjetos.Add(new HabLista(MainWindow.convertirImagen(hab.Imagen), hab.Id_hab + " - " + hab.Nombre));
            }
            seleccionHabitacion.ItemsSource = listaObjetos;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cr != null)
            {
                if (seleccionHabitacion.SelectedIndex == -1)
                {
                    MessageBox.Show("Tienes que seleccionar una habitacion primero", "ERROR");
                }
                else
                {
                    try
                    {
                        Habitaciones habit = await Habitaciones.ObtenerUnoAsync(listaObjetos[seleccionHabitacion.SelectedIndex].Name.Split("-")[0].Trim());
                        cr.actualizarCampos(habit);
                        cr.hab = habit;
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("No se pudo cargar la habitacion: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else if (lr != null)
            {
                if (seleccionHabitacion.SelectedIndex == -1)
                {
                    MessageBox.Show("Tienes que seleccionar una habitacion primero", "ERROR");
                }
                else
                {
                    try
                    {
                        Habitaciones hab = await Habitaciones.ObtenerUnoAsync(listaObjetos[seleccionHabitacion.SelectedIndex].Name.Split("-")[0].Trim());
                        lr.actualizaHabitacion(hab);
                        this.Close();
                    } catch (Exception ex) {
                        MessageBox.Show("No se pudo cargar la habitacion: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    
                }
            }
            else if (er != null)
            {
                if (seleccionHabitacion.SelectedIndex == -1)
                {
                    MessageBox.Show("Tienes que seleccionar una habitacion primero", "ERROR");
                }
                else
                {
                    try
                    {
                        Habitaciones hab = await Habitaciones.ObtenerUnoAsync(listaObjetos[seleccionHabitacion.SelectedIndex].Name.Split("-")[0].Trim());
                        er.actualizarCampos(hab);
                        this.Close();
                    }
                    catch (Exception ex) {
                        MessageBox.Show("No se pudo cargar la habitacion: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    
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
}

public class HabLista
{
    public HabLista(BitmapImage image, string name)
    {
        Image = image;
        Name = name;
    }

    public BitmapImage Image { get; set; }

    public string Name { get; set; }
}
