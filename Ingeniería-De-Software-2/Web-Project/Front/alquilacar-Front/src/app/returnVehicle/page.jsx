"use client";
import styles from "./returnVehicle.module.scss";
import { poppins } from "@/lib/fonts/fonts";
import CreateReturnVehicleForm from "@/components/forms/createReturnVehicleForm/CreateReturnVehicleForm";
import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";
import { useEffect, useState } from "react";

export default function ReturnVehicle() {
    const loading = useProtectedRoute(["Employee"]);
    const token = localStorage.getItem("token");

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
                // console.log("brands: ", data.data);

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
                // console.log("politicas de canc: ", data.data);

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
                    // console.log(brand);

                    return {
                        id: vehicleType.id,
                        Marca: brand ? brand.name : "Marca no encontrada",
                        model: vehicleType.model,
                        passengerCapacity: vehicleType.passengerCapacity,
                    };
                });
                // console.log("vehicle types formated: ", vehicleTypesUpdated);

                setVehicleTypes(vehicleTypesUpdated);
            } catch (error) {
                console.error("Error al obtener tipos de vehiculos:", error);
            }
        };

        fetchVehicleTypes();
    }, [brands]);

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
                // console.log("branches: ", data.data);

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
    // reservaciones , en curso y de la sucursal del empleado logueado
    // para que el empleado elija que reservacion es la del cliente, serian las opciones
    // de un select que envia el rentalid y muetsra la info necesaria para que el empleado elija
    // cual quiere devolver el cliente
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
                    throw new Error("Error al obtener todas las reservas");
                }

                const data = await response.json();

                // filtro las que estan en alquiler
                const inProgress = data.data.rentals.filter(
                    (r) => r.status === 1
                );
                // console.log(inProgress);

                const formatedReservations = inProgress.map((reservation) => {
                    const vehicle = vehicles.find(
                        (vehicle) =>
                            vehicle.id === reservation.deliveredVehicleId
                    );
                    const vehicleType = vehicleTypes.find(
                        (vehicleType) =>
                            vehicleType.id === vehicle.vehicleTypeId
                    );

                    const customer = customers.find(
                        (customer) => customer.id === reservation.customerId
                    );
                    return {
                        value: reservation.id,
                        name: `
                            ${
                                customer
                                    ? `${customer.mail}`
                                    : "email del cliente no encontrado"
                            } |
                            ${
                                vehicle
                                    ? `${vehicle.licensePlate}`
                                    : "patente no encontrada"
                            } |
                            ${
                                vehicleType
                                    ? `${vehicleType.Marca} - ${vehicleType.model} - capacidad ${vehicleType.passengerCapacity}`
                                    : "Tipo de vehículo no encontrado"
                            } |
                            devolución: ${reservation.returnDate.split("T")[0]} 
                            
                            `,
                    };
                });
                // console.log("formated reservations: ", formatedReservations);

                setReservations(formatedReservations);
            } catch (error) {
                console.error(
                    "Error al obtener todas las reservas en alquiler:",
                    error
                );
            }
        };

        fetchReservations();
    }, [vehicleTypes, branches, cancellationPolicies, customers, vehicles]);

    if (loading) return null;
    return (
        <div className={styles.mainContainer}>
            <h1 className={`${styles.title} ${poppins.className} `}>
                Realizando Devolución del vehiculo
            </h1>
            <CreateReturnVehicleForm reservations={reservations} />
        </div>
    );
}
