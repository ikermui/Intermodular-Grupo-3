package com.example.intermodularusuarios

import android.os.Build
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.annotation.RequiresApi
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import com.example.intermodularusuarios.View.ListaHabitaciones
import com.example.intermodularusuarios.View.habitacion
import com.example.intermodularusuarios.ui.theme.IntermodularUsuariosTheme
import com.example.intermodularusuarios.view.DatosUserScreen
import com.example.intermodularusuarios.view.LoginScreen
import com.example.intermodularusuarios.view.RegisterUserScreen
import com.example.intermodularusuarios.view.VistaBuscador

class MainActivity : ComponentActivity() {
    @RequiresApi(Build.VERSION_CODES.O)
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            IntermodularUsuariosTheme {
                val navController = rememberNavController()
                NavHost(navController = navController, startDestination = "main") {
                    // Pantalla de login
                    composable("main") { LoginScreen(navController) }

                    // Nueva pantalla: VistaBuscador
                    composable("vistaBuscador") { VistaBuscador(navController) }

                    // Resto de pantallas
                    composable("DatosUser") { DatosUserScreen(navController) }
                    composable("RegisterUser") { RegisterUserScreen(navController) }

                    // Ejemplo de navegación a crearReserva con parámetros
                    composable("crearReserva/{startdate}/{enddate}/{guests}") { backStackEntry ->
                        val startdate = backStackEntry.arguments?.getString("startdate")?.replace("-", "/") ?: ""
                        val enddate = backStackEntry.arguments?.getString("enddate")?.replace("-", "/") ?: ""
                        val guests = backStackEntry.arguments?.getString("guests") ?: "0"
                        com.example.intermodularusuarios.view.crearReservaScreen(
                            navController = navController,
                            startDate = startdate,
                            endDate = enddate,
                            guests = guests.toInt()
                        )
                    }
                    composable("vistaHabitaciones") { ListaHabitaciones(navController) }
                    composable("habitacion/{habitacionId}") { backStackEntry ->
                        val habitacionId = backStackEntry.arguments?.getString("habitacionId") ?: ""
                        habitacion(habitacionId, navController)
                    }
                }
            }
        }
    }
}
