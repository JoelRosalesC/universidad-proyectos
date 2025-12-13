"use client";
import { useActionState, useEffect, useState } from "react";
import { submitUpdateVehicleTypeForm } from "@/lib/submitActions/submitUpdateVehicleTypeForm";
import BaseForm from "@/components/forms/baseForm/BaseForm";
import SimpleInput from "@/components/inputs/SimpleInput";
import styles from "./updateVehicleTypeForm.module.scss";
import { toast } from "sonner";
import { useRouter } from "next/navigation";
import SelectInput from "@/components/inputs/SelectInput/SelectInput";

export default function UpdateVehicleTypeForm({ vehicleType }) {
    const customSubmitAction = async (prevState, formData) => {
        const token = localStorage.getItem("token");
        if (token) formData.append("token", token);
        return await submitUpdateVehicleTypeForm(prevState, formData);
    };

    const [state, submitAction, isPending] = useActionState(
        customSubmitAction,
        null
    );
    const router = useRouter();
    useEffect(() => {
        if (state?.success) {
            toast.success("tipo de vehiculo editado con éxito", {
                duration: 3000,
                closeButton: true,
            });
            setTimeout(() => {
                router.push("/showVehicleTypes");
            }, 1500);
        }
    }, [state]);
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
            <input type="hidden" name="id" value={vehicleType.id} />
            <SelectInput
                key={`brandid-${Date.now()}`}
                label={"Marca"}
                name={"BrandId"}
                options={brandOptions}
                defaultSelectedOption={
                    state?.inputs?.BrandId ?? vehicleType?.brandId
                }
                error={state?.error?.BrandId}
            />
            <SimpleInput
                label={"Modelo"}
                name={"Model"}
                type={"text"}
                defaultValue={state?.inputs?.Model ?? vehicleType?.model}
                error={state?.error?.Model}
            />
            <SimpleInput
                label={"Cantidad de pasajeros"}
                name={"PassengerCapacity"}
                type={"text"}
                defaultValue={
                    state?.inputs?.PassengerCapacity ??
                    vehicleType?.passengerCapacity
                }
                error={state?.error?.PassengerCapacity}
            />

            <SimpleInput
                label={"Precio por dia"}
                name={"PricePerDay"}
                type={"text"}
                defaultValue={
                    state?.inputs?.PricePerDay ?? vehicleType?.pricePerDay
                }
                error={state?.error?.PricePerDay}
            />
            <SelectInput
                key={`category-${Date.now()}`}
                label={"Categoria"}
                name={"Category"}
                defaultSelectedOption={(
                    state?.inputs?.Category ?? vehicleType?.category
                )?.toString()}
                options={[
                    { value: "0", name: "Suv" },
                    { value: "1", name: "Apto para discapacitados" },
                    { value: "2", name: "Chico" },
                    { value: "3", name: "Van" },
                    { value: "4", name: "Deportivo" },
                    { value: "5", name: "Mediano" },
                ]}
                error={state?.error?.Category}
            />
            <SelectInput
                key={`cancellationPolicyId-${Date.now()}`}
                label={"Politica de cancelación"}
                name={"CancellationPolicyId"}
                defaultSelectedOption={
                    state?.inputs?.CancellationPolicyId ??
                    vehicleType?.cancellationPolicyId
                }
                options={cancellationPoliciyOptions}
                error={state?.error?.CancellationPolicyId}
            />
            <p className={styles.currentImageLabel}>
                Vista previa de la imagen actual
            </p>

            <img
                src={`http://localhost:5296/images/${vehicleType.imageUrl}`}
                alt={vehicleType.model}
                onError={(e) => {
                    e.currentTarget.onerror = null;
                    e.currentTarget.src = "/img/missingCar.jpeg";
                }}
                style={{
                    width: "100px",
                    height: "100px",
                    objectFit: "cover",
                    borderRadius: "8px",
                }}
            />
            <SimpleInput
                label={"Imagen"}
                name={"image"}
                type={"file"}
                accept=".jpg,.jpeg,.png"
                error={state?.error?.image}
            />
        </BaseForm>
    );
}
