export const CancelReservation = async (id, token) => {
    try {
        const response = await fetch(`http://localhost:5296/api/Rental/${id}`, {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`, // Agregar el token en los headers
            },
        });

        const data = await response.json();

        if (!response.ok) {
            return {
                success: false,
                error: data.errors || {
                    generalError: ["Error al cancelar la reserva"],
                },
            };
        }

        return {
            success: true,
        };
    } catch (error) {
        console.error("Error al cancelar la reserva:", error);
        return {
            error: {
                generalError: ["Ocurrió algo inesperado, intente más tarde."],
            },
        };
    }
};
