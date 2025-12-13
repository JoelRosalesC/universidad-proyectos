"use client";
import styles from "./createVehicleType.module.scss";
import { poppins } from "@/lib/fonts/fonts";
import CreateVehicleTypeForm from "@/components/forms/CreateVehicleTypeForm/CreateVehicleTypeForm";
import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";

export default function CreateVehicleType() {
    const loading = useProtectedRoute(["Admin"]);

    if (loading) return null;
    return (
        <div className={styles.mainContainer}>
            <h1 className={`${styles.title} ${poppins.className} `}>
                Crear tipo de vehiculo
            </h1>
            <CreateVehicleTypeForm />
        </div>
    );
}
