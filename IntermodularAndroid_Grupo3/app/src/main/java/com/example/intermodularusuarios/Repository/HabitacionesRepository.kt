package com.example.intermodularusuarios.Repository

import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.util.Base64
import android.util.Log
import com.example.intermodularusuarios.model.Habitaciones
import com.example.intermodularusuarios.model.RetrofitClient
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext

class HabitacionesRepository {
    private val apiService = RetrofitClient.apiService

    suspend fun obtenerHabitaciones(): List<Habitaciones>? {
        return withContext(Dispatchers.IO) {
            val response = apiService.obtenerHabitaciones()
            if (response.isSuccessful) {
                response.body()
            } else {
                null
            }
        }
    }

    suspend fun obtenerHabitacionPorId(id: String): Habitaciones? {
        return withContext(Dispatchers.IO) {
            Log.d("mensaje", "${id} HabitacionesRepository")
            val response = apiService.obtenerHabitacionPorId(id)
            if (response.isSuccessful) {
                response.body()
            } else {
                null
            }
        }
    }

    suspend fun filtrarHabitaciones(nombre: String, huespedes: Int, cuna: Boolean?, cama: Boolean?): List<Habitaciones>? {
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