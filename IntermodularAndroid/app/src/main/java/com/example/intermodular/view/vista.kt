package com.example.intermodular.view

import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.lazy.rememberLazyListState
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Clear
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.*
import androidx.compose.ui.draw.shadow
import androidx.compose.ui.graphics.*
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.*
import androidx.compose.ui.unit.*
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.NavController
import androidx.navigation.compose.currentBackStackEntryAsState
import com.example.intermodular.R
import com.example.intermodular.decodeBase64Image
import com.example.intermodular.models.Habitacion
import com.example.intermodular.viewmodel.HabitacionesViewModel
import androidx.compose.material3.TriStateCheckbox
import androidx.compose.ui.state.ToggleableState

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun ListaHabitaciones(
    navController: NavController,
    viewModel: HabitacionesViewModel = viewModel()
) {
    val habitaciones by viewModel.habitaciones.collectAsState()
    //val nombres = habitaciones.distinctBy { it.nombre }
    val scrollState = rememberLazyListState()

    var showFilters by remember { mutableStateOf(false) } // Estado para mostrar el pop-up

    // Estados para los filtros
    var nombre by remember { mutableStateOf("") }
    var numHuespedes by remember { mutableStateOf(-1) } // Cantidad de huéspedes (1 a 4)
    var cunaState by remember { mutableStateOf(ToggleableState.Indeterminate) }
    var camaExtraState by remember { mutableStateOf(ToggleableState.Indeterminate) }
    var expanded by remember { mutableStateOf(false) } // Control del combo box

    val currentRoute = navController.currentBackStackEntryAsState().value?.destination?.route

    fun habitacionesFiltradas() {
        val cuna = when (cunaState) {
            ToggleableState.On -> true
            ToggleableState.Off -> false
            ToggleableState.Indeterminate -> null
        }

        val camaExtra = when (camaExtraState) {
            ToggleableState.On -> true
            ToggleableState.Off -> false
            ToggleableState.Indeterminate -> null
        }

        viewModel.cargarHabitacionesFiltradas(cuna = cuna, cama = camaExtra, huespedes = numHuespedes, nombre = nombre)
    }

    Scaffold(
        topBar = {
            CenterAlignedTopAppBar(
                title = {
                    Text(
                        text = "Habitaciones",
                        fontSize = 24.sp,
                        fontWeight = FontWeight.SemiBold,
                        letterSpacing = 2.sp
                    )
                },
                colors = TopAppBarDefaults.topAppBarColors(containerColor = Color(0xFF64B5F6))
            )
        },
        bottomBar = {
            BottomAppBar(
                containerColor = Color(0xFF64B5F6),
                contentColor = Color(0xFF1976D2),
                modifier = Modifier.height(80.dp)
            ) {
                Row(
                    modifier = Modifier.fillMaxWidth(),
                    horizontalArrangement = Arrangement.SpaceAround
                ) {
                    FooterButton(navController, "vistaHabitaciones", R.drawable.info, currentRoute)
                    FooterButton(navController, "home", R.drawable.home, currentRoute)
                    FooterButton(navController, "perfil", R.drawable.user, currentRoute)
                }
            }
        }
    ) { innerPadding ->
        Box(
            modifier = Modifier
                .fillMaxSize()
                .background(Color(0xFFEEEEEE))
                .padding(innerPadding)
        ) {
            Column(
                modifier = Modifier
                    .fillMaxSize()
                    .padding(horizontal = 12.dp)
            ) {
                Row(
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(vertical = 8.dp),
                    verticalAlignment = Alignment.CenterVertically,
                    horizontalArrangement = Arrangement.SpaceBetween
                ) {
                    IconButton(onClick = { habitacionesFiltradas() }) {
                        Icon(
                            painter = painterResource(R.drawable.search),
                            contentDescription = null,
                            tint = Color.Black,
                            modifier = Modifier.size(36.dp)
                        )
                    }

                    TextField(
                        value = nombre,
                        onValueChange = { nombre = it },
                        modifier = Modifier
                            .weight(1f)
                            .padding(10.dp, 0.dp, 8.dp, 0.dp),
                        placeholder = { Text("Buscar por nombre...") },
                        colors = TextFieldDefaults.textFieldColors(
                            containerColor = Color.White,
                            focusedIndicatorColor = Color.Transparent,
                            unfocusedIndicatorColor = Color.Transparent
                        ),
                        shape = RoundedCornerShape(8.dp),
                        singleLine = true
                    )

                    IconButton(
                        onClick = { showFilters = true }
                    ) {
                        Icon(
                            painter = painterResource(R.drawable.puntos),
                            contentDescription = null,
                            tint = Color.Black,
                            modifier = Modifier.size(36.dp)
                        )
                    }
                }

                LazyColumn(
                    modifier = Modifier.fillMaxSize(),
                    state = scrollState
                ) {
                    items(items = habitaciones, key = { it.id }) { habitacion ->
                        HabitacionItem(habitacion, navController)
                        Spacer(modifier = Modifier.height(10.dp))
                    }
                }
            }
        }

        // Pop-up de filtros
        if (showFilters) {
            AlertDialog(
                onDismissRequest = { showFilters = false },
                title = { Text("Filtros") },
                text = {
                    Column {
                        Text("Cantidad de huéspedes", modifier = Modifier.padding(bottom = 8.dp))

                        ExposedDropdownMenuBox(
                            expanded = expanded,
                            onExpandedChange = { expanded = !expanded }
                        ) {
                            TextField(
                                value = if (numHuespedes == -1) {
                                    "Sin seleccionar"
                                } else {
                                    "$numHuespedes huésped${if (numHuespedes > 1) "es" else ""}"
                                },
                                onValueChange = {},
                                readOnly = true,
                                modifier = Modifier.menuAnchor(),
                                trailingIcon = {
                                    ExposedDropdownMenuDefaults.TrailingIcon(expanded = expanded)
                                }
                            )
                            ExposedDropdownMenu(
                                expanded = expanded,
                                onDismissRequest = { expanded = false }
                            ) {
                                DropdownMenuItem(text = { Text(text = "Sin seleccionar") }, onClick = {
                                    numHuespedes = -1
                                    expanded = false
                                })
                                for (i in 1..4) {
                                    DropdownMenuItem(
                                        text = { Text("$i huésped${if (i > 1) "es" else ""}") },
                                        onClick = {
                                            numHuespedes = i
                                            expanded = false
                                        }
                                    )
                                }
                            }
                        }

                        Row(verticalAlignment = Alignment.CenterVertically, modifier = Modifier.padding(top = 25.dp)) {
                            Box(contentAlignment = Alignment.Center) {
                                TriStateCheckbox(
                                    state = cunaState,
                                    onClick = {
                                        cunaState = when (cunaState) {
                                            ToggleableState.Off -> ToggleableState.On
                                            ToggleableState.On -> ToggleableState.Indeterminate
                                            ToggleableState.Indeterminate -> ToggleableState.Off
                                        }
                                    },
                                    colors = CheckboxDefaults.colors(
                                        checkedColor = Color.Green,  // Verde cuando está activado
                                        uncheckedColor = Color.Black, // Gris cuando está desactivado
                                    ),
                                    modifier = Modifier.size(30.dp)
                                )
                                // Si está en estado Off, mostramos la cruz roja
                                if (cunaState == ToggleableState.Off) {
                                    Icon(
                                        imageVector = Icons.Default.Clear,
                                        contentDescription = "Desactivado",
                                        tint = Color.Black,
                                        modifier = Modifier
                                            .size(20.dp)
                                            .background(Color.Red)
                                    )
                                }
                                // Si está en estado Indeterminate, cambiamos el fondo a amarillo
                                if (cunaState == ToggleableState.Indeterminate) {
                                    Icon(
                                        painter = painterResource(id = R.drawable.guion),
                                        contentDescription = "Desactivado",
                                        tint = Color.White,
                                        modifier = Modifier
                                            .size(20.dp)
                                            .background(Color.Black)
                                    )
                                }
                            }
                            Text("Cuna", modifier = Modifier.padding(start = 8.dp))
                        }

                        Row(verticalAlignment = Alignment.CenterVertically, modifier = Modifier.padding(top = 20.dp)) {
                            Box(contentAlignment = Alignment.Center) {
                                TriStateCheckbox(
                                    state = camaExtraState,
                                    onClick = {
                                        camaExtraState = when (camaExtraState) {
                                            ToggleableState.Off -> ToggleableState.On
                                            ToggleableState.On -> ToggleableState.Indeterminate
                                            ToggleableState.Indeterminate -> ToggleableState.Off
                                        }
                                    },
                                    colors = CheckboxDefaults.colors(
                                        checkedColor = Color.Green,  // Verde cuando está activado
                                        uncheckedColor = Color.Black, // Gris cuando está desactivado
                                    ),
                                    modifier = Modifier.size(30.dp)
                                )
                                // Si está en estado Off, mostramos la cruz roja
                                if (camaExtraState == ToggleableState.Off) {
                                    Icon(
                                        imageVector = Icons.Default.Clear,
                                        contentDescription = "Desactivado",
                                        tint = Color.Black,
                                        modifier = Modifier
                                            .size(20.dp)
                                            .background(Color.Red)
                                    )
                                }
                                // Si está en estado Indeterminate, cambiamos el fondo a amarillo
                                if (camaExtraState == ToggleableState.Indeterminate) {
                                    Icon(
                                        painter = painterResource(id = R.drawable.guion),
                                        contentDescription = "Desactivado",
                                        tint = Color.White,
                                        modifier = Modifier
                                            .size(20.dp)
                                            .background(Color.Black)
                                    )
                                }
                            }
                            Text("Cama extra", modifier = Modifier.padding(start = 8.dp))
                        }

                       /*Row(
                            verticalAlignment = Alignment.CenterVertically,
                            modifier = Modifier.padding(vertical = 8.dp)
                        ) {
                            TriStateCheckbox(
                                state = camaExtraState,
                                onClick = {
                                    camaExtraState = when (camaExtraState) {
                                        ToggleableState.Off -> ToggleableState.On
                                        ToggleableState.On -> ToggleableState.Indeterminate
                                        ToggleableState.Indeterminate -> ToggleableState.Off
                                    }
                                }
                            )
                            Text(
                                "Cama extra",
                                modifier = Modifier.padding(start = 8.dp),
                                color = when (camaExtraState) {
                                    ToggleableState.Off -> Color.Red
                                    ToggleableState.On -> Color.Green
                                    ToggleableState.Indeterminate -> Color.Gray
                                }
                            )
                        }*/
                    }
                },
                confirmButton = {
                    Button(onClick = { showFilters = false },
                        colors = ButtonDefaults.buttonColors(containerColor = Color(0xFF1976D2))) {
                        Text("Aplicar")
                    }
                },
                dismissButton = {
                    TextButton(onClick = { showFilters = false }) {
                        Text("Cancelar", color = Color(0xFF1976D2))
                    }
                }
            )
        }
    }
}




@Composable
fun HabitacionItem(habitacion: Habitacion, navController: NavController) {
    Card(
        modifier = Modifier
            .fillMaxWidth()
            .padding(8.dp),
        shape = RoundedCornerShape(12.dp),
        colors = CardDefaults.cardColors(containerColor = Color.White),
        elevation = CardDefaults.cardElevation(5.dp)
    ) {
        Column(modifier = Modifier.padding(16.dp)) {
            Image(
                bitmap = decodeBase64Image(habitacion.imagen)?.asImageBitmap() ?: ImageBitmap(1, 1),
                contentDescription = habitacion.nombre,
                modifier = Modifier
                    .fillMaxWidth()
                    .height(140.dp)
                    .background(Color(0xFF64B5F6), RoundedCornerShape(8.dp)), // Azul más intenso
                contentScale = ContentScale.Crop
            )

            Spacer(modifier = Modifier.height(12.dp))

            Text(
                text = habitacion.nombre,
                fontSize = 18.sp,
                fontWeight = FontWeight.Medium,
                color = Color(0xFF2C3E50) // Azul oscuro
            )

            Spacer(modifier = Modifier.height(4.dp))

            Text(
                text = "Desde ${habitacion.oferta - 10}€ / noche",
                fontSize = 14.sp,
                color = Color(0xFF455A64) // Gris oscuro
            )

            Spacer(modifier = Modifier.height(10.dp))

            Button(
                onClick = { navController.navigate("habitacion/${habitacion.id}") },
                modifier = Modifier.fillMaxWidth(),
                shape = RoundedCornerShape(8.dp),
                colors = ButtonDefaults.buttonColors(containerColor = Color(0xFF1976D2)) // Azul fuerte
            ) {
                Text(text = "Ver detalles", fontSize = 14.sp, color = Color.White)
            }
        }
    }
}

@Composable
fun FooterButton(navController: NavController, route: String, iconRes: Int, currentRoute: String?) {
    val isSelected = currentRoute == route

    Box(
        modifier = Modifier.fillMaxHeight(),
        contentAlignment = Alignment.Center
    ) {
        IconButton(onClick = { navController.navigate(route) }) {
            Icon(
                painter = painterResource(id = iconRes),
                contentDescription = null,
                tint = if (isSelected) Color.Unspecified else Color.DarkGray,
                modifier = Modifier.size(36.dp)
            )
        }
    }
}
