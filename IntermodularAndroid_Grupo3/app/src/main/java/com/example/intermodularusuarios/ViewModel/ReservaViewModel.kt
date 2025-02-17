package com.example.intermodularusuarios.viewmodel

import android.util.Log
import androidx.compose.runtime.mutableStateOf
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.intermodularusuarios.model.Habitaciones
import com.example.intermodularusuarios.model.Reservas
import com.example.intermodularusuarios.model.RetrofitClient
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch

class ReservaViewModel : ViewModel() {
    private val _reservas = MutableStateFlow<List<Reservas>>(emptyList())
    val reservas: StateFlow<List<Reservas>> = _reservas

    init {
        fetchReservas()
    }

    // GET: Obtener todas las reservas
    fun fetchReservas() {
        viewModelScope.launch {
            try {
                val response = RetrofitClient.reservaApiService.getAllReservas()
                _reservas.value = response
                Log.d("ReservaViewModel", "Reservas recibidas: $response")
            } catch (e: Exception) {
                Log.e("ReservaViewModel", "Error al obtener reservas", e)
            }
        }
    }

    // GET: Obtener una reserva por ID
    private val _reserva = MutableStateFlow<Reservas?>(null)
    val reserva: StateFlow<Reservas?> = _reserva

    fun getOneReserva(id: String) {
        viewModelScope.launch {
            try {
                val reservaResponse = RetrofitClient.reservaApiService.getReservaById(id)
                _reserva.value = reservaResponse
            } catch (e: Exception) {
                Log.e("ReservaViewModel", "Error al obtener la reserva", e)
            }
        }
    }

    // GET: Obtener reservas filtradas
    private val _reservasFiltradas = MutableStateFlow<List<Reservas>>(emptyList())
    val reservasFiltradas: StateFlow<List<Reservas>> = _reservasFiltradas
    var mostrarVacio = mutableStateOf<Boolean>(false)

    fun getReservasByFilters(
        dni: String? = null,
        idHab: String? = null,
        fechaIni: String? = null,
        fechaFin: String? = null,
        numHuespedes: Int? = null
    ) {
        viewModelScope.launch {
            try {
                mostrarVacio.value = false
                _reservasFiltradas.value = listOf(Reservas("", "", "", "", "", 0))
                val filters = mutableMapOf<String, String>()
                dni?.let { filters["dni"] = it }
                idHab?.let { filters["id_hab"] = it }
                fechaIni?.let { filters["fecha_ini"] = it }
                fechaFin?.let { filters["fecha_fin"] = it }
                numHuespedes?.let { filters["numHuespedes"] = it.toString() }

                val reservasResponse = RetrofitClient.reservaApiService.getReservasByFilter(filters)
                _reservasFiltradas.value = reservasResponse
            } catch (e: Exception) {
                mostrarVacio.value = true
                _reservasFiltradas.value = emptyList()
                Log.e("ReservaViewModel", "Error al obtener reservas filtradas", e)
            }
        }
    }

    // POST: Crear una nueva reserva
    private val _reservaCreada = MutableStateFlow<Reservas?>(null)
    val reservaCreada: StateFlow<Reservas?> = _reservaCreada

    private val _error = MutableStateFlow<String?>(null)
    val error: StateFlow<String?> = _error

    fun createReserva(
        dni: String,
        idHab: String,
        fechaIni: String,
        fechaFin: String,
        numHuespedes: Int
    ) {
        viewModelScope.launch {
            try {
                val nuevaReserva = Reservas(
                    id = "",
                    dni = dni,
                    id_hab = idHab,
                    fechaEntrada = fechaIni,
                    fechaSalida = fechaFin,
                    numHuespedes = numHuespedes
                )
                val reservaResponse = RetrofitClient.reservaApiService.createReserva(nuevaReserva)
                _reservaCreada.value = reservaResponse
            } catch (e: Exception) {
                _error.value = "Error al crear la reserva: ${e.message}"
            }
        }
    }

    // Lógica para obtener habitaciones
    var numHuespedesSeleccionados = mutableStateOf<Int?>(5)
    private val _habitacion = MutableStateFlow<Habitaciones?>(null)
    val habitacion: MutableStateFlow<Habitaciones?> = _habitacion

    suspend fun fetchHabitacion(id: String): Habitaciones? {
        return try {
            RetrofitClient.reservaApiService.getHabitacionesById(id)
        } catch (e: Exception) {
            null
        }
    }

    fun getHabitacionesByHuespedes(numHuespedes: Int, onResult: (List<Habitaciones>) -> Unit) {
        viewModelScope.launch {
            try {
                val habitacionesResponse = RetrofitClient.reservaApiService.getHabitacionesByHuespedes(numHuespedes)
                onResult(habitacionesResponse)
            } catch (e: Exception) {
                Log.e("ReservaViewModel", "Error al obtener habitaciones filtradas por huéspedes", e)
                onResult(emptyList())
            }
        }
    }
}
