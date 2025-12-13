"use server";

export async function submitLoginForm(prevState, formData) {
    try {
        const email = formData.get("email");
        const password = formData.get("password");

        const response = await fetch("http://localhost:5296/api/Auth/Login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                mail: email,
                password: password,
            }),
        });

        const result = await response.json();

        if (response.ok) {
            if (result.require_code) {
                return {
                    success: "Formulario enviado con exito",
                    require_code: true,
                    email: email,
                };
            } else {
                const token = result.data.token;
                return {
                    success: "Formulario enviado con exito",
                    token,
                };
            }
        } else {
            return {
                success: false,
                error: result.errors || {
                    generalError: ["Error al iniciar sesión"],
                },
            };
        }
    } catch (error) {
        console.error("Error en submitJoinForm:", error);
        return {
            error: {
                generalError: ["Ocurrió algo inesperado, intente más tarde."],
            },
        };
    }
}
