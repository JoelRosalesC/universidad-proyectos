export const PutIntoMaintenance = async (id, token, isUnderMaintenance) => {
    try {
        const response = await fetch(
            `http://localhost:5296/api/Vehicle/Maintenance`,
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`, // Agregar el token en los headers
                },
                body: JSON.stringify({
                    id: id,
                    isUnderMaintenance: isUnderMaintenance,
                }),
            }
        );

        const data = await response.json();

        if (!response.ok) {
            return {
                success: false,
                error: data.errors || {
                    generalError: [
                        "Error al poner el vehiculo en mantenimiento",
                    ],
                },
            };
        }

        return {
            success: true,
        };
    } catch (error) {
        console.error("Error al poner el vehiculo en mantenimiento:", error);
        return {
            error: {
                generalError: ["Ocurrió algo inesperado, intente más tarde."],
            },
        };
    }
};
