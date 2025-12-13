import { useEffect, useRef } from "react";
import styles from "./userMenu.module.scss";
import { useRouter } from "next/navigation";
import Cross from "@/lib/svg/Cross";

export default function UserMenu({
    children,
    className,
    setLogin,
    setShowUserMenu,
}) {
    const classes = `${styles.userMenu} ${className || ""}`;
    const router = useRouter();
    const menuRef = useRef(null);

    const logOut = () => {
        localStorage.removeItem("token");
        setLogin(false);
        setShowUserMenu(false);
        router.push("/login");
    };

    useEffect(() => {
        const handleClickOutside = (event) => {
            if (menuRef.current && !menuRef.current.contains(event.target)) {
                setShowUserMenu(false);
            }
        };

        document.addEventListener("mousedown", handleClickOutside);
        return () => {
            document.removeEventListener("mousedown", handleClickOutside);
        };
    }, [setShowUserMenu]);

    return (
        <div ref={menuRef} className={classes}>
            <button
                className={styles.closeButton}
                onClick={() => setShowUserMenu(false)}
            >
                <Cross />
            </button>
            {children}
            <button className={styles.logout} onClick={() => logOut()}>
                Cerrar sesión
            </button>
        </div>
    );
}
