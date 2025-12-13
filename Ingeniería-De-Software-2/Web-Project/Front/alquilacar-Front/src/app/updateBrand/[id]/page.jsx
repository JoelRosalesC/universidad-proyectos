"use client";

import { useEffect, useState } from "react";
import styles from "./updateBrand.module.scss";
import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";
import UpdateBrandForm from "@/components/forms/updateBrandForm/UpdateBrandForm";
import { useParams } from "next/navigation";
import { poppins } from "@/lib/fonts/fonts";

export default function Page() {
    const loading = useProtectedRoute(["Admin", "Employee"]);
    const { id } = useParams();
    const token = localStorage.getItem("token");
    const [brand, setBrand] = useState(null);
    // Hacer la solicitud a la API
    useEffect(() => {
        const fetchBrands = async () => {
            try {
                const response = await fetch(
                    `http://localhost:5296/api/Brand/${id}`,
                    {
                        method: "GET",
                        headers: {
                            "Content-Type": "application/json",
                            Authorization: `Bearer ${token}`, // Agregar el token en los headers
                        },
                    }
                );

                if (!response.ok) {
                    throw new Error("Error al obtener marcas");
                }

                const data = await response.json();
                setBrand(data.data);
            } catch (error) {
                console.error("Error al obtener marcas:", error);
            }
        };
        if (id) {
            fetchBrands();
        }
    }, [id]);
    if (loading) return null;
    return (
        <div className={styles.mainContainer}>
            {brand ? (
                <>
                    <h1 className={`${styles.title} ${poppins.className} `}>
                        Actualizando Marca {brand.name}
                    </h1>
                    <UpdateBrandForm brand={brand} />
                </>
            ) : (
                <h1 className={`${styles.title} ${poppins.className} `}>
                    No se encontró la marca
                </h1>
            )}
        </div>
    );
}
