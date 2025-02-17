using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotelIES_Grupo3
{
    public class Habitaciones
    {
        public String _id { get; set; }
        public String nombre { get; set; }
        public int huespedes { get; set; }
        public String descripcion { get; set; }
        public String imagen { get; set; } 
        public Double precio { get; set; }
        public Double oferta { get; set; }
        public String finOferta { get; set; }
        public bool baja { get; set; }
        public bool camaExtra { get; set; }
        public bool cuna { get; set; }


        public static readonly HttpClient client = new HttpClient(new TokenExpirationHandler()
        {
            InnerHandler = new HttpClientHandler()
        });
        public Habitaciones() { }
        public static async Task CrearHabitacion(object habitacion)
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


        public static async Task<List<Habitaciones>> ObtenerHabitacionesAsync()
        {
            HttpResponseMessage response = await client.GetAsync("http://localhost:3000/habitaciones/getAll");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Habitaciones>>(json);
            }

            return new List<Habitaciones>();
        }

        public static async Task ModificarHabitacion(object habitacionModificado)
        {
            
                try
                {
                    string url = "http://localhost:3000/habitaciones/update";
                    // Convertir el objeto a JSON
                    string json = JsonConvert.SerializeObject(habitacionModificado);

                    // Crear el contenido de la solicitud
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Realizar la solicitud POST
                    HttpResponseMessage response = await client.PatchAsync(url, content);

                    // Comprobar si la respuesta fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Habitación modificada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Error al modificar la habitación: {responseContent}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocurrió un error al conectar con la API: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            
        }

        public static async Task<Habitaciones> ObtenerUnoAsync(string _id)
        {
            try
            {
                string apiUrl = "http://localhost:3000/habitaciones/getOne";
                var body = JsonConvert.SerializeObject(new { _id = _id });
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Habitaciones>(jsonResponse);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                return null;
            }
        }

        public static async Task<List<Habitaciones>> ObtenerHabitacionesBuscador(int huespedes, Boolean? cuna, Boolean? camaExtra)
        {
            String apiUrl = "http://localhost:3000/habitaciones/getFilterHuespedes?";
            if (huespedes != 0)
            {
                apiUrl += "huespedes=" + huespedes + "&";
            }
            if(cuna != null)
            {
                apiUrl += "cuna=" + cuna.ToString().ToLower() + "&";
            }
            if(camaExtra != null)
            {
                apiUrl += "camaExtra=" + camaExtra.ToString().ToLower() + "&";
            }
            MessageBox.Show(apiUrl);
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Habitaciones>>(json);
            }

            return new List<Habitaciones>();
        }


        public static async Task<List<Habitaciones>> obtenerFiltradosAsync(Dictionary<string, object> filtros)
        {
            string json = JsonConvert.SerializeObject(filtros);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                // Hacer la petición POST a la API
                HttpResponseMessage response = await client.PostAsync("http://localhost:3000/habitaciones/getFilter2", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseJson = await response.Content.ReadAsStringAsync();
                    List<Habitaciones> habitaciones = JsonConvert.DeserializeObject<List<Habitaciones>>(responseJson);
                    return habitaciones;
                }
                else
                {
                    MessageBox.Show(response.ToString());
                    return null;
                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
                return null;
            }
            
        }

        public static async Task<Boolean> eliminarHabitacion(Habitaciones habitacion)
        {
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
                MessageBox.Show(jsonBody.ToString());
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Habitación eliminada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    return true;
                }
                else
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al eliminar la habitación: {errorResponse}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Excepción", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }


        public String toString()
        {
            return "ID: " + _id + "\nNombre: " + nombre + "\nHuespedes: " + huespedes + "\nDescripcion: " + descripcion + "\nPrecio: " + precio + "\nOferta: " + oferta + "\nCama Extra: " + camaExtra + "\nTiene Cuna: " + cuna;
        }
       
        public string Id_hab
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public int Huespedes
        {
            get { return huespedes; }
            set { huespedes = value; }
        }

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        public string Imagen
        {
            get { return imagen; }
            set { imagen = value; }
        }

        public double Precio
        {
            get { return precio; }
            set { precio = value; }
        }

        public double Oferta
        {
            get { return oferta; }
            set { oferta = value; }
        }

        public bool CamaExtra
        {
            get { return camaExtra; }
            set { camaExtra = value; }
        }

        public bool Cuna
        {
            get { return cuna; }
            set { cuna = value; }
        }

        public String FinOferta
        {
            get { return finOferta; }
            set { finOferta = value; }
        }

        public Habitaciones(string _id, string nombre, int huespedes, string descripcion, string imagen, double precio, double oferta, bool camaExtra, bool cuna)
        {
            this._id = _id;
            this.nombre = nombre;
            this.huespedes = huespedes;
            this.descripcion = descripcion;
            this.imagen = imagen;
            this.precio = precio;
            this.oferta = oferta;
            finOferta = "";
            this.camaExtra = camaExtra;
            this.cuna = cuna;
        }
    }
}
