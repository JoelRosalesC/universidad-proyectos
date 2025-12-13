"use client";

import { createContext, useContext, useState, useEffect } from "react";

// Inicializamos el contexto con null
const AuthContext = createContext(null);

// Proveedor del contexto
export function AuthProvider({ children }) {
    const [login, setLogin] = useState(false);
    const [role, setRole] = useState(null);

    useEffect(() => {
        const token = localStorage.getItem("token");
        if (token) {
            setLogin(true);
            try {
                const payload = JSON.parse(atob(token.split(".")[1]));
                setRole(payload.role);
            } catch (err) {
                console.error("Error decoding token:", err);
            }
        } else {
            setLogin(false);
            setRole(null);
        }
    }, []);

    return (
        <AuthContext.Provider value={{ login, setLogin, role, setRole }}>
            {children}
        </AuthContext.Provider>
    );
}

// Hook personalizado para usar el contexto
export function useAuth() {
    const context = useContext(AuthContext);
    if (context === null) {
        throw new Error("useAuth debe usarse dentro de un AuthProvider");
    }
    return context;
}
