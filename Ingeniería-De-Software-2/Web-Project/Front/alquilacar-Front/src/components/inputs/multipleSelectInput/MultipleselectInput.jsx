"use client";
import Chevron from "@/lib/svg/Chevron";
import styles from "./MultipleSelectInput.module.scss";
import { useEffect, useRef, useState } from "react";
import Cross from "@/lib/svg/Cross";

export default function MultipleSelectInput({
    className,
    required,
    options,
    label,
    name,
    error,
    defaultSelectedOptions = [],
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
    const [selectedOptions, setSelectedOptions] = useState(
        defaultSelectedOptions
    );
    const classes = `${styles.container} ${className || ""}`;
    const wrapperRef = useRef(null);

    useEffect(() => {
        setSelectedOptions(defaultSelectedOptions);
    }, [defaultSelectedOptions]);

    // Función para manejar la selección múltiple
    const handleMultipleSelect = (option) => {
        // Verificamos si la opción ya está en el array de seleccionados
        const isSelected = selectedOptions.some(
            (opt) => opt.value === option.value
        );

        if (isSelected) {
            // Si ya está seleccionada, la eliminamos
            setSelectedOptions((prevSelectedOptions) =>
                prevSelectedOptions.filter((opt) => opt.value !== option.value)
            );
        } else {
            // Si no está seleccionada, la agregamos
            setSelectedOptions((prevSelectedOptions) => [
                ...prevSelectedOptions,
                option,
            ]);
        }
    };

    const renderDropdownOptions = (options) =>
        options.map((option) => (
            <div
                key={option.value}
                className={` ${styles.option} ${
                    selectedOptions.some(
                        (selectedOption) =>
                            selectedOption.value === option.value
                    )
                        ? styles.optionSelected
                        : ""
                } `}
                onClick={() => handleMultipleSelect(option)}
            >
                {option.name}
            </div>
        ));

    const handleRemoveSelected = (value) => {
        setSelectedOptions((prevSelectedOptions) =>
            prevSelectedOptions.filter((opt) => opt.value !== value)
        );
    };

    const renderMultipleSelectedTags = () =>
        selectedOptions.map((option, index) => (
            <div
                key={`${option.value}-${index}`}
                className={styles.selectedTag}
                onClick={(e) => {
                    e.stopPropagation();
                    handleRemoveSelected(option.value);
                }}
            >
                {option.name}
                <Cross
                    onClick={(e) => {
                        e.stopPropagation();
                        handleRemoveSelected(option.value);
                    }}
                />
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
                } ${selectedOptions.length > 0 ? styles.withValue : ""} ${
                    error ? styles.inputError : ""
                } `}
                ref={wrapperRef}
                onClick={() => setIsOpen(!isOpen)}
            >
                <label className={styles.label} htmlFor={name}>
                    {label}
                </label>
                <span className={styles.hiddenSpan}>{label + "aa"}</span>
                {/* Select multiple oculto para accesibilidad y compatibilidad con form*/}
                <select
                    multiple
                    name={name}
                    required={required}
                    value={selectedOptions.map((option) => option.value)}
                    className={styles.hiddenSelect}
                    onChange={(e) => {
                        const selectedValues = Array.from(
                            e.target.selectedOptions,
                            (opt) => opt.value
                        );
                        setSelectedOptions(
                            options.filter((opt) =>
                                selectedValues.includes(opt.value)
                            )
                        );
                    }}
                >
                    {options.map((option) => (
                        <option key={option.value} value={option.value}>
                            {option.name}
                        </option>
                    ))}
                </select>

                <div className={styles.tagsContainer}>
                    {renderMultipleSelectedTags()}
                </div>

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
