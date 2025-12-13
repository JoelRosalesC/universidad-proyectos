export default function Cross(props) {
    return (
        <svg
            xmlns="http://www.w3.org/2000/svg"
            width={32}
            height={32}
            viewBox="6 7 12 10"
            {...props}
        >
            <path
                fill="none"
                stroke="currentColor"
                strokeLinecap="round"
                strokeWidth={1.5}
                d="M8.464 15.535l7.072-7.07m-7.072 0l7.072 7.07"
            />
        </svg>
    );
}
