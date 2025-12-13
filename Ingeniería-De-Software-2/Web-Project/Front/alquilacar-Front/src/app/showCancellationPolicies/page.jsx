"use client";

import { useEffect, useState } from "react";
import styles from "./showCancellationPolicies.module.scss";
import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";
import Table from "@/components/table/Table";
import CreateBtn from "@/components/createBtn/CreateBtn";
import { poppins } from "@/lib/fonts/fonts";
import { useAuth } from "@/context/AuthContext";

export default function Page() {
    const [cancellationPolicies, setCancellationPolicies] = useState([]);
    const loading = useProtectedRoute(["Admin", "Employee"]);
    const { role } = useAuth();

    const token = localStorage.getItem("token");
    // Hacer la solicitud a la API
    useEffect(() => {
        const fetchcancellationPolicies = async () => {
            try {
                const response = await fetch(
                    "http://localhost:5296/api/CancellationPolicy",
                    {
                        method: "GET",
                        headers: {
                            "Content-Type": "application/json",
                            Authorization: `Bearer ${token}`, // Agregar el token en los headers
                        },
                    }
                );

                if (!response.ok) {
                    throw new Error("Error al obtener empleados");
                }

                const data = await response.json();
                setCancellationPolicies(data.data);
            } catch (error) {
                console.error(
                    "Error al obtener politicas de cancelacion:",
                    error
                );
            }
        };

        fetchcancellationPolicies();
    }, []);
    if (loading) return null;
    return (
        <div className={styles.mainContainer}>
            <div className={styles.header}>
                <h1 className={`${styles.title} ${poppins.className} `}>
                    Lista de Políticas de cancelación
                </h1>
                {role === "Admin" && (
                    <CreateBtn href="/createCancellationPolicy" />
                )}
            </div>
            <Table
                data={cancellationPolicies}
                editPath={"/updateCancellationPolicy/"}
            />
        </div>
    );
}
