"use client";
import { useActionState, useEffect, useState } from "react";
import { submitRegisterForm } from "@/lib/submitActions/submitRegisterForm";
import BaseForm from "@/components/forms/baseForm/BaseForm";
import SimpleInput from "@/components/inputs/SimpleInput";
import styles from "./registerForm.module.scss";
import { useRouter } from "next/navigation";
import TwofaForm from "../TwofaForm/TwofaForm";
import { poppins } from "@/lib/fonts/fonts";

export default function RegisterForm({ email }) {
    const router = useRouter();
    // const [show2faPopup, setShow2faPopup] = useState(false);
    const [state, submitAction, isPending] = useActionState(
        submitRegisterForm,
        null
    );

    useEffect(() => {
        if (state?.success) {
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
                defaultValue={email}
                error={state?.error?.Mail}
                readOnly
            />
            <SimpleInput
                label={"Contraseña"}
                name={"password"}
                type={"password"}
                required
                defaultValue={state?.inputs?.password}
                error={state?.error?.Password}
            />
            <SimpleInput
                label={"Nombre"}
                name={"firstName"}
                type={"text"}
                required
                defaultValue={state?.inputs?.firstName}
                error={state?.error?.FirstName}
            />
            <SimpleInput
                label={"Apellido"}
                name={"lastName"}
                type={"text"}
                required
                defaultValue={state?.inputs?.lastName}
                error={state?.error?.LastName}
            />
            <SimpleInput
                label={"Dni"}
                name={"dni"}
                type={"text"}
                required
                defaultValue={state?.inputs?.dni}
                error={state?.error?.Dni}
            />
            <SimpleInput
                label={"Fecha de nacimiento"}
                name={"birthdate"}
                type={"date"}
                required
                defaultValue={state?.inputs?.birthdate}
                error={state?.error?.Birthdate}
            />
            <SimpleInput
                label={"Teléfono"}
                name={"phoneNumber"}
                type={"text"}
                required
                defaultValue={state?.inputs?.phoneNumber}
                error={state?.error?.PhoneNumber}
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
