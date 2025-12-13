"use client";

import { useEffect, useState } from "react";
import styles from "./showVehicles.module.scss";
import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";
import Table from "@/components/table/Table";
import CreateBtn from "@/components/createBtn/CreateBtn";
import { poppins } from "@/lib/fonts/fonts";
import { useAuth } from "@/context/AuthContext";
import { DeleteVehicle } from "@/lib/deleteVehicle";
import { toast } from "sonner";

export default function Page() {
    const [vehicles, setvehicles] = useState([]);
    const loading = useProtectedRoute(["Admin", "Employee"]);
    const { role } = useAuth();

    const token = localStorage.getItem("token");

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

                const vehicleUpdated = data.data.map((vehicle) => {
                    const branch = branches.find(
                        (branch) => branch.id === vehicle.currentBranchId
                    );
                    const vehicleType = vehicleTypes.find(
                        (vehicleType) =>
                            vehicleType.id === vehicle.vehicleTypeId
                    );
                    return {
                        id: vehicle.id,
                        licensePlate: vehicle.licensePlate,
                        Año: vehicle.year,
                        color: vehicle.color,
                        Sucursal: branch
                            ? branch.name
                            : "sucursal no encontrada",
                        vehicleType: vehicleType
                            ? `${vehicleType.Marca} - ${vehicleType.model} - capacidad ${vehicleType.passengerCapacity}  `
                            : "Tipo de vehículo no encontrado",
                        isUnderMaintenance: vehicle.isUnderMaintenance,
                    };
                });
                setvehicles(vehicleUpdated);
            } catch (error) {
                console.error("Error al obtener vehiculos:", error);
            }
        };

        fetchVehicles();
    }, [branches, vehicleTypes]);

    const handleDelete = async (item, token) => {
        const response = await DeleteVehicle(item.id, token);
        if (response.success) {
            toast.success(`Vehículo eliminado exitosamente. `, {
                duration: 3000,
                closeButton: true,
            });
            setTimeout(() => {
                window.location.reload();
            }, 1300);
        } else {
            toast.error(
                response?.error?.generalError ||
                    "Error al eliminar el vehículo",
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
                    Lista de vehiculos
                </h1>
                {role === "Admin" && <CreateBtn href="/createVehicle" />}
            </div>
            <Table
                data={vehicles}
                editPath={"updateVehicle/"}
                onDelete={handleDelete}
                confirmationModalText="¿Estás seguro de eliminar el vehículo?"
            />
        </div>
    );
}
