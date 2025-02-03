const express = require('express');
const mongoose = require('mongoose');
const bodyParser = require('body-parser');
const path = require('path');
const app = express();
const reservasSchema = require('./models/models'); 

app.use(bodyParser.json());

//De esta manera indicamos que no vamos a recibir peticiones enviadas directamente de un formulario, sino que sera todo enviado en json

app.use(bodyParser.urlencoded({extended: false}))
app.listen(3000, () => {
console.log(`Server Started at ${PORT}`)
})

const routerReservas = require('./routes/router');
//middleware para acceder a usuarios
app.use('/reservas', routerReservas)

require('dotenv').config();

const PORT = process.env.PORT || 3000;
const mongoString = process.env.DATABASE_URL
const mongoStringLocal = process.env.DATABASE_URL_LOCAL

mongoose.connect(mongoString);
const database = mongoose.connection;

database.on('error', (error) => {
    console.log(error)
})

database.once('connected', () => {
    console.log('Database Connected');
})



//middleware
//app.use(express.json());
//Nos permite manejar peticiones y enviar respuesta en formato json