"use client";
import LoginForm from "@/components/forms/loginForm/LoginForm";
import styles from "./login.module.scss";
import { poppins } from "@/lib/fonts/fonts";
import Link from "next/link";

export default function Login() {
    return (
        <div className={styles.mainContainer}>
            <div className={styles.leftContainer}>
                <h2 className={`${styles.textOverlay} ${poppins.className}`}>
                    Elegí, reservá y manejá. <br /> Así de simple.
                </h2>
                <img
                    className={styles.leftImg}
                    src="/img/login.jpeg"
                    alt="auto en primer plano"
                />
            </div>
            <div className={styles.rightContainer}>
                <h1 className={`${styles.title} ${poppins.className}`}>
                    Iniciá sesión y <br /> empezá tu viaje
                </h1>
                <LoginForm />
                <Link className={styles.link} href="/passwordRecovery">
                    ¿Olvidaste tu contraseña?
                </Link>

                <Link className={styles.link} href="/register">
                    ¿Todavía no tenés cuenta?&nbsp;
                    <p>Registrate</p>
                </Link>
            </div>
        </div>
    );
}
