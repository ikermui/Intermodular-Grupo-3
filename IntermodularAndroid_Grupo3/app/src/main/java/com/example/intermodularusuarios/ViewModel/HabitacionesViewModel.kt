package com.example.intermodularusuarios.ViewModel

import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.intermodularusuarios.Repository.HabitacionesRepository
import com.example.intermodularusuarios.model.Habitaciones
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch

class HabitacionesViewModel : ViewModel() {
    private val repository = HabitacionesRepository()
    private val _habitaciones = MutableStateFlow<List<Habitaciones>>(emptyList())
    val habitaciones: StateFlow<List<Habitaciones>> get() = _habitaciones

    init {
        cargarHabitaciones()
    }

    private fun cargarHabitaciones() {
        viewModelScope.launch {
            try {
                val lista = repository.obtenerHabitaciones()
                if (lista != null) {
                    _habitaciones.value = lista
                    Log.d("Habitaciones", "Datos cargados: $lista")
                } else {
                    _habitaciones.value = emptyList()
                    Log.d("Habitaciones", "Respuesta nula o lista vacía.")
                }
            } catch (e: Exception) {
                Log.e("Habitaciones", "Error al obtener habitaciones: ${e.message}")
                _habitaciones.value = emptyList()
            }
        }
    }

    private val _habitacionSeleccionada = MutableStateFlow<Habitaciones?>(null)
    val habitacionSeleccionada: StateFlow<Habitaciones?> get() = _habitacionSeleccionada

    fun cargarHabitacionPorId(id: String) {
        Log.d("Mensaje", "${id} cargado")
        viewModelScope.launch {
            _habitacionSeleccionada.value = repository.obtenerHabitacionPorId(id)
        }
    }

    fun cargarHabitacionesFiltradas(nombre: String, huespedes: Int, cuna: Boolean?, cama: Boolean?) {
        viewModelScope.launch {
            try {
                val lista = repository.filtrarHabitaciones(nombre, huespedes, cuna, cama)
                if (lista != null) {
                    _habitaciones.value = lista
                    Log.d("Habitaciones", "Datos filtrados cargados: $lista")
                } else {
                    _habitaciones.value = emptyList()
                    Log.d("Habitaciones", "No se encontraron habitaciones con esos filtros.")
                }
            } catch (e: Exception) {
                Log.e("Habitaciones", "Error al obtener habitaciones filtradas: ${e.message}")
                _habitaciones.value = emptyList()
            }
        }
    }
}