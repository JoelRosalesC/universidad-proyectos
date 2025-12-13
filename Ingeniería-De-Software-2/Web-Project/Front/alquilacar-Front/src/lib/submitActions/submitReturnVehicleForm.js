"use server";

export async function submitReturnVehicleForm(prevState, formData) {
    try {
        const rentalId = formData.get("rentalId");

        const token = formData.get("token");

        const response = await fetch(
            "http://localhost:5296/api/Rental/ReturnVehicle",
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`,
                },
                body: JSON.stringify({
                    rentalId,
                }),
            }
        );
        // console.log(response);

        let result = {};
        try {
            result = await response.json();
        } catch (e) {
            // Esto ocurre si la respuesta no tiene body válido (como en un 401 vacío)
            result = {};
        }

        if (response.ok) {
            return {
                success: "Formulario enviado con exito",
            };
        } else {
            if (response.status === 401) {
                return {
                    success: false,
                    unauthorized: true,
                    error: {
                        generalError: [
                            "Sesión expirada. Iniciá sesión nuevamente.",
                        ],
                    },
                };
            }
            return {
                success: false,
                inputs: {
                    rentalId,
                },
                error: result.errors || {
                    generalError: ["Error al devolver el auto"],
                },
            };
        }
    } catch (error) {
        console.error("Error en submitReturnVehicleForm:", error);
        return {
            error: {
                generalError: ["Ocurrió algo inesperado, intente más tarde."],
            },
        };
    }
}
