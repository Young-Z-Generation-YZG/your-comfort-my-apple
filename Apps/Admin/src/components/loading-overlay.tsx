import React from "react";
import { Loader2 } from "lucide-react";

interface LoadingOverlayProps {
  isLoading: boolean;
}

export const LoadingOverlay: React.FC<LoadingOverlayProps> = ({
  isLoading,
}) => {
  if (!isLoading) return null;

  return (
    <div
      className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50"
      style={{ pointerEvents: "all" }}
    >
      <div className="bg-white rounded-lg p-6 flex items-center space-x-4">
        <Loader2 className="h-6 w-6 animate-spin text-primary" />
        <p className="text-lg font-semibold">Loading...</p>
      </div>
    </div>
  );
};
