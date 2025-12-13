"use client";

import { useEffect, useState } from "react";
import styles from "./updateVehicleType.module.scss";
import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";
import UpdateVehicleTypeForm from "@/components/forms/updateVehicleTypeForm/UpdateVehicleTypeForm";
import { useParams } from "next/navigation";
import { poppins } from "@/lib/fonts/fonts";

export default function Page() {
    const loading = useProtectedRoute(["Admin", "Employee"]);
    const { id } = useParams();
    const token = localStorage.getItem("token");
    const [vehicleType, setVehicleType] = useState(null);

    // Hacer la solicitud a la API
    useEffect(() => {
        const fetchVehicleType = async () => {
            try {
                const response = await fetch(
                    `http://localhost:5296/api/VehicleType/${id}`,
                    {
                        method: "GET",
                        headers: {
                            "Content-Type": "application/json",
                            Authorization: `Bearer ${token}`,
                        },
                    }
                );

                if (!response.ok) {
                    throw new Error("Error al obtener tipos de vehiculos");
                }

                const data = await response.json();

                setVehicleType(data.data);
            } catch (error) {
                console.error("Error al obtener marcas:", error);
            }
        };
        if (id) {
            fetchVehicleType();
        }
    }, [id]);
    if (loading) return null;
    return (
        <div className={styles.mainContainer}>
            {vehicleType ? (
                <>
                    <h1 className={`${styles.title} ${poppins.className} `}>
                        Actualizando Tipo de vehiculo {vehicleType.description}
                    </h1>
                    <UpdateVehicleTypeForm vehicleType={vehicleType} />
                </>
            ) : (
                <h1 className={`${styles.title} ${poppins.className} `}>
                    No se encontró el tipo de vehiculo
                </h1>
            )}
        </div>
    );
}
