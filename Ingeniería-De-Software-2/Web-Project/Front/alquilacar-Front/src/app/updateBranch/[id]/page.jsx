"use client";

import { useEffect, useState } from "react";
import styles from "./updateBranch.module.scss";
import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";
import UpdateBranchForm from "@/components/forms/updateBranchForm/UpdateBranchForm";
import { useParams } from "next/navigation";
import { poppins } from "@/lib/fonts/fonts";

export default function Page() {
    const loading = useProtectedRoute(["Admin", "Employee"]);
    const { id } = useParams();
    const token = localStorage.getItem("token");
    const [branch, setBranch] = useState(null);
    // Hacer la solicitud a la API
    useEffect(() => {
        const fetchBranches = async () => {
            try {
                const response = await fetch(
                    `http://localhost:5296/api/Branch/${id}`,
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
                setBranch(data.data);
            } catch (error) {
                console.error("Error al obtener sucursales:", error);
            }
        };
        if (id) {
            fetchBranches();
        }
    }, [id]);
    if (loading) return null;
    return (
        <div className={styles.mainContainer}>
            {branch ? (
                <>
                    <h1 className={`${styles.title} ${poppins.className} `}>
                        Actualizando Sucursal {branch.name}
                    </h1>
                    <UpdateBranchForm branch={branch} />
                </>
            ) : (
                <h1 className={`${styles.title} ${poppins.className} `}>
                    No se encontró la sucursal
                </h1>
            )}
        </div>
    );
}
