"use client";
import { useActionState, useEffect } from "react";
import { submitRecoveryPasswordForm } from "@/lib/submitActions/submitRecoveryPasswordForm";
import BaseForm from "@/components/forms/baseForm/BaseForm";
import SimpleInput from "@/components/inputs/SimpleInput";
import styles from "./recoveryPasswordForm.module.scss";
import { useRouter } from "next/navigation";
import { toast } from "sonner";

export default function RecoveryPasswordForm({ email }) {
    const router = useRouter();
    const [state, submitAction, isPending] = useActionState(
        submitRecoveryPasswordForm,
        null
    );

    useEffect(() => {
        if (state?.success) {
            toast.success("Contraseña cambiada exitosamente", {
                duration: 3000,
                closeButton: true,
            });
            const timeout = setTimeout(() => {
                router.push("/login");
            }, 1200);
            return () => clearTimeout(timeout);
        }
    }, [state, router]);

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
                label={"Email"}
                name={"email"}
                type={"email"}
                required
                readOnly
                defaultValue={email}
                error={state?.error?.Mail}
            />
            <SimpleInput
                label={"Nueva Contraseña"}
                name={"password"}
                type={"password"}
                required
                defaultValue={state?.inputs?.password}
                error={state?.error?.Password}
            />

            <SimpleInput
                label={"Código"}
                name={"code"}
                type={"text"}
                required
                defaultValue={state?.inputs?.code}
                error={state?.error?.Code}
            />
        </BaseForm>
    );
}
