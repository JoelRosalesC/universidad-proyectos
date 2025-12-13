"use server";

export async function submitCreateVehicleForm(prevState, formData) {
    try {
        const licensePlate = formData.get("licensePlate");
        const color = formData.get("color");
        const Year = formData.get("Year");
        const currentBranchId = Number(formData.get("currentBranchId"));
        const vehicleTypeId = Number(formData.get("vehicleTypeId"));

        const token = formData.get("token");

        const response = await fetch("http://localhost:5296/api/Vehicle", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
            body: JSON.stringify({
                licensePlate: licensePlate,
                color: color,
                Year: Year,
                currentBranchId: currentBranchId,
                vehicleTypeId: vehicleTypeId,
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
                    licensePlate,
                    color,
                    Year,
                    currentBranchId,
                    vehicleTypeId,
                },
                error: result.errors || {
                    generalError: ["Error al crear el vehiculo"],
                },
            };
        }
    } catch (error) {
        console.error("Error en submitCreateVehicleForm:", error);
        return {
            error: {
                generalError: ["Ocurrió algo inesperado, intente más tarde."],
            },
        };
    }
}
