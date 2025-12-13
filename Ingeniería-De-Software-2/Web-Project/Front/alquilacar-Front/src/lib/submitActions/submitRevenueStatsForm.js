"use server";

export async function submitRevenueStatsForm(prevState, formData) {
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
            console.log(rentals);
            const cancellationPolicies = {
                1: 0.2, // 20% de devolución → se cobra el 80%
                2: 0.0, // sin devolución → se cobra el 100%
                3: 1.0, // 100% devolución → se cobra el 0%
            };
            const rentalStatuses = {
                0: "Reservada",
                1: "Alquilada",
                2: "Cancelada",
                3: "Devuelta",
                4: "Anulada",
            };

            const cancellationPolicyDescriptions = {
                1: "20% de devolución",
                2: "Sin devolución",
                3: "100% de devolución",
            };
            const validRentals = rentals
                .filter((rental) => {
                    const creationDate = rental.registrationDate;
                    const date = new Date(creationDate);

                    const isInRange = date >= start && date <= end;
                    const isRelevantStatus = [0, 1, 2, 3].includes(
                        rental.status
                    );

                    // ❌ Excluir canceladas con política sin devolución (id: 2)
                    const isExcludedCancelled =
                        rental.status === 2 &&
                        rental.cancellationPolicyId === 3;

                    return (
                        isInRange && isRelevantStatus && !isExcludedCancelled
                    );
                })
                .sort(
                    (a, b) =>
                        new Date(a.registrationDate) -
                        new Date(b.registrationDate)
                )
                .map((rental) => {
                    const statusName =
                        rentalStatuses[rental.status] || "Desconocido";
                    const cancellationPolicy =
                        cancellationPolicyDescriptions[
                            rental.cancellationPolicyId
                        ] || null;

                    return {
                        ...rental,
                        cancellationPolicy: cancellationPolicy,
                        Estado: statusName,
                    };
                });

            const countByDay = validRentals.reduce((acc, rental) => {
                const date = new Date(rental.registrationDate)
                    .toISOString()
                    .split("T")[0]; // yyyy-mm-dd

                let netTotal = rental.totalPrice;

                if (rental.status === 2) {
                    const returnPercentage =
                        cancellationPolicies[rental.cancellationPolicyId] ?? 0;
                    netTotal = rental.totalPrice * (1 - returnPercentage);
                }

                acc[date] = (acc[date] || 0) + netTotal;
                return acc;
            }, {});

            const registrationsPerDay = Object.entries(countByDay).map(
                ([day, total]) => ({ day, total })
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
                        "Error al buscar los ingresos en un rango de fechas",
                    ],
                },
            };
        }
    } catch (error) {
        console.error("Error en submitRevenueStatsForm:", error);
        return {
            error: {
                generalError: ["Ocurrió algo inesperado, intente más tarde."],
            },
        };
    }
}
