export default function Loading(props) {
    return (
        <svg
            xmlns="http://www.w3.org/2000/svg"
            width={32}
            height={32}
            viewBox="0 0 24 24"
            {...props}
        >
            <path
                fill="none"
                stroke="currentColor"
                strokeDasharray={16}
                strokeDashoffset={16}
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M12 3a9 9 0 019 9"
            >
                <animate
                    fill="freeze"
                    attributeName="stroke-dashoffset"
                    dur="0.2s"
                    values="16;0"
                />
                <animateTransform
                    attributeName="transform"
                    dur="1.5s"
                    repeatCount="indefinite"
                    type="rotate"
                    values="0 12 12;360 12 12"
                />
            </path>
        </svg>
    );
}
