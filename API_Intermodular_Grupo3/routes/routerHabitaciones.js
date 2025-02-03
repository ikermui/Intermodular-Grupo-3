const express = require('express');
const router = express.Router();
const ModelUser = require('../models/modelHabitaciones.js'); 
//middleware para acceder 
router.get('/getAll', async (req, res) => {
    try{
    const data = await ModelUser.find();
    res.status(200).json(data);
    }
    catch(error){
    res.status(500).json({message: error.message});
    }
    });

router.get('/getOne', async (req, res) => {
    try{
    const id = req.body._id;
    const habitacionesDB = await ModelUser.findOne({ _id: id });
    console.log(habitacionesDB);
    if (!habitacionesDB) {
        return res.status(404).json({ message: 'Documento no encontrado' });
    }
    res.status(200).json(habitacionesDB);
    }
    catch(error){
        res.status(500).json({message: error.message});
    }
    });


/*router.post('/getFilter', async (req, res) => {
    try{
    const condiciones = {};
    if (req.body.nombre) condiciones.nombre = req.body.nombre;
    if (req.body.huespedes) condiciones.huespedes = req.body.huespedes;
    if (req.body.descripcion) condiciones.descripcion = req.body.descripcion;
    if (req.body.imagen) condiciones.imagen = req.body.imagen;
    if (req.body.precio) condiciones.precio = req.body.precio;
    if (req.body.oferta) condiciones.oferta = req.body.oferta;
    if (req.body.finOferta) condiciones.finOferta = req.body.finOferta;
    if (req.body.camaExtra) condiciones.camaExtra = req.body.camaExtra;
    if (req.body.cuna) condiciones.cuna = req.body.cuna;
    const data = await ModelUser.find(condiciones);
    if (data.length === 0) {
    return res.status(404).json({ message: 'Documento no encontrado' });
    }
    res.status(200).json(data);
    }
    catch(error){
    res.status(500).json({message: error.message});
    }
});*/


router.post('/getFilter2', async (req, res) => {
    try{
        const condiciones = {};
        if (req.body.huespedes) condiciones.huespedes = req.body.huespedes;
        if (req.body.ofertaMin !== undefined || req.body.ofertaMax !== undefined) {
            condiciones.oferta = {};
            if (req.body.ofertaMin !== undefined) condiciones.oferta.$gte = req.body.ofertaMin;
            if (req.body.ofertaMax !== undefined) condiciones.oferta.$lte = req.body.ofertaMax;
        }
        if (req.body.baja) condiciones.baja = req.body.baja;
        if (req.body.camaExtra) condiciones.camaExtra = req.body.camaExtra;
        if (req.body.cuna) condiciones.cuna = req.body.cuna;
        const data = await ModelUser.find(condiciones);
        if (data.length === 0) {
            return res.status(404).json({ message: 'Documento no encontrado' });
        }
        res.status(200).json(data);
    }
    catch(error){
    res.status(500).json({message: error.message});
    }
});

router.post('/new', async (req, res) => {
    const data = new ModelUser({
        nombre: req.body.nombre,
        huespedes: req.body.huespedes,
        descripcion: req.body.descripcion,
        imagen: req.body.imagen,
        precio: req.body.precio,
        oferta: req.body.oferta,
        finOferta: req.body.finOferta,
        camaExtra: req.body.camaExtra,
        baja: req.body.baja,
        cuna: req.body.cuna
    })

    try {
    const dataToSave = await data.save();
    res.status(200).json(dataToSave);
    }
    catch (error) {
    res.status(400).json({message: error.message});
    }
    });

router.patch("/update", async (req, res) => {
    try {
    const id = req.body._id;

    const resultado = await ModelUser.updateOne(
    { _id: id }, { $set: {
        nombre: req.body.nombre,
        huespedes: req.body.huespedes,
        descripcion: req.body.descripcion,
        imagen: req.body.imagen,
        precio: req.body.precio,
        oferta: req.body.oferta,
        finOferta: req.body.finOferta,
        camaExtra: req.body.camaExtra,
        baja: req.body.baja,
        cuna: req.body.cuna
    },});
    
    if (resultado.modifiedCount === 0) {
        return res.status(404).json({ message: "Documento no encontrado" });
    }
    
    res.status(200).json({ message: "Documento actualizado exitosamente"
    });
    } catch (error) {
        res.status(400).json({ message: error.message });
    }
});

router.delete('/delete', async (req, res) => {
    try {
    const id = req.body._id;
    const data = await ModelUser.deleteOne({ _id: id })
    if (data.deletedCount === 0) {
        return res.status(404).json({ message: 'Documento no encontrado' });
    }

    res.status(200).json({ message: `Document with ${id} has been deleted..` })
    }
    catch (error) {
        res.status(400).json({ message: error.message })
    }
    })

module.exports = router;
