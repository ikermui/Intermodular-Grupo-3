const express = require('express');
const usuariosSchema = require('../models/modelsUsuarios'); 
const router = express.Router()

// get All

router.get('/getAll', async (req, res) => {
    try{
    const data = await usuariosSchema.find(); 
    res.status(200).json(data);
    }
    catch(error){
    res.status(500).json({message: error.message});
    }
    });

// GET ONE
router.post("/getOne", async (req, res) => {
    try {
        const { dni } = req.body;
        const usuarioDB = await usuariosSchema.findOne({ dni});

        if (!usuarioDB) {
            return res.status(404).json({ message: "Documento no encontrado" });
        }

        res.status(200).json(usuarioDB);
    } catch (error) {
        res.status(500).json({ message: error.message });
    }
});

// GET FILTER
router.post("/getFilter", async (req, res) => {
    try {
        const condiciones = {};

        if (req.body.dni) condiciones.dni = req.body.dni;
        if (req.body.nombre) condiciones.nombre = req.body.nombre;
        if (req.body.apellido) condiciones.apellido = req.body.apellido;
        if (req.body.rol) condiciones.rol = req.body.rol;
        if (req.body.email) condiciones.email = req.body.email;
        if (req.body.ciudad) condiciones.ciudad = req.body.ciudad;
        if (req.body.sexo) condiciones.sexo = req.body.sexo;

        const data = await usuariosSchema.find(condiciones);
        if (data.length === 0) {
            return res.status(404).json({ message: "Documento no encontrado" });
        }

        res.status(200).json(data);
    } catch (error) {
        res.status(500).json({ message: error.message });
    }
});

// POST NEW
router.post("/new", async (req, res) => {
    const data = new usuariosSchema({
        dni: req.body.dni,
        nombre: req.body.nombre,
        apellido: req.body.apellido,
        rol: req.body.rol,
        email: req.body.email,
        password: req.body.password,
        fecha_nac: req.body.fecha_nac,
        ciudad: req.body.ciudad,
        sexo: req.body.sexo,
        imagen: req.body.imagen
    });

    try {
        const dataToSave = await data.save();
        res.status(200).json(dataToSave);
    } catch (error) {
        res.status(400).json({ message: error.message });
    }
});

// UPDATE
router.patch("/update", async (req, res) => {
    try {
        const { dni } = req.body;

        const resultado = await usuariosSchema.updateOne(
            { dni },
            {
                $set: {
                    nombre: req.body.nombre,
                    apellido: req.body.apellido,
                    rol: req.body.rol,
                    email: req.body.email,
                    password: req.body.password,
                    fecha_nac: req.body.fecha_nac,
                    ciudad: req.body.ciudad,
                    sexo: req.body.sexo,
                    imagen: req.body.imagen, 
                },
            }
        );

        if (resultado.modifiedCount === 0) {
            return res.status(404).json({ message: "Documento no encontrado" });
        }

        res.status(200).json({ message: "Documento actualizado exitosamente" });
    } catch (error) {
        res.status(400).json({ message: error.message });
    }
});

// DELETE
router.delete("/delete", async (req, res) => {
    try {
        const {dni} = req.body;
        const data = await usuariosSchema.deleteOne({dni});

        if (data.deletedCount === 0) {
            return res.status(404).json({ message: "Documento no encontrado" });
        }

        res.status(200).json({ message: `Documento con dni: ${dni} eliminado` });
    } catch (error) {
        res.status(400).json({ message: error.message });
    }
});


//GET ONE PASSWORD Y EMAIL

router.post("/getOneEmail", async (req, res) => {
    try {
        const { email, password } = req.body;
        const usuarioDB = await usuariosSchema.findOne({ email, password });

        if (!usuarioDB) {
            return res.status(404).json({ message: "Documento no encontrado" });
        }

        res.status(200).json(usuarioDB);
    } catch (error) {
        res.status(500).json({ message: error.message });
    }
});

module.exports = router;