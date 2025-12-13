"use client";
import { useActionState, useEffect, useState } from "react";
import { submitLoginForm } from "@/lib/submitActions/submitLoginForm";
import BaseForm from "@/components/forms/baseForm/BaseForm";
import SimpleInput from "@/components/inputs/SimpleInput";
import styles from "./loginForm.module.scss";
import { useRouter } from "next/navigation";
import TwofaForm from "../TwofaForm/TwofaForm";
import { poppins } from "@/lib/fonts/fonts";
import { useAuth } from "@/context/AuthContext";

export default function LoginForm() {
    const router = useRouter();
    const [show2faPopup, setShow2faPopup] = useState(false);
    const [state, submitAction, isPending] = useActionState(
        submitLoginForm,
        null
    );
    const { setLogin, setRole } = useAuth();

    useEffect(() => {
        if (state?.success && state?.token) {
            localStorage.setItem("token", state.token);
            // Parsear rol del token para pasarlo al contexto
            let role = null;
            try {
                const payload = JSON.parse(atob(state.token.split(".")[1]));
                role = payload.role;
            } catch (err) {
                console.error("Error decoding token:", err);
            }

            // Actualizar contexto antes de redirigir

            setLogin(true);

            setRole(role);
            const timeout = setTimeout(() => {
                router.push("/");
            }, 1200);
            return () => clearTimeout(timeout);
        }
    }, [state, router]);

    useEffect(() => {
        if (state?.success && state?.require_code && state?.email) {
            setShow2faPopup(true);
        }
    }, [state]);
    return (
        <>
            {show2faPopup && (
                <div className={styles.twofaContainer}>
                    <div className={styles.twofaFormContainer}>
                        <h1 className={`${styles.title} ${poppins.className}`}>
                            Ingrese el código de verificación
                        </h1>
                        <TwofaForm
                            email={state?.email}
                            className={styles.twofaForm}
                            setShow2faPopup={setShow2faPopup}
                        />
                    </div>
                </div>
            )}
            <BaseForm
                className={styles.loginFormContainer}
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
                <SimpleInput
                    label={"Contraseña"}
                    name={"password"}
                    type={"password"}
                    required
                    defaultValue={state?.inputs?.password}
                    error={state?.error?.Password}
                />
            </BaseForm>
        </>
    );
}
