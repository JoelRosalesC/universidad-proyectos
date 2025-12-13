"use client";
import SelectInput from "@/components/inputs/SelectInput/SelectInput";
import SimpleInput from "@/components/inputs/SimpleInput";
import { useRouter } from "next/navigation";
import { useActionState, useEffect, useRef, useState } from "react";
import styles from "./rentalStatsForm.module.scss";
import BaseForm from "@/components/forms/baseForm/BaseForm";
import Check from "@/lib/svg/Check";
import { useAuth } from "@/context/AuthContext";
import { submitRentalStatsForm } from "@/lib/submitActions/submitRentalStatsForm";

export default function RentalStatsForm({
    setDataPerDay,
    setRentals,
    setHasSearched,
}) {
    const formRef = useRef(null);
    const [startDate, setStartDate] = useState("");
    const [endDate, setEndDate] = useState("");

    const afterTomorrow = new Date(Date.now() + 2 * 86400000)
        .toISOString()
        .split("T")[0];

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
    const customSubmitAction = async (prevState, formData) => {
        const token = localStorage.getItem("token");
        if (token) formData.append("token", token);
        return await submitRentalStatsForm(prevState, formData);
    };
    const [state, submitAction, isPending] = useActionState(
        customSubmitAction,
        null
    );
    useEffect(() => {
        if (state !== null) {
            // solo después de que el form se procesó
            setHasSearched(true); // marca que ya hubo una búsqueda

            if (state.success && state.data) {
                setDataPerDay(state.data.registrationsPerDay);
                setRentals(state.data.countedRentals);
            } else {
                // búsqueda sin resultados o con error
                setDataPerDay([]);
                setRentals([]);
            }
        }
    }, [state, setDataPerDay, setRentals, setHasSearched]);

    return (
        <BaseForm
            className={styles.formContainer}
            submitAction={submitAction}
            state={state}
            isPending={isPending}
            generalError={
                !state?.success && state?.error?.generalError
                    ? state.error.generalError
                    : ""
            }
        >
            <div className={styles.inputs}>
                <SimpleInput
                    className={styles.input}
                    label={"Fecha de inicio"}
                    name={"startDate"}
                    type={"date"}
                    required
                    max={getDayBefore(endDate)}
                    value={startDate}
                    onChange={(e) => setStartDate(e.target.value)}
                />
                <SimpleInput
                    className={styles.input}
                    label={"Fecha de fin"}
                    name={"endDate"}
                    type={"date"}
                    required
                    min={getDayAfter(startDate)}
                    value={endDate}
                    onChange={(e) => setEndDate(e.target.value)}
                />
            </div>
        </BaseForm>
    );
}
