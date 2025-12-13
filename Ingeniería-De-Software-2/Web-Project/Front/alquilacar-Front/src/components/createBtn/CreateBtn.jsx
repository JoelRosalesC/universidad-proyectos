import Cross from "@/lib/svg/Cross";
import styles from "./createBtn.module.scss";
export default function CreateBtn({ href = "#" }) {
    return (
        <a className={styles.crearBtn} href={href}>
            Crear
            <Cross />
        </a>
    );
}
