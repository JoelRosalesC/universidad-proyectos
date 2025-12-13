"use server";

export async function submitUpdateBranchForm(prevState, formData) {
    try {
        const id = formData.get("id");
        const name = formData.get("name");
        const province = formData.get("province");
        const locality = formData.get("locality");
        const token = formData.get("token");

        const response = await fetch("http://localhost:5296/api/Branch", {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
            body: JSON.stringify({
                id: id,
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
                inputs: {
                    Name: name,
                    Province: province,
                    Locality: locality,
                },
                error: result.errors || {
                    generalError: ["Error al editar la sucursal"],
                },
            };
        }
    } catch (error) {
        console.error("Error en submitUpdateBranchForm:", error);
        return {
            error: {
                generalError: ["Ocurrió algo inesperado, intente más tarde."],
            },
        };
    }
}
