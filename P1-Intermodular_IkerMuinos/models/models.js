const mongoose = require("mongoose");

const reservasSchema = new mongoose.Schema({
 _id: { type: String},
 dni: {
   required: true,
   type: String,
},
 id_hab: {
    required: true,
    type: String,
 },
 
 fecha_ini: {
    required: true,
    type: Date,
 },
 fecha_fin: {
    required: true,
    type: Date,
 },
 numHuespedes: {
    required: true,
    type: Number,
 },
 
});

reservasSchema.pre('save', async function (next) {
   const reserva = this;
   if (!reserva.isNew) return next();
   try {
      const ultimaRes = await this.constructor.findOne({}).sort({ _id: -1 }).exec();

      let nuevoID = "R001";

      if (ultimaRes && ultimaRes._id) { // Verificar que ultimaHab no es null
         const match = ultimaRes._id.match(/^R(\d{3})$/);
         if (match) {
            // Incrementar el número y asegurarse de que siga teniendo 3 cifras con padding
            const ultimoNumero = parseInt(match[1], 10);
            const nuevoNumero = (ultimoNumero + 1).toString().padStart(3, '0');
            nuevoID = `R${nuevoNumero}`;
         }
      }

      reserva._id = nuevoID;
      next();
   } catch (err) {
      next(err);
   }
});


module.exports = mongoose.model("reservas", reservasSchema);