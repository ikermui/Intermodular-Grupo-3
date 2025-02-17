using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Lógica de interacción para UpdateUser.xaml
    /// </summary>
    public partial class UpdateUser : Window
    {
        private Usuarios usuario;
        private string base64Imagen;  // Variable para almacenar la imagen en Base64

        public UpdateUser(Usuarios cliente)
        {
            InitializeComponent();
            usuario = cliente;  // Se recibe el objeto completo (incluido el token)
            CargarDatos();
        }

        private void CargarDatos()
        {
            // Asigna los valores a los controles existentes en XAML.
            txtDni.Text = usuario.dni;
            txtNombre.Text = usuario.nombre;
            txtApellido.Text = usuario.apellido;
            txtEmail.Text = usuario.email;

            // Convertir la fecha (si es válida) y asignarla al DatePicker.
            if (DateTime.TryParse(usuario.fecha_nac, out DateTime fecha))
                txtFecha_nac.SelectedDate = fecha;
            else
                txtFecha_nac.SelectedDate = null;

            txtCiudad.Text = usuario.ciudad;

            // Seleccionar el rol en el ComboBox "rolSeleccionado"
            foreach (ComboBoxItem item in rolSeleccionado.Items)
            {
                if (item.Content.ToString().Trim().Equals(usuario.rol, StringComparison.OrdinalIgnoreCase))
                {
                    rolSeleccionado.SelectedItem = item;
                    break;
                }
            }

            // Seleccionar el RadioButton correspondiente al sexo
            if (usuario.sexo.Equals("Hombre", StringComparison.OrdinalIgnoreCase))
                hombre.IsChecked = true;
            else if (usuario.sexo.Equals("Mujer", StringComparison.OrdinalIgnoreCase))
                mujer.IsChecked = true;
            else if (usuario.sexo.Equals("Indeterminado", StringComparison.OrdinalIgnoreCase))
                indeterminado.IsChecked = true;

            // Asigna la contraseña al PasswordBox "txtPassword"
            txtPassword.Password = usuario.contrasena;

            // Si existe imagen (en Base64), cargarla en el control Image y asignarla a la variable base64Imagen.
            if (!string.IsNullOrEmpty(usuario.imagen))
            {
                try
                {
                    byte[] imageBytes = Convert.FromBase64String(usuario.imagen);
                    BitmapImage bitmap = new BitmapImage();
                    using (MemoryStream stream = new MemoryStream(imageBytes))
                    {
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = stream;
                        bitmap.EndInit();
                    }
                    imgPreview.Source = bitmap;
                    base64Imagen = usuario.imagen;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar la imagen: " + ex.Message);
                }
            }
        }

        private void Button_SelectImage_Click(object sender, RoutedEventArgs e)
        {
            // Permite seleccionar una imagen y la convierte a Base64.
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Archivos de imagen (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|Todos los archivos (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                string filePath = dlg.FileName;
                byte[] imageBytes = File.ReadAllBytes(filePath);
                base64Imagen = Convert.ToBase64String(imageBytes);
                BitmapImage bitmap = new BitmapImage(new Uri(filePath));
                imgPreview.Source = bitmap;
            }
        }

        // MÉTODOS DE VALIDACIÓN

        private bool ValidarNombre(string nombre)
        {
            // Permite solo letras (mayúsculas o minúsculas) y espacios.
            return Regex.IsMatch(nombre.Trim(), @"^[A-Za-z\s]+$");
        }

        private bool ValidarApellido(string apellido)
        {
            // Permite solo letras y espacios.
            return Regex.IsMatch(apellido.Trim(), @"^[A-Za-z\s]+$");
        }

        private bool ValidarEmail(string email)
        {
            // Ejemplo de regex que acepta dominios comunes y extensiones .com, .es, .org, etc.
            return Regex.IsMatch(email.Trim(), @"^[a-zA-Z0-9._%+-]+@(gmail|hotmail|outlook|yahoo|icloud|live)\.(com|es|org|net|edu|gov)$", RegexOptions.IgnoreCase);
        }

        private bool ValidarFecha(DateTime? fecha)
        {
            // La fecha debe ser válida y no ser posterior a la fecha actual.
            if (!fecha.HasValue)
                return false;

            DateTime hoy = DateTime.Today;
            return fecha.Value.Date <= hoy;
        }

        // Manejador para el botón "Actualizar"
        private async void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            // Validaciones:
            if (!ValidarNombre(txtNombre.Text))
            {
                MessageBox.Show("El nombre solo debe contener letras y espacios.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!ValidarApellido(txtApellido.Text))
            {
                MessageBox.Show("El apellido solo debe contener letras y espacios.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!ValidarEmail(txtEmail.Text))
            {
                MessageBox.Show("El formato del email es inválido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!ValidarFecha(txtFecha_nac.SelectedDate))
            {
                MessageBox.Show("La fecha de nacimiento es inválida o posterior a la fecha actual.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Construir un objeto Clientes con los datos actuales de los controles.
            Usuarios updatedUser = new Usuarios()
            {
                dni = txtDni.Text.Trim(),  // Este campo no se edita, ya que está deshabilitado.
                nombre = txtNombre.Text.Trim(),
                apellido = txtApellido.Text.Trim(),
                email = txtEmail.Text.Trim(),
                contrasena = txtPassword.Password.Trim(),
                // Formatear la fecha según "d/M/yyyy"
                fecha_nac = txtFecha_nac.SelectedDate?.ToString("d/M/yyyy") ?? "",
                ciudad = txtCiudad.Text.Trim(),
                // Obtener el rol del ComboBox (rolSeleccionado)
                rol = (rolSeleccionado.SelectedItem as ComboBoxItem)?.Content.ToString().Trim() ?? "",
                // Determinar el sexo según el RadioButton marcado
                sexo = (hombre.IsChecked == true) ? "Hombre" : (mujer.IsChecked == true) ? "Mujer" : "Indeterminado",
                // La imagen en Base64
                imagen = base64Imagen,
                // Se conserva el token recibido en el objeto original
                Token = usuario.Token
            };

            // Llamar a la función de actualización en la API.
            bool result = await Usuarios.ActualizarAsync(updatedUser);
            if (result)
            {
                MessageBox.Show("Usuario actualizado exitosamente.");
                this.Close();
            }
            else
            {
                MessageBox.Show("Error al actualizar el usuario.");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
