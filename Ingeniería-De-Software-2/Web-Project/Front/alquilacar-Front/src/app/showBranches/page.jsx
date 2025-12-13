"use client";

import { useEffect, useState } from "react";
import styles from "./showBranches.module.scss";
import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";
import Table from "@/components/table/Table";
import { useRouter } from "next/navigation";
import CreateBtn from "@/components/createBtn/CreateBtn";
import { poppins } from "@/lib/fonts/fonts";
import { useAuth } from "@/context/AuthContext";
import { toast } from "sonner";
import { DeleteBranch } from "@/lib/deleteBranch";

export default function Page() {
    const token = localStorage.getItem("token");
    const loading = useProtectedRoute(["Admin", "Employee"]);
    const { role } = useAuth();
    const [branches, setBranches] = useState([]);
    // Hacer la solicitud a la API
    useEffect(() => {
        const fetchBranches = async () => {
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
                setBranches(data.data);
            } catch (error) {
                console.error("Error al obtener sucursales:", error);
            }
        };

        fetchBranches();
    }, []);

    const handleDelete = async (item, token) => {
        const response = await DeleteBranch(item.id, token);
        if (response.success) {
            toast.success(`Sucursal eliminada exitosamente. `, {
                duration: 3000,
                closeButton: true,
            });
            setTimeout(() => {
                window.location.reload();
            }, 1300);
        } else {
            toast.error(
                response?.error?.generalError ||
                    "Error al eliminar la sucursal",
                {
                    duration: 3000,
                    closeButton: true,
                }
            );
        }
    };

    if (loading) return null;
    return (
        <div className={styles.mainContainer}>
            <div className={styles.header}>
                <h1 className={`${styles.title} ${poppins.className} `}>
                    Lista de sucursales
                </h1>
                {role === "Admin" && <CreateBtn href="/createBranch" />}
            </div>
            <Table
                data={branches}
                editPath={"/updateBranch/"}
                onDelete={handleDelete}
                confirmationModalText="¿Estás seguro de eliminar la sucursal?"
            />
        </div>
    );
}
