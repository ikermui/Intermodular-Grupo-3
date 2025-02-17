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

namespace HotelIES_Grupo3
{
    /// <summary>
    /// Lógica de interacción para BuscadorUsuarios.xaml
    /// </summary>
    public partial class BuscadorUsuarios : Window
    {
        // Variable para almacenar la lista completa de clientes obtenidos
        private List<Usuarios> usuariosOriginal;
        // Variable para almacenar el cliente logueado (con token)
        private Usuarios loggedUser;

        // Constructor modificado para recibir el objeto Clientes
        public BuscadorUsuarios(Usuarios cliente)
        {
            InitializeComponent();
            loggedUser = cliente;
            // Mostrar el token en un MessageBox al abrir la ventana
            listaUsuarios();
        }

        public async void listaUsuarios()
        {
            List<CamposLista> campos = new List<CamposLista>();

            // Obtener la lista completa de clientes de la API y guardarla en usuariosOriginal
            usuariosOriginal = await Usuarios.ObtenerTodosAsync();
            if (usuariosOriginal == null)
            {
                MessageBox.Show("No se pudieron cargar los usuarios.");
                return;
            }

            // Crear la lista de CamposLista para mostrar en el ListView
            foreach (Usuarios cliente in usuariosOriginal)
            {
                string nomAp = cliente.nombre + " " + cliente.apellido;
                campos.Add(new CamposLista(MainWindow.convertirImagen(cliente.imagen), nomAp, cliente.rol, cliente.ciudad, cliente.fecha_nac, cliente.sexo));
            }
            listaUsers.ItemsSource = campos;
        }

        private async void Buscar_Click(object sender, RoutedEventArgs e)
        {
            List<CamposLista> campos = new List<CamposLista>();

            // Obtener los filtros desde los controles
            ComboBoxItem rolSeleccionado = rolComboBox.SelectedItem as ComboBoxItem;
            string valorRol = rolSeleccionado != null ? rolSeleccionado.Content.ToString().Trim() : string.Empty;

            DateTime? fechaSeleccionada = fechaNacimiento.SelectedDate;
            string valorFecha = fechaSeleccionada != null ? fechaSeleccionada.Value.ToString("d/M/yyyy") : string.Empty;

            string ciudadFiltro = ciudad.Text.Trim();

            string sexoSeleccionado = string.Empty;
            if (hombre.IsChecked == true)
                sexoSeleccionado = "Hombre";
            else if (mujer.IsChecked == true)
                sexoSeleccionado = "Mujer";
            else if (indeterminado.IsChecked == true)
                sexoSeleccionado = "Indeterminado";

            // Llamar al método de filtrado de la API
            List<Usuarios> usuariosFiltrados = await Usuarios.ObtenerFiltradosAsync(valorRol, valorFecha, ciudadFiltro, sexoSeleccionado);

            if (usuariosFiltrados != null)
            {
                // Actualizar el ListView y la lista original (para edición posterior)
                List<CamposLista> camposFiltrados = new List<CamposLista>();
                foreach (Usuarios cliente in usuariosFiltrados)
                {
                    string nomAp = cliente.nombre + " " + cliente.apellido;
                    camposFiltrados.Add(new CamposLista(MainWindow.convertirImagen(cliente.imagen), nomAp, cliente.rol, cliente.ciudad, cliente.fecha_nac, cliente.sexo));
                }
                listaUsers.ItemsSource = camposFiltrados;
                usuariosOriginal = usuariosFiltrados;
            }
            else
            {
                MessageBox.Show("No se encontraron usuarios con los filtros proporcionados.");
            }
        }

        // Evento para el botón de agregar usuario
        private void Click_AddUser(object sender, RoutedEventArgs e)
        {
            AddUser addUser = new AddUser();
            addUser.Show();
        }

        // Evento para el botón de editar usuario
        private void Click_EditUser(object sender, RoutedEventArgs e)
        {
            if (listaUsers.SelectedIndex >= 0)
            {
                Usuarios usuarioSeleccionado = usuariosOriginal[listaUsers.SelectedIndex];

                // Si el usuario logueado es Empleado y el usuario seleccionado es Administrador, no permitir editar
                if (loggedUser.rol.Equals("Empleado", StringComparison.OrdinalIgnoreCase) &&
                    usuarioSeleccionado.rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("No se permite editar a un usuario Administrador.", "Acceso denegado", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Abrir la ventana de actualización pasando el objeto Clientes seleccionado

                UpdateUser ventanaUpdate = new UpdateUser(usuarioSeleccionado);
                ventanaUpdate.ShowDialog();

                // Recargar la lista de usuarios después de la actualización
                listaUsuarios();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un usuario para editar.");
            }
        }

        // Evento para eliminar usuario
        private async void Click_DeleteUser(object sender, RoutedEventArgs e)
        {
            if (listaUsers.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, selecciona un usuario para eliminar.");
                return;
            }

            Usuarios usuarioSeleccionado = usuariosOriginal[listaUsers.SelectedIndex];

            // Si el usuario logueado es Empleado y el usuario seleccionado es Administrador, no permitir eliminar
            if (loggedUser.rol.Equals("Empleado", StringComparison.OrdinalIgnoreCase) &&
                usuarioSeleccionado.rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("No se permite eliminar a un usuario Administrador.", "Acceso denegado", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult confirmacion = MessageBox.Show(
                $"¿Estás seguro de eliminar el usuario {usuarioSeleccionado.nombre} {usuarioSeleccionado.apellido}?",
                "Confirmación de eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirmacion == MessageBoxResult.Yes)
            {
                bool eliminado = await Usuarios.EliminarAsync(usuarioSeleccionado.dni);
                if (eliminado)
                {
                    MessageBox.Show("Usuario eliminado correctamente.");
                    listaUsuarios();
                }
                else
                {
                    MessageBox.Show("Error al eliminar el usuario.");
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BuscarReserva br = new BuscarReserva(loggedUser);
            br.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            BuscadorUsuarios bu = new BuscadorUsuarios(loggedUser);
            bu.Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ListaReservas lr = new ListaReservas(loggedUser);
            lr.Show();
            this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            rolComboBox.SelectedIndex = -1;
            fechaNacimiento.Text = "";
            ciudad.Text = "";
            hombre.IsChecked = false;
            mujer.IsChecked = false;
            indeterminado.IsChecked = false;
            listaUsuarios();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            BuscarHabitacion bh = new BuscarHabitacion(loggedUser);
            bh.Show();
            this.Close();
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
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
    }

    class CamposLista
    {
        public CamposLista(BitmapImage imagenUsu, string nombreUsu, string rolUsu, String ciudadUsu, String fechaUsu, String sexoUsu)
        {
            this.imagenUsu = imagenUsu;
            this.nombreUsu = nombreUsu;
            this.rolUsu = rolUsu;
            this.ciudadUsu = ciudadUsu;
            this.fechaUsu = fechaUsu;
            this.sexoUsu = sexoUsu;
        }

        public BitmapImage imagenUsu { get; set; }
        public String nombreUsu { get; set; }
        public String rolUsu { get; set; }
        public String ciudadUsu { get; set; }
        public String fechaUsu { get; set; }
        public String sexoUsu { get; set; }

    }
}