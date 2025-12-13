import styles from "./submitBtn.module.scss";

export default function SubmitBtn({
    type,
    children,
    className,
    onClick,
    disabled,
}) {
    const classes = `${styles.Button} ${className || ""}`;

    return (
        <button
            className={classes}
            disabled={disabled}
            onClick={onClick}
            type={type}
        >
            {children}
        </button>
    );
}
