"use client";
import { useActionState, useEffect, useState } from "react";
import { submitUpdateEmployeeForm } from "@/lib/submitActions/submitUpdateEmployeeForm";
import BaseForm from "@/components/forms/baseForm/BaseForm";
import SimpleInput from "@/components/inputs/SimpleInput";
import styles from "./updateEmployeeForm.module.scss";
import { toast } from "sonner";
import SelectInput from "@/components/inputs/SelectInput/SelectInput";
import { useRouter } from "next/navigation";

export default function UpdateEmployeeForm({ employee }) {
    const customSubmitAction = async (prevState, formData) => {
        const token = localStorage.getItem("token");
        if (token) formData.append("token", token);
        return await submitUpdateEmployeeForm(prevState, formData);
    };
    const [state, submitAction, isPending] = useActionState(
        customSubmitAction,
        null
    );
    const router = useRouter();

    const [currentBranchOptions, setcurrentBranchOptions] = useState([]);
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

                setcurrentBranchOptions(options);
            } catch (error) {
                console.error("Error al obtener sucursales:", error);
            }
        };

        fetchBranches();
    }, []);

    useEffect(() => {
        if (state?.success) {
            toast.success("Empleado editado con éxito", {
                duration: 3000,
                closeButton: true,
            });
            setTimeout(() => {
                router.push("/showEmployees");
            }, 1500);
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
        >
            <input type="hidden" name="id" value={employee.id} />

            <SimpleInput
                label={"Email"}
                name={"email"}
                type={"email"}
                required
                error={state?.error?.Mail}
                defaultValue={state?.inputs?.mail ?? employee?.mail}
            />
            <SimpleInput
                label={"Nombre"}
                name={"firstName"}
                type={"text"}
                required
                defaultValue={state?.inputs?.firstName ?? employee?.firstName}
                error={state?.error?.FirstName}
            />
            <SimpleInput
                label={"Apellido"}
                name={"lastName"}
                type={"text"}
                required
                defaultValue={state?.inputs?.lastName ?? employee?.lastName}
                error={state?.error?.LastName}
            />
            <SimpleInput
                label={"Dni"}
                name={"dni"}
                type={"text"}
                required
                defaultValue={state?.inputs?.dni ?? employee?.dni}
                error={state?.error?.Dni}
            />
            <SimpleInput
                label={"Fecha de nacimiento"}
                name={"birthdate"}
                type={"date"}
                required
                defaultValue={state?.inputs?.birthdate ?? employee?.birthdate}
                error={state?.error?.Birthdate}
            />
            <SimpleInput
                label={"Teléfono"}
                name={"phoneNumber"}
                type={"text"}
                required
                defaultValue={
                    state?.inputs?.phoneNumber ?? employee?.phoneNumber
                }
                error={state?.error?.PhoneNumber}
            />
            {/* pongo una key siempre distinta asi se hace un rerender
                         porque sino el defaultSelectedOption={state?.inputs?.workBranch} en reenvios 
                         del form donde no cambias nada se pierde el valor del input */}
            <SelectInput
                key={`workBranch-${Date.now()}`}
                label={"Sucursal"}
                name={"workBranch"}
                required
                defaultSelectedOption={
                    state?.inputs?.workBranch ?? employee?.workBranch
                }
                options={currentBranchOptions}
                error={state?.error?.workBranch}
            />
        </BaseForm>
    );
}
