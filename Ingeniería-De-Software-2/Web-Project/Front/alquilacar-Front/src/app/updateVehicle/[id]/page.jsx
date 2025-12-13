"use client";

import { useEffect, useState } from "react";
import styles from "./updateVehicle.module.scss";
import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";
import UpdateVehicleForm from "@/components/forms/updateVehicleForm/UpdateVehicleForm";
import { useParams } from "next/navigation";
import { poppins } from "@/lib/fonts/fonts";

export default function Page() {
    const loading = useProtectedRoute(["Admin", "Employee"]);
    const { id } = useParams();
    const token = localStorage.getItem("token");
    const [vehicle, setVehicle] = useState(null);

    // Hacer la solicitud a la API
    useEffect(() => {
        const fetchVehicle = async () => {
            try {
                const response = await fetch(
                    `http://localhost:5296/api/Vehicle/${id}`,
                    {
                        method: "GET",
                        headers: {
                            "Content-Type": "application/json",
                            Authorization: `Bearer ${token}`,
                        },
                    }
                );

                if (!response.ok) {
                    throw new Error("Error al obtener el vehiculo");
                }

                const data = await response.json();

                setVehicle(data.data);
            } catch (error) {
                console.error("Error al obtener el vehiculo:", error);
            }
        };
        if (id) {
            fetchVehicle();
        }
    }, [id]);
    if (loading) return null;
    return (
        <div className={styles.mainContainer}>
            {vehicle ? (
                <>
                    <h1 className={`${styles.title} ${poppins.className} `}>
                        Actualizando el vehiculo {vehicle.description}
                    </h1>
                    <UpdateVehicleForm vehicle={vehicle} />
                </>
            ) : (
                <h1 className={`${styles.title} ${poppins.className} `}>
                    No se encontró el vehiculo
                </h1>
            )}
        </div>
    );
}
