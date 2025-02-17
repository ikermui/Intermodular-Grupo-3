package com.example.intermodularusuarios.model

import retrofit2.Response
import retrofit2.http.*

interface UsuarioApi {

    // Endpoint de login: NO requiere token
    @POST("auth/login")
    suspend fun login(@Body credentials: Map<String, String>): Response<LoginResponse>

    // Endpoint para obtener todos los usuarios (requiere token)
    @GET("user/getAll")
    suspend fun getAllUsuarios(@Header("Authorization") auth: String): Response<List<Usuario>>

    // Endpoint para actualizar usuario (requiere token)
    @PATCH("user/update")
    suspend fun updateUsuario(
        @Header("Authorization") auth: String,
        @Body request: Map<String, String>
    ): Response<Map<String, String>>

    // Endpoint para eliminar usuario (requiere token)
    @HTTP(method = "DELETE", path = "user/delete", hasBody = true)
    suspend fun deleteUsuario(
        @Header("Authorization") auth: String,
        @Body request: Map<String, String>
    ): Response<Map<String, String>>

    // Endpoint para obtener un usuario por email (requiere token)
    @POST("user/getOneEmail")
    suspend fun getOneByEmail(
        @Header("Authorization") auth: String,
        @Body request: Map<String, String>
    ): Response<Usuario>

    // Endpoint para registrar usuario sin token
    @POST("user/register")
    suspend fun registerWithoutToken(@Body usuario: Usuario): Response<Usuario>
}
