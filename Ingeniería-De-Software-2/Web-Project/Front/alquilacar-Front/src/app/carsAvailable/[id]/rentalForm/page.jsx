"use client";
import styles from "./rentalForm.module.scss";
import { poppins } from "@/lib/fonts/fonts";
import { useEffect, useState } from "react";
import { useRouter, useSearchParams } from "next/navigation";
import { useParams } from "next/navigation";
import Star from "@/lib/svg/Star";
import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";
import CreateRentalForm from "@/components/forms/createRentalForm/CreateRentalForm";

export default function rentalForm() {
    const router = useRouter();
    const params = useParams();
    const vehicleTypeId = params.id;
    const searchParams = useSearchParams();
    const sucursalID = searchParams.get("withdrawalBranch");
    const inicio = searchParams.get("startDate");
    const fin = searchParams.get("endDate");
    const loading = useProtectedRoute();

    const [brands, setBrands] = useState([]);
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
        const token = localStorage.getItem("token");
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
                    throw new Error(
                        "Error al obtener politicas de cancelacion:"
                    );
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
    const [vehicleType, setVehicleType] = useState([]);
    // Hacer la solicitud a la API
    useEffect(() => {
        // const token = localStorage.getItem("token");
        const fetchVehicleTypes = async () => {
            try {
                const response = await fetch(
                    `http://localhost:5296/api/VehicleType/${vehicleTypeId}`,
                    {
                        method: "GET",
                        headers: {
                            "Content-Type": "application/json",
                        },
                    }
                );

                if (!response.ok) {
                    throw new Error("Error al obtener los tipos de vehiculos");
                }

                const data = await response.json();
                const vehicleType = data.data;

                const brand = brands.find(
                    (brand) => brand.id === vehicleType.brandId
                );
                const cancellationPolicy = cancellationPolicies.find(
                    (policy) => policy.id === vehicleType.cancellationPolicyId
                );

                const categoryNames = {
                    0: "Suv",
                    1: "Apto para discapacitados",
                    2: "Chico",
                    3: "Van",
                    4: "Deportivo",
                    5: "Mediano",
                };
                const categoria =
                    categoryNames[vehicleType.category] ||
                    "Categoría no encontrada";
                const vehicleTypeUpdated = {
                    id: vehicleType.id,
                    marca: brand ? brand.name : "Marca no encontrada",
                    model: vehicleType.model,
                    passengerCapacity: vehicleType.passengerCapacity,
                    Categoría: vehicleType.category,
                    pricePerDay: vehicleType.pricePerDay,
                    cancellationPolicyId: vehicleType.cancellationPolicyId,
                    imageUrl: vehicleType.imageUrl,
                    brandId: vehicleType.brandId,
                    category: vehicleType.category,
                    categoria: categoria,
                    cancellationPolicy: cancellationPolicy
                        ? cancellationPolicy.description
                        : "Política de cancelación no encontrada",
                };
                setVehicleType(vehicleTypeUpdated);
            } catch (error) {
                console.error(
                    "Error al obtener los tipos de vehiculos:",
                    error
                );
            }
        };

        fetchVehicleTypes();
    }, [vehicleTypeId, brands, cancellationPolicies]);

    const [branch, setBranch] = useState("");
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
                const foundBranch = data.data.find(
                    (branch) => branch.id.toString() === sucursalID
                );

                if (foundBranch) {
                    setBranch(foundBranch.name);
                }
            } catch (error) {
                console.error("Error al obtener sucursales:", error);
            }
        };

        fetchBranches();
    }, []);

    function getDaysBetweenDates(start, end) {
        const startDate = new Date(start);
        const endDate = new Date(end);

        // Restamos las fechas y dividimos por los milisegundos de un día
        const millisecondsPerDay = 1000 * 60 * 60 * 24;
        const diffInMilliseconds = endDate.getTime() - startDate.getTime();

        return Math.ceil(diffInMilliseconds / millisecondsPerDay);
    }
    if (loading) return null;
    return (
        <div className={styles.mainContainer}>
            <h1 className={`${styles.title} ${poppins.className} `}>
                Completa el formulario para confirmar tu reserva
            </h1>
            <p className={styles.subtitle}>
                En {branch || "no se encontro la sucursal"} entre el {inicio} y
                el {fin}
            </p>
            <div className={styles.content}>
                <div className={styles.form}>
                    <CreateRentalForm
                        endDate={fin}
                        startDate={inicio}
                        vehicleTypeId={vehicleTypeId}
                        withdrawalBranch={sucursalID}
                    />
                </div>

                <div key={vehicleType.id} className={styles.vehicleCard}>
                    <img
                        src={`http://localhost:5296/images/${vehicleType.imageUrl}`}
                        alt={vehicleType.model}
                        onError={(e) => {
                            e.currentTarget.onerror = null;
                            e.currentTarget.src = "/img/missingCar.jpeg";
                        }}
                        className={styles.vehicleImg}
                    />
                    <div className={styles.secondContainer}>
                        <h1>
                            {vehicleType.marca} {vehicleType.model}
                        </h1>

                        <div className={styles.caracteristicas}>
                            <div className={styles.caracteristicaCard}>
                                <p className={styles.type}>
                                    <strong>Capacidad: </strong>
                                    {vehicleType.passengerCapacity} pasajeros
                                </p>
                            </div>
                            <div className={styles.caracteristicaCard}>
                                <p className={styles.type}>
                                    <strong>Precio por día:</strong>{" "}
                                    {vehicleType.pricePerDay} $
                                </p>
                            </div>
                            <div className={styles.caracteristicaCard}>
                                <p className={styles.type}>
                                    <strong>Política de cancelación:</strong>{" "}
                                    {vehicleType.cancellationPolicy}
                                </p>
                            </div>
                            <div className={styles.caracteristicaCard}>
                                <p className={styles.type}>
                                    <strong>Total a pagar:</strong>{" "}
                                    {getDaysBetweenDates(inicio, fin) *
                                        vehicleType.pricePerDay}{" "}
                                    $
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
