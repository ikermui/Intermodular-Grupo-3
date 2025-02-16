const express = require('express'); // Importa Express para crear el servidor
const usuariosSchema = require('../models/modelUsuarios.js'); // Importa el modelo de usuario definido en Mongoose
const router = express.Router(); // Crea un enrutador para manejar rutas específicas

// GET ALL: Obtiene todos los documentos de usuarios
router.get('/getAll', async (req, res) => {
    try {
        const data = await usuariosSchema.find(); // Busca todos los usuarios en la base de datos
        res.status(200).json(data); // Devuelve los usuarios encontrados con estado 200
    } catch(error) {
        res.status(500).json({ message: error.message }); // En caso de error, devuelve estado 500 con el mensaje de error
    }
});

// GET ONE: Obtiene un usuario basado en el 'dni' enviado en el body
router.post("/getOne", async (req, res) => {
    try {
        const { dni } = req.body; // Extrae el dni del cuerpo de la petición
        const usuarioDB = await usuariosSchema.findOne({ dni }); // Busca el usuario con el dni especificado

        if (!usuarioDB) {
            return res.status(404).json({ message: "Documento no encontrado" }); // Si no se encuentra, responde con 404
        }

        res.status(200).json(usuarioDB); // Devuelve el usuario encontrado con estado 200
    } catch (error) {
        res.status(500).json({ message: error.message }); // Maneja errores inesperados con estado 500
    }
});

// GET FILTER: Obtiene usuarios que cumplan ciertas condiciones enviadas en el body
router.get("/getFilter", async (req, res) => {
    try {
        const condiciones = {}; // Inicializa el objeto de condiciones para filtrar usuarios

        // Agrega condiciones según los parámetros recibidos en el body
        if (req.body.dni) condiciones.dni = req.body.dni;
        if (req.body.nombre) condiciones.nombre = req.body.nombre;
        if (req.body.apellido) condiciones.apellido = req.body.apellido;
        if (req.body.rol) condiciones.rol = req.body.rol;
        if (req.body.email) condiciones.email = req.body.email;
        if (req.body.ciudad) condiciones.ciudad = req.body.ciudad;
        if (req.body.sexo) condiciones.sexo = req.body.sexo;
        if (req.body.fecha_nac) condiciones.fecha_nac = req.body.fecha_nac

        const data = await usuariosSchema.find(condiciones); // Busca usuarios que cumplan las condiciones establecidas
        if (data.length === 0) {
            return res.status(404).json({ message: "Documento no encontrado" }); // Responde 404 si no se encuentra ningún usuario
        }

        res.status(200).json(data); // Devuelve los usuarios filtrados con estado 200
    } catch (error) {
        res.status(500).json({ message: error.message }); // Maneja errores inesperados con estado 500
    }
});

// POST NEW: Crea un nuevo usuario con los datos proporcionados
router.post("/new", async (req, res) => {
    const data = new usuariosSchema({
        dni: req.body.dni,         // Asigna el dni recibido en el body
        nombre: req.body.nombre,   // Asigna el nombre recibido en el body
        apellido: req.body.apellido, // Asigna el apellido recibido en el body
        rol: req.body.rol,         // Asigna el rol recibido en el body
        email: req.body.email,     // Asigna el email recibido en el body
        password: req.body.password, // Asigna el password recibido en el body
        fecha_nac: req.body.fecha_nac, // Asigna la fecha de nacimiento recibida en el body
        ciudad: req.body.ciudad,   // Asigna la ciudad recibida en el body
        sexo: req.body.sexo,       // Asigna el sexo recibido en el body
        imagen: req.body.imagen    // Asigna la imagen recibida en el body
    });

    try {
        const dataToSave = await data.save(); // Guarda el nuevo usuario en la base de datos
        res.status(200).json(dataToSave); // Devuelve el usuario guardado con estado 200
    } catch (error) {
        res.status(400).json({ message: error.message }); // En caso de error, responde con estado 400 y el mensaje de error
    }
});

// UPDATE: Actualiza un usuario basado en el dni proporcionado
router.patch("/update", async (req, res) => {
    try {
        const { dni } = req.body; // Extrae el dni del body para identificar el usuario a actualizar

        const resultado = await usuariosSchema.updateOne(
            { dni }, // Condición de búsqueda basada en el dni
            {
                $set: {
                    nombre: req.body.nombre,     // Actualiza el nombre si se proporciona en el body
                    apellido: req.body.apellido, // Actualiza el apellido si se proporciona en el body
                    rol: req.body.rol,           // Actualiza el rol si se proporciona en el body
                    password: req.body.password, // Actualiza la contraseña si se proporciona en el body
                    fecha_nac: req.body.fecha_nac, // Actualiza la fecha de nacimiento si se proporciona en el body
                    ciudad: req.body.ciudad,     // Actualiza la ciudad si se proporciona en el body
                    sexo: req.body.sexo,         // Actualiza el sexo si se proporciona en el body
                    imagen: req.body.imagen,     // Actualiza la imagen si se proporciona en el body
                },
            }
        );

        if (resultado.modifiedCount === 0) {
            return res.status(404).json({ message: "Documento no encontrado" }); // Si no se modifica ningún documento, responde 404
        }

        res.status(200).json({ message: "Documento actualizado exitosamente" }); // Confirma la actualización exitosa con estado 200
    } catch (error) {
        res.status(400).json({ message: error.message }); // Maneja errores con estado 400 y el mensaje correspondiente
    }
});

// DELETE: Elimina un usuario basado en el dni proporcionado
router.delete("/delete", async (req, res) => {
    try {
        const { dni } = req.body; // Extrae el dni del body para identificar el usuario a eliminar
        const data = await usuariosSchema.deleteOne({ dni }); // Elimina el usuario que coincide con el dni

        if (data.deletedCount === 0) {
            return res.status(404).json({ message: "Documento no encontrado" }); // Si no se elimina ningún documento, responde 404
        }

        res.status(200).json({ message: `Documento con dni: ${dni} eliminado` }); // Confirma la eliminación con estado 200 y muestra el dni eliminado
    } catch (error) {
        res.status(400).json({ message: error.message }); // Maneja errores con estado 400 y el mensaje de error
    }
});

/*funciones para el intermodular*/

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

module.exports = router; // Exporta el enrutador para utilizarlo en otras partes de la aplicación