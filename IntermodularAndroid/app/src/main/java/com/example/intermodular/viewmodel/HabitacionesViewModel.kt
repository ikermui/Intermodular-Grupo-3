package com.example.intermodular.viewmodel

import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.intermodular.models.Habitacion
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch

class HabitacionesViewModel : ViewModel() {
    private val repository = HabitacionesRepository()
    private val _habitaciones = MutableStateFlow<List<Habitacion>>(emptyList())
    val habitaciones: StateFlow<List<Habitacion>> get() = _habitaciones

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

    private val _habitacionSeleccionada = MutableStateFlow<Habitacion?>(null)
    val habitacionSeleccionada: StateFlow<Habitacion?> get() = _habitacionSeleccionada

    fun cargarHabitacionPorId(id: String) {
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