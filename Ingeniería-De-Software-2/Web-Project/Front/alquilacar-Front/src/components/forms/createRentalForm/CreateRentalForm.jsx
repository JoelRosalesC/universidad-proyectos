"use client";
import { useActionState, useEffect, useState } from "react";
import { submitCreateRentalForm } from "@/lib/submitActions/submitCreateRentalForm";
import BaseForm from "@/components/forms/baseForm/BaseForm";
import SimpleInput from "@/components/inputs/SimpleInput";
import styles from "./createRentalForm.module.scss";
import { toast } from "sonner";
import CheckboxGroup from "@/components/inputs/checkboxGroup/CheckboxGroup";
import { useRouter } from "next/navigation";

export default function CreateRentalForm({
    vehicleTypeId,
    withdrawalBranch,
    startDate,
    endDate,
}) {
    const router = useRouter();

    const customSubmitAction = async (prevState, formData) => {
        const token = localStorage.getItem("token");
        if (token) formData.append("token", token);
        if (withdrawalBranch && startDate && endDate && vehicleTypeId) {
            formData.append("branchId", withdrawalBranch);
            formData.append("startDate", startDate);
            formData.append("endDate", endDate);
            formData.append("vehicleTypeId", vehicleTypeId);
        }
        return await submitCreateRentalForm(prevState, formData);
    };
    const [state, submitAction, isPending] = useActionState(
        customSubmitAction,
        null
    );

    useEffect(() => {
        if (state?.success) {
            toast.success("Reserva creada con éxito", {
                duration: 3000,
                closeButton: true,
            });
            setTimeout(() => {
                router.push("/reservationHistory");
            }, 1500);
        }
    }, [state]);

    useEffect(() => {
        if (state?.error?.emptyCard) {
            toast.error("Pago rechazado por falta de fondos.", {
                duration: 3000,
                closeButton: true,
            });
        }
    }, [state]);

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
            submitLabel="Pagar"
        >
            <SimpleInput
                label={"Número de tarjeta"}
                name={"cardNumber"}
                required
                error={state?.error?.CardNumber}
                defaultValue={state?.inputs?.cardNumber}
            />
            <SimpleInput
                label={"Fecha de vencimiento"}
                name={"expirationDate"}
                required
                error={state?.error?.ExpirationDate}
                defaultValue={state?.inputs?.expirationDate}
            />
            <SimpleInput
                label={"Código de seguridad"}
                name={"cvvCode"}
                required
                defaultValue={state?.inputs?.cvvCode}
                error={state?.error?.CvvCode}
            />
            <CheckboxGroup
                name="cancellationPolicy"
                label={"He leído y acepto la política de cancelación"}
                options={[
                    {
                        value: "Acepto",
                        label: "Acepto",
                    },
                ]}
                required
                defaultValues={state?.inputs?.cancellationPolicy || []}
                error={state?.error?.cancellationPolicy}
            />
        </BaseForm>
    );
}
