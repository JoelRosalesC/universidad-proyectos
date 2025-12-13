"use client";
import Chevron from "@/lib/svg/Chevron";
import styles from "./selectInput.module.scss";
import { useEffect, useRef, useState } from "react";
//defaultSelectedOption seria solo el value, no el objeto completo y
// segun ese value se selecciona la opcion que corresponde en el select
export default function SelectInput({
    className,
    required,
    options,
    label,
    name,
    error,
    defaultSelectedOption = {},
}) {
    //  options: [
    //     {
    //         name: "name 1",
    //         value: "value 1",
    //     },
    //     {
    //         name: "name 2",
    //         value: "value 2",
    //     },
    //     {
    //         name: "name 3",
    //         value: "value 3",
    //     },
    //     {
    //         name: "name 4",
    //         value: "value 4",
    //     },...
    // ]

    const [isOpen, setIsOpen] = useState(false);
    const [selectedOption, setSelectedOption] = useState({});
    const classes = `${styles.container} ${className || ""}`;
    const wrapperRef = useRef(null);
    const handleSelect = (value) => {
        const selected = options.find((opt) => opt.value === value);
        setSelectedOption({
            value: selected?.value ?? "",
            name: selected?.name ?? "",
        });

        setIsOpen(false);
    };

    useEffect(() => {
        if (defaultSelectedOption) {
            const selected = options.find(
                (opt) => opt.value == defaultSelectedOption
            );
            if (selected) {
                setSelectedOption({
                    value: selected.value,
                    name: selected.name,
                });
            }
        }
    }, [defaultSelectedOption, options]);

    const renderDropdownOptions = (options) =>
        options.map((option, index) => (
            <div
                key={index}
                className={` ${styles.option} ${
                    selectedOption.value !== "" &&
                    selectedOption.value == option.value
                        ? styles.selectedOption
                        : ""
                } `}
                onClick={() => handleSelect(option.value)}
            >
                {option.name}
            </div>
        ));

    useEffect(() => {
        function handleClickOutside(event) {
            if (
                wrapperRef.current &&
                !wrapperRef.current.contains(event.target)
            ) {
                setIsOpen(false);
            }
        }

        document.addEventListener("pointerdown", handleClickOutside);
        return () => {
            document.removeEventListener("pointerdown", handleClickOutside);
        };
    }, []);
    return (
        <div className={classes}>
            <div
                className={`${styles.selectWrapper} ${
                    isOpen ? styles.focus : ""
                } ${
                    selectedOption.value !== undefined &&
                    selectedOption.value !== null
                        ? styles.withValue
                        : ""
                } ${error ? styles.inputError : ""} `}
                ref={wrapperRef}
                onClick={() => setIsOpen(!isOpen)}
            >
                <label className={styles.label} htmlFor={name}>
                    {label}
                </label>
                <span className={styles.hiddenSpan}>{label + "aa"}</span>
                <select
                    name={name}
                    required={required}
                    value={selectedOption.value ?? ""}
                    className={styles.hiddenSelect} // Ocultamos el select con CSS
                    onChange={(e) => {
                        setSelectedOption({
                            value: e.target.value,
                            name:
                                options.find(
                                    (opt) => opt.value == e.target.value
                                )?.name || "",
                        });
                    }}
                >
                    <option value=""></option>
                    {options.map((option, index) => (
                        <option key={index} value={option.value}>
                            {option.name}
                        </option>
                    ))}
                </select>

                <div>{selectedOption.name}</div>

                <Chevron
                    className={`${styles.chevronDown} ${
                        isOpen ? styles.open : ""
                    }`}
                />
                {isOpen && (
                    <div className={styles.dropdown} tabIndex="0">
                        {renderDropdownOptions(options)}
                    </div>
                )}
            </div>
            {error && <div className={styles.error}>{error}</div>}
        </div>
    );
}
