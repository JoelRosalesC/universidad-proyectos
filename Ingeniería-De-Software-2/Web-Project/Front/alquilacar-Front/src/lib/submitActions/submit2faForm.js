"use server";

export async function submit2faForm(prevState, formData) {
    try {
        const mail = formData.get("email");
        const code = formData.get("code");

        const response = await fetch(
            "http://localhost:5296/api/Auth/Login2FA",
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    mail: mail,
                    code: code,
                }),
            }
        );

        const result = await response.json();

        if (response.ok) {
            const token = result.data.token;
            return {
                success: "Formulario enviado con exito",
                token,
            };
        } else {
            return {
                success: false,
                error: result.errors || {
                    generalError: ["Error al iniciar sesión"],
                },
            };
        }
    } catch (error) {
        console.error("Error en submit2FAForm:", error);
        return {
            error: {
                generalError: ["Ocurrió algo inesperado, intente más tarde."],
            },
        };
    }
}
