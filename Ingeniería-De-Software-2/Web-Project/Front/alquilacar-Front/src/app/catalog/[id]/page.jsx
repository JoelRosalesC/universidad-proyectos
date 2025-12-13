"use client";
import styles from "./vehicleTypeDetail.module.scss";
import { poppins } from "@/lib/fonts/fonts";
import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import { useParams } from "next/navigation";
import Star from "@/lib/svg/Star";

export default function VehicleDetail() {
    const router = useRouter();
    const params = useParams();
    const vehicleTypeId = params.id;

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
                    Marca: brand ? brand.name : "Marca no encontrada",
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

    return (
        <div className={styles.mainContainer}>
            <div className={styles.vehiclesContainer}>
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
                            {vehicleType.Marca} {vehicleType.model}
                        </h1>
                        <div className={styles.calification}>
                            <Star />
                            <Star />
                            <Star />
                            <Star />
                            <Star />
                            <p>5.0 (1M Calificaciones)</p>
                        </div>
                        <div className={styles.caracteristicas}>
                            <div className={styles.caracteristicaCard}>
                                <p className={styles.type}>Categoria</p>
                                <p className={styles.value}>
                                    <strong>{vehicleType.categoria}</strong>
                                </p>
                            </div>
                            <div className={styles.caracteristicaCard}>
                                <p className={styles.type}>Capacidad</p>
                                <p className={styles.value}>
                                    <strong>
                                        {vehicleType.passengerCapacity}{" "}
                                        pasajeros
                                    </strong>
                                </p>
                            </div>
                            <div className={styles.caracteristicaCard}>
                                <p className={styles.type}>Precio por día</p>
                                <p className={styles.value}>
                                    <strong>{vehicleType.pricePerDay} $</strong>
                                </p>
                            </div>
                            <div className={styles.caracteristicaCard}>
                                <p className={styles.type}>
                                    Política de cancelación
                                </p>

                                <p className={styles.value}>
                                    <strong>
                                        {vehicleType.cancellationPolicy}
                                    </strong>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
