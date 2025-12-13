"use server";

export async function submitCodeForRecoveryPassword(prevState, formData) {
    try {
        const email = formData.get("email");

        const response = await fetch(
            "http://localhost:5296/api/Auth/Forgotpassword",
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    mail: email,
                }),
            }
        );

        const result = await response.json();

        if (response.ok) {
            return {
                success: "Formulario enviado con exito",
                code_sent: true,
                email: email,
            };
        } else {
            return {
                success: false,
                error: result.errors || {
                    generalError: ["Error al enviar el código"],
                },
            };
        }
    } catch (error) {
        console.error("Error en submitCodeForRecoveryPassword:", error);
        return {
            error: {
                generalError: ["Ocurrió algo inesperado, intente más tarde."],
            },
        };
    }
}
