"use client";
import styles from "./createVehicle.module.scss";
import { poppins } from "@/lib/fonts/fonts";
import CreateVehicleForm from "@/components/forms/createvehicleForm/CreateVehicleForm";
import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";

export default function CreateVehicle() {
    const loading = useProtectedRoute(["Admin"]);

    if (loading) return null;
    return (
        <div className={styles.mainContainer}>
            <h1 className={`${styles.title} ${poppins.className} `}>
                Crear vehiculo
            </h1>
            <CreateVehicleForm />
        </div>
    );
}
