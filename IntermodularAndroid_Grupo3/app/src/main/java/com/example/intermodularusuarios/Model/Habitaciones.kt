package com.example.intermodularusuarios.model

import com.google.gson.annotations.SerializedName
import java.io.Serializable

data class Habitaciones(
    @SerializedName("_id") val id: String,
    @SerializedName("nombre") val nombre: String,
    @SerializedName("huespedes") val huespedes: Int,
    @SerializedName("descripcion") val descripcion: String,
    @SerializedName("baja") val baja: Boolean,
    @SerializedName("imagen") val imagen: String,
    @SerializedName("precio") val precio: Double,
    @SerializedName("oferta") val oferta: Double,
    @SerializedName("camaExtra") val camaExtra: Boolean,
    @SerializedName("cuna") val cuna: Boolean
) : Serializable
