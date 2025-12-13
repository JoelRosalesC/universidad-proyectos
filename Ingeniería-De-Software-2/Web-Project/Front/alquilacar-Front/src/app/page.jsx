"use client";
import Image from "next/image";
import styles from "./page.module.scss";
import Cup from "@/lib/svg/Cup";
import Car from "@/lib/svg/Car";
import Branch from "@/lib/svg/Branch";
import Star from "@/lib/svg/Star";
import CarsAvailableForm from "@/components/forms/carsAvailableForm/CarsAvailableForm";
import { poppins } from "@/lib/fonts/fonts";
import { useAuth } from "@/context/AuthContext";

export default function Home() {
    const { login, role } = useAuth();

    return (
        <div className={styles.homeContainer}>
            {/* <h1>{role ? `Hola ${role}` : "Home Page"}</h1> */}
            <div className={styles.heroSection}>
                <video
                    className={styles.videoBackground}
                    loop
                    autoPlay
                    muted
                    playsInline
                >
                    <source src="video/video1.mp4" type="video/mp4" />
                    Tu navegador no soporta videos HTML5.
                </video>
                <div className={styles.videoOverlay}></div>
                <div className={styles.tag}>
                    <p>
                        <Cup />
                        +20 años de experiencia
                    </p>
                </div>
                <h1 className={`${styles.title} ${poppins.className} `}>
                    Líderes en Alquiler de Vehículos
                </h1>
                <h2 className={`${styles.title2} ${poppins.className} `}>
                    Experiencia, Confianza y Movilidad a Tu Medida
                </h2>
                <p className={styles.subtitle}>
                    Más de dos decadas brindando el mejor servicio y los mejores
                    precios del mercado
                </p>
                <a href="/catalog" className={styles.catalogo}>
                    Conoce nuestro catálogo
                </a>
                <div className={styles.tag}>
                    <p>
                        <Car />
                        +5000 Autos
                    </p>
                    <p>
                        <Branch />
                        50+ Sucursales
                    </p>
                    <p>
                        <Star className={styles.star} />
                        +1M Clientes
                    </p>
                </div>
            </div>
            {login && role == "Customer" && (
                <div
                    className={styles.reservationSection}
                    id="reservationSection"
                >
                    <h1 className={`${styles.title} ${poppins.className} `}>
                        Tu Aventura Comienza Aquí
                    </h1>
                    <p className={styles.subtitle}>
                        Encuentra el auto perfecto para tu próximo viaje.
                    </p>
                    <CarsAvailableForm />
                </div>
            )}
        </div>
    );
}
