package com.example.intermodularusuarios.view

import android.annotation.SuppressLint
import android.app.DatePickerDialog
import android.graphics.BitmapFactory
import android.util.Base64
import android.util.Log
import android.widget.DatePicker
import androidx.compose.foundation.BorderStroke
import androidx.compose.foundation.Image
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.BottomAppBar
import androidx.compose.material3.Button
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.CenterAlignedTopAppBar
import androidx.compose.material3.DropdownMenuItem
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.ExposedDropdownMenuBox
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.TextFieldDefaults
import androidx.compose.material3.TopAppBarDefaults
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.asImageBitmap
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.input.TextFieldValue
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.NavController
import androidx.navigation.compose.currentBackStackEntryAsState
import com.example.intermodularusuarios.R
import com.example.intermodularusuarios.model.Habitaciones
import com.example.intermodularusuarios.model.Usuario
import com.example.intermodularusuarios.viewmodel.ReservaViewModel
import java.text.SimpleDateFormat
import java.util.Calendar
import java.util.Locale

@OptIn(ExperimentalMaterial3Api::class)
@SuppressLint("UnusedMaterial3ScaffoldPaddingParameter")
@Composable
fun VistaBuscador(navController: NavController, reservaViewModel: ReservaViewModel = viewModel()) {
    var searchQuery by remember { mutableStateOf(TextFieldValue()) }
    var fechaInicio by remember { mutableStateOf("") }
    var startDate by remember { mutableStateOf("") }
    var endDate by remember { mutableStateOf("") }
    var selectedGuests by remember { mutableStateOf("") }
    val guestOptions = listOf("1", "2", "3", "4")
    var expanded by remember { mutableStateOf(false) }
    var errorMessage by remember { mutableStateOf("") }

    // Recupera el usuario desde el backStack o usa uno por defecto
    val usuario = navController.previousBackStackEntry
        ?.savedStateHandle
        ?.get<Usuario>("usuario")
        ?: Usuario("00000000Z", "", "", "cliente", "", "", "", "", "", null)

    navController.currentBackStackEntry
        ?.savedStateHandle
        ?.set("usuario", usuario)

    val localContext = LocalContext.current
    val calendar = Calendar.getInstance()
    val startDatePickerDialog = DatePickerDialog(
        localContext,
        { _: DatePicker, year: Int, month: Int, day: Int ->
            startDate = "$day/${month + 1}/$year"
        },
        calendar.get(Calendar.YEAR),
        calendar.get(Calendar.MONTH),
        calendar.get(Calendar.DAY_OF_MONTH)
    ).apply { datePicker.minDate = System.currentTimeMillis() }

    val endDatePickerDialog = DatePickerDialog(
        localContext,
        { _: DatePicker, year: Int, month: Int, day: Int ->
            endDate = "$day/${month + 1}/$year"
        },
        calendar.get(Calendar.YEAR),
        calendar.get(Calendar.MONTH),
        calendar.get(Calendar.DAY_OF_MONTH)
    ).apply { datePicker.minDate = System.currentTimeMillis() }

    fun buscarReservas() {
        if (startDate.isEmpty() || endDate.isEmpty() || selectedGuests.isEmpty()) {
            errorMessage = "Todos los campos deben estar rellenados."
            return
        }
        val dateFormat = SimpleDateFormat("dd/MM/yyyy", Locale.getDefault())
        val start = dateFormat.parse(startDate)
        val end = dateFormat.parse(endDate)
        if (start != null && end != null && start.after(end)) {
            errorMessage = "La fecha de inicio debe ser anterior a la fecha de fin."
            return
        }
        errorMessage = ""
        reservaViewModel.getReservasByFilters(
            dni = null,
            idHab = null,
            fechaIni = startDate,
            fechaFin = endDate,
            numHuespedes = null
        )
    }

    Scaffold(
        topBar = {
            CenterAlignedTopAppBar(
                title = {
                    Text(
                        text = "Buscador de Reservas",
                        fontSize = 26.sp,
                        fontWeight = FontWeight.ExtraBold,
                        letterSpacing = 1.sp,
                        modifier = androidx.compose.ui.Modifier.padding(bottom = 4.dp)
                    )
                },
                colors = TopAppBarDefaults.centerAlignedTopAppBarColors(
                    containerColor = Color(0xFF64B5F6)
                )
            )
        },
        containerColor = Color(0xFFF1F3F5),
        bottomBar = {
            val currentRoute = navController.currentBackStackEntryAsState().value?.destination?.route
            BottomAppBar(
                containerColor = Color(0xFF64B5F6),
                contentColor = Color(0xFF1976D2),
                modifier = androidx.compose.ui.Modifier.height(80.dp)
            ) {
                Row(
                    modifier = androidx.compose.ui.Modifier.fillMaxWidth(),
                    horizontalArrangement = Arrangement.SpaceAround
                ) {
                    FooterButton(navController, "vistaHabitaciones", R.drawable.info, currentRoute)
                    FooterButton(navController, "vistaBuscador", R.drawable.home, currentRoute)
                    FooterButton(navController, "DatosUser", R.drawable.user, currentRoute)
                }
            }
        }
    ) { paddingValues ->
        Column(
            modifier = androidx.compose.ui.Modifier
                .fillMaxSize()
                .padding(paddingValues)
                .padding(horizontal = 16.dp, vertical = 10.dp),
            verticalArrangement = Arrangement.spacedBy(20.dp)
        ) {
            Card(
                modifier = androidx.compose.ui.Modifier.fillMaxWidth(),
                shape = RoundedCornerShape(16.dp),
                colors = CardDefaults.cardColors(containerColor = Color(0xDDB0BEC5))
            ) {
                Column(
                    modifier = androidx.compose.ui.Modifier
                        .fillMaxWidth()
                        .padding(20.dp),
                    verticalArrangement = Arrangement.spacedBy(16.dp)
                ) {
                    OutlinedTextField(
                        value = startDate,
                        onValueChange = { },
                        label = { Text("Fecha de inicio") },
                        readOnly = true,
                        modifier = androidx.compose.ui.Modifier.fillMaxWidth(),
                        colors = TextFieldDefaults.outlinedTextFieldColors(
                            focusedBorderColor = Color(0xFF1A237E),
                            unfocusedBorderColor = Color(0xFF000000),
                            cursorColor = Color(0xFF1A237E),
                        ),
                        trailingIcon = {
                            IconButton(
                                onClick = { startDatePickerDialog.show() },
                                modifier = androidx.compose.ui.Modifier.size(50.dp)
                            ) {
                                Icon(
                                    painter = painterResource(id = R.drawable.calendar),
                                    tint = Color.Unspecified,
                                    contentDescription = "Seleccionar fecha",
                                    modifier = androidx.compose.ui.Modifier.size(38.dp)
                                )
                            }
                        }
                    )
                    OutlinedTextField(
                        value = endDate,
                        onValueChange = { },
                        label = { Text("Fecha de fin") },
                        readOnly = true,
                        modifier = androidx.compose.ui.Modifier.fillMaxWidth(),
                        colors = TextFieldDefaults.outlinedTextFieldColors(
                            focusedBorderColor = Color(0xFF1A237E),
                            unfocusedBorderColor = Color(0xFF000000),
                            cursorColor = Color(0xFF1A237E),
                        ),
                        trailingIcon = {
                            IconButton(
                                onClick = { endDatePickerDialog.show() },
                                modifier = androidx.compose.ui.Modifier.size(50.dp)
                            ) {
                                Icon(
                                    painter = painterResource(id = R.drawable.calendar),
                                    tint = Color.Unspecified,
                                    contentDescription = "Seleccionar fecha",
                                    modifier = androidx.compose.ui.Modifier.size(38.dp)
                                )
                            }
                        }
                    )
                    ExposedDropdownMenuBox(
                        expanded = expanded,
                        onExpandedChange = { expanded = !expanded }
                    ) {
                        OutlinedTextField(
                            value = selectedGuests,
                            onValueChange = { },
                            label = { Text("Número de Huéspedes") },
                            readOnly = true,
                            modifier = androidx.compose.ui.Modifier
                                .fillMaxWidth()
                                .menuAnchor(),
                            colors = TextFieldDefaults.outlinedTextFieldColors(
                                focusedBorderColor = Color(0xFF1A237E),
                                unfocusedBorderColor = Color(0xFF000000),
                                cursorColor = Color(0xFF1A237E)
                            )
                        )
                        ExposedDropdownMenu(
                            expanded = expanded,
                            onDismissRequest = { expanded = false }
                        ) {
                            guestOptions.forEach { option ->
                                DropdownMenuItem(
                                    text = { Text(option) },
                                    onClick = {
                                        reservaViewModel.numHuespedesSeleccionados.value = option.toIntOrNull()
                                        selectedGuests = option
                                        expanded = false
                                    }
                                )
                            }
                        }
                    }
                    if (errorMessage.isNotEmpty()) {
                        Text(
                            text = errorMessage,
                            color = Color.Red,
                            modifier = androidx.compose.ui.Modifier.align(Alignment.CenterHorizontally),
                            fontSize = 14.sp
                        )
                    }
                }
            }
            Button(
                modifier = androidx.compose.ui.Modifier
                    .fillMaxWidth()
                    .height(46.dp),
                onClick = { buscarReservas() },
                shape = RoundedCornerShape(12.dp),
                colors = ButtonDefaults.buttonColors(containerColor = Color(0xFF1976D2))
            ) {
                Text("Buscar", color = Color.White, fontSize = 18.sp)
            }
            HabitacionesFromReservasScreen(
                reservaViewModel = reservaViewModel,
                navController = navController,
                startDate = startDate,
                endDate = endDate,
                selectedGuests = selectedGuests
            )
        }
    }
}

@Composable
fun HabitacionesFromReservasScreen(
    reservaViewModel: ReservaViewModel = viewModel(),
    navController: NavController,
    startDate: String,
    endDate: String,
    selectedGuests: String
) {
    val reservas by reservaViewModel.reservasFiltradas.collectAsState()
    val habitaciones = remember { mutableStateListOf<Habitaciones>() }

    LaunchedEffect(reservas) {
        val distinctRoomIds = reservas.map { it.id_hab }.distinct()
        val numHuespedes = reservaViewModel.numHuespedesSeleccionados.value ?: return@LaunchedEffect
        reservaViewModel.getHabitacionesByHuespedes(numHuespedes) { habitacionesFiltradas ->
            val habitacionesDisponibles = habitacionesFiltradas.filter { it.id !in distinctRoomIds }
            habitaciones.clear()
            habitaciones.addAll(habitacionesDisponibles)
            if (habitacionesDisponibles.isEmpty()) {
                reservaViewModel.mostrarVacio.value = true
            }
        }
    }

    Column(
        modifier = androidx.compose.ui.Modifier
            .fillMaxSize()
            .padding(vertical = 8.dp)
    ) {
        if (habitaciones.isEmpty() && reservaViewModel.mostrarVacio.value == true) {
            Column(
                modifier = androidx.compose.ui.Modifier.fillMaxSize(),
                verticalArrangement = Arrangement.Center,
                horizontalAlignment = Alignment.CenterHorizontally
            ) {
                Text("No se encontraron habitaciones.", fontSize = 18.sp)
                Spacer(modifier = androidx.compose.ui.Modifier.height(16.dp))
                Image(
                    painter = painterResource(id = android.R.drawable.ic_menu_report_image),
                    contentDescription = "No se encontraron reservas",
                    modifier = androidx.compose.ui.Modifier.size(250.dp)
                )
            }
        } else {
            LazyColumn(
                contentPadding = androidx.compose.foundation.layout.PaddingValues(vertical = 8.dp),
                verticalArrangement = Arrangement.spacedBy(12.dp)
            ) {
                items(habitaciones) { room ->
                    RoomItem(room, navController, startDate, endDate, selectedGuests)
                }
            }
        }
    }
}

@Composable
fun RoomItem(
    room: Habitaciones,
    navController: NavController,
    startDate: String,
    endDate: String,
    selectedGuests: String
) {
    Card(
        modifier = androidx.compose.ui.Modifier
            .fillMaxWidth()
            .padding(horizontal = 8.dp),
        shape = RoundedCornerShape(16.dp),
        colors = CardDefaults.cardColors(containerColor = Color(0xFFDDE1E6)),
        elevation = CardDefaults.cardElevation(defaultElevation = 4.dp)
    ) {
        Column(
            modifier = androidx.compose.ui.Modifier.padding(16.dp),
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.spacedBy(12.dp)
        ) {
            Base64Image(base64String = room.imagen)
            Text(
                text = room.nombre,
                style = MaterialTheme.typography.titleLarge,
                fontWeight = FontWeight.Bold
            )
            Text(
                text = String.format("%.2f €/noche", room.oferta),
                style = MaterialTheme.typography.bodyLarge,
                color = Color(0xFF1A237E)
            )
            Button(
                onClick = { navegarCrearReserva(navController, room, startDate, endDate, selectedGuests) },
                modifier = androidx.compose.ui.Modifier.fillMaxWidth(),
                shape = RoundedCornerShape(12.dp),
                colors = ButtonDefaults.buttonColors(containerColor = Color(0xFF4FB3C1))
            ) {
                Text("Reservar Habitación", color = Color.White)
            }
        }
    }
}

fun navegarCrearReserva(
    navController: NavController,
    room: Habitaciones,
    startDate: String,
    endDate: String,
    selectedGuests: String
) {
    try {
        val fecha_ini = startDate.replace("/", "-")
        val fecha_fin = endDate.replace("/", "-")
        navController.currentBackStackEntry?.savedStateHandle?.set("habitacion", room)
        navController.navigate("crearReserva/$fecha_ini/$fecha_fin/$selectedGuests")
    } catch (e: Exception) {
        Log.d("ERRORNav", "${e.message}")
    }
}

@Composable
fun FooterButton(navController: NavController, route: String, iconRes: Int, currentRoute: String?) {
    val isSelected = currentRoute == route

    Box(
        modifier = androidx.compose.ui.Modifier.fillMaxHeight(),
        contentAlignment = Alignment.Center
    ) {
        IconButton(onClick = { navController.navigate(route) }) {
            Icon(
                painter = painterResource(id = iconRes),
                contentDescription = null,
                tint = if (isSelected) Color.Black else Color.DarkGray,
                modifier = androidx.compose.ui.Modifier.size(36.dp)
            )
        }
    }
}

@Composable
fun Base64Image(base64String: String) {
    val imageBitmap = remember(base64String) { decodeBase64ToBitmap(base64String) }
    imageBitmap?.let {
        Box(
            modifier = androidx.compose.ui.Modifier
                .size(150.dp)
                .clip(RoundedCornerShape(12.dp))
                .border(BorderStroke(2.dp, Color.White))
        ) {
            Image(
                bitmap = it.asImageBitmap(),
                contentDescription = "Imagen de la habitación",
                contentScale = ContentScale.Crop,
                modifier = androidx.compose.ui.Modifier.fillMaxSize()
            )
        }
    }
}

fun decodeBase64ToBitmap(base64String: String): android.graphics.Bitmap? {
    return try {
        val decodedBytes = Base64.decode(base64String, Base64.DEFAULT)
        BitmapFactory.decodeByteArray(decodedBytes, 0, decodedBytes.size)
    } catch (e: Exception) {
        e.printStackTrace()
        null
    }
}
