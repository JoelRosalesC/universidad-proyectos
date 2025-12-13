import { useEffect, useState } from "react";
import styles from "./checkboxGroup.module.scss";
// options = [
//   { value: "telefono", label: "Teléfono" },
//   { value: "email", label: "Email" },
//   { value: "whatsapp", label: "WhatsApp" },
// ]

// defaultValues = ["telefono", "email"];

// onChange Callback that is triggered when the CheckboxGroup changes a value. Intended to be used with useState to lift the selected values to the parent component.

export default function CheckboxGroup({
    name,
    label,
    options = [],
    required = false,
    defaultValues = [],
    className = "",
    error = "",
    onChange,
}) {
    const classes = `${styles.mainContainer} ${className || ""}`;
    const [selectedValues, setSelectedValues] = useState(defaultValues || []);

    const handleChange = (value) => {
        let updatedValues;
        if (selectedValues.includes(value)) {
            updatedValues = selectedValues.filter((v) => v !== value);
        } else {
            updatedValues = [...selectedValues, value];
        }
        setSelectedValues(updatedValues);
        onChange?.(updatedValues);
    };

    return (
        <fieldset className={classes}>
            {label && <legend>{label}</legend>}
            {options.map(({ value, label: optionLabel }) => (
                <label key={value} className={styles.checkboxLabel}>
                    <input
                        type="checkbox"
                        name={name}
                        value={value}
                        defaultChecked={defaultValues.includes(value)}
                        required={required && defaultValues.length === 0}
                        onChange={() => handleChange(value)}
                    />
                    {optionLabel}
                </label>
            ))}
            {error && <p className={styles.error}>{error}</p>}
        </fieldset>
    );
}
