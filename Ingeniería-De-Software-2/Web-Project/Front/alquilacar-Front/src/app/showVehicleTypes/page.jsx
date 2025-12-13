"use client";

import { useEffect, useState } from "react";
import styles from "./showVehicleTypes.module.scss";
import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";
import Table from "@/components/table/Table";
import CreateBtn from "@/components/createBtn/CreateBtn";
import { poppins } from "@/lib/fonts/fonts";
import { useAuth } from "@/context/AuthContext";

export default function Page() {
    const [vehicleTypes, setvehicleTypes] = useState([]);
    const loading = useProtectedRoute(["Admin", "Employee"]);
    const { role } = useAuth();

    const token = localStorage.getItem("token");

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
                const categoryNames = {
                    0: "Suv",
                    1: "Apto para discapacitados",
                    2: "Chico",
                    3: "Van",
                    4: "Deportivo",
                    5: "Mediano",
                };
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
                        Categoría:
                            categoryNames[vehicleType.category] ||
                            "Categoría desconocida",
                        pricePerDay: vehicleType.pricePerDay,
                        cancellationPolicyId: cancellationPolicy
                            ? cancellationPolicy.description
                            : "Política de cancelación no encontrada",
                        imageUrl: vehicleType.imageUrl,
                        brandId: vehicleType.brandId,
                        category: vehicleType.category,
                    };
                });
                setvehicleTypes(vehicleTypesUpdated);
            } catch (error) {
                console.error("Error al obtener tipos de vehiculos:", error);
            }
        };

        fetchVehicleTypes();
    }, [brands]);
    if (loading) return null;
    return (
        <div className={styles.mainContainer}>
            <div className={styles.header}>
                <h1 className={`${styles.title} ${poppins.className} `}>
                    Lista de tipos de vehiculos
                </h1>
                {role === "Admin" && <CreateBtn href="/createVehicleType" />}
            </div>
            <Table data={vehicleTypes} editPath={"updateVehicleType/"} />
        </div>
    );
}
