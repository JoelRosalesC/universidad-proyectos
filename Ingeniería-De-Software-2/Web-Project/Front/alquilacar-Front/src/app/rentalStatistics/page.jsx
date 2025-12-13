"use client";

import styles from "./rentalStatistics.module.scss";
import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";
import Table from "@/components/table/Table";
import { poppins } from "@/lib/fonts/fonts";
import { useEffect, useState } from "react";
import { CancelReservation } from "@/lib/cancelReservation";
import { toast } from "sonner";
import RentalStatsForm from "@/components/forms/rentalStatsForm/RentalStatsForm";
import {
    BarChart,
    Bar,
    XAxis,
    YAxis,
    Tooltip,
    CartesianGrid,
    ResponsiveContainer,
} from "recharts";

export default function Page() {
    const token = localStorage.getItem("token");
    const loading = useProtectedRoute(["Admin"]);
    const [rentals, setRentals] = useState([]);
    const [formatedRentals, setFormatedRentals] = useState([]);
    const [dataPerDay, setDataPerDay] = useState([]);
    const [hasSearched, setHasSearched] = useState(false);
    const [detailsOpen, setDetailsOpen] = useState(false);
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

                setCustomers(data.data);
            } catch (error) {
                console.error("Error al obtener los clientes:", error);
            }
        };

        fetchCustomers();
    }, []);

    useEffect(() => {
        if (rentals) {
            const statusList = [
                { value: 0, name: "En curso" },
                { value: 1, name: "En alquiler" },
                { value: 2, name: "Cancelada" },
                { value: 3, name: "Devuelto" },
            ];
            const formatedRentals = rentals.map((reservation) => {
                const statusObj = statusList.find(
                    (status) => status.value === reservation.status
                );
                const vehicle = vehicles.find(
                    (vehicle) => vehicle.id === reservation.deliveredVehicleId
                );
                const vehicleType = vehicleTypes.find(
                    (vehicleType) => vehicleType.id === vehicle?.vehicleTypeId
                );

                const sucursal = branches.find(
                    (sucursal) => sucursal.id === reservation.pickedUpBranchId
                );
                const returnedBranch = branches.find(
                    (sucursal) => sucursal.id === reservation.returnedBranchId
                );
                const customer = customers.find(
                    (customer) => customer.id === reservation.customerId
                );
                return {
                    id: reservation.id,
                    Cliente: customer ? customer.mail : "cliente no encontrado",
                    pickedUpBranch: sucursal?.name || "sucursal no encontrada",
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
            });
            setFormatedRentals(formatedRentals);
        }
    }, [rentals, vehicleTypes, branches, vehicles, customers]);

    if (loading) return null;
    return (
        <div className={styles.mainContainer}>
            <div className={styles.header}>
                <h1 className={`${styles.title} ${poppins.className} `}>
                    Buscar Alquileres
                </h1>
            </div>
            <RentalStatsForm
                setDataPerDay={setDataPerDay}
                setRentals={setRentals}
                setHasSearched={setHasSearched}
            />
            {hasSearched ? (
                dataPerDay.length > 0 ? (
                    <div className={styles.dataContainer}>
                        <ResponsiveContainer width="100%" height={300}>
                            <BarChart data={dataPerDay}>
                                <CartesianGrid strokeDasharray="3 3" />
                                <XAxis dataKey="day" />
                                <YAxis
                                    allowDecimals={false}
                                    label={{
                                        value: "Alquileres", // ← texto del eje Y
                                        angle: -90, // ← rotado vertical
                                        position: "insideLeft", // ← posición dentro del eje
                                        offset: 10, // ← espacio desde el eje
                                        style: {
                                            textAnchor: "middle",
                                            fill: "#666",
                                        }, // opcional: estilo
                                    }}
                                />
                                <Tooltip />
                                <Bar
                                    dataKey="count"
                                    fill="#8884d8"
                                    name="Alquileres"
                                />
                            </BarChart>
                        </ResponsiveContainer>
                        <button
                            className={styles.button}
                            onClick={() => setDetailsOpen(!detailsOpen)}
                        >
                            {detailsOpen ? "Ocultar detalles" : "Ver detalles"}
                        </button>
                        {detailsOpen && <Table data={formatedRentals} />}
                    </div>
                ) : (
                    <p>No se encontraron alquileres en el rango de fechas</p>
                )
            ) : null}
        </div>
    );
}
