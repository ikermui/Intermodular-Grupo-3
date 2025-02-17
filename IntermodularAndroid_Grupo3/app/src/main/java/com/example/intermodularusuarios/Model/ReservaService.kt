package com.example.intermodularusuarios.model

import retrofit2.http.*

interface ReservaService {
    @GET("reservas/getAll")
    suspend fun getAllReservas(): List<Reservas>

    @GET("reservas/getOne/{id}")
    suspend fun getReservaById(@Path("id") id: String): Reservas

    @GET("reservas/getFilterBuscador")
    suspend fun getReservasByFilter(@QueryMap filters: Map<String, String>): List<Reservas>

    @POST("reservas/new")
    suspend fun createReserva(@Body reserva: Reservas): Reservas

    @PATCH("reservas/update/{id}")
    suspend fun updateReserva(@Path("id") id: String, @Body reserva: Reservas): Reservas

    @DELETE("reservas/delete/{id}")
    suspend fun deleteReserva(@Path("id") id: String)

    // Métodos para habitaciones
    @GET("habitaciones/getOne/{id}")
    suspend fun getHabitacionesById(@Path("id") id: String): Habitaciones

    @GET("habitaciones/getFilterHuespedes")
    suspend fun getHabitacionesByHuespedes(@Query("huespedes") numHuespedes: Int): List<Habitaciones>

    @GET("habitaciones/getOneHuespedes/{id}")
    suspend fun getHabitacionesCondiciones(
        @Path("id") id: String,
        @Query("numHuespedes") numHuespedes: Int
    ): Habitaciones
}
