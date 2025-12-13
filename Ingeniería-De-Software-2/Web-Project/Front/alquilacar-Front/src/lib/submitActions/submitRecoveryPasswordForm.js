"use server";

export async function submitRecoveryPasswordForm(prevState, formData) {
    try {
        const mail = formData.get("email");
        const password = formData.get("password");
        const code = formData.get("code");

        const response = await fetch(
            "http://localhost:5296/api/Auth/ResetPassword",
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    mail: mail,
                    password: password,
                    code: code,
                }),
            }
        );

        const result = await response.json();

        if (response.ok) {
            return {
                success: "Formulario enviado con exito",
            };
        } else {
            return {
                success: false,
                error: result.errors || {
                    generalError: ["Error al cambiar la contraseña "],
                },
            };
        }
    } catch (error) {
        console.error("Error en submitRecoveryPasswordForm:", error);
        return {
            error: {
                generalError: ["Ocurrió algo inesperado, intente más tarde."],
            },
        };
    }
}
