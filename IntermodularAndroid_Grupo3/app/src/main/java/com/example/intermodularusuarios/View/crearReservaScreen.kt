package com.example.intermodularusuarios.view

import android.annotation.SuppressLint
import android.os.Build
import android.util.Log
import androidx.annotation.RequiresApi
import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.AlertDialog
import androidx.compose.material3.Button
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.TopAppBar
import androidx.compose.material3.TopAppBarDefaults
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.getValue
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.NavController
import com.example.intermodularusuarios.R
import com.example.intermodularusuarios.model.Habitaciones
import com.example.intermodularusuarios.model.Usuario
import com.example.intermodularusuarios.viewmodel.ReservaViewModel
import java.time.LocalDate
import java.time.format.DateTimeFormatter
import java.time.temporal.ChronoUnit

@OptIn(ExperimentalMaterial3Api::class)
@RequiresApi(Build.VERSION_CODES.O)
@SuppressLint("UnusedMaterial3ScaffoldPaddingParameter")
@Composable
fun crearReservaScreen(
    navController: NavController,
    startDate: String,
    endDate: String,
    guests: Int
) {
    val reservaViewModel: ReservaViewModel = viewModel()

    var showDialogError by remember { mutableStateOf(false) }
    var showDialogExito by remember { mutableStateOf(false) }
    var showDialog by remember { mutableStateOf(false) }

    val formatter = DateTimeFormatter.ofPattern("d/M/yyyy")
    val startLocalDate = LocalDate.parse(startDate, formatter)
    val endLocalDate = LocalDate.parse(endDate, formatter)
    val days = ChronoUnit.DAYS.between(startLocalDate, endLocalDate).toInt() + 1
    val room = navController.previousBackStackEntry?.savedStateHandle?.get<Habitaciones>("habitacion")
    val usuario = navController.previousBackStackEntry?.savedStateHandle?.get<Usuario>("usuario")
    Log.d("comprobar", usuario?.dni.toString())
    val pricePerNight = room?.oferta
    val totalPrice = pricePerNight?.times(maxOf(days, 1))
    Log.d("GUEST", "$guests")
    Log.d("USUARIO", "${usuario?.dni}")

    Scaffold(
        topBar = {
            TopAppBar(
                title = {
                    Text(
                        "Reserva de Habitación",
                        style = MaterialTheme.typography.titleLarge.copy(fontSize = 24.sp)
                    )
                },
                colors = TopAppBarDefaults.topAppBarColors(
                    containerColor = Color(0xFF1A237E),
                    titleContentColor = Color.White
                ),
                navigationIcon = {
                    IconButton(onClick = { navController.navigate("vistaBuscador") },
                        modifier = Modifier.background(Color.Unspecified)) {
                        Icon(
                            painter = painterResource(id = R.drawable.flecha),
                            contentDescription = "Volver",
                            tint = Color.Black,
                            modifier = Modifier.size(30.dp)
                        )
                    }
                }
            )
        },
        bottomBar = {
            Button(
                onClick = { showDialog = true },
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(horizontal = 16.dp, vertical = 45.dp),
                shape = RoundedCornerShape(12.dp),
                colors = ButtonDefaults.buttonColors(containerColor = Color(0xFF1A237E))
            ) {
                Text(
                    "Realizar Reserva",
                    color = Color.White,
                    fontWeight = FontWeight.Bold,
                    fontSize = 24.sp
                )
            }
        }
    ) { innerPadding ->
        LazyColumn(
            modifier = androidx.compose.ui.Modifier
                .fillMaxSize()
                .padding(innerPadding),
            verticalArrangement = Arrangement.spacedBy(16.dp)
        ) {
            item {
                Card(
                    shape = RoundedCornerShape(16.dp),
                    elevation = CardDefaults.cardElevation(8.dp),
                    modifier = androidx.compose.ui.Modifier
                        .fillMaxWidth()
                        .padding(horizontal = 16.dp, vertical = 10.dp)
                ) {
                    Column(modifier = androidx.compose.ui.Modifier.padding(16.dp)) {
                        Image(
                            painter = painterResource(R.drawable.habitaciones),
                            contentDescription = "Habitación",
                            modifier = androidx.compose.ui.Modifier
                                .size(200.dp)
                                .align(Alignment.CenterHorizontally)
                                .fillMaxWidth()
                                .clip(RoundedCornerShape(12.dp))
                        )
                        Spacer(modifier = androidx.compose.ui.Modifier.height(15.dp))
                        room?.let {
                            Text(
                                it.nombre,
                                modifier = androidx.compose.ui.Modifier.padding(top = 10.dp),
                                style = MaterialTheme.typography.headlineSmall.copy(
                                    fontSize = 34.sp,
                                    fontWeight = FontWeight.Bold
                                )
                            )
                        }
                        Spacer(modifier = androidx.compose.ui.Modifier.height(5.dp))
                        room?.descripcion?.let {
                            Text(
                                it,
                                fontSize = 18.sp,
                                style = MaterialTheme.typography.bodyMedium,
                                color = Color.Gray
                            )
                        }
                        Spacer(modifier = androidx.compose.ui.Modifier.height(8.dp))
                        Row(horizontalArrangement = Arrangement.spacedBy(8.dp)) {
                            room?.let {
                                BadgeInfo(label = "Cuna", value = if (it.cuna) "Sí" else "No")
                            }
                            room?.let {
                                BadgeInfo(label = "Cama Extra", value = if (it.camaExtra) "Sí" else "No")
                            }
                        }
                    }
                }
            }
            item {
                Card(
                    shape = RoundedCornerShape(16.dp),
                    elevation = CardDefaults.cardElevation(8.dp),
                    modifier = androidx.compose.ui.Modifier
                        .fillMaxWidth()
                        .padding(horizontal = 16.dp, vertical = 10.dp)
                ) {
                    Column(modifier = androidx.compose.ui.Modifier.padding(16.dp)) {
                        Text(
                            "Detalles de la Reserva",
                            style = MaterialTheme.typography.titleMedium.copy(
                                fontSize = 24.sp,
                                fontWeight = FontWeight.Bold
                            )
                        )
                        Spacer(modifier = androidx.compose.ui.Modifier.height(8.dp))
                        DetailRow("Fecha de Inicio", startDate)
                        DetailRow("Fecha de Fin", endDate)
                        DetailRow("Huéspedes", "$guests")
                        Spacer(modifier = androidx.compose.ui.Modifier.height(8.dp))
                        Row(
                            modifier = androidx.compose.ui.Modifier.fillMaxWidth(),
                            horizontalArrangement = Arrangement.SpaceBetween
                        ) {
                            Text(
                                "Precio Total:",
                                style = MaterialTheme.typography.headlineSmall.copy(
                                    fontSize = 26.sp,
                                    fontWeight = FontWeight.Bold
                                )
                            )
                            Text(
                                "%.2f €".format(totalPrice),
                                style = MaterialTheme.typography.headlineSmall.copy(
                                    fontSize = 26.sp,
                                    fontWeight = FontWeight.Bold
                                )
                            )
                        }
                    }
                }
            }
        }
        if (showDialogExito){
            AlertDialog(
                onDismissRequest = {
                    showDialogExito = false
                    navController.navigate("vistaBuscador")
                },
                confirmButton = {
                    Button(onClick = {
                        showDialogExito = false
                        navController.navigate("vistaBuscador")
                    }) {
                        Text("Aceptar")
                    }
                },
                title = { Text("Reserva Añadida") },
                text = { Text("La reserva se ha añadido correctamente") }
            )
        }
        if (showDialogError){
            AlertDialog(
                onDismissRequest = { showDialogError = false },
                confirmButton = {
                    Button(onClick = { showDialogError = false }) {
                        Text("Aceptar")
                    }
                },
                title = { Text("ERROR") },
                text = { Text("No se ha podido crear la reserva, intentalo más tarde") }
            )
        }
        if (showDialog) {
            AlertDialog(
                onDismissRequest = { showDialog = false },
                confirmButton = {
                    Button(onClick = {
                        if (room != null && usuario != null) {
                            reservaViewModel.createReserva(
                                dni = usuario.dni,
                                idHab = room.id,
                                fechaIni = startDate,
                                fechaFin = endDate,
                                numHuespedes = guests
                            )
                            showDialogExito = true
                        } else {
                            showDialogError = true
                            Log.e("Reserva", "No se han recuperado los datos de la habitación o el usuario.")
                        }
                        showDialog = false
                    }) {
                        Text("Confirmar", fontSize = 18.sp)
                    }
                },
                dismissButton = {
                    Button(onClick = { showDialog = false }) {
                        Text("Cancelar", fontSize = 18.sp)
                    }
                },
                title = { Text("Confirmación", fontSize = 20.sp, fontWeight = FontWeight.Bold) },
                text = { Text("¿Está seguro de que desea realizar la reserva?", fontSize = 18.sp) }
            )
        }
    }
}

@Composable
fun DetailRow(label: String, value: String) {
    Row(
        modifier = androidx.compose.ui.Modifier.fillMaxWidth(),
        horizontalArrangement = Arrangement.SpaceBetween
    ) {
        Text(label, fontWeight = FontWeight.SemiBold, fontSize = 18.sp)
        Text(value, color = Color.Gray, fontSize = 18.sp)
    }
}

@Composable
fun BadgeInfo(label: String, value: String) {
    Card(
        shape = RoundedCornerShape(50),
        colors = CardDefaults.cardColors(containerColor = Color(0xFFE3F2FD)),
        modifier = androidx.compose.ui.Modifier.padding(4.dp)
    ) {
        Text(
            text = "$label: $value",
            modifier = androidx.compose.ui.Modifier.padding(horizontal = 12.dp, vertical = 4.dp),
            style = MaterialTheme.typography.bodySmall.copy(fontSize = 16.sp),
            color = Color.Black
        )
    }
}
