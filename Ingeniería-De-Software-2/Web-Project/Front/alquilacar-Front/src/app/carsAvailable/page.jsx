"use client";
import styles from "./carsAvailable.module.scss";
import { poppins } from "@/lib/fonts/fonts";
import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";
import { useSearchParams } from "next/navigation";
import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";

export default function CarsAvailable() {
    const searchParams = useSearchParams();
    const loading = useProtectedRoute();
    const sucursalID = searchParams.get("withdrawalBranch");
    const inicio = searchParams.get("startDate");
    const fin = searchParams.get("endDate");
    const router = useRouter();

    const [brands, setBrands] = useState([]);
    useEffect(() => {
        const token = localStorage.getItem("token");
        const fetchBrands = async () => {
            try {
                const response = await fetch(
                    "http://localhost:5296/api/Brand",
                    {
                        method: "GET",
                        headers: {
                            "Content-Type": "application/json",
                            Authorization: `Bearer ${token}`,
                        },
                    }
                );

                if (!response.ok) {
                    throw new Error("Error al obtener marcas");
                }

                const data = await response.json();

                setBrands(data.data);
            } catch (error) {
                console.error("Error al obtener marcas:", error);
            }
        };

        fetchBrands();
    }, []);

    const [carsAvailable, setCarsAvailable] = useState([]);
    // Hacer la solicitud a la API
    useEffect(() => {
        const fetchCarsAvailable = async () => {
            const token = localStorage.getItem("token");
            try {
                const response = await fetch(
                    "http://localhost:5296/api/VehicleType/Availables",
                    {
                        method: "POST",
                        headers: {
                            "Content-Type": "application/json",
                            Authorization: `Bearer ${token}`, // Agregar el token en los headers
                        },
                        body: JSON.stringify({
                            branchId: sucursalID,
                            startDate: inicio,
                            endDate: fin,
                        }),
                    }
                );

                if (!response.ok) {
                    throw new Error("Error al obtener autos disponibles");
                }

                const data = await response.json();
                const vehicleTypesUpdated = data.data.map((vehicleType) => {
                    const brand = brands.find(
                        (brand) => brand.id === vehicleType.brandId
                    );

                    return {
                        ...vehicleType,
                        marca: brand ? brand.name : "Marca no encontrada",
                    };
                });

                setCarsAvailable(vehicleTypesUpdated);
            } catch (error) {
                console.error("Error al obtener autosdisponibles:", error);
            }
        };

        fetchCarsAvailable();
    }, [sucursalID, inicio, fin, brands]);

    const [branch, setBranch] = useState("");
    // Hacer la solicitud a la API
    useEffect(() => {
        const fetchBranches = async () => {
            const token = localStorage.getItem("token");
            try {
                const response = await fetch(
                    "http://localhost:5296/api/Branch",
                    {
                        method: "GET",
                        headers: {
                            "Content-Type": "application/json",
                            Authorization: `Bearer ${token}`, // Agregar el token en los headers
                        },
                    }
                );

                if (!response.ok) {
                    throw new Error("Error al obtener sucursales");
                }

                const data = await response.json();
                const foundBranch = data.data.find(
                    (branch) => branch.id.toString() === sucursalID
                );

                if (foundBranch) {
                    setBranch(foundBranch.name);
                }
            } catch (error) {
                console.error("Error al obtener sucursales:", error);
            }
        };

        fetchBranches();
    }, []);

    if (loading) return null;

    return (
        <div className={styles.mainContainer}>
            <h1 className={`${styles.title} ${poppins.className} `}>
                Explora nuestros autos disponibles
            </h1>
            <p className={styles.subtitle}>
                En {branch || "no se encontro la sucursal"} entre el {inicio} y
                el {fin}
            </p>

            {carsAvailable?.length > 0 ? (
                <div className={styles.vehiclesContainer}>
                    {carsAvailable.map((vehicle) => (
                        <div key={vehicle.id} className={styles.vehicleCard}>
                            <img
                                src={`http://localhost:5296/images/${vehicle.imageUrl}`}
                                alt={vehicle.model}
                                onError={(e) => {
                                    e.currentTarget.onerror = null;
                                    e.currentTarget.src =
                                        "/img/missingCar.jpeg";
                                }}
                                className={styles.vehicleImg}
                            />
                            <h2>
                                {vehicle.marca} {vehicle.model}
                            </h2>
                            <div className={styles.secondContainer}>
                                <p>
                                    <strong>Capacidad:</strong>{" "}
                                    {vehicle.passengerCapacity} pasajeros
                                </p>
                                <p>
                                    <strong>Precio por día:</strong> $
                                    {vehicle.pricePerDay}
                                </p>
                                <button
                                    onClick={() => {
                                        const params = searchParams.toString();
                                        router.push(
                                            `carsAvailable/${vehicle.id}?${params}`
                                        );
                                    }}
                                >
                                    Ver Detalles
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            ) : (
                <h2>No se encontró ningún vehículo disponible</h2>
            )}
        </div>
    );
}
