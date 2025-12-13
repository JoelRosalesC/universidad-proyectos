"use server";

export async function submitRentalStatsForm(prevState, formData) {
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
            const rentals = result.data.rentals || [];
            const start = new Date(startDate);
            const end = new Date(`${endDate}T23:59:59.999Z`);

            const validRentals = rentals
                .filter((rental) => {
                    const creationDate = rental.rentalDate;

                    const date = new Date(creationDate);

                    return (
                        date >= start &&
                        date <= end &&
                        (rental.status === 1 || rental.status === 3)
                    );
                })
                .sort(
                    (a, b) => new Date(a.rentalDate) - new Date(b.rentalDate)
                );
            const countByDay = validRentals.reduce((acc, rental) => {
                const date = new Date(rental.rentalDate)
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
                    countedRentals: validRentals,
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
                        "Error al buscar los alquileres en un rango de fechas",
                    ],
                },
            };
        }
    } catch (error) {
        console.error("Error en submitRentalStatsForm:", error);
        return {
            error: {
                generalError: ["Ocurrió algo inesperado, intente más tarde."],
            },
        };
    }
}
