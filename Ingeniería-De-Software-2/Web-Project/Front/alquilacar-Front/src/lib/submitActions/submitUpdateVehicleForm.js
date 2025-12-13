"use server";

export async function submitUpdateVehicleForm(prevState, formData) {
    try {
        const id = Number(formData.get("id"));
        const licensePlate = formData.get("licensePlate");
        const color = formData.get("color");
        const year = formData.get("Year");
        const vehicleTypeId = Number(formData.get("vehicleTypeId"));
        const currentBranchId = Number(formData.get("currentBranchId"));

        const token = formData.get("token");

        const response = await fetch("http://localhost:5296/api/Vehicle", {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
            body: JSON.stringify({
                id,
                licensePlate,
                color,
                year,
                vehicleTypeId,
                currentBranchId,
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
                    id,
                    licensePlate,
                    color,
                    year,
                    vehicleTypeId,
                    currentBranchId,
                },
                error: result.errors || {
                    generalError: ["Error al editar el vehiculo"],
                },
            };
        }
    } catch (error) {
        console.error("Error en submitUpdateVehicleForm:", error);
        return {
            error: {
                generalError: ["Ocurrió algo inesperado, intente más tarde."],
            },
        };
    }
}
