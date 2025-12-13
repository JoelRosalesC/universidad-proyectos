"use client";
import { useActionState, useEffect, useMemo, useState } from "react";
import { submitCreateRentWithoutReservationForm } from "@/lib/submitActions/submitCreateRentWithoutReservationForm";
import BaseForm from "@/components/forms/baseForm/BaseForm";
import styles from "./createRentalWithoutReservationForm.module.scss";
import { toast } from "sonner";
import SelectInput from "@/components/inputs/SelectInput/SelectInput";
import MultipleSelectInput from "@/components/inputs/multipleSelectInput/MultipleselectInput";
import SimpleInput from "@/components/inputs/SimpleInput";

export default function CreateRentalWithoutReservationForm() {
    const token = localStorage.getItem("token");

    const customSubmitAction = async (prevState, formData) => {
        const token = localStorage.getItem("token");
        if (token) formData.append("token", token);
        return await submitCreateRentWithoutReservationForm(
            prevState,
            formData
        );
    };
    const [state, submitAction, isPending] = useActionState(
        customSubmitAction,
        null
    );
    const [endDate, setEndDate] = useState("");
    const tomorrow = new Date(Date.now() + 86400000)
        .toISOString()
        .split("T")[0];

    const [customers, setCustomers] = useState([]);
    // Hacer la solicitud a la API
    useEffect(() => {
        const fetchCustomers = async () => {
            try {
                const response = await fetch(
                    "http://localhost:5296/api/Customer",
                    {
                        method: "GET",
                        headers: {
                            "Content-Type": "application/json",
                            Authorization: `Bearer ${token}`, // Agregar el token en los headers
                        },
                    }
                );

                if (!response.ok) {
                    throw new Error("Error al obtener los clientes");
                }

                const data = await response.json();
                // console.log("clientes: ", data.data);

                const customersUpdated = data.data.map((customer) => {
                    return {
                        value: customer.id,
                        name: customer.mail,
                    };
                });
                setCustomers(customersUpdated);
            } catch (error) {
                console.error("Error al obtener los clientes:", error);
            }
        };

        fetchCustomers();
    }, []);

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
                // console.log(data.data);

                const vehicleTypesUpdated = data.data.map((vehicleType) => {
                    const brand = brands.find(
                        (brand) => brand.id === vehicleType.brandId
                    );
                    return {
                        id: vehicleType.id,
                        Marca: brand ? brand.name : "Marca no encontrada",
                        model: vehicleType.model,
                        passengerCapacity: vehicleType.passengerCapacity,
                        pricePerDay: vehicleType.pricePerDay,
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
                            ${vehicle.color} |
                            Precio por día: ${
                                vehicleType
                                    ? `${vehicleType.pricePerDay} $  `
                                    : "Precio no encontrado no encontrado"
                            }$
                            
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
            setTimeout(() => {
                window.location.reload();
            }, 1500);
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
                label={"Clientes"}
                name={"customerId"}
                required
                defaultSelectedOption={state?.inputs?.customerId}
                options={customers}
                error={state?.error?.customerId}
            />
            <SimpleInput
                className={styles.input}
                label={"Fecha de devolucíon"}
                name={"endDate"}
                type={"date"}
                required
                min={tomorrow}
                value={endDate}
                onChange={(e) => setEndDate(e.target.value)}
            />
            <SelectInput
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
