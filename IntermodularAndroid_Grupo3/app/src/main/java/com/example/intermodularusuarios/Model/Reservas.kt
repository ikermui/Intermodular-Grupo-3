package com.example.intermodularusuarios.model

import com.google.gson.annotations.SerializedName

data class Reservas(
    @SerializedName("_id") val id: String,
    @SerializedName("dni") var dni: String,
    @SerializedName("id_hab") val id_hab: String,
    @SerializedName("fecha_ini") val fechaEntrada: String,
    @SerializedName("fecha_fin") val fechaSalida: String,
    @SerializedName("numHuespedes") val numHuespedes: Int
)
