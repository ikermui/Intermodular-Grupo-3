package com.example.intermodular.models

import com.google.gson.annotations.SerializedName

data class Habitacion(
    @SerializedName("_id") val id: String,
    @SerializedName("nombre") val nombre: String,
    @SerializedName("huespedes") val huespedes: Int,
    @SerializedName("descripcion") val descripcion: String,
    @SerializedName("imagen") val imagen: String,
    @SerializedName("precio") val precio: Double,
    @SerializedName("oferta") val oferta: Double,
    @SerializedName("camaExtra") val camaExtra: Boolean,
    @SerializedName("baja") val baja: Boolean,
    @SerializedName("cuna") val cuna: Boolean
)