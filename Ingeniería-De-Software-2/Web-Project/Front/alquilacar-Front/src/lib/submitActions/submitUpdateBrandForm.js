"use server";

export async function submitUpdateBrandForm(prevState, formData) {
    try {
        const id = formData.get("id");
        const name = formData.get("name");
        const token = formData.get("token");

        const response = await fetch("http://localhost:5296/api/Brand", {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
            body: JSON.stringify({
                id: id,
                name: name,
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
                },
                error: result.errors || {
                    generalError: ["Error al editar la marca"],
                },
            };
        }
    } catch (error) {
        console.error("Error en submitUpdateBrandForm:", error);
        return {
            error: {
                generalError: ["Ocurrió algo inesperado, intente más tarde."],
            },
        };
    }
}
