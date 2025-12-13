"use client";
import { useActionState, useEffect } from "react";
import { submitCreateBrandForm } from "@/lib/submitActions/submitCreateBrandForm";
import BaseForm from "@/components/forms/baseForm/BaseForm";
import SimpleInput from "@/components/inputs/SimpleInput";
import styles from "./createBrandForm.module.scss";
import { toast } from "sonner";

export default function CreateBrandForm() {
    const customSubmitAction = async (prevState, formData) => {
        const token = localStorage.getItem("token");
        if (token) formData.append("token", token);
        return await submitCreateBrandForm(prevState, formData);
    };
    const [state, submitAction, isPending] = useActionState(
        customSubmitAction,
        null
    );

    useEffect(() => {
        if (state?.success) {
            toast.success("Marca creada con éxito", {
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
                label={"Marca"}
                name={"name"}
                type={"text"}
                required
                defaultValue={state?.inputs?.Name}
                error={state?.error?.Name}
            />
        </BaseForm>
    );
}
