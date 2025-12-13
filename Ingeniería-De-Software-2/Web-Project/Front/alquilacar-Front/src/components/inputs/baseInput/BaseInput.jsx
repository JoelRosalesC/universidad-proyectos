"use client";
import React from "react";
import styles from "./baseInput.module.scss";
// Base styling for inputs, label animation, and allowing an input or textarea as children.
export default function BaseInput({ className, label, name, error, children }) {
    const classes = `${styles.container} ${className || ""}`;

    return (
        <div className={classes}>
            <div className={styles.inputContainer}>
                {/* input o textarea */}
                {React.cloneElement(children, {
                    className: `${styles.input} ${
                        error ? styles.inputError : ""
                    } ${children.props.className || ""}`,
                })}

                <label className={styles.label} htmlFor={name}>
                    {label}
                </label>
                <span className={styles.hiddenSpan}>{label + "aa"}</span>
            </div>
            {error &&
                (Array.isArray(error) ? (
                    <div className={styles.error}>{error.join(". ")}</div>
                ) : (
                    <div className={styles.error}>{error}</div>
                ))}
        </div>
    );
}
