"use client";

import { useEffect, useState } from "react";
import styles from "./updateCancellationPolicy.module.scss";
import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";
import UpdateCancellationPolicyForm from "@/components/forms/updateCancellationPolicyForm/UpdateCancellationPolicyForm";
import { useParams } from "next/navigation";
import { poppins } from "@/lib/fonts/fonts";

export default function Page() {
    const loading = useProtectedRoute(["Admin", "Employee"]);
    const { id } = useParams();
    const token = localStorage.getItem("token");
    const [cancellationPolicy, setCancelationPolicy] = useState(null);
    // Hacer la solicitud a la API
    useEffect(() => {
        const fetchCancellationPolicies = async () => {
            try {
                const response = await fetch(
                    `http://localhost:5296/api/cancellationPolicy/${id}`,
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
                setCancelationPolicy(data.data);
            } catch (error) {
                console.error("Error al obtener marcas:", error);
            }
        };
        if (id) {
            fetchCancellationPolicies();
        }
    }, [id]);
    if (loading) return null;
    return (
        <div className={styles.mainContainer}>
            {cancellationPolicy ? (
                <>
                    <h1 className={`${styles.title} ${poppins.className} `}>
                        Actualizando Política de cancelación{" "}
                        {cancellationPolicy.description}
                    </h1>
                    <UpdateCancellationPolicyForm
                        cancellationPolicy={cancellationPolicy}
                    />
                </>
            ) : (
                <h1 className={`${styles.title} ${poppins.className} `}>
                    No se encontró la política de cancelación
                </h1>
            )}
        </div>
    );
}
