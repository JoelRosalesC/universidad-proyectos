"use client";
import styles from "./createEmployee.module.scss";
import { poppins } from "@/lib/fonts/fonts";
import CreateEmployeeForm from "@/components/forms/createEmployeeForm/CreateEmployeeForm";
import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";

export default function CreateEmployee() {
    const loading = useProtectedRoute(["Admin"]);

    if (loading) return null;
    return (
        <div className={styles.mainContainer}>
            <h1 className={`${styles.title} ${poppins.className} `}>
                Crear empleado
            </h1>
            <CreateEmployeeForm />
        </div>
    );
}
