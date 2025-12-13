"use client";

import styles from "./cancelReservation.module.scss";
import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";
import Table from "@/components/table/Table";
import { poppins } from "@/lib/fonts/fonts";
import { useEffect, useState } from "react";
import { CancelReservation } from "@/lib/cancelReservation";
import { toast } from "sonner";

export default function Page() {
    const token = localStorage.getItem("token");
    const loading = useProtectedRoute(["Employee"]);
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

    useEffect(() => {
        const fetchReservations = async () => {
            try {
                const response = await fetch(
                    "http://localhost:5296/api/Statistics/GetDB",
                    {
                        method: "GET",
                        headers: {
                            "Content-Type": "application/json",
                            Authorization: `Bearer ${token}`, // Agregar el token en los headers
                        },
                    }
                );

                if (!response.ok) {
                    throw new Error("Error al obtener las reservas");
                }

                const data = await response.json();
                // console.log(data.data);

                const statusList = [
                    { value: 0, name: "En curso" },
                    { value: 1, name: "Completa" },
                    { value: 2, name: "Cancelada" },
                    { value: 3, name: "Completa" },
                    { value: 4, name: "Anulada" },
                ];
                const prioridadStatus = {
                    0: 0, // En curso
                    2: 1, // Cancelada
                    4: 2, // Anulada
                    1: 3, // Completa
                    3: 3, // Completa también
                };
                const formatedReservations = data.data.rentals
                    .filter((res) => {
                        return res.status == 0;
                    })
                    .map((reservation) => {
                        const statusObj = statusList.find(
                            (status) => status.value === reservation.status
                        );
                        const vehicleType = vehicleTypes.find(
                            (vehicleType) =>
                                vehicleType.id ===
                                reservation.selectedVehicleTypeId
                        );

                        const sucursal = branches.find(
                            (sucursal) =>
                                sucursal.id === reservation.pickedUpBranchId
                        );

                        return {
                            id: reservation.id,
                            registrationDate: reservation.registrationDate,
                            Sucursal:
                                sucursal?.name || "sucursal no encontrada",
                            rentalDate: reservation.rentalDate,
                            returnDate: reservation.returnDate,
                            totalPrice: reservation.totalPrice,
                            cancellationPolicy: vehicleType?.cancellationPolicy,
                            Vehiculo: vehicleType
                                ? `${vehicleType.Marca} - ${vehicleType.model} - capacidad ${vehicleType.passengerCapacity}  `
                                : "Tipo de vehículo no encontrado",
                            Estado: statusObj.name || "Desconocido",
                            order: reservation.status,
                        };
                    })
                    .sort(
                        (a, b) =>
                            prioridadStatus[a.order] - prioridadStatus[b.order]
                    );
                setReservations(formatedReservations);
            } catch (error) {
                console.error("Error al obtener las reservas:", error);
            }
        };

        fetchReservations();
    }, [vehicleTypes, branches]);

    const handleDelete = async (item, token) => {
        const policyMessages = {
            "Sin devolución": "No tendrá devolución del dinero.",
            "20% de devolución": "Se devolverá el 20% del precio total.",
            "100% de devolución": "Se devolverá el 100% del precio total.",
        };
        const message =
            policyMessages[item?.cancellationPolicy] ||
            `Política: ${item?.cancellationPolicy}`;
        const response = await CancelReservation(item.id, token);
        if (response.success) {
            toast.success(`Reserva cancelada exitosamente. ${message}`, {
                duration: 3000,
                closeButton: true,
            });
            setTimeout(() => {
                window.location.reload();
            }, 1500);
        } else {
            toast.error(
                response?.error?.generalError || "Error al cancelar la reserva",
                {
                    duration: 3000,
                    closeButton: true,
                }
            );
        }
    };

    if (loading) return null;
    return (
        <div className={styles.mainContainer}>
            <div className={styles.header}>
                <h1 className={`${styles.title} ${poppins.className} `}>
                    Reservas En curso
                </h1>
            </div>
            {reservations.length > 0 ? (
                <Table
                    data={reservations}
                    onDelete={handleDelete}
                    confirmationModalText="¿Estás seguro de cancelar la reserva?"
                />
            ) : (
                <p>No hay reservas en curso</p>
            )}
        </div>
    );
}
