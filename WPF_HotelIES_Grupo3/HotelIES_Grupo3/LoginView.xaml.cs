using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
    /// Lógica de interacción para LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private async void BtnIniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            TextBox txtEmail = FindName("txtEmail") as TextBox;
            PasswordBox txtPassword = FindName("txtPassword") as PasswordBox;

            if (txtEmail != null && txtPassword != null)
            {
                string email = txtEmail.Text;
                string password = txtPassword.Password;

                // Se verifica el login y se obtiene el usuario (con el token asignado)
                Usuarios cliente = await VerificarCredenciales(email, password);

                if (cliente != null)
                {
                    // Verificar el rol del usuario
                    if (cliente.rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase) ||
                        cliente.rol.Equals("Empleado", StringComparison.OrdinalIgnoreCase))
                    {
                        // Mostrar token junto al mensaje de bienvenida
                        MessageBox.Show($"Bienvenido {cliente.nombre}",
                            "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Pasar el objeto cliente (con token) a la ventana BuscadorUsuarios
                        //BuscadorUsuarios buscador = new BuscadorUsuarios(cliente);
                        //buscador.Show();
                        BuscarReserva br = new BuscarReserva(cliente);
                        br.Show();
                        this.Close();
                    }
                    else if (cliente.rol.Equals("Cliente", StringComparison.OrdinalIgnoreCase))
                    {
                        // Mostrar mensaje de acceso denegado para rol Cliente
                        MessageBox.Show("No puedes acceder",
                            "Acceso denegado", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        // Para otros roles que no tengan acceso
                        MessageBox.Show("Tu rol no tiene acceso a esta ventana",
                            "Acceso denegado", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Email o contraseña incorrectos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No se han encontrado los controles de entrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
        }
        private async Task<Usuarios> VerificarCredenciales(string email, string password)
        {
            try
            {
                // 1. Llamar a /auth/login para obtener el token
                var loginRequestBody = new { email = email, contrasena = password };
                string loginJson = JsonSerializer.Serialize(loginRequestBody);
                var loginContent = new StringContent(loginJson, Encoding.UTF8, "application/json");

                HttpResponseMessage loginResponse = await Usuarios.client.PostAsync("http://localhost:3000/auth/login", loginContent);
                if (!loginResponse.IsSuccessStatusCode)
                {
                    MessageBox.Show("Credenciales inválidas", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                // Aquí se deserializa la respuesta del login, que contiene el token
                string loginResponseJson = await loginResponse.Content.ReadAsStringAsync();
                LoginResponse loginResult = JsonSerializer.Deserialize<LoginResponse>(
                    loginResponseJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                // Asignar el token en la cabecera para futuras peticiones
                Usuarios.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.Token);
                Reservas.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.Token);
                Habitaciones.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.Token);

                // 2. Llamar a /user/getOneEmail para obtener los datos completos del usuario
                // Ahora se envía solo el email, ya que la contraseña se validó en /auth/login
                var userRequestBody = new { email = email };
                string userJson = JsonSerializer.Serialize(userRequestBody);
                var userContent = new StringContent(userJson, Encoding.UTF8, "application/json");

                HttpResponseMessage userResponse = await Usuarios.client.PostAsync("http://localhost:3000/user/getOneEmail", userContent);
                if (!userResponse.IsSuccessStatusCode)
                {
                    MessageBox.Show("No se encontró ningún usuario con esos datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                // Deserializa el usuario completo
                string userResponseJson = await userResponse.Content.ReadAsStringAsync();
                Usuarios cliente = JsonSerializer.Deserialize<Usuarios>(
                    userResponseJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                // Asignar el token obtenido al objeto cliente
                cliente.Token = loginResult.Token;
                return cliente;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la conexión: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }
}
