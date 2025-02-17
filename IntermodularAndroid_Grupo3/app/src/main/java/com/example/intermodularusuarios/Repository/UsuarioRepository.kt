package com.example.intermodularusuarios.repository

import com.example.intermodularusuarios.model.RetrofitClient
import com.example.intermodularusuarios.model.Usuario
import com.example.intermodularusuarios.model.LoginResponse

class UsuarioRepository {
    private val api = RetrofitClient.instance

    suspend fun login(email: String, contrasena: String): LoginResponse? {
        val credentials = mapOf("email" to email, "contrasena" to contrasena)
        val response = api.login(credentials)
        return if (response.isSuccessful) response.body() else null
    }

    suspend fun getAllUsuarios(token: String): List<Usuario>? {
        val response = api.getAllUsuarios("Bearer $token")
        return if (response.isSuccessful) response.body() else null
    }

    suspend fun updateUsuario(dni: String, updatedData: Map<String, String>, token: String): Boolean {
        val request = mutableMapOf<String, String>("dni" to dni)
        request.putAll(updatedData)
        val response = api.updateUsuario("Bearer $token", request)
        return response.isSuccessful
    }

    suspend fun deleteUsuario(dni: String, token: String): Boolean {
        val request = mapOf("dni" to dni)
        val response = api.deleteUsuario("Bearer $token", request)
        return response.isSuccessful
    }

    suspend fun getOneByEmail(email: String, contrasena: String, token: String): Usuario? {
        val request = mapOf("email" to email, "contrasena" to contrasena)
        val response = api.getOneByEmail("Bearer $token", request)
        return if (response.isSuccessful) response.body() else null
    }

    // Método para registrar usuario sin token
    suspend fun registerWithoutToken(usuario: Usuario): Usuario? {
        val response = api.registerWithoutToken(usuario)
        if (!response.isSuccessful) {
            val errorMsg = response.errorBody()?.string() ?: "Error desconocido"
            throw Exception(errorMsg)
        }
        return response.body()
    }
}
