"use client";
import { useActionState, useEffect } from "react";
import { submitCreateCancellationPolicyForm } from "@/lib/submitActions/submitCreateCancellationPolicyForm";
import BaseForm from "@/components/forms/baseForm/BaseForm";
import SimpleInput from "@/components/inputs/SimpleInput";
import styles from "./createCancellationPolicyForm.module.scss";
import { toast } from "sonner";

export default function CreateCancellationPolicyForm() {
    const customSubmitAction = async (prevState, formData) => {
        const token = localStorage.getItem("token");
        if (token) formData.append("token", token);
        return await submitCreateCancellationPolicyForm(prevState, formData);
    };
    const [state, submitAction, isPending] = useActionState(
        customSubmitAction,
        null
    );

    useEffect(() => {
        if (state?.success) {
            toast.success("Política de cancelación creada con éxito", {
                duration: 3000,
                closeButton: true,
            });
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
            <SimpleInput
                label={"descripción"}
                name={"description"}
                type={"text"}
                // required
                defaultValue={state?.inputs?.Description}
                error={state?.error?.Description}
            />
            <SimpleInput
                label={"Porcentaje de devolución"}
                name={"returnPercentage"}
                type={"text"}
                // required
                defaultValue={state?.inputs?.ReturnPercentage}
                error={state?.error?.ReturnPercentage}
            />
        </BaseForm>
    );
}
