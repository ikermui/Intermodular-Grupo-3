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
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using System.Text.RegularExpressions;

namespace HotelIES_Grupo3
{
    /// <summary>
    /// Lógica de interacción para AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        private string base64Imagen;


        public AddUser()
        {
            InitializeComponent();
        }

        private async void BtnAñadir_Click(object sender, RoutedEventArgs e)
        {
            // Obtener valores de los controles

            //dni
            string dniAdd = txtDni.Text.Trim();

            // Validar el DNI.
            /* if (!ValidarDni(dniAdd))
             {
                 MessageBox.Show("El DNI no tiene un formato correcto.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                 return;
             } */

            //nombre y apellido
            string nombreAdd = txtNombre.Text.Trim();
            string apellidoAdd = txtApellido.Text.Trim();

            // Validar que el nombre y el apellido solo contengan letras y espacios.
            if (!ValidarNombreYApellido(nombreAdd, apellidoAdd))
            {
                MessageBox.Show("El nombre y el apellido solo pueden contener letras y espacios.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //email
            string emailAdd = txtEmail.Text.Trim();

            if (!ValidarEmail(emailAdd))
            {
                MessageBox.Show("El correo electrónico no tiene un formato válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            string passwordAdd = txtPassword.Password.Trim();
            string ciudadAdd = txtCiudad.Text.Trim();

            //fecha nacimiento
            string fechaAdd = txtFecha_nac.SelectedDate?.ToString("d/M/yyyy") ?? "";

            if (!ValidarFechaNacimiento(fechaAdd))
            {
                MessageBox.Show("La fecha de nacimiento no es válida o no es coherente.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            // Validar selección de rol
            string valorRol = "";
            if (rolSeleccionado.SelectedItem is ComboBoxItem rolSelect)
            {
                valorRol = rolSelect.Content.ToString();
            }

            // Validar selección de sexo
            string sexoAdd = hombre.IsChecked == true ? "Hombre" :
                             mujer.IsChecked == true ? "Mujer" :
                             indeterminado.IsChecked == true ? "Indeterminado" : "";

            // Validar que todos los campos obligatorios estén llenos
            if (string.IsNullOrWhiteSpace(dniAdd) ||
                string.IsNullOrWhiteSpace(nombreAdd) ||
                string.IsNullOrWhiteSpace(apellidoAdd) ||
                string.IsNullOrWhiteSpace(valorRol) ||
                string.IsNullOrWhiteSpace(emailAdd) ||
                string.IsNullOrWhiteSpace(passwordAdd) ||
                string.IsNullOrWhiteSpace(fechaAdd) ||
                string.IsNullOrWhiteSpace(ciudadAdd) ||
                string.IsNullOrWhiteSpace(sexoAdd))
            {
                MessageBox.Show("Todos los campos son obligatorios, excepto la imagen.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //logica API
            // Crear objeto de tipo Clientes con los datos obtenidos, asignando "" a imagen si no se envía
            Usuarios nuevoUsuario = new Usuarios()
            {
                dni = dniAdd,
                nombre = nombreAdd,
                apellido = apellidoAdd,
                rol = valorRol,
                email = emailAdd,
                contrasena = passwordAdd,
                fecha_nac = fechaAdd,
                ciudad = ciudadAdd,
                sexo = sexoAdd,
                imagen = base64Imagen

            };

            // Llamar a la función que interactúa con la API para crear el nuevo usuario
            MessageBox.Show(dniAdd, nombreAdd + apellidoAdd + valorRol + emailAdd + passwordAdd + fechaAdd + ciudadAdd + sexoAdd + base64Imagen);
            Usuarios resultado = await Usuarios.CrearNuevoAsync(nuevoUsuario);
            if (resultado != null)
            {
                MessageBox.Show("Usuario creado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Error al crear el usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        /*Convertir imagen a base64*/

        private void Button_SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Archivos de imagen (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|Todos los archivos (*.*)|*.*";

            if (dlg.ShowDialog() == true)
            {
                // Obtén la ruta del archivo seleccionado
                string filePath = dlg.FileName;

                // Leer todos los bytes del archivo
                byte[] imageBytes = File.ReadAllBytes(filePath);

                // Convertir los bytes a una cadena en Base64 y almacenarla en la variable
                base64Imagen = Convert.ToBase64String(imageBytes);

                // Opcional: actualizar la vista previa de la imagen en el control Image
                BitmapImage bitmap = new BitmapImage(new Uri(filePath));
                imgPreview.Source = bitmap;

            }
        }

        /*validacion DNI*/

        private bool ValidarDni(string dni)
        {
            // Comprobar que no sea nulo o vacío.
            if (string.IsNullOrWhiteSpace(dni))
                return false;

            // Verificar que cumpla el formato: 8 dígitos seguidos de una letra.
            // Ejemplo: 12345678Z
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^\d{8}[A-Za-z]$");
            if (!regex.IsMatch(dni))
                return false;

            // Extra: validar la letra de control.
            // Las letras válidas en orden para el cálculo.
            string letrasValidas = "TRWAGMYFPDXBNJZSQVHLCKE";

            // Convertir los primeros 8 caracteres a número.
            if (!int.TryParse(dni.Substring(0, 8), out int numero))
                return false;

            // Calcular la letra que corresponde.
            char letraCalculada = letrasValidas[numero % 23];

            // Comparar la letra calculada con la letra proporcionada (sin diferenciar mayúsculas/minúsculas).
            char letraDni = char.ToUpper(dni[8]);

            return letraCalculada == letraDni;
        }

        /*validacion nombre*/

        private bool ValidarNombreYApellido(string nombre, string apellido)
        {
            // Verifica que ninguno de los campos esté vacío o compuesto solo de espacios.
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido))
                return false;

            // Expresión regular que permite únicamente letras (de cualquier idioma) y espacios.
            Regex regex = new Regex(@"^[\p{L}\s]+$");

            // Valida que ambos campos cumplan la condición.
            return regex.IsMatch(nombre) && regex.IsMatch(apellido);
        }


        /*validación correo*/
        private bool ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Expresión regular para validar correos electrónicos
            Regex regex = new Regex(@"^[a-zA-Z0-9._%+-]+@(gmail|hotmail|outlook|yahoo|icloud|live)\.(com|org|net|es|edu|gov)$");

            return regex.IsMatch(email);
        }

        /*validacion fecha de nacimiento*/
        private bool ValidarFechaNacimiento(string fechaNacimiento)
        {
            if (string.IsNullOrWhiteSpace(fechaNacimiento))
                return false;

            // Intentar convertir la cadena a una fecha válida
            if (!DateTime.TryParseExact(fechaNacimiento, "d/M/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime fecha))
                return false;

            // Definir un rango lógico para la fecha de nacimiento
            DateTime fechaMinima = new DateTime(1900, 1, 1); // No nacimientos antes de 1900
            DateTime fechaMaxima = DateTime.Today.AddYears(-18); // Mínimo 18 años de edad

            return fecha >= fechaMinima && fecha <= fechaMaxima;
        }


        /*validacion ciudad*/
        private bool ValidarCiudadEspaña(string ciudad)
        {
            if (string.IsNullOrWhiteSpace(ciudad))
                return false;
            //StringComparer.OrdinalIgnoreCase permite que la comparacion no distinga entre mayusculas y minusculas
            HashSet<string> ciudadesEspaña = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Madrid", "Barcelona", "Valencia", "Sevilla", "Zaragoza", "Málaga", "Murcia", "Palma", "Las Palmas", "Bilbao",
                "Alicante", "Córdoba", "Valladolid", "Vigo", "Gijón", "L'Hospitalet de Llobregat", "A Coruña", "Vitoria-Gasteiz",
                "Granada", "Elche", "Oviedo", "Santa Cruz de Tenerife", "Badalona", "Cartagena", "Terrassa", "Jerez de la Frontera",
                "Sabadell", "Móstoles", "Alcalá de Henares", "Pamplona", "Fuenlabrada", "Almería", "Leganés", "San Sebastián",
                "Burgos", "Santander", "Castellón de la Plana", "Getafe", "Albacete", "Alcorcón", "Logroño", "Badajoz", "Salamanca",
                "Huelva", "Lleida", "Marbella", "Tarragona", "León", "Cádiz", "Jaén", "Orense", "Gerona", "Lugo"
            };

            return ciudadesEspaña.Contains(ciudad);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
