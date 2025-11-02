"use client";

import { WalletConnectButton } from "@/components/wallet-connect-button";
import { PaymentCard } from "@/components/payment-card";
import { MemoCard } from "@/components/memo-card";

export default function Home() {
    return (
        <div className="min-h-screen flex items-center justify-center p-4 bg-gray-50 dark:bg-gray-900">
            <div className="w-full max-w-md bg-white dark:bg-gray-800 rounded-lg border shadow-lg p-6 space-y-6">
                <div className="flex justify-center">
                    <WalletConnectButton />
                </div>
                <PaymentCard />
                <MemoCard />
            </div>
        </div>
    );
}
