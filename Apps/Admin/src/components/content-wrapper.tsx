import { cn } from "../infrastructure/lib/utils";

type ContentWrapperProps = {
    children: React.ReactNode;
    className?: string;
}

const ContentWrapper = (
    { children, className = '' }: ContentWrapperProps
) => {
    return (
        <div
            className={cn("bg-muted/50 rounded-xl p-3", className)}
        >
            {children}
        </div>
    )
}

export default ContentWrapper