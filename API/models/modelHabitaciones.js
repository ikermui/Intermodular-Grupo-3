const mongoose = require("mongoose");
const HabitacionesSchema = new mongoose.Schema({
 _id: { type: String},

 nombre: { required: true, type: String },

 huespedes: { required: true, type: Number },

 descripcion: { required: true, type: String },

 imagen: { required: false, type: String },

 precio: { required: true, type: Number },

 oferta: { required: true, type: Number },

 finOferta: { required: false, type: String },

 camaExtra: { required: true, type: Boolean },

 baja: { required: true, type: Boolean },

 cuna: { required: true, type: Boolean }}, {__v: false});

 HabitacionesSchema.pre('save', async function (next) {
   const habitacion = this;
   if (!habitacion.isNew) return next();
   try {
      const ultimaHab = await this.constructor.findOne({}).sort({ _id: -1 }).exec();

      let nuevoID = "H001";

      if (ultimaHab && ultimaHab._id) { // Verificar que ultimaHab no es null
         const match = ultimaHab._id.match(/^H(\d{3})$/);
         if (match) {
            // Incrementar el número y asegurarse de que siga teniendo 3 cifras con padding
            const ultimoNumero = parseInt(match[1], 10);
            const nuevoNumero = (ultimoNumero + 1).toString().padStart(3, '0');
            nuevoID = `H${nuevoNumero}`;
         }
      }

      habitacion._id = nuevoID;
      habitacion.finOferta = "";
      next();
   } catch (err) {
      next(err);
   }
});

module.exports = mongoose.model("habitaciones", HabitacionesSchema); 