using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;

namespace HotelIES_Grupo3
{
    public class Reservas
    {

        public static readonly HttpClient client = new HttpClient(new TokenExpirationHandler()
        {
            InnerHandler = new HttpClientHandler()
        });
        private String _id;
        private String id_hab;
        private String dni;
        private String fecha_ini;
        private String fecha_fin;
        private int numHuespedes;
        private double precioTotal;

        public Reservas(string _id, string dni, string id_hab, string fecha_ini, string fecha_fin, int numHuespedes)
        {
            this._id = _id;
            this.id_hab = id_hab;
            this.dni = dni;
            this.fecha_ini = fecha_ini;
            this.fecha_fin = fecha_fin;
            this.numHuespedes = numHuespedes;
        }

        public static int calcularPrecioTotal(String fechaInicio, String fechaFinal, double precioNoche)
        {
            DateTime fechaIni = DateTime.ParseExact(fechaInicio, "d/M/yyyy", CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(fechaFinal, "d/M/yyyy", CultureInfo.InvariantCulture);
            int dias = (fechaFin - fechaIni).Days + 1;
            if (dias <= 0)
            {
                return -1;
            }
            return dias;
        }

        public String toString()
        {
            return "ID Reserva: " + _id + "\nID Habitacion: " + id_hab + "\nDNI: " + dni + "\nFecha Inicio: " + fecha_ini + "\nFecha Fin: " + fecha_fin + "\nNumero de Huespedes: " + numHuespedes + "\nPrecio Total: " + precioTotal;
        }

        public String ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public String Dni {  
            get { return dni; } 
            set { dni = value; }
        }
        public String Id_hab
        {
            get { return id_hab; }
            set { id_hab = value; }
        }
        public String Fecha_ini
        {
            get { return fecha_ini; }
            set { fecha_ini = value; }
        }
        public String Fecha_fin
        {
            get { return fecha_fin; }
            set { fecha_fin = value; }
        }
        public int NumHuespedes
        {
            get { return numHuespedes; }
            set { numHuespedes = value; }
        }

        public double PrecioTotal
        {
            get { return precioTotal; }
            set { precioTotal = value; }
        }

        public static String[] camposJsonSplitter(String[] campos)
        {
            for (int i = 0; i < campos.Length; i++)
            {
                campos[i] = campos[i].Split(":")[1];
            }
            return campos;
        }


        // Añadir Reserva
        public static async void anadirReserva(Reservas reserva)
        {
            try
            {
                String apiUrl = "http://localhost:3000/reservas/new";
                String body = "{" + "\n\"id_hab\": \"" + reserva.Id_hab + "\",\n\"dni\": \"" + reserva.Dni + "\",\n\"fecha_ini\": \"" + reserva.Fecha_ini + "\",\n\"fecha_fin\": \"" + reserva.Fecha_fin + "\",\n\"numHuespedes\": " + reserva.NumHuespedes + "\n}";
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        // Obtener Todas las Reservas

        public static async Task<Reservas> GetReserva(string _id)
        {
            {
                try
                {
                    String apiUrl = "http://localhost:3000/reservas/getOne/";
                    apiUrl += _id;
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();
                    string respuestaServidor = await response.Content.ReadAsStringAsync();
                    respuestaServidor = respuestaServidor.Replace("{", "").Replace("}", "").Replace("\"", "");
                    String[] respuestaCampos = respuestaServidor.Split(",");
                    respuestaCampos = camposJsonSplitter(respuestaCampos);
                    Reservas res = new Reservas(respuestaCampos[0], respuestaCampos[1], respuestaCampos[2], respuestaCampos[3], respuestaCampos[4], Convert.ToInt32(respuestaCampos[5]));
                    return res;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                    return null;
                }

            }
        }

        // Actualizar Reserva

        public static async Task<Boolean> updateReserva(string _id, string dni, string id_hab, string fecha_ini, string fecha_fin, int numHuespedes)
        {
            try
            {
                String apiUrl = "http://localhost:3000/reservas/update";

                String jsonBody = "{";

                    if (_id != "")
                    {
                        jsonBody += "\"_id\": \"" + _id + "\",";
                    }
                    if (dni != "")
                    {
                        jsonBody += "\"dni\": \"" + dni + "\",";
                    }
                    if (id_hab != "")
                    {
                        jsonBody += "\"id_hab\": \"" + id_hab + "\",";
                    }
                    if (fecha_ini != "")
                    {
                        jsonBody += "\"fecha_ini\": \"" + fecha_ini + "\",";
                    }
                    if (fecha_fin != "")
                    {
                        jsonBody += "\"fecha_fin\": \"" + fecha_fin + "\",";
                    }
                    if (numHuespedes != 0)
                    {
                        jsonBody += "\"numHuespedes\": " + numHuespedes + ",";
                    }

                    jsonBody += "}";
                    jsonBody = jsonBody.Replace(",}", "}");
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PatchAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();
                string respuestaServidor = await response.Content.ReadAsStringAsync();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
                return false;
            }
            
        }

        // Borrar Reserva

        public static async Task<Boolean> deleteReserva(string _id)
        {
            {
                try
                {
                    String apiUrl = "http://localhost:3000/reservas/delete/";
                    apiUrl += _id;
                    HttpResponseMessage response = await client.DeleteAsync(apiUrl);
                    response.EnsureSuccessStatusCode();
                    string respuestaServidor = await response.Content.ReadAsStringAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                    return false;
                }
            }
        }

        // Obtener una reserva
        public static async Task<List<Reservas>> GetAllReservas()
        {
            {
                try
                {
                    String apiUrl = "http://localhost:3000/reservas/getAll";
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();
                    string respuestaServidor = await response.Content.ReadAsStringAsync();
                    respuestaServidor = respuestaServidor.Replace("[{", "").Replace("}]", "").Replace("\"", "");
                    String[] reservasResp = respuestaServidor.Split("},{");
                    List<Reservas> reservas = new List<Reservas>();
                    foreach (String reservasStr in reservasResp)
                    {
                        String[] campos = camposJsonSplitter(reservasStr.Split(","));
                        reservas.Add(new Reservas(campos[0], campos[1], campos[2], campos[3], campos[4], Convert.ToInt32(campos[5])));
                    }
                    return reservas;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                    return null;
                }

            }
        }

        // Obtener Reservas por Filtro
        public static async Task<List<Reservas>> GetReservaFiltro(string _id, string dni, string id_hab, string fecha_ini, string fecha_fin, int numHuespedes)
        {
            {
                try
                {
                    String apiUrl = "http://localhost:3000/reservas/getFilter";

                    String jsonBody = "{";

                    if (_id != "")
                    {
                        jsonBody += "\"_id\": \"" + _id + "\",";
                    }
                    if (dni != "")
                    {
                        jsonBody += "\"dni\": \"" + dni + "\",";
                    }
                    if (id_hab != "")
                    {
                        jsonBody += "\"id_hab\": \"" + id_hab + "\",";
                    }
                    if (fecha_ini != "")
                    {
                        jsonBody += "\"fecha_ini\": \"" + fecha_ini + "\",";
                    }
                    if (fecha_fin != "")
                    {
                        jsonBody += "\"fecha_fin\": \"" + fecha_fin + "\",";
                    }
                    if (numHuespedes != 0)
                    {
                        jsonBody += "\"numHuespedes\": " + numHuespedes + ",";
                    }

                    jsonBody += "}";
                    jsonBody = jsonBody.Replace(",}", "}");
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    response.EnsureSuccessStatusCode();
                    string respuestaServidor = await response.Content.ReadAsStringAsync();
                    respuestaServidor = respuestaServidor.Replace("[{", "").Replace("}]", "").Replace("\"", "");
                    String[] reservasResp = respuestaServidor.Split("},{");
                    List<Reservas> reservas = new List<Reservas>();
                    foreach (String reservasStr in reservasResp)
                    {
                        String[] campos = camposJsonSplitter(reservasStr.Split(","));
                        reservas.Add(new Reservas(campos[0], campos[1], campos[2], campos[3], campos[4], Convert.ToInt32(campos[5])));
                    }


                    return reservas;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                    return null;
                }

            }
        }

        // Obtener reservas por filtro entre 2 fechas

        public static async Task<List<Reservas>> GetReservaFiltroDate(string _id, string dni, string id_hab, string fecha_ini, string fecha_fin, int numHuespedes)
        {
            {
                try
                {
                    String apiUrl = "http://localhost:3000/reservas/getFilterDate";

                    String jsonBody = "{";
                    
                    if (_id != "")
                    {
                        jsonBody += "\"_id\": \"" + _id + "\",";
                    }
                    if (dni != "")
                    {
                        jsonBody += "\"dni\": \"" + dni + "\",";
                    }
                    if (id_hab != "")
                    {
                        jsonBody += "\"id_hab\": \"" + id_hab + "\",";
                    }
                    if (fecha_ini != "")
                    {
                        jsonBody += "\"fecha_ini\": \"" + fecha_ini + "\",";
                    }
                    if (fecha_fin != "")
                    {
                        jsonBody += "\"fecha_fin\": \"" + fecha_fin + "\",";
                    }
                    if (numHuespedes != 0)
                    {
                        jsonBody += "\"numHuespedes\": " + numHuespedes + ",";
                    }

                    jsonBody += "}";
                    jsonBody = jsonBody.Replace(",}", "}");
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    response.EnsureSuccessStatusCode();
                    string respuestaServidor = await response.Content.ReadAsStringAsync();
                    respuestaServidor = respuestaServidor.Replace("[{", "").Replace("}]", "").Replace("\"", "");
                    String[] reservasResp = respuestaServidor.Split("},{");
                    List<Reservas> reservas = new List<Reservas>();
                    foreach (String reservasStr in reservasResp)
                    {
                        String[] campos = camposJsonSplitter(reservasStr.Split(","));
                        reservas.Add(new Reservas(campos[0], campos[1], campos[2], campos[3], campos[4], Convert.ToInt32(campos[5])));
                    }


                    return reservas;
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
        }

        public static async Task<List<Reservas>> GetReservaBuscador(string _id, string dni, string id_hab, string fecha_ini, string fecha_fin, int numHuespedes)
        {
            {
                try
                {
                    String apiUrl = "http://localhost:3000/reservas/getFilterBuscador?";

                    if (_id != "")
                    {
                        apiUrl += "_id=" + _id + "&";
                    }
                    if (dni != "")
                    {
                        apiUrl += "dni=" + dni + "&";
                    }
                    if (id_hab != "")
                    {
                        apiUrl += "id_hab=" + id_hab + "&";
                    }
                    if (fecha_ini != "")
                    {
                        apiUrl += "fecha_ini=" + fecha_ini + "&";
                    }
                    if (fecha_fin != "")
                    {
                        apiUrl += "fecha_fin=" + fecha_fin + "&";
                    }
                    if (numHuespedes != 0)
                    {
                        apiUrl += "numHuespedes=" + numHuespedes + "&";
                    }
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();
                    string respuestaServidor = await response.Content.ReadAsStringAsync();
                    respuestaServidor = respuestaServidor.Replace("[{", "").Replace("}]", "").Replace("\"", "");
                    String[] reservasResp = respuestaServidor.Split("},{");
                    List<Reservas> reservas = new List<Reservas>();
                    foreach (String reservasStr in reservasResp)
                    {
                        String[] campos = camposJsonSplitter(reservasStr.Split(","));
                        reservas.Add(new Reservas(campos[0], campos[1], campos[2], campos[3], campos[4], Convert.ToInt32(campos[5])));
                    }
                    return reservas;
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
        }
    }
}
