package com.example.intermodular.viewmodel.network

import com.example.intermodular.models.Habitacion
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.Query

interface ApiService {
    @GET("habitaciones/getAll")
    suspend fun obtenerHabitaciones(): Response<List<Habitacion>>

    @GET("habitaciones/getOne")
    suspend fun obtenerHabitacionPorId(@Query("_id") id: String): Response<Habitacion>;

    @GET("habitaciones/getFilterAndroid")
    suspend fun filtrarHabitaciones(
        @Query("nombre") nombre: String,
        @Query("huespedes") huespedes: Int,
        @Query("cuna") cuna: Boolean?,
        @Query("camaExtra") cama: Boolean?
    ): Response<List<Habitacion>>
}