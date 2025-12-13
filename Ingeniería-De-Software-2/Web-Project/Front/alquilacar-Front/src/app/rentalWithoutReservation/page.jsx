"use client";
import styles from "./rentalWithoutReservation.module.scss";
import { poppins } from "@/lib/fonts/fonts";
import CreateRentalWithoutReservationForm from "@/components/forms/createRentalWithoutReservationForm/CreateRentalWithoutReservationForm";
import { useProtectedRoute } from "@/lib/customHooks/useProtectedRoute";
import { useEffect, useState } from "react";

export default function RentalWithoutReservation() {
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

    //reservaciones de hoy , en curso y de la sucursal del empleado logueado
    //para que el empleado elija que reservacion es la del cliente, serian las opciones
    //de un select que envia el rentalid y muetsra la info necesaria para que el empleado elija
    useEffect(() => {
        const fetchReservations = async () => {
            try {
                const response = await fetch(
                    "http://localhost:5296/api/Rental/GetRentalsInBranch",
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
                        "Error al obtener las reservas de la sucursal en donde trabajo"
                    );
                }

                const data = await response.json();
                // console.log("reservation data: ", data);
                const isToday = (isoDate) => {
                    const d = new Date(isoDate);
                    const today = new Date();
                    return (
                        d.getFullYear() === today.getFullYear() &&
                        d.getMonth() === today.getMonth() &&
                        d.getDate() === today.getDate()
                    );
                };

                const onlyTodayAndInProgress = data.data.filter(
                    (r) => isToday(r.rentalDate) && r.status === 0 // 0 = En curso
                );
                const statusList = [
                    { value: 0, name: "En curso" },
                    { value: 1, name: "Completa" },
                    { value: 2, name: "Cancelada" },
                    { value: 3, name: "Completa" },
                ];
                const formatedReservations = onlyTodayAndInProgress.map(
                    (reservation) => {
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
                        const cancellationPolicy = cancellationPolicies.find(
                            (policy) =>
                                policy.id === reservation.cancellationPolicyId
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
                                vehicleType
                                    ? `${vehicleType.Marca} - ${vehicleType.model} - capacidad ${vehicleType.passengerCapacity}`
                                    : "Tipo de vehículo no encontrado"
                            } |
                            devolución: ${reservation.returnDate.split("T")[0]} 
                            
                            `,

                            // id: reservation.id,
                            // customerId: reservation.customerId,
                            // registrationDate: reservation.registrationDate,
                            // Sucursal:
                            //     sucursal?.name || "sucursal no encontrada",
                            // rentalDate: reservation.rentalDate,
                            // returnDate: reservation.returnDate,
                            // totalPrice: reservation.totalPrice,
                            // cancellationPolicy: cancellationPolicy
                            //     ? cancellationPolicy.description
                            //     : "Política de cancelación no encontrada",
                            // Vehiculo: vehicleType
                            //     ? `${vehicleType.Marca} - ${vehicleType.model} - capacidad ${vehicleType.passengerCapacity}  `
                            //     : "Tipo de vehículo no encontrado",
                            // Estado: statusObj.name || "Desconocido",
                        };
                    }
                );
                // console.log("formated reservations: ", formatedReservations);

                setReservations(formatedReservations);
            } catch (error) {
                console.error(
                    "Error al obtener las reservas de la sucursal en donde trabajo:",
                    error
                );
            }
        };

        fetchReservations();
    }, [vehicleTypes, branches, cancellationPolicies, customers]);

    if (loading) return null;
    return (
        <div className={styles.mainContainer}>
            <h1 className={`${styles.title} ${poppins.className} `}>
                Realizando Alquiler sin reserva
            </h1>

            <CreateRentalWithoutReservationForm reservations={reservations} />
        </div>
    );
}
