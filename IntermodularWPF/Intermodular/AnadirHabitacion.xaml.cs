using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Intermodular
{
    /// <summary>
    /// Interaction logic for AnadirHabitacion.xaml
    /// </summary>
    public partial class AnadirHabitacion : Window
    {
        public AnadirHabitacion()
        {
            InitializeComponent();
        }

        private void cerrarAnadirHabitacion(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void seleccionarArchivos(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Todos los archivos (*.*)|*.*",
                Title = "Seleccionar archivo"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Colocar la ruta del archivo en el TextBox
                archivosTextBox.Text = openFileDialog.FileName;
            }
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

        private async void btnCrearHabitacion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Capturar los valores de los TextBox
                string nombre = nombreTextBox.Text.Trim();
                string descripcion = descripcionTextBox.Text.Trim();
                int huespedes = huespedesComboBox.SelectedIndex + 1;
                bool baja = false;

                // Validar y convertir el precio
                if (!double.TryParse(precioTextBox.Text.Trim(), out double precio))
                {
                    MessageBox.Show("Por favor, ingresa un precio válido.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(ofertaTextBox.Text.Trim(), out double oferta))
                {
                    MessageBox.Show("Por favor, ingresa una oferta válido.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                bool cuna = cunaCheckBox.IsChecked ?? false;
                bool cama = camaCheckBox.IsChecked ?? false;
                string urlimagen = archivosTextBox.Text;

                // Validar que los campos no estén vacíos
                if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(descripcion))
                {
                    MessageBox.Show("Por favor, completa todos los campos requeridos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string imagenBase64 = ConvertirImagenABase64(urlimagen);

                if (bajaSiRadio.IsChecked == true)
                {
                    baja = true;
                }
                else if (bajaNoRadio.IsChecked == true)
                {
                    baja = false;
                }
                else {
                    MessageBox.Show("Por favor, completa el campo si está de baja", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                

                // Crear el objeto que enviaremos
                var habitacion = new
                {
                    nombre = nombre,
                    huespedes = huespedes,
                    descripcion = descripcion,
                    precio = precio,
                    oferta = oferta,
                    cuna = cuna,
                    baja = baja,
                    camaExtra = cama,
                    imagen = imagenBase64
                };
                // Llamar a la función que realiza la solicitud HTTP
                await CrearHabitacion(habitacion);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        public async Task CrearHabitacion(object habitacion)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = "http://localhost:3000/habitaciones/new";
                    // Convertir el objeto a JSON
                    string json = JsonConvert.SerializeObject(habitacion);

                    // Crear el contenido de la solicitud
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Realizar la solicitud POST
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    // Comprobar si la respuesta fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Habitación creada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Error al crear la habitación: {responseContent}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocurrió un error al conectar con la API: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
