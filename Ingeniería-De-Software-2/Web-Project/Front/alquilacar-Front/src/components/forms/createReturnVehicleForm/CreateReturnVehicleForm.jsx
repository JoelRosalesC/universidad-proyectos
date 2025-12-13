"use client";
import { useActionState, useEffect } from "react";
import { submitReturnVehicleForm } from "@/lib/submitActions/submitReturnVehicleForm";
import BaseForm from "@/components/forms/baseForm/BaseForm";
import styles from "./createReturnVehicleForm.module.scss";
import { toast } from "sonner";
import SelectInput from "@/components/inputs/SelectInput/SelectInput";

export default function CreateReturnVehicleForm({ reservations }) {
    const token = localStorage.getItem("token");

    const customSubmitAction = async (prevState, formData) => {
        const token = localStorage.getItem("token");
        if (token) formData.append("token", token);
        return await submitReturnVehicleForm(prevState, formData);
    };
    const [state, submitAction, isPending] = useActionState(
        customSubmitAction,
        null
    );

    useEffect(() => {
        if (state?.success) {
            toast.success("Vehiculo devuelto exitosamente", {
                duration: 3000,
                closeButton: true,
            });
            setTimeout(() => {
                window.location.reload();
            }, 1500);
        }
    }, [state]);

    return (
        <BaseForm
            className={styles.formContainer}
            submitAction={submitAction}
            state={state}
            isPending={isPending}
            generalError={
                !state?.success && state?.error?.generalError
                    ? state.error.generalError
                    : ""
            }
        >
            <SelectInput
                key={`rentalId-${Date.now()}`}
                label={"Reservaciones en curso"}
                name={"rentalId"}
                required
                defaultSelectedOption={state?.inputs?.rentalId}
                options={reservations}
                error={state?.error?.rentalId}
            />
        </BaseForm>
    );
}
