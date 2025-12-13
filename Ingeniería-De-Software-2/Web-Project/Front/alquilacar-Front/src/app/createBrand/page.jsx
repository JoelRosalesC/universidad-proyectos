"use client";
import styles from "./createBrand.module.scss";
import { poppins } from "@/lib/fonts/fonts";
import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";
import CreateBrandForm from "@/components/forms/createBrandForm/CreateBrandForm";

export default function CreateBrand() {
    const loading = useProtectedRoute(["Admin"]);

    if (loading) return null;
    return (
        <div className={styles.mainContainer}>
            <h1 className={`${styles.title} ${poppins.className} `}>
                Agregar Marca
            </h1>
            <CreateBrandForm />
        </div>
    );
}
