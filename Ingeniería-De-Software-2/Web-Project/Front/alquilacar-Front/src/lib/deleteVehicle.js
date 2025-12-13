export const DeleteVehicle = async (id, token) => {
    try {
        const response = await fetch(
            `http://localhost:5296/api/Vehicle/${id}`,
            {
                method: "DELETE",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`, // Agregar el token en los headers
                },
            }
        );

        const data = await response.json();

        if (!response.ok) {
            return {
                success: false,
                error: data.errors || {
                    generalError: ["Error al eliminar el vehículo"],
                },
            };
        }

        return {
            success: true,
        };
    } catch (error) {
        console.error("Error al eliminar el vehículo:", error);
        return {
            error: {
                generalError: ["Ocurrió algo inesperado, intente más tarde."],
            },
        };
    }
};
