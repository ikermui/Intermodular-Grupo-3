package com.example.intermodular.view

import android.annotation.SuppressLint
import androidx.compose.animation.animateContentSize
import androidx.compose.foundation.*
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ArrowBack
import androidx.compose.material.icons.filled.ShoppingCart
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.*
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.*
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.navigation.NavController
import com.example.intermodular.R
import com.example.intermodular.decodeBase64Image
import com.example.intermodular.viewmodel.HabitacionesViewModel

@OptIn(ExperimentalMaterial3Api::class)
@SuppressLint("UnusedMaterial3ScaffoldPaddingParameter")
@Composable
fun habitacion(
    habitacionId: String,
    navController: NavController,
    viewModel: HabitacionesViewModel = androidx.lifecycle.viewmodel.compose.viewModel()
) {
    LaunchedEffect(habitacionId) {
        viewModel.cargarHabitacionPorId(habitacionId)
    }

    val habitacion by viewModel.habitacionSeleccionada.collectAsState()

    Scaffold(
        containerColor = Color.Transparent,
        topBar = {
            CenterAlignedTopAppBar(
                title = { Text("Detalles", fontSize = 25.sp, fontWeight = FontWeight.Bold, color = Color.White) },
                navigationIcon = {
                    IconButton(onClick = { navController.popBackStack() }) {
                        Icon(Icons.Default.ArrowBack, contentDescription = "Volver", tint = Color.White)
                    }
                },
                colors = TopAppBarDefaults.topAppBarColors(containerColor = Color.Transparent)
            )
        }
    ) { paddingValues ->
        Box(
            modifier = Modifier
                .fillMaxSize()
                .background(
                    Brush.verticalGradient(
                        colors = listOf(Color(0xFF1A237E), Color(0xFF7986CB))
                    )
                )
                .padding(paddingValues)
        ) {
            habitacion?.let { habitacionData ->
                Column(
                    modifier = Modifier
                        .fillMaxWidth()
                        .verticalScroll(rememberScrollState())
                ) {
                    Image(
                        bitmap = decodeBase64Image(habitacionData.imagen)?.asImageBitmap()
                            ?: ImageBitmap(1, 1),
                        contentDescription = habitacionData.nombre,
                        modifier = Modifier
                            .fillMaxWidth()
                            .aspectRatio(16f / 11f),
                        contentScale = ContentScale.Crop
                    )

                    Spacer(modifier = Modifier.height(70.dp))

                    Card(
                        modifier = Modifier
                            .fillMaxWidth()
                            .padding(horizontal = 16.dp)
                            .clip(RoundedCornerShape(16.dp))
                            .background(Color.White.copy(alpha = 0.9f))
                            .animateContentSize(),
                        elevation = CardDefaults.cardElevation(8.dp)
                    ) {
                        Column(modifier = Modifier.padding(16.dp)) {
                            Text(
                                text = habitacionData.nombre,
                                fontSize = 26.sp,
                                fontWeight = FontWeight.Bold,
                                color = Color(0xFF1A237E)
                            )
                            Spacer(modifier = Modifier.height(8.dp))
                            Text(
                                text = "Para ${habitacionData.huespedes} huésped(es)",
                                fontSize = 22.sp,
                                fontWeight = FontWeight.Medium
                            )
                            Spacer(modifier = Modifier.height(12.dp))
                            Text(
                                text = habitacionData.descripcion,
                                fontSize = 20.sp,
                                textAlign = TextAlign.Justify,
                                color = Color.DarkGray
                            )
                        }
                    }

                    Spacer(modifier = Modifier.height(12.dp))

                    Text(
                        text = "Desde ${habitacionData.oferta - 10}€ por noche",
                        fontSize = 22.sp,
                        fontWeight = FontWeight.Bold,
                        color = Color.White,
                        modifier = Modifier.padding(horizontal = 16.dp).align(Alignment.CenterHorizontally)
                    )
                }
            } ?: CenteredLoadingText()
        }
    }
}

@Composable
fun CenteredLoadingText() {
    Box(modifier = Modifier.fillMaxSize(), contentAlignment = Alignment.Center) {
        Text("Cargando...", fontSize = 20.sp, color = Color.White)
    }
}
