"use client";
import SelectInput from "@/components/inputs/SelectInput/SelectInput";
import SimpleInput from "@/components/inputs/SimpleInput";
import { useRouter } from "next/navigation";
import { useEffect, useRef, useState } from "react";
import styles from "./carsAvailableForm.module.scss";
import Check from "@/lib/svg/Check";
import { useAuth } from "@/context/AuthContext";

export default function CarsAvailableForm() {
    const router = useRouter();
    const formRef = useRef(null);
    const { login } = useAuth();
    const [startDate, setStartDate] = useState("");
    const [endDate, setEndDate] = useState("");

    const tomorrow = new Date(Date.now() + 86400000)
        .toISOString()
        .split("T")[0];
    const afterTomorrow = new Date(Date.now() + 2 * 86400000)
        .toISOString()
        .split("T")[0];
    const handleSubmit = (e) => {
        e.preventDefault();
        if (!login) {
            router.push("/login");
            return;
        }
        const formData = new FormData(formRef.current);
        const withdrawalBranch = formData.get("withdrawalBranch");
        const startDate = formData.get("startDate");
        const endDate = formData.get("endDate");

        const params = new URLSearchParams({
            withdrawalBranch,
            startDate,
            endDate,
        }).toString();
        router.push(`/carsAvailable?${params}`);
    };
    const [workBranchOptions, setWorkBranchOptions] = useState([]);
    // Hacer la solicitud a la API
    useEffect(() => {
        const fetchBranches = async () => {
            const token = localStorage.getItem("token");
            try {
                const response = await fetch(
                    "http://localhost:5296/api/Branch",
                    {
                        method: "GET",
                        headers: {
                            "Content-Type": "application/json",
                            Authorization: `Bearer ${token}`, // Agregar el token en los headers
                        },
                    }
                );

                if (!response.ok) {
                    throw new Error("Error al obtener sucursales");
                }

                const data = await response.json();
                const options = data.data.map((branch) => ({
                    value: branch.id,
                    name: branch.name,
                }));

                setWorkBranchOptions(options);
            } catch (error) {
                console.error("Error al obtener sucursales:", error);
            }
        };

        fetchBranches();
    }, []);
    const getDayBefore = (dateStr) => {
        if (!dateStr) return undefined;
        const date = new Date(dateStr);
        date.setDate(date.getDate() - 1);
        return date.toISOString().split("T")[0];
    };
    const getDayAfter = (dateStr) => {
        if (!dateStr) return afterTomorrow;
        const date = new Date(dateStr);
        date.setDate(date.getDate() + 1);
        return date.toISOString().split("T")[0];
    };
    return (
        <form className={styles.form} ref={formRef} onSubmit={handleSubmit}>
            <div className={styles.inputs}>
                <SelectInput
                    className={styles.input}
                    label={"Sucursal de retiro"}
                    name={"withdrawalBranch"}
                    required
                    options={workBranchOptions}
                />
                <SimpleInput
                    className={styles.input}
                    label={"Fecha de inicio"}
                    name={"startDate"}
                    type={"date"}
                    required
                    min={tomorrow}
                    max={getDayBefore(endDate)}
                    value={startDate}
                    onChange={(e) => setStartDate(e.target.value)}
                />
                <SimpleInput
                    className={styles.input}
                    label={"Fecha de devolucíon"}
                    name={"endDate"}
                    type={"date"}
                    required
                    min={getDayAfter(startDate)}
                    value={endDate}
                    onChange={(e) => setEndDate(e.target.value)}
                />
            </div>

            <button type="submit">Buscar autos disponibles</button>
            <div className={styles.characteristicsContainer}>
                <div className={styles.characteristic}>
                    <Check />
                    <p className={styles.title}>Sin costos ocultos</p>
                    <p className={styles.subtitle}>
                        Precio final garantizado, 100% transparente
                    </p>
                </div>
                <div className={styles.characteristic}>
                    <Check />
                    <p className={styles.title}>Reserva rápida y segura</p>
                    <p className={styles.subtitle}>
                        Alquilá en minutos y sin complicaciones
                    </p>
                </div>
                <div className={styles.characteristic}>
                    <Check />
                    <p className={styles.title}>Soporte 24/7</p>
                    <p className={styles.subtitle}>
                        Asistencia completa y sin costo adicional
                    </p>
                </div>
            </div>
        </form>
    );
}
