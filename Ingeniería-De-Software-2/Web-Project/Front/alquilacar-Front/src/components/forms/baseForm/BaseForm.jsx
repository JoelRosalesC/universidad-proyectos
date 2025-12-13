"use client";
import SubmitBtn from "@/components/submitBtn/submitBtn";
import styles from "./baseForm.module.scss";
import Loading from "@/lib/svg/Loading";
import Check from "@/lib/svg/Check";
import Cross from "@/lib/svg/Cross";
import { useEffect, useState } from "react";

export default function BaseForm({
    className,
    submitAction,
    children,
    generalError = null,
    isPending,
    state,
    moreButtons = null,
    submitLabel = "Enviar",
}) {
    const classes = ` ${styles.formContainer}  ${className || ""}`;
    const [thereIsError, setThereIsError] = useState(false);
    const [ThereWasSuccess, setThereWasSuccess] = useState(false);

    useEffect(() => {
        // para manejar los iconos en el boton
        let timer;
        if (state?.success || state?.error) {
            if (state?.success) {
                setThereWasSuccess(true);
            }
            if (state?.error) {
                setThereIsError(true);
            }
            timer = setTimeout(() => {
                setThereWasSuccess(false);
                setThereIsError(false);
            }, 2000);
        }
        return () => clearTimeout(timer);
    }, [state]);

    return (
        <div className={classes}>
            <form className={styles.form} action={submitAction}>
                {children}
                <div className={styles.buttonsContainer}>
                    <SubmitBtn
                        className={`${styles.sendButton} ${
                            ThereWasSuccess ? styles.success : ""
                        } ${thereIsError ? styles.error : ""}`}
                        disabled={ThereWasSuccess || thereIsError}
                        type="submit"
                    >
                        {isPending ? (
                            <Loading />
                        ) : ThereWasSuccess ? (
                            <Check />
                        ) : thereIsError ? (
                            <Cross />
                        ) : (
                            submitLabel
                        )}
                    </SubmitBtn>
                    {moreButtons}
                </div>
            </form>
            {generalError && (
                <div className={styles.generalError}>{generalError}</div>
            )}
        </div>
    );
}
