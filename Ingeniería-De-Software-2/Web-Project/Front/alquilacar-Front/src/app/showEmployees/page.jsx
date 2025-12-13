"use client";

import { useEffect, useState } from "react";
import styles from "./showEmployees.module.scss";
import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";
import Table from "@/components/table/Table";
import CreateBtn from "@/components/createBtn/CreateBtn";
import { poppins } from "@/lib/fonts/fonts";
import { toast } from "sonner";
import { DeleteEmployee } from "@/lib/deleteEmployee";

export default function Page() {
    const [employees, setEmployees] = useState([]);
    const loading = useProtectedRoute(["Admin"]);

    const token = localStorage.getItem("token");

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
    // Hacer la solicitud a la API
    useEffect(() => {
        const fetchEmployees = async () => {
            try {
                const response = await fetch(
                    "http://localhost:5296/api/Employee",
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
                const employeesWhitBranchName = data.data.map((employee) => {
                    const branch = branches.find(
                        (branch) => branch.id === employee.workBranch
                    );
                    return {
                        ...employee,
                        Sucursal: branch
                            ? branch.name
                            : "Sucursal no encontrada",
                    };
                });
                setEmployees(employeesWhitBranchName);
            } catch (error) {
                console.error("Error al obtener empleados:", error);
            }
        };

        fetchEmployees();
    }, [branches]);

    const handleDelete = async (item, token) => {
        const response = await DeleteEmployee(item.id, token);
        if (response.success) {
            toast.success(`Empleado eliminado exitosamente. `, {
                duration: 3000,
                closeButton: true,
            });
            setTimeout(() => {
                window.location.reload();
            }, 1300);
        } else {
            toast.error(
                response?.error?.generalError ||
                    "Error al eliminar el empleado",
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
                    Lista de Empleados
                </h1>
                <CreateBtn href="/createEmployee" />
            </div>
            {employees.length > 0 ? (
                <Table
                    data={employees}
                    editPath={"/updateEmployee/"}
                    onDelete={handleDelete}
                    confirmationModalText="¿Estás seguro de eliminar el empleado?"
                />
            ) : (
                <p>No se encontraron empleados</p>
            )}
        </div>
    );
}
