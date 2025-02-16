require('dotenv').config();
const express = require('express');
const mongoose = require('mongoose');
const bodyParser = require('body-parser');
const path = require('path');
const PORT = process.env.PORT || 3000;
const mongoString = process.env.DATABASE_URL;
const mongoStringLocal = process.env.DATABASE_URL_LOCAL;
const routerHabitaciones = require('./routes/routerHabitaciones.js');
const routerUsuarios = require('./routes/routerUsuarios.js');

//middleware para acceder a usuarios
//console.log(`mongoString: ${mongoString}`)
//Conexión a Mongodb
mongoose.connect(mongoString);
const database = mongoose.connection;
database.on('error', (error) => {
console.log(error)
})
database.once('connected', () => {
console.log('Database Connected');
})

const app = express();

//middleware
//app.use(express.json());
//Nos permite manejar peticiones y enviar respuesta en formato json
app.use(bodyParser.json());
//De esta manera indicamos que no vamos a recibir peticiones enviadas
//directamente de un formulario, sino que sera todo enviado en json
app.use(bodyParser.urlencoded({extended: false}))
app.listen(3000, () => {
console.log(`Server Started at ${PORT}`)
})

app.use('/habitaciones', routerHabitaciones);
app.use('/usuarios', routerUsuarios);