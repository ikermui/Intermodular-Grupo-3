package com.example.intermodularusuarios.repository

import com.example.intermodularusuarios.model.Reservas
import com.example.intermodularusuarios.model.RetrofitClient

class ReservaRepository {
    suspend fun getAllReservas(): List<Reservas>? {
        return try {
            RetrofitClient.reservaApiService.getAllReservas()
        } catch (e: Exception) {
            null
        }
    }

    suspend fun getReservaById(id: String): Reservas? {
        return try {
            RetrofitClient.reservaApiService.getReservaById(id)
        } catch (e: Exception) {
            null
        }
    }

    suspend fun getReservasByFilter(filters: Map<String, String>): List<Reservas>? {
        return try {
            RetrofitClient.reservaApiService.getReservasByFilter(filters)
        } catch (e: Exception) {
            null
        }
    }

    suspend fun createReserva(reserva: Reservas): Reservas? {
        return try {
            RetrofitClient.reservaApiService.createReserva(reserva)
        } catch (e: Exception) {
            null
        }
    }
}
