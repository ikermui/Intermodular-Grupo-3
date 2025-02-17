package com.example.intermodularusuarios.model

import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.Path
import retrofit2.http.Query

interface HabitacionesService {
    /*@GET("habitaciones/getOne/{id}")
    suspend fun getHabitacionesById(@Path("id") id: String): Response<Habitaciones>*/

    @GET("habitaciones/getOne")
    suspend fun obtenerHabitacionPorId(@Query("_id") id: String): Response<Habitaciones>;

    @GET("habitaciones/getAll")
    suspend fun obtenerHabitaciones(): Response<List<Habitaciones>>

    @GET("habitaciones/getFilterAndroid")
    suspend fun filtrarHabitaciones(
        @Query("nombre") nombre: String,
        @Query("huespedes") huespedes: Int,
        @Query("cuna") cuna: Boolean?,
        @Query("camaExtra") cama: Boolean?
    ): Response<List<Habitaciones>>
}
