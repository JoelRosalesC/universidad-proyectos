"use client";
import { useActionState, useEffect, useState } from "react";
import { submitCreateVehicleTypeForm } from "@/lib/submitActions/submitCreateVehicleTypeForm";
import BaseForm from "@/components/forms/baseForm/BaseForm";
import SimpleInput from "@/components/inputs/SimpleInput";
import styles from "./createVehicleTypeForm.module.scss";
import { toast } from "sonner";
import SelectInput from "@/components/inputs/SelectInput/SelectInput";

export default function CreateVehicletypeForm() {
    const customSubmitAction = async (prevState, formData) => {
        const token = localStorage.getItem("token");
        if (token) formData.append("token", token);
        return await submitCreateVehicleTypeForm(prevState, formData);
    };
    const [state, submitAction, isPending] = useActionState(
        customSubmitAction,
        null
    );

    const [brandOptions, setBrandOptions] = useState([]);
    // Hacer la solicitud a la API
    useEffect(() => {
        const token = localStorage.getItem("token");
        const fetchBrands = async () => {
            try {
                const response = await fetch(
                    "http://localhost:5296/api/Brand",
                    {
                        method: "GET",
                        headers: {
                            "Content-Type": "application/json",
                            Authorization: `Bearer ${token}`,
                        },
                    }
                );

                if (!response.ok) {
                    throw new Error("Error al obtener marcas");
                }

                const data = await response.json();

                const options = data.data.map((brand) => ({
                    value: brand.id,
                    name: brand.name,
                }));

                setBrandOptions(options);
            } catch (error) {
                console.error("Error al obtener marcas:", error);
            }
        };

        fetchBrands();
    }, []);

    const [cancellationPoliciyOptions, setCancellationPoliciyOptions] =
        useState([]);

    // Hacer la solicitud a la API
    useEffect(() => {
        const fetchCancellationPolicies = async () => {
            const token = localStorage.getItem("token");
            try {
                const response = await fetch(
                    "http://localhost:5296/api/CancellationPolicy",
                    {
                        method: "GET",
                        headers: {
                            "Content-Type": "application/json",
                            Authorization: `Bearer ${token}`, // Agregar el token en los headers
                        },
                    }
                );

                if (!response.ok) {
                    throw new Error("Error al obtener empleados");
                }

                const data = await response.json();
                const options = data.data.map((cancellationPolicy) => ({
                    value: cancellationPolicy.id,
                    name: cancellationPolicy.description,
                }));
                setCancellationPoliciyOptions(options);
            } catch (error) {
                console.error(
                    "Error al obtener politicas de cancelacion:",
                    error
                );
            }
        };

        fetchCancellationPolicies();
    }, []);

    useEffect(() => {
        if (state?.success) {
            toast.success("Tipo de vehculo creado con éxito", {
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
        >
            <SelectInput
                key={`brandid-${Date.now()}`}
                label={"Marca"}
                name={"BrandId"}
                required
                options={brandOptions}
                defaultSelectedOption={state?.inputs?.BrandId}
                error={state?.error?.BrandId}
            />
            <SimpleInput
                label={"Modelo"}
                name={"Model"}
                type={"text"}
                required
                defaultValue={state?.inputs?.Model}
                error={state?.error?.Model}
            />
            <SimpleInput
                label={"Cantidad de pasajeros"}
                name={"PassengerCapacity"}
                type={"text"}
                required
                defaultValue={state?.inputs?.PassengerCapacity}
                error={state?.error?.PassengerCapacity}
            />

            <SimpleInput
                label={"Precio por dia"}
                name={"PricePerDay"}
                type={"text"}
                required
                defaultValue={state?.inputs?.PricePerDay}
                error={state?.error?.PricePerDay}
            />
            <SelectInput
                key={`category-${Date.now()}`}
                label={"Categoria"}
                name={"Category"}
                required
                defaultSelectedOption={state?.inputs?.Category}
                options={[
                    { value: 0, name: "Suv" },
                    { value: 1, name: "Apto para discapacitados" },
                    { value: 2, name: "Chico" },
                    { value: 3, name: "Van" },
                    { value: 4, name: "Deportivo" },
                    { value: 5, name: "Mediano" },
                ]}
                error={state?.error?.Category}
            />
            <SelectInput
                key={`cancellationPolicyId-${Date.now()}`}
                label={"Politica de cancelación"}
                name={"CancellationPolicyId"}
                required
                defaultSelectedOption={state?.inputs?.CancellationPolicyId}
                options={cancellationPoliciyOptions}
                error={state?.error?.CancellationPolicyId}
            />
            <SimpleInput
                label={"Imagen"}
                name={"image"}
                type={"file"}
                accept=".jpg,.jpeg,.png"
                required
                error={state?.error?.image}
            />
        </BaseForm>
    );
}
