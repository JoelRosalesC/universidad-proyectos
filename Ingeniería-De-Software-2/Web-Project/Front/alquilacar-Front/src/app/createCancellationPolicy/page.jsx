"use client";
import styles from "./createCancellationPolicy.module.scss";
import { poppins } from "@/lib/fonts/fonts";

import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";
import CreateCancellationPolicyForm from "@/components/forms/createCancellationPolicyForm/CreateCancellationPolicyForm";

export default function createCancellationPolicy() {
    const loading = useProtectedRoute(["Admin"]);

    if (loading) return null;
    return (
        <div className={styles.mainContainer}>
            <h1 className={`${styles.title} ${poppins.className} `}>
                Crear política de cancelación
            </h1>
            <CreateCancellationPolicyForm />
        </div>
    );
}
