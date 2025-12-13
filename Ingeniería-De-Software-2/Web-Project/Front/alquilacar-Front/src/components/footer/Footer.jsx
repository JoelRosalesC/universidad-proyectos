"use client";

import styles from "./footer.module.scss";
import Link from "next/link";
import { usePathname } from "next/navigation";

export default function Footer() {
    const pathname = usePathname();

    return (
        <div className={styles.footerContainer}>
            <img className={styles.logo} src="/img/logo.png" alt="" />
            <div className={styles.linksContainer}>
                <Link
                    className={`${styles.link} ${
                        pathname === "/" ? styles.selected : ""
                    }`}
                    href={"/"}
                >
                    Inicio
                </Link>
                <Link
                    className={`${styles.link} ${
                        pathname === "/catalog" ? styles.selected : ""
                    }`}
                    href={"/catalog"}
                >
                    Catalogo
                </Link>
                <Link
                    className={`${styles.link} ${
                        pathname === "/login" ? styles.selected : ""
                    }`}
                    href={"/login"}
                >
                    Iniciar sesión
                </Link>
                <Link
                    className={`${styles.link} ${
                        pathname === "/register" ? styles.selected : ""
                    }`}
                    href={"/register"}
                >
                    Registrarse
                </Link>
            </div>
            <p>© 2025 Alquilapp Car. Todos los derechos reservados.</p>
        </div>
    );
}
