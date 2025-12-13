"use client";
import styles from "./catalog.module.scss";
import { poppins } from "@/lib/fonts/fonts";
import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";

export default function catalog() {
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

    const [vehicleTypes, setVehicleTypes] = useState([]);
    // Hacer la solicitud a la API
    useEffect(() => {
        // const token = localStorage.getItem("token");
        const fetchVehicleTypes = async () => {
            try {
                const response = await fetch(
                    "http://localhost:5296/api/VehicleType",
                    {
                        method: "GET",
                        headers: {
                            "Content-Type": "application/json",
                            // Authorization: `Bearer ${token}`,
                        },
                    }
                );

                if (!response.ok) {
                    throw new Error("Error al obtener los tipos de vehiculos");
                }

                const data = await response.json();

                const vehicleTypesUpdated = data.data.map((vehicleType) => {
                    const brand = brands.find(
                        (brand) => brand.id === vehicleType.brandId
                    );

                    return {
                        id: vehicleType.id,
                        Marca: brand ? brand.name : "Marca no encontrada",
                        model: vehicleType.model,
                        passengerCapacity: vehicleType.passengerCapacity,
                        Categoría: vehicleType.category,
                        pricePerDay: vehicleType.pricePerDay,
                        cancellationPolicyId: vehicleType.cancellationPolicyId,
                        imageUrl: vehicleType.imageUrl,
                        brandId: vehicleType.brandId,
                        category: vehicleType.category,
                    };
                });
                setVehicleTypes(vehicleTypesUpdated);
            } catch (error) {
                console.error(
                    "Error al obtener los tipos de vehiculos:",
                    error
                );
            }
        };

        fetchVehicleTypes();
    }, [brands]);

    return (
        <div className={styles.mainContainer}>
            <h1 className={`${styles.title} ${poppins.className} `}>
                Explora nuestra extensa flota de vehiculos
            </h1>
            <div className={styles.vehiclesContainer}>
                {vehicleTypes.map((vehicle) => (
                    <div key={vehicle.id} className={styles.vehicleCard}>
                        <img
                            src={`http://localhost:5296/images/${vehicle.imageUrl}`}
                            alt={vehicle.model}
                            onError={(e) => {
                                e.currentTarget.onerror = null;
                                e.currentTarget.src = "/img/missingCar.jpeg";
                            }}
                            className={styles.vehicleImg}
                        />
                        <h2>
                            {vehicle.Marca} {vehicle.model}
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
                                    router.push(`/catalog/${vehicle.id}`);
                                }}
                            >
                                Ver Detalles
                            </button>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}
