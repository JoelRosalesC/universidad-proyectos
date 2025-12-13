"use client";
import BaseInput from "./baseInput/BaseInput";

export default function SimpleInput({
    label,
    name,
    type = "text",
    error,
    className,
    ...props
}) {
    return (
        <BaseInput
            className={className}
            label={label}
            name={name}
            error={error}
        >
            <input
                type={type}
                name={name}
                id={name}
                placeholder=""
                {...props}
            />
        </BaseInput>
    );
}
