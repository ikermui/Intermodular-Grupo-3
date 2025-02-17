using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace HotelIES_Grupo3
{
    public class Usuarios
    {
        [JsonPropertyName("dni")]
        public String dni { get; set; }
        [JsonPropertyName("nombre")]
        public String nombre { get; set; }
        [JsonPropertyName("apellido")]
        public String apellido { get; set; }
        [JsonPropertyName("rol")]
        public String rol { get; set; }
        [JsonPropertyName("email")]
        public String email { get; set; }
        [JsonPropertyName("contrasena")]
        public String contrasena { get; set; }
        [JsonPropertyName("fecha_nac")]
        public String fecha_nac { get; set; }
        [JsonPropertyName("ciudad")]
        public String ciudad { get; set; }
        [JsonPropertyName("sexo")]
        public String sexo { get; set; }
        [JsonPropertyName("imagen")]
        public String imagen { get; set; }
        public String Token { get; set; }

        public static readonly HttpClient client = new HttpClient(new TokenExpirationHandler()
        {
            InnerHandler = new HttpClientHandler()
        });



        public static async Task<List<Usuarios>> ObtenerTodosAsync()
        {
            try
            {
                
                string apiUrl = "http://localhost:3000/user/getAll";
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();
                List<Usuarios> listaProbar = JsonSerializer.Deserialize<List<Usuarios>>(jsonResponse);
                return listaProbar;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // GET FILTER

        public static async Task<List<Usuarios>> ObtenerFiltradosAsync(string rol, string fechaNacimiento, string ciudad, string sexo)
        {
            try
            {
                string apiUrl = "http://localhost:3000/user/getFilterInter";
                var filtros = new
                {
                    rol = rol,
                    fechaNacimiento = fechaNacimiento,
                    ciudad = ciudad,
                    sexo = sexo
                };

                string body = JsonSerializer.Serialize(filtros);
                var content = new StringContent(body, Encoding.UTF8, "application/json");

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
                {
                    Content = content
                };

                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Usuarios>>(jsonResponse);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // GET ONE: Obtiene un usuario basado en el dni enviado en el body
       /* public static async Task<Clientes> ObtenerUnoAsync(string dni)
        {
            try
            {
                string apiUrl = "http://localhost:3000/user/getOne";
                var body = JsonSerializer.Serialize(new { dni = dni.Trim() });
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Clientes>(jsonResponse);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                return null;
            }
        }*/

        // POST NEW: Crea un nuevo usuario en la API
        public static async Task<Usuarios> CrearNuevoAsync(Usuarios usuario)
        {
            try
            {
                string apiUrl = "http://localhost:3000/user/new";
                string body = JsonSerializer.Serialize(usuario);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Usuarios>(jsonResponse);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                return null;
            }
        }

        // UPDATE: Actualiza un usuario basado en el dni proporcionado
        public static async Task<bool> ActualizarAsync(Usuarios usuario)
        {
            try
            {
                string apiUrl = "http://localhost:3000/user/update";
                string body = JsonSerializer.Serialize(usuario);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PatchAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                return false;
            }
        }

        public static async Task<bool> EliminarAsync(string dni)
        {
            try
            {
                string apiUrl = "http://localhost:3000/user/delete";
                var body = JsonSerializer.Serialize(new { dni = dni });
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, apiUrl)
                {
                    Content = content
                };
                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                return false;
            }
        }

        // GET FILTER: Obtiene usuarios filtrados según el rol proporcionado
        /*public static async Task<List<Clientes>> ObtenerFiltradosAsync(string rol)
        {
            try
            {
                string apiUrl = "http://localhost:3000/user/getFilter";
                var body = JsonSerializer.Serialize(new { rol = rol });
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apiUrl)
                {
                    Content = content
                };
                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Clientes>>(jsonResponse);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                return null;
            }
        }*/

        // GET ONE EMAIL: Obtiene un usuario mediante email y contrasena
        public static async Task<Usuarios> ObtenerPorEmailAsync(string email, string contrasena)
        {
            try
            {
                string apiUrl = "http://localhost:3000/user/getOneEmail";
                var body = JsonSerializer.Serialize(new { email = email, contrasena = contrasena });
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Usuarios>(jsonResponse);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                return null;
            }
        }

        public static async Task<Usuarios> ObtenerUnoAsync(string dni)
        {
            try
            {
                string apiUrl = "http://localhost:3000/user/getOneDni";
                var body = JsonSerializer.Serialize(new { dni = dni});
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Usuarios>(jsonResponse);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public String toString()
        {

            return "DNI: " + dni + "\nNombre: " + nombre + "\nApellido: " + apellido + "\nRol: " + rol + "\nEmail: " + email + "\nContraseña: " + contrasena + "\nFecha de Nacimiento: " + fecha_nac + "\nCiudad: " + ciudad + "\nSexo: " + sexo;
        }

        public Usuarios() { 
        }
        public Usuarios(string dni, string nombre, string apellido, string rol, string email, string contrasena, string fecha_nac, string ciudad, string sexo, string imagen)
        {
            this.dni = dni;
            this.nombre = nombre;
            this.apellido = apellido;
            this.rol = rol;
            this.email = email;
            this.contrasena = contrasena;
            this.fecha_nac = fecha_nac;
            this.ciudad = ciudad;
            this.sexo = sexo;
            this.imagen = imagen;
        }
    }
}
