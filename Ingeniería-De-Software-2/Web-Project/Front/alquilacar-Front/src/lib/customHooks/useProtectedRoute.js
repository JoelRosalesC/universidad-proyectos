import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";

export function useProtectedRoute(requiredRoles = []) {
    const router = useRouter();
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const token = localStorage.getItem("token");

        if (!token) {
            router.push("/login");
            return;
        }

        try {
            const payload = JSON.parse(atob(token.split(".")[1]));
            const isExpired = payload.exp && Date.now() / 1000 > payload.exp;

            const hasRequiredRole =
                requiredRoles.length === 0 ||
                requiredRoles.includes(payload.role);

            if (isExpired || !hasRequiredRole) {
                localStorage.removeItem("token");
                router.push("/login");
            } else {
                setLoading(false);
            }
        } catch (err) {
            console.error("Token inválido:", err);
            localStorage.removeItem("token");
            router.push("/login");
        }
    }, [requiredRoles, router]);

    return loading;
}
