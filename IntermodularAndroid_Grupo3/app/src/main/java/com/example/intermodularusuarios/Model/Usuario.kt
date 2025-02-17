package com.example.intermodularusuarios.model

import com.google.gson.annotations.SerializedName
import java.io.Serializable

data class Usuario(
    @SerializedName("dni")
    val dni: String,
    @SerializedName("nombre")
    val nombre: String,
    @SerializedName("apellido")
    val apellido: String,
    @SerializedName("rol")
    val rol: String,
    @SerializedName("email")
    val email: String,
    @SerializedName("contrasena")
    val contrasena: String,
    @SerializedName("fecha_nac")
    val fechaNac: String,
    @SerializedName("ciudad")
    val ciudad: String,
    @SerializedName("sexo")
    val sexo: String,
    @SerializedName("imagen")
    val imagen: String? = null,
    var token: String? = null
) : Serializable

