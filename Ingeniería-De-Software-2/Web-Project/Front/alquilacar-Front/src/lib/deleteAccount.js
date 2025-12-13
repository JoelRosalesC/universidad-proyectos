export const DeleteAccount = async (token) => {
    try {
        const response = await fetch(`http://localhost:5296/api/Auth`, {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`, // Agregar el token en los headers
            },
        });
        // console.log(response);

        const data = await response.json();

        if (!response.ok) {
            return {
                success: false,
                error: data.errors || {
                    generalError: ["Error al eliminar mi cuenta"],
                },
            };
        }

        return {
            success: true,
        };
    } catch (error) {
        console.error("Error al eliminar mi cuenta:", error);
        return {
            error: {
                generalError: ["Ocurrió algo inesperado, intente más tarde."],
            },
        };
    }
};
