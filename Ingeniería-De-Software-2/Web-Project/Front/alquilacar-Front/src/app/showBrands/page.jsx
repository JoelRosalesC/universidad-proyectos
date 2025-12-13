"use client";

import { useEffect, useState } from "react";
import styles from "./showBrands.module.scss";
import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";
import { poppins } from "@/lib/fonts/fonts";
import Table from "@/components/table/Table";
import CreateBtn from "@/components/createBtn/CreateBtn";
import { useAuth } from "@/context/AuthContext";

export default function Page() {
    const [brands, setBrands] = useState([]);
    const loading = useProtectedRoute(["Admin", "Employee"]);
    const { role } = useAuth();

    // Hacer la solicitud a la API
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
    if (loading) return null;

    return (
        <div className={styles.mainContainer}>
            <div className={styles.header}>
                <h1 className={`${styles.title} ${poppins.className} `}>
                    Lista de Marcas
                </h1>
                {role === "Admin" && <CreateBtn href="/createBrand" />}
            </div>
            <Table data={brands} editPath={"/updateBrand/"} />
        </div>
    );
}
