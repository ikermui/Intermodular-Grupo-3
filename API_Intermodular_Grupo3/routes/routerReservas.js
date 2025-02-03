const express = require('express');
const reservasSchema = require('../models/modelsReservas');
const router = express.Router()

const parseFecha = (fecha) => {
    const [day, month, year] = fecha.split('/').map(Number);
    const date = new Date(Date.UTC(year, month - 1, day));
    return date.toISOString();
};

// get All

router.get('/getAll', async (req, res) => {
    try{
    const data = await reservasSchema.find();
    res.status(200).json(data);
    }
    catch(error){
    res.status(500).json({message: error.message});
    }
    });


// get One

router.post('/getOne', async (req, res) => {
    try{
        const id = req.body._id;
        const reservasDB = await reservasSchema.findOne({ _id: id});
        console.log(reservasDB);
        if (!reservasDB) {
            return res.status(404).json({message: 'Documento no encontrado'});
        }
        res.status(200).json(reservasDB);
    }
    catch(error){
        res.status(500).json({message: error.message});
    }
})

// getFilter

router.post('/getFilter', async (req, res) => {
    try{

    const condiciones = {};

    if (req.body._id) condiciones._id = req.body._id;
    if (req.body.dni) condiciones.dni = req.body.dni;
    if (req.body.id_hab) condiciones.id_hab = req.body.id_hab;
    if (req.body.fecha_ini) condiciones.fecha_ini = req.body.fecha_ini;
    if (req.body.fecha_fin) condiciones.fecha_fin = req.body.fecha_fin;
    if (req.body.numHuespedes) condiciones.numHuespedes = req.body.numHuespedes;


    const data = await reservasSchema.find(condiciones);
    if (data.length === 0) {
    return res.status(404).json({ message: 'Documento no encontrado' });
    }
    res.status(200).json(data);
    }
    catch(error){
    res.status(500).json({message: error.message});
    }
});

// /getFilterDate

router.post('/getFilterDate', async (req, res) => {
    try{
    
        const condiciones = {};
        if(req.body.fecha_ini && req.body.fecha_fin){
            const fInicio = parseFecha(req.body.fecha_ini);
            const fFin = parseFecha(req.body.fecha_fin);
            condiciones.$and = [
                { fecha_ini: { $gte: fInicio } }, 
                { fecha_fin: { $lte: fFin } }
            ];
            
        }
        if (req.body._id) condiciones._id = req.body._id;
        if (req.body.dni) condiciones.dni = req.body.dni;
        if (req.body.id_hab) condiciones.id_hab = req.body.id_hab;      
        if (req.body.numHuespedes) condiciones.numHuespedes = req.body.numHuespedes;
        const data = await reservasSchema.find(condiciones);
        if (data.length === 0) {
        return res.status(404).json({ message: 'Documento no encontrado' });
        }
        res.status(200).json(data);
        }
        catch(error){
        res.status(500).json({message: error.message});
        }
});

// /new

router.post('/new', async (req, res) => {
    const data = new reservasSchema({
        dni: req.body.dni,
        id_hab: req.body.id_hab,
        fecha_ini: parseFecha(req.body.fecha_ini),
        fecha_fin: parseFecha(req.body.fecha_fin),
        numHuespedes: req.body.numHuespedes
    })

    //console.log(req.body)

    try {
        const dataToSave = await data.save();
        res.status(200).json(dataToSave);
    }
    catch (error) {
        res.status(400).json({message: error.message});
    }
});

// /update

router.patch("/update", async (req, res) => {
    try {
        const reserva = req.body._id;
        //const updatedData = req.body;
        const resultado = await reservasSchema.updateOne(
            { _id: reserva },
            {
                $set: {
                    dni: req.body.dni,
                    id_hab: req.body.id_hab,
                    fecha_ini: parseFecha(req.body.fecha_ini),
                    fecha_fin: parseFecha(req.body.fecha_fin),
                    numHuespedes: req.body.numHuespedes,
                },
            }
        );

        //console.log(resultado)
        if (resultado.modifiedCount === 0) {
            return res.status(404).json({ message: "Documento no encontrado" });
        }
   
        //res.send(resultado)
        res.status(200).json({ 
            message: "Documento actualizado exitosamente"
        });
    } catch (error) {
        res.status(400).json({ message: error.message });
    }
});
    
// /delete

router.post('/delete', async (req, res) => {
    try {
        const reserva = req.body._id;
        const data = await reservasSchema.deleteOne({ _id: reserva });
        if (data.deletedCount === 0) {
            return res.status(404).json({ message: 'Documento no encontrado' });
        }
        //res.send(`Document with ${data.usuario} has been deleted..`)
        res.status(200).json({ message: `Document with ${reserva} has been deleted..` })
    }
    catch (error) {
        res.status(400).json({ message: error.message })
    }
    })

module.exports = router;