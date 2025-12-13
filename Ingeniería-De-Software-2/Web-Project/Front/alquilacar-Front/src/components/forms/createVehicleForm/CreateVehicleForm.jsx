"use client";
import { useActionState, useEffect, useState } from "react";
import { submitCreateVehicleForm } from "@/lib/submitActions/submitCreateVehicleForm";
import BaseForm from "@/components/forms/baseForm/BaseForm";
import SimpleInput from "@/components/inputs/SimpleInput";
import styles from "./createVehicleForm.module.scss";
import { toast } from "sonner";
import SelectInput from "@/components/inputs/SelectInput/SelectInput";

export default function CreateVehicleForm() {
    const customSubmitAction = async (prevState, formData) => {
        const token = localStorage.getItem("token");
        if (token) formData.append("token", token);
        return await submitCreateVehicleForm(prevState, formData);
    };
    const [state, submitAction, isPending] = useActionState(
        customSubmitAction,
        null
    );

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

    const [brands, setBrand] = useState([]);
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

                setBrand(data.data);
            } catch (error) {
                console.error("Error al obtener marcas:", error);
            }
        };

        fetchBrands();
    }, []);

    const [vehicleTypeOptions, setVehicleTypeOptions] = useState([]);
    // Hacer la solicitud a la API
    useEffect(() => {
        const fetchVehicleTypes = async () => {
            const token = localStorage.getItem("token");
            try {
                const response = await fetch(
                    "http://localhost:5296/api/VehicleType",
                    {
                        method: "GET",
                        headers: {
                            "Content-Type": "application/json",
                            Authorization: `Bearer ${token}`,
                        },
                    }
                );

                if (!response.ok) {
                    throw new Error("Error al obtener los tipos de vehiculos");
                }

                const data = await response.json();

                const options = data.data.map((vehicleType) => {
                    const brand = brands.find(
                        (b) => b.id === vehicleType.brandId
                    );
                    const brandName = brand ? brand.name : "Marca desconocida";
                    return {
                        value: vehicleType.id,
                        name: `${brandName} - ${vehicleType.model} - ${vehicleType.passengerCapacity} pasajeros`,
                    };
                });

                setVehicleTypeOptions(options);
            } catch (error) {
                console.error(
                    "Error al obtener los tipos de vehiculos:",
                    error
                );
            }
        };

        if (brands.length > 0) {
            fetchVehicleTypes();
        }
    }, [brands]);

    useEffect(() => {
        if (state?.success) {
            toast.success("Vehiculo creado con éxito", {
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
            <SimpleInput
                label={"patente"}
                name={"licensePlate"}
                type={"text"}
                required
                defaultValue={state?.inputs?.licensePlate}
                error={state?.error?.LicensePlate}
            />
            <SimpleInput
                label={"color"}
                name={"color"}
                type={"text"}
                required
                defaultValue={state?.inputs?.color}
                error={state?.error?.Color}
            />
            <SimpleInput
                label={"Año"}
                name={"Year"}
                type={"text"}
                required
                defaultValue={state?.inputs?.Year}
                error={state?.error?.Year}
            />
            {/* pongo una key siempre distinta asi se hace un rerender porque sino el defaultSelectedOption={state?.inputs?.workBranch} en reenvios del form donde no cambias nada se pierde el valor del input */}
            <SelectInput
                key={`branch-${Date.now()}`}
                label={"Sucursal"}
                name={"currentBranchId"}
                required
                defaultSelectedOption={state?.inputs?.currentBranchId}
                options={currentBranchOptions}
                error={state?.error?.CurrentBranchId}
            />
            <SelectInput
                key={`vehicleTypeId-${Date.now()}`}
                label={"Tipo de vehiculo"}
                name={"vehicleTypeId"}
                required
                defaultSelectedOption={state?.inputs?.vehicleTypeId}
                options={vehicleTypeOptions}
                error={state?.error?.VehicleTypeId}
            />
        </BaseForm>
    );
}
