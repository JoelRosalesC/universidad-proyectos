import "./globals.css";
import { AuthProvider } from "../context/AuthContext"; // Ajustá el path si usás src/

import Navbar from "@/components/navbar/Navbar";
import Footer from "@/components/footer/Footer";
import { inter } from "@/lib/fonts/fonts";
import { Toaster } from "sonner";

export const metadata = {
    title: "Alquilapp Car",
    description: "Manejá tu camino",
};

export default function RootLayout({ children }) {
    return (
        <html lang="es">
            <body className={inter.className}>
                <AuthProvider>
                    <Navbar />
                    <main> {children}</main>
                    <Toaster
                        richColors
                        position="bottom-right"
                        toastOptions={{
                            classNames: {
                                toast: "toast",
                                title: "toast-title",
                                description: "toast-description",
                                closeButton: "toast-close",
                            },
                        }}
                    />
                    <Footer />
                </AuthProvider>
            </body>
        </html>
    );
}
