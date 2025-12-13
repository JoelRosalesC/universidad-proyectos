"use server";

export async function submitCreateRentWithoutReservationForm(
    prevState,
    formData
) {
    try {
        const customerId = formData.get("customerId");
        const endDate = formData.get("endDate");
        const deliveredVehicleId = formData.get("deliveredVehicleId");

        const additionalsRaw = formData.getAll("additionals");

        const additionals = additionalsRaw.map((a) => Number(a));

        const token = formData.get("token");

        const response = await fetch(
            "http://localhost:5296/api/Rental/RentalByUser",
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`,
                },
                body: JSON.stringify({
                    customerId,
                    endDate,
                    deliveredVehicleId,
                    additionals,
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
                    customerId,
                    endDate,
                    deliveredVehicleId,
                    additionals,
                },
                error: result.errors || {
                    generalError: ["Error al crear el alquiler"],
                },
            };
        }
    } catch (error) {
        console.error(
            "Error en submitCreateRentalWithoutReservationForm:",
            error
        );
        return {
            error: {
                generalError: ["Ocurrió algo inesperado, intente más tarde."],
            },
        };
    }
}
