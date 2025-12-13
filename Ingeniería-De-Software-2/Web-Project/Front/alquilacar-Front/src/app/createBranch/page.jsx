"use client";
import styles from "./createBranch.module.scss";
import { poppins } from "@/lib/fonts/fonts";
import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";
import CreateBranchForm from "@/components/forms/createBranchForm/CreateBranchForm";

export default function CreateBrand() {
    const loading = useProtectedRoute(["Admin"]);

    if (loading) return null;
    return (
        <div className={styles.mainContainer}>
            <h1 className={`${styles.title} ${poppins.className} `}>
                Crear Sucursal
            </h1>
            <CreateBranchForm />
        </div>
    );
}
