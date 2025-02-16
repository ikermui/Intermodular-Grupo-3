using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Intermodular
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class Habitacion
    {
        [JsonProperty("_id")]
        public string Id { get; set; }
        public string Nombre { get; set; }
        public int Huespedes { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public double Precio { get; set; }
        public double Oferta { get; set; }
        public bool baja { get; set; }
        public string FinOferta { get; set; }
        public bool CamaExtra { get; set; }
        public bool Cuna { get; set; }
    }

    public class Usuario
    {
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Rol { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FechaNacimiento { get; set; }
        public string Ciudad { get; set; }
        public string Sexo { get; set; }
        public string Imagen { get; set; }
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            cargarHabitacionesIni();
            cunaCheckBox.IsChecked = null;
            camaCheckBox.IsChecked = null;
            CargarUsuario();
        }

        private async Task CargarUsuario()
        {
            Usuario? usuarioObtenido = await ObtenerUsuario(); // Esperar la respuesta
            if (usuarioObtenido != null)
            {
                usuario.Text = $"Bienvenido {usuarioObtenido.Nombre}";
            }
            else
            {
                usuario.Text = "No se encontró el usuario.";
            }
        }

        public async Task<Usuario?> ObtenerUsuario()
        {
            using HttpClient client = new HttpClient();

            try
            {
                // Crear JSON con el DNI
                string json = JsonConvert.SerializeObject(new { dni = "87654321A" });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Hacer la petición POST
                HttpResponseMessage response = await client.PostAsync("http://localhost:3000/usuarios/getOne", content);

                // Verificar si la respuesta es exitosa
                if (response.IsSuccessStatusCode)
                {
                    string responseJson = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Usuario>(responseJson);
                }
                else
                {
                    // Log o mensaje en caso de error HTTP
                    Console.WriteLine($"Error en la solicitud: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
            }

            // Retornar null si hubo algún problema
            return null;
        }

        public async void cargarHabitacionesIni() {
            LoadHabitacionesAsync(await ObtenerHabitacionesAsync());
        }

        public async Task<List<Habitacion>> ObtenerHabitacionesAsync()
        {
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:3000/habitaciones/getAll");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Habitacion>>(json);
            }

            return new List<Habitacion>();
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

        private async Task LoadHabitacionesAsync(List<Habitacion> habitaciones)
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

                if (habitacion.Precio > habitacion.Oferta) {
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

        private void editarHabitacion(object sender, RoutedEventArgs e, Habitacion habitacion)
        {
            EditarHabitacion editarHabitacion = new EditarHabitacion(habitacion);
            editarHabitacion.Show();
        }

        private async void eliminarHabitacion(Habitacion habitacion)
        {
            MessageBoxResult resultado = MessageBox.Show(
                "¿Estás seguro de que deseas eliminar esta habitación?",
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (resultado == MessageBoxResult.Yes)
            {
                using HttpClient client = new HttpClient();
                string url = "http://localhost:3000/habitaciones/delete";

                try
                {
                    var jsonBody = JsonConvert.SerializeObject(new { _id = habitacion.Id });
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri(url),
                        Content = content
                    };

                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Habitación eliminada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        recargarHabitaciones();
                    }
                    else
                    {
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Error al eliminar la habitación: {errorResponse}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Excepción", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void recargarHabitaciones(object sender = null, RoutedEventArgs e = null)
        {
            _ = LoadHabitacionesAsync(await ObtenerHabitacionesAsync());
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
            using HttpClient client = new HttpClient();

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

            string json = JsonConvert.SerializeObject(filtros);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            // Hacer la petición POST a la API
            HttpResponseMessage response = await client.PostAsync("http://localhost:3000/habitaciones/getFilter2", content);

            if (response.IsSuccessStatusCode)
            {
                string responseJson = await response.Content.ReadAsStringAsync();
                List<Habitacion> habitaciones = JsonConvert.DeserializeObject<List<Habitacion>>(responseJson);
                LoadHabitacionesAsync(habitaciones);
            }
            else
            {
                WrapPanel wrapPanel = this.FindName("WrapPanelResultados") as WrapPanel;
                wrapPanel.Children.Clear();
                MessageBox.Show("Ninguna habitación cumple con los filtros establecidos", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}