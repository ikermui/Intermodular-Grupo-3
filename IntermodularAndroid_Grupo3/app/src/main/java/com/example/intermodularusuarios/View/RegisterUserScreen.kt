package com.example.intermodularusuarios.view

import android.widget.Toast
import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material3.Button
import androidx.compose.material3.ElevatedCard
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Text
import androidx.compose.material3.TextFieldDefaults
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.shadow
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.NavController
import com.example.intermodularusuarios.R
import com.example.intermodularusuarios.model.Usuario
import com.example.intermodularusuarios.viewmodel.UsuarioViewModel
import java.text.SimpleDateFormat
import java.util.Locale

@Composable
fun RegisterUserScreen(
    navController: NavController,
    usuarioViewModel: UsuarioViewModel = viewModel(navController.getBackStackEntry("main"))
) {
    var dni by remember { mutableStateOf("") }
    var nombre by remember { mutableStateOf("") }
    var apellidos by remember { mutableStateOf("") }
    var fechaNacimiento by remember { mutableStateOf("") }
    var ciudad by remember { mutableStateOf("") }
    var sexo by remember { mutableStateOf("") }
    var email by remember { mutableStateOf("") }
    var contrasena by remember { mutableStateOf("") }

    val context = LocalContext.current
    val errorMessage by usuarioViewModel.errorMessage.collectAsState()
    val usuario by usuarioViewModel.usuario.collectAsState()

    LaunchedEffect(errorMessage) {
        errorMessage?.let { msg ->
            Toast.makeText(context, msg, Toast.LENGTH_LONG).show()
        }
    }
    LaunchedEffect(usuario) {
        usuario?.let {
            Toast.makeText(context, "Usuario registrado correctamente", Toast.LENGTH_SHORT).show()
            navController.navigate("DatosUser")
        }
    }

    Box(
        modifier = Modifier
            .fillMaxSize()
            .background(MaterialTheme.colorScheme.surfaceVariant)
            .padding(16.dp),
        contentAlignment = Alignment.Center
    ) {
        Column(horizontalAlignment = Alignment.CenterHorizontally) {
            Image(
                painter = painterResource(id = R.drawable.logo),
                contentDescription = "Logo",
                modifier = Modifier.height(150.dp)
            )
            Spacer(modifier = Modifier.height(16.dp))
            Text(
                text = "Bienvenido a Hoteles Pere Maria",
                fontSize = 24.sp,
                fontWeight = FontWeight.Bold,
                color = MaterialTheme.colorScheme.primary
            )
            Text(
                text = "Regístrate",
                fontSize = 18.sp,
                fontWeight = FontWeight.Medium,
                color = MaterialTheme.colorScheme.secondary
            )
            Spacer(modifier = Modifier.height(16.dp))
            ElevatedCard(
                modifier = Modifier.fillMaxWidth(),
                shape = RoundedCornerShape(16.dp)
            ) {
                LazyColumn(
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(16.dp),
                    verticalArrangement = Arrangement.spacedBy(8.dp)
                ) {
                    item { InputField(value = dni, onValueChange = { dni = it }, label = "DNI") }
                    item { InputField(value = nombre, onValueChange = { nombre = it }, label = "Nombre") }
                    item { InputField(value = apellidos, onValueChange = { apellidos = it }, label = "Apellidos") }
                    item { InputField(value = fechaNacimiento, onValueChange = { fechaNacimiento = it }, label = "Fecha de nacimiento (d/M/yyyy)") }
                    item { InputField(value = ciudad, onValueChange = { ciudad = it }, label = "Ciudad") }
                    item { InputField(value = sexo, onValueChange = { sexo = it }, label = "Sexo (Hombre/Mujer/Indeterminado)") }
                    item { InputField(value = email, onValueChange = { email = it }, label = "Email") }
                    item {
                        PasswordInputField(
                            value = contrasena,
                            onValueChange = { contrasena = it },
                            label = "Contraseña"
                        )
                    }
                }
            }
            Spacer(modifier = Modifier.height(10.dp))
            Button(
                onClick = {
                    if (!isValidDNI(dni)) {
                        Toast.makeText(context, "DNI inválido", Toast.LENGTH_SHORT).show()
                        return@Button
                    }
                    if (!isValidFecha(fechaNacimiento)) {
                        Toast.makeText(context, "Fecha de nacimiento inválida (formato: d/M/yyyy)", Toast.LENGTH_SHORT).show()
                        return@Button
                    }
                    if (!isValidSexo(sexo)) {
                        Toast.makeText(context, "El campo sexo debe ser 'hombre', 'mujer' o 'indeterminado'", Toast.LENGTH_SHORT).show()
                        return@Button
                    }
                    if (!isValidEmail(email)) {
                        Toast.makeText(context, "Email inválido. Debe tener un formato como nombre@gmail.com, yahoo.es, outlook.org, etc.", Toast.LENGTH_SHORT).show()
                        return@Button
                    }
                    val newUser = Usuario(
                        dni = dni,
                        nombre = nombre,
                        apellido = apellidos,
                        rol = "cliente",
                        email = email,
                        contrasena = contrasena,
                        fechaNac = fechaNacimiento,
                        ciudad = ciudad,
                        sexo = sexo,
                        imagen = "iVBORw0KGgoAAAANSUhEUgAAAgAAAAIACAYAAAD0eNT6AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAOxAAADsQBlSsOGwAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAACAASURBVHic7N13nBT1/cfx12d27+hF..."
                    )
                    usuarioViewModel.registerUsuario(newUser)
                    Toast.makeText(context, "Registrando usuario...", Toast.LENGTH_SHORT).show()
                },
                modifier = Modifier
                    .fillMaxWidth()
                    .height(50.dp)
                    .shadow(4.dp, RoundedCornerShape(12.dp)),
                shape = RoundedCornerShape(12.dp)
            ) {
                Text(text = "Registrarse", fontSize = 18.sp)
            }
        }
    }
}

@Composable
fun InputField(value: String, onValueChange: (String) -> Unit, label: String) {
    OutlinedTextField(
        value = value,
        onValueChange = onValueChange,
        label = { Text(text = label) },
        modifier = Modifier.fillMaxWidth()
    )
}

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun PasswordInputField(value: String, onValueChange: (String) -> Unit, label: String) {
    OutlinedTextField(
        value = value,
        onValueChange = onValueChange,
        label = { Text(text = label) },
        visualTransformation = PasswordVisualTransformation(),
        keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Password),
        modifier = Modifier.fillMaxWidth(),
        colors = TextFieldDefaults.outlinedTextFieldColors()
    )
}

// Función para validar el DNI
fun isValidDNI(dni: String): Boolean {
    val dniRegex = "^[0-9]{8}[A-Za-z]$".toRegex()
    return dniRegex.matches(dni)
}

// Función para validar la fecha de nacimiento en formato "d/M/yyyy"
fun isValidFecha(fecha: String): Boolean {
    return try {
        val sdf = SimpleDateFormat("d/M/yyyy", Locale.getDefault())
        sdf.isLenient = false
        sdf.parse(fecha)
        true
    } catch (e: Exception) {
        false
    }
}

// Función para validar el sexo
fun isValidSexo(sexo: String): Boolean {
    val s = sexo.lowercase(Locale.getDefault())
    return s == "hombre" || s == "mujer" || s == "indeterminado"
}

// Función para validar email
fun isValidEmail(email: String): Boolean {
    val emailRegex = "^[a-zA-Z0-9._%+-]+@(gmail|yahoo|outlook|hotmail)\\.(com|es|org)$".toRegex()
    return emailRegex.matches(email)
}
