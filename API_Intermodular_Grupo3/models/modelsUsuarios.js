const mongoose = require("mongoose");

const usuariosSchema = new mongoose.Schema({
 dni: {
    required: true,
    type: String,
 },
 nombre: {
    required: true,
    type: String,
 },
 apellido: {
    required: true,
    type: String,
 },
 rol: {
    required: true,
    type: String,
 },
 email: {
    required: true,
    type: String,
 },
 password: {
    required: true,
    type: String,
 },
 fecha_nac: {
    required: true,
    type: String,
 },
 ciudad: {
    required: true,
    type: String,
 },
 sexo: {
    required: true,
    type: String,
 },
 imagen: {
    required: true,
    type: String,
 },
});

module.exports = mongoose.model("usuarios", usuariosSchema);