"use client";
import { useActionState, useEffect } from "react";
import { submitUpdateBranchForm } from "@/lib/submitActions/submitUpdateBranchForm";
import BaseForm from "@/components/forms/baseForm/BaseForm";
import SimpleInput from "@/components/inputs/SimpleInput";
import styles from "./updateBranchForm.module.scss";
import { toast } from "sonner";
import { useRouter } from "next/navigation";

export default function UpdateBranchForm({ branch }) {
    const customSubmitAction = async (prevState, formData) => {
        const token = localStorage.getItem("token");
        if (token) formData.append("token", token);
        return await submitUpdateBranchForm(prevState, formData);
    };
    const [state, submitAction, isPending] = useActionState(
        customSubmitAction,
        null
    );
    const router = useRouter();
    useEffect(() => {
        if (state?.success) {
            toast.success("Sucursal editada con éxito", {
                duration: 3000,
                closeButton: true,
            });
            setTimeout(() => {
                router.push("/showBranches");
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
            <input type="hidden" name="id" value={branch.id} />
            <SimpleInput
                label={"Nombre"}
                name={"name"}
                type={"text"}
                defaultValue={state?.inputs?.Name ?? branch?.name}
                error={state?.error?.Name}
            />
            <SimpleInput
                label={"Provincia/Estado"}
                name={"province"}
                type={"text"}
                defaultValue={state?.inputs?.Province ?? branch?.province}
                error={state?.error?.Province}
            />
            <SimpleInput
                label={"Localidad/Ciudad/Municipio"}
                name={"locality"}
                type={"text"}
                defaultValue={state?.inputs?.Locality ?? branch?.locality}
                error={state?.error?.Locality}
            />
        </BaseForm>
    );
}
