using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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

    public partial class BuscarHabitacion : Window
    {
        Usuarios cliUsu;
        public BuscarHabitacion(Usuarios cliUsu)
        {
            InitializeComponent();
            cargarHabitacionesIni();
            cunaCheckBox.IsChecked = null;
            camaCheckBox.IsChecked = null;
            this.cliUsu = cliUsu;
            CargarUsuario();
        }

        private async Task CargarUsuario()
        {
            if (cliUsu != null)
            {
                usuario.Text = $"Bienvenido {cliUsu.nombre}";
            }
            else
            {
                usuario.Text = "No se encontró el usuario.";
            }
        }

        public async void cargarHabitacionesIni()
        {
            LoadHabitacionesAsync(await Habitaciones.ObtenerHabitacionesAsync());
        }

        

        private BitmapImage ConvertBase64ToImage(string base64String)
        {
            // Convertir el string Base64 a un arreglo de bytes
            byte[] imageBytes = Convert.FromBase64String(base64String);

            // Crear un MemoryStream con los bytes de la imagen
            using (var stream = new MemoryStream(imageBytes))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                return bitmap;
            }
        }

        private async Task LoadHabitacionesAsync(List<Habitaciones> habitaciones)
        {
            //  List<Habitacion> habitaciones = await ObtenerHabitacionesAsync();
            // Obtener el WrapPanel del ScrollViewer
            WrapPanel wrapPanel = this.FindName("WrapPanelResultados") as WrapPanel;
            wrapPanel.Children.Clear();

            foreach (var habitacion in habitaciones)
            {
                // Crear el StackPanel para cada habitación
                StackPanel habitacionPanel = new StackPanel
                {
                    Width = 250,
                    Margin = new Thickness(0, 0, 40, 30)
                };

                // Imagen de la habitación
                BitmapImage imagenBitmap = ConvertBase64ToImage(habitacion.Imagen);

                Image imagen = new Image
                {
                    Source = imagenBitmap,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                habitacionPanel.Children.Add(imagen);

                // Nombre de la habitación
                TextBlock nombreTextBlock = new TextBlock
                {
                    Text = habitacion.Nombre,
                    FontSize = 25,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 5, 0, 5)
                };
                habitacionPanel.Children.Add(nombreTextBlock);

                // Precio dentro de un DockPanel
                DockPanel precioDockPanel = new DockPanel
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    LastChildFill = false,
                    Margin = new Thickness(0, 0, 0, 10)
                };

                if (habitacion.Precio > habitacion.Oferta)
                {
                    TextBlock precioTextBlock = new TextBlock
                    {
                        Text = $"{habitacion.Precio}€",
                        FontSize = 15,
                        TextDecorations = TextDecorations.Strikethrough
                    };
                    DockPanel.SetDock(precioTextBlock, Dock.Left);
                    precioDockPanel.Children.Add(precioTextBlock);
                }

                TextBlock ofertaTextBlock = new TextBlock
                {
                    Text = $"{habitacion.Oferta}€",
                    FontSize = 15,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(7, 0, 0, 0)
                };
                DockPanel.SetDock(ofertaTextBlock, Dock.Left);
                precioDockPanel.Children.Add(ofertaTextBlock);

                TextBlock porNocheTextBlock = new TextBlock
                {
                    Text = "/ noche",
                    FontSize = 15,
                    Margin = new Thickness(5, 0, 0, 0)
                };
                DockPanel.SetDock(porNocheTextBlock, Dock.Right);
                precioDockPanel.Children.Add(porNocheTextBlock);

                habitacionPanel.Children.Add(precioDockPanel);

                // Botones de Reservar y Editar
                Border borderContenedor = new Border
                {
                    CornerRadius = new CornerRadius(25),
                    BorderThickness = new Thickness(0),
                    BorderBrush = Brushes.Transparent,
                    Background = Brushes.Transparent,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                LinearGradientBrush gradientBrush = new LinearGradientBrush();
                gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(159, 169, 194), 0.5));   // Azul al inicio
                gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(243, 108, 108), 0.5));

                borderContenedor.Background = gradientBrush;

                DockPanel botonesDockPanel = new DockPanel
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                Button editarButton = new Button
                {
                    Content = new Image
                    {
                        Source = new BitmapImage(new Uri("/editar.png", UriKind.Relative)),
                        Width = 35
                    },
                    Width = 100,
                    Height = 50,
                    Background = Brushes.Transparent,
                    Margin = new Thickness(0, 5, 0, 5),
                    BorderThickness = new Thickness(0)
                };

                ControlTemplate editarTemplate = new ControlTemplate(typeof(Button));
                FrameworkElementFactory editarBorderFactory = new FrameworkElementFactory(typeof(Border), "border");

                // Establecer propiedades del Border
                editarBorderFactory.SetValue(Border.BackgroundProperty, Brushes.Transparent);
                editarBorderFactory.SetValue(Border.BorderBrushProperty, Brushes.Transparent);
                editarBorderFactory.SetValue(Border.BorderThicknessProperty, new Thickness(0));

                // Crear un ContentPresenter para mantener el contenido del botón
                FrameworkElementFactory editarContentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
                editarContentPresenter.SetValue(ContentPresenter.ContentProperty, new TemplateBindingExtension(Button.ContentProperty));

                // Añadir el ContentPresenter al Border
                editarBorderFactory.AppendChild(editarContentPresenter);

                // Asignar la estructura visual a la plantilla
                editarTemplate.VisualTree = editarBorderFactory;
                editarButton.Template = editarTemplate;



                editarButton.Click += (sender, e) => editarHabitacion(sender, e, habitacion);
                botonesDockPanel.Children.Add(editarButton);

                Button eliminarButton = new Button
                {
                    Content = new Image
                    {
                        Source = new BitmapImage(new Uri("/papelera.png", UriKind.Relative)),
                        Width = 40
                    },
                    Width = 100,
                    Height = 50,
                    Background = Brushes.Transparent,
                    Margin = new Thickness(0, 5, 0, 5),
                    BorderThickness = new Thickness(0),
                    Focusable = false
                };

                ControlTemplate template = new ControlTemplate(typeof(Button));
                FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border), "border");

                // Establecer propiedades del Border
                borderFactory.SetValue(Border.BackgroundProperty, Brushes.Transparent);
                borderFactory.SetValue(Border.BorderBrushProperty, Brushes.Transparent);
                borderFactory.SetValue(Border.BorderThicknessProperty, new Thickness(0));

                // Crear un ContentPresenter para mantener el contenido del botón
                FrameworkElementFactory contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
                contentPresenter.SetValue(ContentPresenter.ContentProperty, new TemplateBindingExtension(Button.ContentProperty));

                // Añadir el ContentPresenter al Border
                borderFactory.AppendChild(contentPresenter);

                // Asignar la estructura visual a la plantilla
                template.VisualTree = borderFactory;
                eliminarButton.Template = template;

                botonesDockPanel.Children.Add(eliminarButton);
                eliminarButton.Click += (sender, e) => eliminarHabitacion(habitacion);
                borderContenedor.Child = botonesDockPanel;


                habitacionPanel.Children.Add(borderContenedor);

                wrapPanel.Children.Add(habitacionPanel);
            }
        }

        private void anadirHabitacion(object sender, RoutedEventArgs e)
        {
            AnadirHabitacion anadirHabitacion = new AnadirHabitacion();
            anadirHabitacion.Show();
        }

        private void editarHabitacion(object sender, RoutedEventArgs e, Habitaciones habitacion)
        {
            EditarHabitacion editarHabitacion = new EditarHabitacion(habitacion);
            editarHabitacion.Show();
        }

        private async void eliminarHabitacion(Habitaciones habitacion)
        {
            MessageBoxResult resultado = MessageBox.Show(
                "¿Estás seguro de que deseas eliminar esta habitación?",
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            MessageBox.Show(habitacion.Id_hab + " | ");

            if (resultado == MessageBoxResult.Yes)
            {
                if (await Habitaciones.eliminarHabitacion(habitacion))
                {
                    recargarHabitaciones();
                }
                
            }
        }

        private async void recargarHabitaciones(object sender = null, RoutedEventArgs e = null)
        {
            _ = LoadHabitacionesAsync(await Habitaciones.ObtenerHabitacionesAsync());
            huespedesComboBox.SelectedIndex = -1;

            precioSlider.LowerValue = precioSlider.Minimum;
            precioSlider.HigherValue = precioSlider.Maximum;

            cunaCheckBox.IsChecked = null;
            camaCheckBox.IsChecked = null;
            bajaSiRadio.IsChecked = false;
            bajaNoRadio.IsChecked = false;
        }

        private void CheckBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            // Evita que el CheckBox cambie automáticamente
            e.Handled = true;

            // Ciclo manual de estados: null → true → false → null
            if (checkBox.IsChecked == true)
            {
                checkBox.IsChecked = false; // Pasa a false si estaba en true
            }
            else if (checkBox.IsChecked == false)
            {
                checkBox.IsChecked = null; // Pasa a null si estaba en false
            }
            else
            {
                checkBox.IsChecked = true; // Pasa a true si estaba en null
            }
        }

        private async void filtrarHabitaciones(object sender, RoutedEventArgs e)
        {
            

            int? huespedes = (huespedesComboBox.SelectedIndex != -1) ? huespedesComboBox.SelectedIndex + 1 : (int?)null;
            double precioMin = precioSlider.LowerValue;
            double precioMax = precioSlider.HigherValue;

            // Manejo de CheckBox con tres estados
            bool? cuna = cunaCheckBox.IsChecked;
            bool? cama = camaCheckBox.IsChecked;
            bool? baja = bajaSiRadio.IsChecked == true ? true : bajaNoRadio.IsChecked == true ? false : (bool?)null;

            // Validar que al menos un filtro esté seleccionado
            if (huespedes == null && precioMin == 0 && precioMax == 1000 && cuna == null && cama == null && baja == null)
            {
                MessageBox.Show("Error: Selecciona cualquier filtro para filtrar según sus preferencias", "Excepción", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Crear el objeto de filtros, solo enviamos valores si no son nulos
            var filtros = new Dictionary<string, object>();

            if (huespedes.HasValue) filtros["huespedes"] = huespedes.Value;
            if (precioMin > 0 || precioMax < 1000)
            {
                filtros["ofertaMin"] = precioMin;
                filtros["ofertaMax"] = precioMax;
            }
            if (cama.HasValue) filtros["camaExtra"] = cama.Value;
            if (cuna.HasValue) filtros["cuna"] = cuna.Value;
            if (baja.HasValue) filtros["baja"] = baja.Value;


            List<Habitaciones> habitaciones = await Habitaciones.obtenerFiltradosAsync(filtros);
            if(habitaciones != null && habitaciones.Count > 0)
            {
                LoadHabitacionesAsync(habitaciones);

            } else
            {
                WrapPanel wrapPanel = this.FindName("WrapPanelResultados") as WrapPanel;
                wrapPanel.Children.Clear();
                MessageBox.Show("Ninguna habitación cumple con los filtros establecidos", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BuscarReserva br = new BuscarReserva(cliUsu);
            br.Show();
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            BuscadorUsuarios bu = new BuscadorUsuarios(cliUsu);
            bu.Show();
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            BuscarHabitacion bh = new BuscarHabitacion(cliUsu);
            bh.Show();
            Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ListaReservas lr = new ListaReservas(cliUsu);
            lr.Show();
            Close();
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
    }
}