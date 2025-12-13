"use server";

export async function submitCreateBranchForm(prevState, formData) {
    try {
        const name = formData.get("name");
        const province = formData.get("province");
        const locality = formData.get("locality");
        const token = formData.get("token");

        const response = await fetch("http://localhost:5296/api/Branch", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
            body: JSON.stringify({
                name: name,
                province: province,
                locality: locality,
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
                error: result.errors || {
                    generalError: ["Error al crear la sucursal"],
                },
            };
        }
    } catch (error) {
        console.error("Error en submitBranchForm:", error);
        return {
            error: {
                generalError: ["Ocurrió algo inesperado, intente más tarde."],
            },
        };
    }
}
