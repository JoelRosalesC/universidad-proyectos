"use client";
import styles from "./register.module.scss";
import { poppins } from "@/lib/fonts/fonts";
import Link from "next/link";
import GenerateSignUpCodeForm from "@/components/forms/generateSignUpCodeForm/GenerateSignUpCodeForm";
import { useState } from "react";
import RegisterForm from "@/components/forms/registerForm/RegisterForm";

export default function Register() {
    const [showRegisterForm, setShowRegisterForm] = useState(false);
    const [email, setEmail] = useState(null);
    return (
        <div
            className={`${styles.mainContainer} ${
                showRegisterForm ? styles.withRegisterForm : ""
            }`}
        >
            <div className={styles.leftContainer}>
                <h2 className={`${styles.textOverlay} ${poppins.className}`}>
                    Sentite libre, manejá con estilo.
                    <br /> Unite a Alquilapp Car hoy.
                </h2>
                <img
                    className={styles.leftImg}
                    src="/img/register.jpeg"
                    alt="auto en primer plano"
                />
            </div>
            <div className={styles.rightContainer}>
                {!showRegisterForm ? (
                    <h1 className={`${styles.title} ${poppins.className}`}>
                        Registrate <br /> en segundos
                    </h1>
                ) : (
                    <h1
                        className={`${styles.title} ${styles.codeInstruction} ${poppins.className}`}
                    >
                        Revisá tu correo electrónico.
                        <br /> Te enviamos un código para completar tu registro.
                    </h1>
                )}

                {!showRegisterForm ? (
                    <GenerateSignUpCodeForm
                        setShowRegisterForm={setShowRegisterForm}
                        setEmail={setEmail}
                    />
                ) : (
                    <RegisterForm email={email} />
                )}
                <Link className={styles.link} href="/login">
                    ¿Ya tenés cuenta?&nbsp;
                    <p>Ingresa</p>
                </Link>
            </div>
        </div>
    );
}
