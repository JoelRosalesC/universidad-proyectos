"use server";

export async function submitCreateRentalForm(prevState, formData) {
    try {
        const vehicleTypeId = formData.get("vehicleTypeId");
        const branchId = formData.get("branchId");
        const startDate = formData.get("startDate");
        const endDate = formData.get("endDate");
        const cardNumber = formData.get("cardNumber");
        const expirationDate = formData.get("expirationDate");
        const cvvCode = formData.get("cvvCode");

        const token = formData.get("token");

        if (!formData.get("cancellationPolicy")) {
            return {
                success: false,
                error: {
                    cancellationPolicy:
                        "Debés aceptar la política de cancelación.",
                },
                inputs: {
                    cardNumber,
                    expirationDate,
                    cvvCode,
                },
            };
        }
        const response = await fetch("http://localhost:5296/api/Rental", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
            body: JSON.stringify({
                vehicleTypeId,
                branchId,
                startDate,
                endDate,
                cardNumber,
                expirationDate,
                cvvCode,
            }),
        });

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
                    cardNumber,
                    expirationDate,
                    cvvCode,
                },
                error: result.errors || {
                    generalError: ["Error al crear la reserva"],
                },
            };
        }
    } catch (error) {
        console.error("Error en submitCreateRentalForm:", error);
        return {
            error: {
                generalError: ["Ocurrió algo inesperado, intente más tarde."],
            },
        };
    }
}
