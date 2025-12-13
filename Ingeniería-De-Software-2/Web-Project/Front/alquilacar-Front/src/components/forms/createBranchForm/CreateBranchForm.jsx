"use client";
import { useActionState, useEffect } from "react";
import { submitCreateBranchForm } from "@/lib/submitActions/submitCreateBranchForm";
import BaseForm from "@/components/forms/baseForm/BaseForm";
import SimpleInput from "@/components/inputs/SimpleInput";
import styles from "./createBranchForm.module.scss";
import { toast } from "sonner";

export default function CreateBranchForm() {
    const customSubmitAction = async (prevState, formData) => {
        const token = localStorage.getItem("token");
        if (token) formData.append("token", token);
        return await submitCreateBranchForm(prevState, formData);
    };
    const [state, submitAction, isPending] = useActionState(
        customSubmitAction,
        null
    );

    useEffect(() => {
        if (state?.success) {
            toast.success("Sucursal creada con éxito", {
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
                label={"Nombre"}
                name={"name"}
                type={"text"}
                required
                defaultValue={state?.inputs?.Name}
                error={state?.error?.Name}
            />
            <SimpleInput
                label={"Provincia/Estado"}
                name={"province"}
                type={"text"}
                required
                defaultValue={state?.inputs?.Province}
                error={state?.error?.Province}
            />
            <SimpleInput
                label={"Localidad/Ciudad/Municipio"}
                name={"locality"}
                type={"text"}
                required
                defaultValue={state?.inputs?.Locality}
                error={state?.error?.Locality}
            />
        </BaseForm>
    );
}
