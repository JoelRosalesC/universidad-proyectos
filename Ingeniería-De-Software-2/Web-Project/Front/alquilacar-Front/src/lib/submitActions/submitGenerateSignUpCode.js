"use server";

export async function submitGenerateSignUpCode(prevState, formData) {
    try {
        const email = formData.get("email");

        const response = await fetch(
            "http://localhost:5296/api/Auth/GenerateSignUpCode",
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
                    generalError: ["Error al registrarse"],
                },
            };
        }
    } catch (error) {
        console.error("Error en submitGenerateSignUpCode:", error);
        return {
            error: {
                generalError: ["Ocurrió algo inesperado, intente más tarde."],
            },
        };
    }
}
