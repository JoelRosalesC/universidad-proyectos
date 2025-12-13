"use server";

export async function submitUpdateCancellationPolicyForm(prevState, formData) {
    try {
        const id = formData.get("id");
        const description = formData.get("description");
        const returnPercentage = formData.get("returnPercentage");
        const token = formData.get("token");

        const response = await fetch(
            "http://localhost:5296/api/CancellationPolicy",
            {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`,
                },
                body: JSON.stringify({
                    id: id,
                    description: description,
                    returnPercentage: returnPercentage,
                }),
            }
        );

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
                    Description: description,
                    ReturnPercentage: returnPercentage,
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
