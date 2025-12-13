"use client";
import { useActionState, useEffect } from "react";
import { submitUpdateBrandForm } from "@/lib/submitActions/submitUpdateBrandForm";
import BaseForm from "@/components/forms/baseForm/BaseForm";
import SimpleInput from "@/components/inputs/SimpleInput";
import styles from "./updateBrand.module.scss";
import { toast } from "sonner";
import { useRouter } from "next/navigation";

export default function UpdateBrandForm({ brand }) {
    const customSubmitAction = async (prevState, formData) => {
        const token = localStorage.getItem("token");
        if (token) formData.append("token", token);
        return await submitUpdateBrandForm(prevState, formData);
    };
    const [state, submitAction, isPending] = useActionState(
        customSubmitAction,
        null
    );
    const router = useRouter();
    useEffect(() => {
        if (state?.success) {
            toast.success("Marca editada con éxito", {
                duration: 3000,
                closeButton: true,
            });
            setTimeout(() => {
                router.push("/showBrands");
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
            <input type="hidden" name="id" value={brand.id} />
            <SimpleInput
                label={"Nombre"}
                name={"name"}
                type={"text"}
                defaultValue={state?.inputs?.Name ?? brand?.name}
                error={state?.error?.Name}
            />
        </BaseForm>
    );
}
