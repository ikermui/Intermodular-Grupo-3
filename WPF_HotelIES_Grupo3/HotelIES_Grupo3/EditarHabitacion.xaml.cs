using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
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
using HotelIES_Grupo3;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace HotelIES_Grupo3
{
    /// <summary>
    /// Interaction logic for EditarHabitacion.xaml
    /// </summary>
    public partial class EditarHabitacion : Window
    {
        public Habitaciones habitacion;

        public EditarHabitacion(Habitaciones habitacion)
        {
            this.habitacion = habitacion;
            InitializeComponent();
            CargarDatosAsync();
        }
        private void cerrarAnadirHabitacion(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void seleccionarArchivos(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Todos los archivos (*.*)|*.*",
                Title = "Seleccionar archivo"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Obtener la ruta del archivo seleccionado
                    string ruta = openFileDialog.FileName;

                    // Convertir la imagen a Base64
                    string base64 = ConvertirImagenABase64(ruta);


                    // Convertir el Base64 de vuelta a una imagen
                    BitmapImage nuevaImagen = ConvertBase64ToImage(base64);

                    // Encontrar el control de imagen existente
                    Image imagenControl = (Image)this.FindName("imagenControl");
                    if (imagenControl != null)
                    {
                        // Actualizar la imagen en el control
                        imagenControl.Source = nuevaImagen;
                        habitacion.Imagen = base64;
                    }
                    else
                    {
                        MessageBox.Show("No se pudo encontrar el control de imagen.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al seleccionar la imagen: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private string ConvertirImagenABase64(string rutaImagen)
        {
            try
            {
                // Leer el archivo como un arreglo de bytes
                byte[] imagenBytes = File.ReadAllBytes(rutaImagen);

                // Convertir los bytes a una cadena Base64
                return Convert.ToBase64String(imagenBytes);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo convertir la imagen a Base64: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
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

        private async Task CargarDatosAsync()
        {
            StackPanel stackPanel = this.FindName("imagen") as StackPanel;
            stackPanel.Children.Clear();

            TextBlock tituloTextBlock = new TextBlock
            {
                Text = "Seleccionar imagen principal",
                FontSize = 25,
                Margin = new Thickness(0, 0, 0, 15)
            };
            stackPanel.Children.Add(tituloTextBlock);

            // Crear el StackPanel para cada habitación
            DockPanel imagenPanel = new DockPanel();

            // Imagen de la habitación
            BitmapImage imagenBitmap = ConvertBase64ToImage(habitacion.Imagen);

            Image imagen = new Image
            {
                Source = imagenBitmap,
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = 160,
                Height = 160,
                Margin = new Thickness(10, 0, 0, 0),
                Stretch = Stretch.Fill
            };
            imagen.Name = "imagenControl";
            stackPanel.RegisterName(imagen.Name, imagen);

            DockPanel.SetDock(imagen, Dock.Left);
            imagenPanel.Children.Add(imagen);

            Border borde = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1f3b91")),
                BorderThickness = new Thickness(0),
                Width = 170,
                Height = 50,
                CornerRadius = new CornerRadius(25)
            };
            DockPanel.SetDock(borde, Dock.Right);
            imagenPanel.Children.Add(borde);
            Button boton = new Button
            {
                Content = "Cambiar imagen",
                Background = Brushes.Transparent,
                Foreground = Brushes.WhiteSmoke,
                FontWeight = FontWeights.Bold,
                FontSize = 15,
                BorderThickness = new Thickness(0),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            ControlTemplate botonTemplate = new ControlTemplate(typeof(Button));
            FrameworkElementFactory BorderFactory = new FrameworkElementFactory(typeof(Border), "border");

            // Establecer propiedades del Border
            BorderFactory.SetValue(Border.BackgroundProperty, Brushes.Transparent);
            BorderFactory.SetValue(Border.BorderBrushProperty, Brushes.Transparent);
            BorderFactory.SetValue(Border.BorderThicknessProperty, new Thickness(0));

            // Crear un ContentPresenter para mantener el contenido del botón
            FrameworkElementFactory botonContentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            botonContentPresenter.SetValue(ContentPresenter.ContentProperty, new TemplateBindingExtension(Button.ContentProperty));

            // Añadir el ContentPresenter al Border
            BorderFactory.AppendChild(botonContentPresenter);

            // Asignar la estructura visual a la plantilla
            botonTemplate.VisualTree = BorderFactory;
            boton.Template = botonTemplate;

            boton.Click += seleccionarArchivos;
            borde.Child = boton;
            stackPanel.Children.Add(imagenPanel);

            if (habitacion.baja)
            {
                bajaSiRadio.IsChecked = true;
            }
            else
            {
                bajaNoRadio.IsChecked = true;
            }
            nombreTextBox.Text = habitacion.Nombre;
            huespedesComboBox.SelectedIndex = habitacion.Huespedes - 1;
            cunaCheckBox.IsChecked = habitacion.Cuna;
            camaCheckBox.IsChecked = habitacion.CamaExtra;
            precioTextBox.Text = habitacion.Precio + "";
            ofertaTextBox.Text = habitacion.Oferta + "";
            descripcionTextBox.Text = habitacion.Descripcion;
        }

        private void precioTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Validar que solo se permitan números y un punto
            e.Handled = !EsEntradaValidaParaDecimal(e.Text, precioTextBox.Text);
        }

        private void ofertaTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Validar que solo se permitan números y un punto
            e.Handled = !EsEntradaValidaParaDecimal(e.Text, ofertaTextBox.Text);
        }

        private bool EsEntradaValidaParaDecimal(string nuevaEntrada, string textoActual)
        {
            // Comprobar si el texto actual más la nueva entrada sigue siendo un número válido
            string textoCompleto = textoActual + nuevaEntrada;
            return double.TryParse(textoCompleto, out _) && textoCompleto.Count(c => c == '.') <= 1;
        }

        private async void btnModificarHabitacion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Capturar los valores de los TextBox
                string nombre = nombreTextBox.Text.Trim();
                string descripcion = descripcionTextBox.Text.Trim();
                int huespedes = huespedesComboBox.SelectedIndex + 1;

                // Validar y convertir el precio
                if (!double.TryParse(precioTextBox.Text.Trim(), out double precio))
                {
                    MessageBox.Show("Por favor, ingresa un precio válido.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(ofertaTextBox.Text.Trim(), out double oferta))
                {
                    MessageBox.Show("Por favor, ingresa un precio válido.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                // Puedes modificar según tu lógica de descuentos
                bool cuna = cunaCheckBox.IsChecked ?? false;
                bool cama = camaCheckBox.IsChecked ?? false;
                bool baja = false;

                // Validar que los campos no estén vacíos
                if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(descripcion))
                {
                    MessageBox.Show("Por favor, completa todos los campos requeridos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (bajaSiRadio.IsChecked == true)
                {
                    baja = true;
                }
                else if (bajaNoRadio.IsChecked == true)
                {
                    baja = false;
                }

                // Crear el objeto que enviaremos
                var habitacionModificado = new
                {
                    _id = habitacion.Id_hab,
                    nombre = nombre,
                    huespedes = huespedes,
                    descripcion = descripcion,
                    precio = precio,
                    oferta = oferta,
                    cuna = cuna,
                    baja = baja,
                    camaExtra = cama,
                    imagen = habitacion.Imagen
                };
                // Llamar a la función que realiza la solicitud HTTP
                await Habitaciones.ModificarHabitacion(habitacionModificado);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

