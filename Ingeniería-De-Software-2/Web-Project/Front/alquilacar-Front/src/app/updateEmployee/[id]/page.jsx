"use client";

import { useEffect, useState } from "react";
import styles from "./updateEmployee.module.scss";
import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";
import UpdateEmployeeForm from "@/components/forms/updateEmployeeForm/UpdateEmployeeForm";
import { useParams } from "next/navigation";
import { poppins } from "@/lib/fonts/fonts";

export default function Page() {
    const loading = useProtectedRoute(["Admin"]);
    const { id } = useParams();
    const token = localStorage.getItem("token");
    const [employee, setEmployee] = useState(null);

    // Hacer la solicitud a la API
    useEffect(() => {
        const fetchEmployee = async () => {
            try {
                const response = await fetch(
                    `http://localhost:5296/api/Employee/${id}`,
                    {
                        method: "GET",
                        headers: {
                            "Content-Type": "application/json",
                            Authorization: `Bearer ${token}`,
                        },
                    }
                );

                if (!response.ok) {
                    throw new Error("Error al obtener el empleado");
                }

                const data = await response.json();

                const formatDate = (dateString) =>
                    new Date(dateString).toISOString().split("T")[0];
                setEmployee({
                    ...data.data,
                    birthdate: formatDate(data.data.birthdate),
                });
            } catch (error) {
                console.error("Error al obtener el empleado:", error);
            }
        };
        if (id) {
            fetchEmployee();
        }
    }, [id]);
    if (loading) return null;
    return (
        <div className={styles.mainContainer}>
            {employee ? (
                <>
                    <h1 className={`${styles.title} ${poppins.className} `}>
                        Actualizando el empleado {employee.email}
                    </h1>
                    <UpdateEmployeeForm employee={employee} />
                </>
            ) : (
                <h1 className={`${styles.title} ${poppins.className} `}>
                    No se encontró el empleado
                </h1>
            )}
        </div>
    );
}
