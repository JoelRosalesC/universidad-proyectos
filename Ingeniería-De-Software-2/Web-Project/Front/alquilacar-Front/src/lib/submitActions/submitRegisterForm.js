"use server";

export async function submitRegisterForm(prevState, formData) {
    try {
        const email = formData.get("email");
        const password = formData.get("password");
        const firstName = formData.get("firstName");
        const lastName = formData.get("lastName");
        const dni = formData.get("dni");
        const birthdate = formData.get("birthdate");
        const phoneNumber = formData.get("phoneNumber");
        const code = formData.get("code");

        const response = await fetch("http://localhost:5296/api/Auth/SignUp", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                mail: email,
                password: password,
                firstName: firstName,
                lastName: lastName,
                dni: dni,
                birthdate: birthdate,
                phoneNumber: phoneNumber,
                code: code,
            }),
        });

        const result = await response.json();

        if (response.ok) {
            return {
                success: "Formulario enviado con exito",
            };
        } else {
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
                },
                error: result.errors || {
                    generalError: ["Error al registrarse"],
                },
            };
        }
    } catch (error) {
        console.error("Error en submitRegisterForm:", error);
        return {
            error: {
                generalError: ["Ocurrió algo inesperado, intente más tarde."],
            },
        };
    }
}
