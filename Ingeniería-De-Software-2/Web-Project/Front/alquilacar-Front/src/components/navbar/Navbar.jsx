"use client";

import User from "@/lib/svg/User";
import styles from "./nabvar.module.scss";
import Link from "next/link";
import { usePathname, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import UserMenu from "../userMenu/UserMenu";
import Chevron from "@/lib/svg/Chevron";
import { useAuth } from "@/context/AuthContext";
import { DeleteAccount } from "@/lib/deleteAccount";
import { toast } from "sonner";

export default function Navbar() {
    const pathname = usePathname();
    const { login, setLogin, role } = useAuth();
    const [showUserMenu, setShowUserMenu] = useState(null);
    const [openConfirmationModal, setOpenConfirmationModal] = useState(false);
    const token = localStorage.getItem("token");
    const router = useRouter();

    const handleDeleteAccount = async (token) => {
        const response = await DeleteAccount(token);
        if (response.success) {
            localStorage.removeItem("token");
            setLogin(false);
            setOpenConfirmationModal(false);
            toast.success(`Cuenta eliminada correctamente`, {
                duration: 3000,
                closeButton: true,
            });
            setTimeout(() => {
                router.push(`/`);
            }, 1500);
        } else {
            toast.error(
                response?.error?.generalError || "Error al eliminar mi cuenta",
                {
                    duration: 3000,
                    closeButton: true,
                }
            );
        }
    };
    return (
        <div className={styles.navbarContainer}>
            {openConfirmationModal && (
                <div className={styles.deleteConfirmationModalContainer}>
                    <div className={styles.deleteConfirmationModal}>
                        <p>¿Seguro desea eliminar su cuenta?</p>

                        <div className={styles.btnsContainer}>
                            <button
                                className={styles.confirm}
                                onClick={() => handleDeleteAccount(token)}
                            >
                                Si, confirmar
                            </button>
                            <button
                                className={styles.cancel}
                                onClick={() => setOpenConfirmationModal(false)}
                            >
                                Volver
                            </button>
                        </div>
                    </div>
                </div>
            )}
            <img className={styles.logo} src="/img/logo.png" alt="" />
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
            {role === "Customer" && login && (
                <Link
                    className={`${styles.link}`}
                    href={"/#reservationSection"}
                >
                    Hace tu Reserva
                </Link>
            )}

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
            {login && (
                <button
                    className={styles.userMenu}
                    onClick={() => {
                        setShowUserMenu(!showUserMenu);
                    }}
                >
                    <Chevron
                        className={`${styles.chevron} ${
                            showUserMenu ? styles.open : ""
                        }`}
                    />
                    <User />
                </button>
            )}
            {showUserMenu && (
                <UserMenu setLogin={setLogin} setShowUserMenu={setShowUserMenu}>
                    {role === "Admin" && (
                        <>
                            <Link
                                className={styles.menuItem}
                                href={"/showBranches"}
                            >
                                Sucursales
                            </Link>

                            <Link
                                className={styles.menuItem}
                                href={"/showEmployees"}
                            >
                                Empleados
                            </Link>

                            {/*
                            <Link
                                className={styles.menuItem}
                                href={"/showCancellationPolicies"}
                            >
                                Políticas de cancelación
                            </Link>
                            <Link
                                className={styles.menuItem}
                                href={"/showBrands"}
                            >
                                Marcas
                            </Link> 
                            <Link
                                className={styles.menuItem}
                                href={"/showVehicleTypes"}
                            >
                                Tipos de vehiculos
                            </Link>*/}
                            <Link
                                className={styles.menuItem}
                                href={"/showVehicles"}
                            >
                                Vehiculos
                            </Link>
                            <Link
                                className={styles.menuItem}
                                href={"/showRentals"}
                            >
                                Alquileres
                            </Link>
                            <Link
                                className={styles.menuItem}
                                href={"/generalStatistics"}
                            >
                                Estadísticas de Clientes
                            </Link>
                            <Link
                                className={styles.menuItem}
                                href={"/rentalStatistics"}
                            >
                                Estadísticas de alquileres
                            </Link>
                            <Link
                                className={styles.menuItem}
                                href={"/revenueStatistics"}
                            >
                                Estadísticas de Ingresos
                            </Link>
                        </>
                    )}
                    {role === "Employee" && (
                        <>
                            {/* <Link
                                className={styles.menuItem}
                                href={"/showBranches"}
                            >
                                Sucursales
                            </Link>

                            <Link
                                className={styles.menuItem}
                                href={"/showCancellationPolicies"}
                            >
                                Políticas de cancelación
                            </Link>
                            <Link
                                className={styles.menuItem}
                                href={"/showBrands"}
                            >
                                Marcas
                            </Link> 
                            <Link
                                className={styles.menuItem}
                                href={"/showVehicleTypes"}
                            >
                                Tipos de vehiculos
                            </Link>*/}
                            <Link
                                className={styles.menuItem}
                                href={"/showVehicles"}
                            >
                                Vehiculos
                            </Link>
                            <Link
                                className={styles.menuItem}
                                href={"/cancelReservation"}
                            >
                                Reservas en curso
                            </Link>
                            <Link
                                className={styles.menuItem}
                                href={"/showRentals"}
                            >
                                Alquileres
                            </Link>
                            <Link
                                className={styles.menuItem}
                                href={"/rentalWithReservation"}
                            >
                                Alquiler con reserva
                            </Link>
                            <Link
                                className={styles.menuItem}
                                href={"/rentalWithoutReservation"}
                            >
                                Alquiler sin reserva
                            </Link>
                            <Link
                                className={styles.menuItem}
                                href={"/returnVehicle"}
                            >
                                Devolución de vehiculos
                            </Link>
                        </>
                    )}
                    {role === "Customer" && (
                        <>
                            <Link
                                className={styles.menuItem}
                                href={"/reservationHistory"}
                            >
                                Historial de mis reservas
                            </Link>
                            <Link
                                className={styles.menuItem}
                                href={"/myRentals"}
                            >
                                Historial de mis alquileres
                            </Link>
                            <button
                                className={`${styles.menuItem} ${styles.deleteAccount}`}
                                onClick={() => {
                                    setOpenConfirmationModal(true);
                                }}
                            >
                                Eliminar mi cuenta
                            </button>
                        </>
                    )}
                </UserMenu>
            )}
        </div>
    );
}
