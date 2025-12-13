"use server";

export async function submitRegisteredCustomersForm(prevState, formData) {
    try {
        const startDate = formData.get("startDate");
        const endDate = formData.get("endDate");

        const token = formData.get("token");

        const response = await fetch(
            "http://localhost:5296/api/Statistics/GetDB",
            {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`,
                },
            }
        );

        let result = {};
        try {
            result = await response.json();
        } catch (e) {
            // Esto ocurre si la respuesta no tiene body válido (como en un 401 vacío)
            result = {};
        }

        if (response.ok) {
            const customers = result.data.customers || [];
            const users = result.data.users || [];
            const start = new Date(startDate);
            const end = new Date(`${endDate}T23:59:59.999Z`);

            // const validCustomers = customers
            //     .filter((customer) => {
            //         const creationDate = customer.creationDate;

            //         const date = new Date(creationDate);

            //         return date >= start && date <= end;
            //     })
            //     .sort(
            //         (a, b) =>
            //             new Date(a.creationDate) - new Date(b.creationDate)
            //     )
            //     .map((customer) => {
            //         const matchedUser = users.find(
            //             (user) => user.id === customer.userId
            //         );
            //         return {
            //             Email: matchedUser?.mail || "email no encontrado",
            //             ...customer,
            //             Estado: matchedUser
            //                 ? matchedUser.status === 0
            //                     ? "Activo"
            //                     : "Eliminado"
            //                 : "estado no encontrado",
            //         };
            //     });
            const validCustomers = customers
                .filter((customer) => {
                    const date = new Date(customer.creationDate);
                    const matchedUser = users.find(
                        (user) => user.id === customer.userId
                    );
                    return (
                        matchedUser && // ← solo si hay match con usuario
                        date >= start &&
                        date <= end
                    );
                })
                .sort(
                    (a, b) =>
                        new Date(a.creationDate) - new Date(b.creationDate)
                )
                .map((customer) => {
                    const matchedUser = users.find(
                        (user) => user.id === customer.userId
                    );
                    return {
                        Email: matchedUser.mail,
                        ...customer,
                    };
                });
            console.log(validCustomers);
            console.log(users);
            const countByDay = validCustomers.reduce((acc, customer) => {
                const date = new Date(customer.creationDate)
                    .toISOString()
                    .split("T")[0]; // yyyy-mm-dd
                acc[date] = (acc[date] || 0) + 1;
                return acc;
            }, {});

            const registrationsPerDay = Object.entries(countByDay).map(
                ([day, count]) => ({ day, count })
            );

            return {
                success: "Formulario enviado con éxito",
                data: {
                    registrationsPerDay,
                    countedCustomers: validCustomers,
                },
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
                    startDate,
                    endDate,
                },
                error: result.errors || {
                    generalError: [
                        "Error al buscar los clientes registrados en un rango de fechas",
                    ],
                },
            };
        }
    } catch (error) {
        console.error("Error en submitRegisteredCustomersForm:", error);
        return {
            error: {
                generalError: ["Ocurrió algo inesperado, intente más tarde."],
            },
        };
    }
}
