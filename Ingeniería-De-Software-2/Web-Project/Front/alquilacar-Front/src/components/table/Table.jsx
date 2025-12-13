import styles from "./table.module.scss";
import Edit from "@/lib/svg/Edit";
import Delete from "@/lib/svg/Delete";
import { useRouter, usePathname } from "next/navigation";
import { useState } from "react";
import { useAuth } from "@/context/AuthContext";
import Maintenance from "@/lib/svg/Maintenance";
import Cross from "@/lib/svg/Cross";
import { toast } from "sonner";
import { PutIntoMaintenance } from "@/lib/putIntoMaintenance";
import { InvalidateReservation } from "@/lib/InvalidateReservation";

export default function Table({
    data,
    editPath,
    onDelete,
    confirmationModalText = "¿Estás seguro?",
}) {
    const token = localStorage.getItem("token");
    const [openConfirmationModal, setOpenConfirmationModal] = useState(false);
    const [openAnulationModal, setOpenAnulationModal] = useState(false);
    const [selectedItem, setSelectedItem] = useState();
    const { role } = useAuth();

    // Mapeo de keys a sus nombres en español
    const keyTranslations = {
        //sucursales
        name: "Nombre",
        province: "Provincia",
        locality: "Localidad",
        //empleados
        mail: "Email",
        firstName: "Nombre",
        lastName: "Apellido",
        dni: "DNI",
        birthdate: "Fecha de nacimiento",
        phoneNumber: "Teléfono",
        creationDate: "Fecha de creación",
        //politicas de cancelacion
        description: "Descripción",
        returnPercentage: "Porcentaje de devolución",
        //tipos de vehiculos
        model: "Modelo",
        passengerCapacity: "Capacidad de pasajeros",
        year: "Año",
        pricePerDay: "Precio por día",
        cancellationPolicyId: "Política de cancelación",
        imageUrl: "Imagen",
        //vehiculos
        licensePlate: "Patente",
        vehicleType: "Tipo de vehiculo",
        isUnderMaintenance: "Mantenimiento",
        //mis reservas
        registrationDate: "Fecha de registro",
        rentalDate: "Fecha de inicio",
        returnDate: "Fecha de devolución",
        totalPrice: "Precio total $",
        cancellationPolicy: "Política de cancelación",
        //mis alquileres
        pickedUpBranch: "Sucursal de retiro",
        returnedBranch: "Sucursal de devolución",
    };

    const formatValue = (key, value, item) => {
        if (key === "isUnderMaintenance") {
            const handleToggleMaintenance = async () => {
                const res = await PutIntoMaintenance(item.id, token, !value);
                if (res.success) {
                    toast.success(
                        value
                            ? "Vehículo quitado de mantenimiento."
                            : "Vehículo puesto en mantenimiento.",
                        {
                            duration: 3000,
                            closeButton: true,
                        }
                    );
                    setTimeout(() => {
                        window.location.reload();
                    }, 1000);
                } else {
                    toast.error(
                        res?.error?.generalError ||
                            "Error al cambiar el mantenimiento del vehiculo",
                        {
                            duration: 3000,
                            closeButton: true,
                        }
                    );
                }
            };

            return value ? (
                <Maintenance
                    className={styles.inMaintenance}
                    onClick={handleToggleMaintenance}
                />
            ) : (
                <Cross
                    className={styles.noMaintenance}
                    onClick={handleToggleMaintenance}
                />
            );
        }
        if (!value) return value;

        if (
            key === "birthdate" ||
            key === "creationDate" ||
            key === "registrationDate" ||
            key === "rentalDate" ||
            key === "returnDate"
        ) {
            return new Date(value).toLocaleDateString("es-ES", {
                day: "numeric",
                month: "long",
                year: "numeric",
            });
        } else {
            if (key === "imageUrl") {
                return (
                    <img
                        src={`http://localhost:5296/images/${value}`}
                        alt={"Imagen"}
                        onError={(e) => {
                            e.currentTarget.onerror = null;
                            e.currentTarget.src = "/img/missingCar.jpeg";
                        }}
                        style={{
                            width: "100px",
                            height: "100px",
                            objectFit: "cover",
                            borderRadius: "5px",
                        }}
                    />
                );
            }
        }

        return value;
    };

    const keys =
        data && data.length > 0
            ? Object.keys(data[0]).filter(
                  (key) =>
                      key !== "id" &&
                      key !== "userId" &&
                      key !== "workBranch" &&
                      key !== "brandId" &&
                      key !== "category" &&
                      key !== "order"
              )
            : [];
    const router = useRouter();
    const pathname = usePathname();
    const isReservationPage = pathname === "/reservationHistory";
    const isReturnVehiclePage = pathname === "/showRentals";
    const isMyRentalsPage = pathname === "/myRentals";
    const isCancelReservationPage = pathname === "/cancelReservation";
    const isGeneralStatisticsPage = pathname === "/generalStatistics";
    const isRentalStatisticsPage = pathname === "/rentalStatistics";
    const isRevenueStatisticsPage = pathname === "/revenueStatistics";
    const isShowBranchesPage = pathname === "/showBranches";
    const isShowVehiclessPage = pathname === "/showVehicles";
    const isShowEmployeesPage = pathname === "/showEmployees";
    const policyMessages = {
        "Sin devolución": "No tendrá devolución del dinero.",
        "20% de devolución": "Se devolverá el 20% del precio total.",
        "100% de devolución": "Se devolverá el 100% del precio total.",
    };

    const handleInvalidateReservation = async (item, token) => {
        const response = await InvalidateReservation(item.id, token);
        if (response.success) {
            toast.success(
                `Reserva anulada exitosamente. Se le devolverá el 100% del monto de la reserva al cliente`,
                {
                    duration: 3000,
                    closeButton: true,
                }
            );
            setTimeout(() => {
                window.location.reload();
            }, 1500);
        } else {
            toast.error(
                res?.error?.generalError || "Error al anular la reserva",
                {
                    duration: 3000,
                    closeButton: true,
                }
            );
        }
    };
    return (
        <div className={styles.tableContainer}>
            {openConfirmationModal && (
                <div className={styles.deleteConfirmationModalContainer}>
                    <div className={styles.deleteConfirmationModal}>
                        <p>
                            {confirmationModalText}
                            <br />
                            {isReservationPage || isCancelReservationPage
                                ? policyMessages[
                                      selectedItem.cancellationPolicy
                                  ]
                                : ""}
                        </p>

                        <div className={styles.btnsContainer}>
                            <button
                                className={styles.confirm}
                                onClick={() => onDelete(selectedItem, token)}
                            >
                                Si, confirmar
                            </button>
                            <button
                                className={styles.cancel}
                                onClick={() => setOpenConfirmationModal(false)}
                            >
                                Volver
                            </button>
                        </div>
                    </div>
                </div>
            )}
            {openAnulationModal && (
                <div className={styles.deleteConfirmationModalContainer}>
                    <div className={styles.deleteConfirmationModal}>
                        <p>
                            ¿Quiere anular la reserva?{<br />}
                            Se le devolverá el 100% del valor de la reserva al
                            cliente
                        </p>

                        <div className={styles.btnsContainer}>
                            <button
                                className={styles.confirm}
                                onClick={() =>
                                    handleInvalidateReservation(
                                        selectedItem,
                                        token
                                    )
                                }
                            >
                                Si, confirmar
                            </button>
                            <button
                                className={styles.cancel}
                                onClick={() => setOpenAnulationModal(false)}
                            >
                                Volver
                            </button>
                        </div>
                    </div>
                </div>
            )}
            <table className={styles.table}>
                <thead>
                    {keys.length > 0 && (
                        <tr>
                            {keys.map((key) => (
                                <th key={key}>{keyTranslations[key] || key}</th>
                            ))}
                            {!isReservationPage &&
                                !isReturnVehiclePage &&
                                !isMyRentalsPage &&
                                !isCancelReservationPage &&
                                !isGeneralStatisticsPage &&
                                !isRentalStatisticsPage &&
                                !isRevenueStatisticsPage && <th>Editar</th>}
                            {(role === "Admin" && isShowBranchesPage) ||
                            isShowEmployeesPage ||
                            isShowVehiclessPage ||
                            (role === "Employee" &&
                                isCancelReservationPage &&
                                !isMyRentalsPage) ||
                            (role === "Customer" && isReservationPage) ? (
                                <th>
                                    {isReservationPage ||
                                    isCancelReservationPage
                                        ? "Cancelar"
                                        : "Eliminar"}
                                </th>
                            ) : null}
                            {isCancelReservationPage && <th>Anular</th>}
                        </tr>
                    )}
                </thead>
                <tbody>
                    {data.map((item) => (
                        <tr key={item.id}>
                            {keys &&
                                keys.map((key) => (
                                    <td key={key}>
                                        {formatValue(key, item[key], item)}
                                    </td>
                                ))}
                            {!isReservationPage &&
                                !isReturnVehiclePage &&
                                !isMyRentalsPage &&
                                !isCancelReservationPage &&
                                !isGeneralStatisticsPage &&
                                !isRentalStatisticsPage &&
                                !isRevenueStatisticsPage && (
                                    <td className={styles.editTD}>
                                        <Edit
                                            className={styles.edit}
                                            onClick={() => {
                                                router.push(
                                                    `${editPath}${item.id}`
                                                );
                                            }}
                                        />
                                    </td>
                                )}
                            {(role === "Admin" && isShowBranchesPage) ||
                            isShowEmployeesPage ||
                            isShowVehiclessPage ||
                            (role === "Employee" &&
                                isCancelReservationPage &&
                                !isMyRentalsPage) ||
                            (role === "Customer" && isReservationPage) ? (
                                <td className={styles.deleteTD}>
                                    {(!isReservationPage ||
                                        (item?.Estado !== "Cancelada" &&
                                            item?.Estado !== "Anulada" &&
                                            item?.Estado !== "Completa")) && (
                                        <Delete
                                            className={styles.delete}
                                            onClick={() => {
                                                setSelectedItem(item);
                                                setOpenConfirmationModal(true);
                                            }}
                                        />
                                    )}
                                </td>
                            ) : null}
                            {isCancelReservationPage && (
                                <td className={styles.deleteTD}>
                                    <Cross
                                        className={styles.delete}
                                        onClick={() => {
                                            setSelectedItem(item);
                                            setOpenAnulationModal(true);
                                        }}
                                    />
                                </td>
                            )}
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}
