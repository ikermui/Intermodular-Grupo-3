package com.example.intermodular.navigation

import android.util.Log
import androidx.compose.runtime.Composable
import androidx.navigation.NavHostController
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import com.example.intermodular.view.ListaHabitaciones
import com.example.intermodular.view.habitacion

@Composable
fun NavManager(navController: NavHostController) {
    NavHost(navController = navController, startDestination = "vistaHabitaciones") {
        composable("vistaHabitaciones") { ListaHabitaciones(navController) }
        composable("habitacion/{habitacionId}") { backStackEntry ->
            val habitacionId = backStackEntry.arguments?.getString("habitacionId") ?: ""
            habitacion(habitacionId, navController)
        }
    }
}
