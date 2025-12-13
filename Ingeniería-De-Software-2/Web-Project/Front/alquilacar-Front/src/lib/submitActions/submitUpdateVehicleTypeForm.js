"use server";

export async function submitUpdateVehicleTypeForm(prevState, formData) {
    try {
        const BrandId = Number(formData.get("BrandId"));
        const Model = formData.get("Model");
        const PassengerCapacity = formData.get("PassengerCapacity");
        const PricePerDay = formData.get("PricePerDay");
        const Category = Number(formData.get("Category"));
        const CancellationPolicyId = Number(
            formData.get("CancellationPolicyId")
        );
        const token = formData.get("token");

        const response = await fetch("http://localhost:5296/api/VehicleType", {
            method: "PUT",
            headers: {
                Authorization: `Bearer ${token}`,
            },
            body: formData,
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
                    BrandId,
                    Model,
                    PassengerCapacity,
                    PricePerDay,
                    Category,
                    CancellationPolicyId,
                },
                error: result.errors || {
                    generalError: [
                        "Error al editar la política de cancelación",
                    ],
                },
            };
        }
    } catch (error) {
        console.error("Error en submitUpdatecancellationPolicyForm:", error);
        return {
            error: {
                generalError: ["Ocurrió algo inesperado, intente más tarde."],
            },
        };
    }
}
