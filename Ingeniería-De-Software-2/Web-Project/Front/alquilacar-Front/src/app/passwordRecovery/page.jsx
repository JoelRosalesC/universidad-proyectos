//fetch que se mande el codigo avisando
//mostrar el form de recuperacion
//enviar fetch a cambiar

"use client";
import EmailForm from "@/components/forms/emaiForm/EmailForm";
import styles from "./passwordRecovery.module.scss";
import { poppins } from "@/lib/fonts/fonts";
import Link from "next/link";
import { useState } from "react";
import RecoveryPasswordForm from "@/components/forms/recoveryPasswordForm/recoveryPasswordForm";

export default function PasswordRecovery() {
    const [codeSent, setcodeSent] = useState(false);
    const [email, setEmail] = useState("");
    return (
        <div className={styles.mainContainer}>
            <h1 className={`${styles.title} ${poppins.className}`}>
                Recupera tu contraseña
            </h1>
            {!codeSent ? (
                <>
                    <h3 className={`${styles.subtitle} ${poppins.className}`}>
                        Ingresa tu correo.
                    </h3>
                    <EmailForm setCodeSent={setcodeSent} setEmail={setEmail} />
                </>
            ) : (
                <>
                    <h3 className={`${styles.subtitle} ${poppins.className}`}>
                        Revisa tu correo , hemos enviado un código para que
                        restablezcas tu contraseña.
                    </h3>
                    <RecoveryPasswordForm email={email} />
                </>
            )}
        </div>
    );
}
