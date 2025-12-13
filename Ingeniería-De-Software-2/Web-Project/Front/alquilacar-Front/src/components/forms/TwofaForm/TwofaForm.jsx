"use client";
import { useRouter } from "next/navigation";
import { useActionState, useEffect } from "react";
import { submit2faForm } from "@/lib/submitActions/submit2faForm";
import BaseForm from "@/components/forms/baseForm/BaseForm";
import SimpleInput from "@/components/inputs/SimpleInput";
import styles from "./TwofaFrom.module.scss";
import SubmitBtn from "@/components/submitBtn/submitBtn";
import { useAuth } from "@/context/AuthContext";

export default function TwofaForm({ className, email, setShow2faPopup }) {
    const classes = `${styles.formContainer} ${className || ""}`;
    const router = useRouter();
    const [state, submitAction, isPending] = useActionState(
        submit2faForm,
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
        document.body.classList.add("overflowHidden");
        return () => {
            document.body.classList.remove("overflowHidden");
        };
    }, []);

    const cancelBtn = (
        <SubmitBtn
            className={styles.cancelBtn}
            type={"button"}
            onClick={() => {
                setShow2faPopup(false);
            }}
        >
            Cancelar
        </SubmitBtn>
    );
    return (
        <BaseForm
            className={classes}
            submitAction={submitAction}
            state={state}
            isPending={isPending}
            generalError={
                !state?.success && state?.error?.generalError
                    ? state.error.generalError
                    : ""
            }
            moreButtons={cancelBtn}
        >
            <SimpleInput
                className={styles.hidden}
                label={""}
                name={"email"}
                type={"hidden"}
                required
                defaultValue={email}
            />
            <SimpleInput
                label={"Código"}
                name={"code"}
                required
                error={state?.error?.code}
            />
        </BaseForm>
    );
}
