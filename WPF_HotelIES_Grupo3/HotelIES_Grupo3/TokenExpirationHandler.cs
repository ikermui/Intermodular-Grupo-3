using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HotelIES_Grupo3
{
    public class TokenExpirationHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Realiza la llamada al siguiente handler (o al servicio real)
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            // Si la respuesta es BadRequest (400) se revisa el contenido
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                string content = await response.Content.ReadAsStringAsync();
                // Si el mensaje contiene "Token inválido" (puedes ajustar el texto según lo que devuelva tu API)
                if (!string.IsNullOrEmpty(content) && content.Contains("Token inválido"))
                {
                    // Ejecuta en el hilo de la UI para mostrar el MessageBox y redirigir al login
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Tu sesión ha expirado. Serás redirigido a la pantalla de inicio de sesión.",
                                          "Sesión expirada",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Warning);

                        LoginView login = new LoginView();
                        login.Show();

                        // Crea una copia de las ventanas abiertas para evitar modificar la colección durante la iteración
                        var ventanasAbiertas = Application.Current.Windows.Cast<Window>().ToList();

                        // Cierra todas las ventanas excepto la de login
                        foreach (Window win in ventanasAbiertas)
                        {
                            if (win != login)
                            {
                                win.Close();
                            }
                        }

                    });
                }
            }
            return response;
            
        }
    }
}
