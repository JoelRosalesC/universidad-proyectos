"use server";

export async function submitCreateEmployeeForm(prevState, formData) {
    try {
        const email = formData.get("email");
        const password = formData.get("password");
        const firstName = formData.get("firstName");
        const lastName = formData.get("lastName");
        const dni = formData.get("dni");
        const birthdate = formData.get("birthdate");
        const phoneNumber = formData.get("phoneNumber");
        const workBranch = Number(formData.get("workBranch"));
        const token = formData.get("token");

        const response = await fetch("http://localhost:5296/api/Employee", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
            body: JSON.stringify({
                mail: email,
                password: password,
                firstName: firstName,
                lastName: lastName,
                dni: dni,
                birthdate: birthdate,
                phoneNumber: phoneNumber,
                workBranch: workBranch,
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
                    mail: email,
                    password: password,
                    firstName: firstName,
                    lastName: lastName,
                    dni: dni,
                    birthdate: birthdate,
                    phoneNumber: phoneNumber,
                    workBranch: workBranch,
                },
                error: result.errors || {
                    generalError: ["Error al crear el empleado"],
                },
            };
        }
    } catch (error) {
        console.error("Error en submitCreateEmployeeForm:", error);
        return {
            error: {
                generalError: ["Ocurrió algo inesperado, intente más tarde."],
            },
        };
    }
}
