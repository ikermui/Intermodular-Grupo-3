package com.example.intermodular.viewmodel

import android.util.Log
import com.example.intermodular.models.Habitacion
import com.example.intermodular.viewmodel.network.RetrofitClient
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext

class HabitacionesRepository {
    private val apiService = RetrofitClient.apiService

    suspend fun obtenerHabitaciones(): List<Habitacion>? {
        return withContext(Dispatchers.IO) {
            val response = apiService.obtenerHabitaciones()
            if (response.isSuccessful) {
                response.body()
            } else {
                null
            }
        }
    }

    suspend fun obtenerHabitacionPorId(id: String): Habitacion? {
        return withContext(Dispatchers.IO) {
            val response = apiService.obtenerHabitacionPorId(id)
            if (response.isSuccessful) {
                response.body()
            } else {
                null
            }
        }
    }

    suspend fun filtrarHabitaciones(nombre: String, huespedes: Int, cuna: Boolean?, cama: Boolean?): List<Habitacion>? {
        return withContext(Dispatchers.IO) {
            val response = apiService.filtrarHabitaciones(nombre, huespedes, cuna, cama)
            if (response.isSuccessful) {
                response.body()
            } else {
                null
            }
        }
    }
}
