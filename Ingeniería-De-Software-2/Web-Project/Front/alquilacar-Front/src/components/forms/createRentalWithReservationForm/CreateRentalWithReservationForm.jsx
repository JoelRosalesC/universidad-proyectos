"use client";
import { useActionState, useEffect, useMemo, useState } from "react";
import { submitCreateRentForm } from "@/lib/submitActions/submitCreateRentForm";
import BaseForm from "@/components/forms/baseForm/BaseForm";
import styles from "./createRentalWithReservationForm.module.scss";
import { toast } from "sonner";
import SelectInput from "@/components/inputs/SelectInput/SelectInput";
import MultipleSelectInput from "@/components/inputs/multipleSelectInput/MultipleselectInput";

export default function CreateRentalWithReservationForm({ reservations }) {
    const token = localStorage.getItem("token");

    const customSubmitAction = async (prevState, formData) => {
        const token = localStorage.getItem("token");
        if (token) formData.append("token", token);
        return await submitCreateRentForm(prevState, formData);
    };
    const [state, submitAction, isPending] = useActionState(
        customSubmitAction,
        null
    );

    const [brands, setBrands] = useState([]);
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

                setBrands(data.data);
            } catch (error) {
                console.error("Error al obtener marcas:", error);
            }
        };

        fetchBrands();
    }, []);
    // Hacer la solicitud a la API
    const [vehicleTypes, setvehicleTypes] = useState([]);

    useEffect(() => {
        const fetchVehicleTypes = async () => {
            try {
                const response = await fetch(
                    "http://localhost:5296/api/vehicleType",
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

                const vehicleTypesUpdated = data.data.map((vehicleType) => {
                    const brand = brands.find(
                        (brand) => brand.id === vehicleType.brandId
                    );
                    return {
                        id: vehicleType.id,
                        Marca: brand ? brand.name : "Marca no encontrada",
                        model: vehicleType.model,
                        passengerCapacity: vehicleType.passengerCapacity,
                        //lo otro no hacia falta
                    };
                });
                setvehicleTypes(vehicleTypesUpdated);
            } catch (error) {
                console.error("Error al obtener tipos de vehiculos:", error);
            }
        };

        fetchVehicleTypes();
    }, [brands]);
    // Hacer la solicitud a la API
    const [availableVehicles, setAvailableVehicles] = useState([]);

    useEffect(() => {
        const fetchAvailableVehicles = async () => {
            try {
                const response = await fetch(
                    "http://localhost:5296/api/Vehicle/AllAvailablesByBranch",
                    {
                        method: "GET",
                        headers: {
                            "Content-Type": "application/json",
                            Authorization: `Bearer ${token}`,
                        },
                    }
                );

                if (!response.ok) {
                    throw new Error("Error al obtener los vehiculos");
                }

                const data = await response.json();

                const vehicleUpdated = data.data
                    .filter((vehicle) => !vehicle.isUnderMaintenance)
                    .map((vehicle) => {
                        const vehicleType = vehicleTypes.find(
                            (vehicleType) =>
                                vehicleType.id === vehicle.vehicleTypeId
                        );
                        return {
                            value: vehicle.id,
                            name: `
                            ${vehicle.licensePlate} |
                            ${
                                vehicleType
                                    ? `${vehicleType.Marca} - ${vehicleType.model} - capacidad ${vehicleType.passengerCapacity}  `
                                    : "Tipo de vehículo no encontrado"
                            } |
                            ${vehicle.year} |
                            ${vehicle.color}
                            
                            `,
                        };
                    });

                setAvailableVehicles(vehicleUpdated);
            } catch (error) {
                console.error("Error al obtener vehiculos:", error);
            }
        };

        fetchAvailableVehicles();
    }, [vehicleTypes]);

    useEffect(() => {
        if (state?.success) {
            toast.success("Vehiculo alquilado con éxito", {
                duration: 3000,
                closeButton: true,
            });
        }
    }, [state]);

    const adicionales = [
        { value: 0, name: "Segundo conductor" },
        { value: 1, name: "Tanque lleno" },
        { value: 2, name: "Silla para bebe" },
        { value: 3, name: "Cadenas para nieve" },
    ];
    /* 🔄 Convierte los IDs devueltos por el server en objetos {name,value}.
      useMemo evita recalcular en cada render. */
    const defaultAdditionals = useMemo(() => {
        if (state?.success) return []; // 🔁 limpiar si se envió con éxito
        const ids = state?.inputs?.additionals ?? []; // ← puede venir undefined
        return ids
            .map((id) =>
                adicionales.find(
                    (a) => a.value === id || a.value === Number(id) // por si viene string
                )
            )
            .filter(Boolean); // saca posibles undefined
    }, [state?.inputs?.additionals, state?.success]);

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
                key={`rentalId-${Date.now()}`}
                label={"Reservaciones"}
                name={"rentalId"}
                required
                defaultSelectedOption={state?.inputs?.rentalId}
                options={reservations}
                error={state?.error?.rentalId}
            />
            <SelectInput
                key={`deliveredVehicleId-${Date.now()}`}
                label={"Vehiculos disponibles"}
                name={"deliveredVehicleId"}
                required
                defaultSelectedOption={state?.inputs?.deliveredVehicleId}
                options={availableVehicles}
                error={state?.error?.deliveredVehicleId}
            />
            <MultipleSelectInput
                label={"Adicionales"}
                name={"additionals"}
                defaultSelectedOptions={defaultAdditionals}
                options={adicionales}
                error={state?.error?.additionals}
            />
        </BaseForm>
    );
}
