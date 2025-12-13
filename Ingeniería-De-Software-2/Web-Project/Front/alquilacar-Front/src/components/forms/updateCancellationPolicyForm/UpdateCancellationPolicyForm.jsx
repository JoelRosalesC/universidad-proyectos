"use client";
import { useActionState, useEffect } from "react";
import { submitUpdateCancellationPolicyForm } from "@/lib/submitActions/submitUpdateCancellationPolicyForm";
import BaseForm from "@/components/forms/baseForm/BaseForm";
import SimpleInput from "@/components/inputs/SimpleInput";
import styles from "./updateCancellationPolicyForm.module.scss";
import { toast } from "sonner";
import { useRouter } from "next/navigation";

export default function UpdateCancellationPolicyForm({ cancellationPolicy }) {
    const customSubmitAction = async (prevState, formData) => {
        const token = localStorage.getItem("token");
        if (token) formData.append("token", token);
        return await submitUpdateCancellationPolicyForm(prevState, formData);
    };
    const [state, submitAction, isPending] = useActionState(
        customSubmitAction,
        null
    );
    const router = useRouter();
    useEffect(() => {
        if (state?.success) {
            toast.success("Política de cancelación editada con éxito", {
                duration: 3000,
                closeButton: true,
            });
            setTimeout(() => {
                router.push("/showCancellationPolicies");
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
            <input type="hidden" name="id" value={cancellationPolicy.id} />
            <SimpleInput
                label={"descripción"}
                name={"description"}
                type={"text"}
                defaultValue={
                    state?.inputs?.Description ??
                    cancellationPolicy?.description
                }
                error={state?.error?.Description}
            />
            <SimpleInput
                label={"Porcentaje de devolución"}
                name={"returnPercentage"}
                type={"text"}
                defaultValue={
                    state?.inputs?.ReturnPercentage ??
                    cancellationPolicy?.returnPercentage
                }
                error={state?.error?.ReturnPercentage}
            />
        </BaseForm>
    );
}
