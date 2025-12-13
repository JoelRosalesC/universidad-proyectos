"use client";

import styles from "./myRentals.module.scss";
import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";
import Table from "@/components/table/Table";
import { poppins } from "@/lib/fonts/fonts";
import { useEffect, useState } from "react";
import { CancelReservation } from "@/lib/cancelReservation";
import { toast } from "sonner";

export default function Page() {
    const token = localStorage.getItem("token");
    const loading = useProtectedRoute();
    const [reservations, setReservations] = useState([]);

    const [brands, setBrands] = useState([]);
    // Hacer la solicitud a la API
    useEffect(() => {
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

    const [cancellationPolicies, setCancellationPolicies] = useState([]);
    useEffect(() => {
        const fetchcancellationPolicies = async () => {
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
                setCancellationPolicies(data.data);
            } catch (error) {
                console.error(
                    "Error al obtener politicas de cancelacion:",
                    error
                );
            }
        };

        fetchcancellationPolicies();
    }, []);

    // Hacer la solicitud a la API
    const [vehicleTypes, setVehicleTypes] = useState([]);

    useEffect(() => {
        if (brands.length === 0 || cancellationPolicies.length === 0) return;
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
                    const cancellationPolicy = cancellationPolicies.find(
                        (policy) =>
                            policy.id === vehicleType.cancellationPolicyId
                    );
                    return {
                        id: vehicleType.id,
                        Marca: brand ? brand.name : "Marca no encontrada",
                        model: vehicleType.model,
                        passengerCapacity: vehicleType.passengerCapacity,
                        cancellationPolicy: cancellationPolicy
                            ? cancellationPolicy.description
                            : "Política de cancelación no encontrada",
                    };
                });
                setVehicleTypes(vehicleTypesUpdated);
            } catch (error) {
                console.error("Error al obtener tipos de vehiculos:", error);
            }
        };

        fetchVehicleTypes();
    }, [brands, cancellationPolicies]);

    const [branches, setBranches] = useState([]);
    // Hacer la solicitud a la API
    useEffect(() => {
        const fetchBranches = async () => {
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

                setBranches(data.data);
            } catch (error) {
                console.error("Error al obtener sucursales:", error);
            }
        };

        fetchBranches();
    }, []);

    const [vehicles, setVehicles] = useState([]);

    useEffect(() => {
        const fetchVehicles = async () => {
            try {
                const response = await fetch(
                    "http://localhost:5296/api/Vehicle/AllAvailables",
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
                // console.log(data.data);

                setVehicles(data.data);
            } catch (error) {
                console.error("Error al obtener vehiculos:", error);
            }
        };

        fetchVehicles();
    }, []);

    useEffect(() => {
        const fetchAllReservations = async () => {
            try {
                const response = await fetch(
                    "http://localhost:5296/api/Rental",
                    {
                        method: "GET",
                        headers: {
                            "Content-Type": "application/json",
                            Authorization: `Bearer ${token}`, // Agregar el token en los headers
                        },
                    }
                );

                if (!response.ok) {
                    throw new Error("Error al obtener todas las reservas");
                }

                const data = await response.json();
                // console.log(data.data);

                const statusList = [
                    { value: 0, name: "En curso" },
                    { value: 1, name: "En alquiler" },
                    { value: 2, name: "Cancelada" },
                    { value: 3, name: "Devuelto" },
                ];

                const formatedReservations = data.data
                    .filter((res) => {
                        return res.status == 1 || res.status == 3;
                    })
                    .map((reservation) => {
                        const statusObj = statusList.find(
                            (status) => status.value === reservation.status
                        );
                        const vehicle = vehicles.find(
                            (vehicle) =>
                                vehicle.id === reservation.deliveredVehicleId
                        );
                        const vehicleType = vehicleTypes.find(
                            (vehicleType) =>
                                vehicleType.id === vehicle?.vehicleTypeId
                        );

                        const sucursal = branches.find(
                            (sucursal) =>
                                sucursal.id === reservation.pickedUpBranchId
                        );
                        const returnedBranch = branches.find(
                            (sucursal) =>
                                sucursal.id === reservation.returnedBranchId
                        );

                        return {
                            id: reservation.id,

                            pickedUpBranch:
                                sucursal?.name || "sucursal no encontrada",
                            rentalDate: reservation.rentalDate,
                            returnDate: reservation.returnDate,
                            returnedBranch: returnedBranch
                                ? returnedBranch.name
                                : "Sin devolver",
                            totalPrice: reservation.totalPrice,
                            Patente: vehicle
                                ? vehicle.licensePlate
                                : "patente no encontrada",
                            Vehiculo: vehicleType
                                ? `${vehicleType.Marca} - ${vehicleType.model} - capacidad ${vehicleType.passengerCapacity}  `
                                : "Tipo de vehículo no encontrado",
                            Estado: statusObj.name || "Desconocido",
                        };
                    })
                    .sort((a, b) => {
                        // Queremos que los de estado 1 vayan antes que los de estado 3
                        if (
                            a.Estado === "En alquiler" &&
                            b.Estado === "Devuelto"
                        )
                            return -1;
                        if (
                            a.Estado === "Devuelto" &&
                            b.Estado === "En alquiler"
                        )
                            return 1;
                        return 0; // si son iguales o no son esos estados
                    });

                setReservations(formatedReservations);
            } catch (error) {
                console.error("Error al obtener todas las reservas:", error);
            }
        };

        fetchAllReservations();
    }, [vehicleTypes, branches, vehicles]);

    if (loading) return null;
    return (
        <div className={styles.mainContainer}>
            <div className={styles.header}>
                <h1 className={`${styles.title} ${poppins.className} `}>
                    Mis Alquileres
                </h1>
            </div>
            {reservations.length > 0 ? (
                <Table data={reservations} />
            ) : (
                <p>No tiene alquileres</p>
            )}
        </div>
    );
}
