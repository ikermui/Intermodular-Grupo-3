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
    /// <summary>
    /// Interaction logic for BuscarReserva.xaml
    /// </summary>
    public partial class BuscarReserva : Window
    {

        String fechaIni = "";
        String fechaFin = "";
        int huespedesNum = 0;
        bool? tieneCuna = null;
        bool? tieneCamaExtra = null;
        Usuarios cliUsu;

        public BuscarReserva(Usuarios cliUsu)
        {
            InitializeComponent();
            this.cliUsu = cliUsu;
            fecha_inicio.DisplayDateStart = DateTime.Today;
            fecha_final.DisplayDateStart = DateTime.Today;
            fecha_inicio.DisplayDateEnd = DateTime.Today.AddYears(1);
            fecha_final.DisplayDateEnd = DateTime.Today.AddYears(1);
            //cunaCheckBox.IsChecked = null;
            //camaCheckBox.IsChecked = null;
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

        public async Task<Usuarios?> ObtenerUsuario()
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
                    return JsonConvert.DeserializeObject<Usuarios>(responseJson);
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

        public async void cargarHabitacionesIni()
        {
            LoadHabitacionesAsync(await ObtenerHabitacionesAsync());
        }

        public async Task<List<Habitaciones>> ObtenerHabitacionesAsync()
        {
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:3000/habitaciones/getAll");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Habitaciones>>(json);
            }

            return new List<Habitaciones>();
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

        private void LoadHabitacionesAsync(List<Habitaciones> habitaciones)
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

                if (habitacion.Precio != habitacion.Oferta)
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

                Button buscarButton = new Button
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(10),
                    Tag = habitacion // Guardamos la habitación en el Tag para identificarla en el evento
                };

                // Definir la plantilla personalizada
                ControlTemplate buttonTemplate = new ControlTemplate(typeof(Button));

                // Crear el Border con esquinas redondeadas y fondo azul oscuro
                FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));
                borderFactory.SetValue(Border.CornerRadiusProperty, new CornerRadius(22));
                borderFactory.SetValue(Border.BackgroundProperty, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1f3b91")));

                // Crear el TextBlock con el texto, color y alineación adecuados
                FrameworkElementFactory textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
                textBlockFactory.SetValue(TextBlock.TextProperty, "Reservar Habitación");
                textBlockFactory.SetValue(TextBlock.MarginProperty, new Thickness(25, 10, 25, 10));
                textBlockFactory.SetValue(TextBlock.ForegroundProperty, Brushes.WhiteSmoke);
                textBlockFactory.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
                textBlockFactory.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                textBlockFactory.SetValue(TextBlock.FontSizeProperty, Convert.ToDouble(18));

                // Agregar el TextBlock dentro del Border
                borderFactory.AppendChild(textBlockFactory);

                // Asignar la plantilla al botón
                buttonTemplate.VisualTree = borderFactory;
                buscarButton.Template = buttonTemplate;

                buscarButton.Click += abrirCrearReservas;

                habitacionPanel.Children.Add(buscarButton);


                wrapPanel.Children.Add(habitacionPanel);
            }
        }

        private void abrirCrearReservas(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Tag is Habitaciones habitacion)
            {
                CrearReservas cr = new CrearReservas(habitacion, null, fechaIni, fechaFin, huespedesNum);
                cr.Show();
            }
        }

        private void anadirHabitacion(object sender, RoutedEventArgs e)
        {
           
        }

        private void editarHabitacion(object sender, RoutedEventArgs e, Habitaciones habitacion)
        {
            
        }

        private async void eliminarHabitacion(Habitaciones habitacion)
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
                    var jsonBody = JsonConvert.SerializeObject(new { _id = habitacion.Id_hab });
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
            //LoadHabitacionesAsync(await ObtenerHabitacionesAsync());
            huespedesComboBox.SelectedIndex = -1;
            fecha_inicio.Text = "";
            fecha_final.Text = "";
            cunaCheckBox.IsChecked = null;
            camaCheckBox.IsChecked = null;
            /*precioSlider.LowerValue = precioSlider.Minimum;
            precioSlider.HigherValue = precioSlider.Maximum;*/

            //cunaCheckBox.IsChecked = null;
            //camaCheckBox.IsChecked = null;
            /*bajaSiRadio.IsChecked = false;
            bajaNoRadio.IsChecked = false;*/
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
            if(fecha_inicio.Text == "" || fecha_final.Text == "" || huespedesComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Faltan datos importantes por rellenar", "Rellena los campos");
            } else
            {
                try
                {
                    DateTime fechaInicio, fechaFinal;

                    if (DateTime.TryParse(fecha_inicio.Text, out fechaInicio) && DateTime.TryParse(fecha_final.Text, out fechaFinal))
                    {
                        if (fechaInicio > fechaFinal)
                        {
                            MessageBox.Show("La fecha de inicio no puede ser posterior a la fecha de finalización", "Error de fechas");
                        }
                        else
                        {
                            tieneCamaExtra = camaCheckBox.IsChecked;
                            tieneCuna = cunaCheckBox.IsChecked;
                            huespedesNum = huespedesComboBox.SelectedIndex + 1;
                            fechaIni = fecha_inicio.Text;
                            fechaFin = fecha_final.Text;
                            List<Habitaciones> habitacionesLibres = new List<Habitaciones>();
                            List<Reservas> reservasOcupadas = await Reservas.GetReservaBuscador("", "", "", fechaIni, fechaFin, 0);
                            if (reservasOcupadas != null)
                            {
                                List<String> habitacionesOcupadas = new List<String>();
                                foreach (Reservas res in reservasOcupadas)
                                {
                                    habitacionesOcupadas.Add(res.Id_hab);
                                }
                                habitacionesOcupadas = habitacionesOcupadas.Distinct().ToList();
                                String ocupado = "";
                                foreach (String str in habitacionesOcupadas)
                                {
                                    ocupado += str + "\n";
                                }
                                List<Habitaciones> habitaciones = await Habitaciones.ObtenerHabitacionesBuscador(huespedesNum, tieneCuna, tieneCamaExtra);
                                habitacionesLibres = new List<Habitaciones>();
                                foreach (Habitaciones hab in habitaciones)
                                {
                                    if (!ocupado.Contains(hab.Id_hab))
                                    {
                                        habitacionesLibres.Add(hab);
                                    }
                                }
                            }
                            else
                            {
                                habitacionesLibres = await Habitaciones.ObtenerHabitacionesBuscador(huespedesComboBox.SelectedIndex + 1, cunaCheckBox.IsChecked, camaCheckBox.IsChecked);
                            }
                            if (habitacionesLibres.Count > 0)
                            {
                                LoadHabitacionesAsync(habitacionesLibres);
                            }
                            else
                            {
                                WrapPanel wrapPanel = this.FindName("WrapPanelResultados") as WrapPanel;
                                wrapPanel.Children.Clear();
                                MessageBox.Show("Ninguna habitación cumple con los filtros establecidos", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                    }
                } catch (Exception ex)
                {
                    MessageBox.Show("No se pudo realizar la busqueda, " + ex.Message, "ERROR");
                }
               
            }
            
            /*using HttpClient client = new HttpClient();

            int? huespedes = (huespedesComboBox.SelectedIndex != -1) ? huespedesComboBox.SelectedIndex + 1 : (int?)null;
            double precioMin = precioSlider.LowerValue;
            //double precioMax = precioSlider.HigherValue;

            // Manejo de CheckBox con tres estados
            bool? cuna = cunaCheckBox.IsChecked;
            bool? cama = camaCheckBox.IsChecked;
            //bool? baja = bajaSiRadio.IsChecked == true ? true : bajaNoRadio.IsChecked == true ? false : (bool?)null;

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
                List<Habitaciones> habitaciones = JsonConvert.DeserializeObject<List<Habitaciones>>(responseJson);
                LoadHabitacionesAsync(habitaciones);
            }
            else
            {
                WrapPanel wrapPanel = this.FindName("WrapPanelResultados") as WrapPanel;
                wrapPanel.Children.Clear();
                MessageBox.Show("Ninguna habitación cumple con los filtros establecidos", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }*/
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BuscarReserva br = new BuscarReserva(cliUsu);
            br.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            BuscadorUsuarios bu = new BuscadorUsuarios(cliUsu);
            bu.Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ListaReservas lr = new ListaReservas(cliUsu);
            lr.Show();
            this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
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

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            BuscarHabitacion bh = new BuscarHabitacion(cliUsu);
            bh.Show();
            this.Close();
        }
    }
}

