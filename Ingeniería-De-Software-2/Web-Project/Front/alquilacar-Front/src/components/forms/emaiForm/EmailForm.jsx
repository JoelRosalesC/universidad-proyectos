"use client";
import { useActionState, useEffect } from "react";
import { submitCodeForRecoveryPassword } from "@/lib/submitActions/submitCodeForRecoveryPassword";
import BaseForm from "@/components/forms/baseForm/BaseForm";
import SimpleInput from "@/components/inputs/SimpleInput";
import styles from "./emailForm.module.scss";

export default function EmailForm({ setCodeSent, setEmail }) {
    const [state, submitAction, isPending] = useActionState(
        submitCodeForRecoveryPassword,
        null
    );

    useEffect(() => {
        if (state?.success && state?.code_sent) {
            setCodeSent(true);
            setEmail(state?.email);
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
                label={"Email"}
                name={"email"}
                type={"email"}
                required
                defaultValue={state?.inputs?.email}
                error={state?.error?.Mail}
            />
        </BaseForm>
    );
}
