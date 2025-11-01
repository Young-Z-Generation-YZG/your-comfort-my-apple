"use client";

import { useState } from "react";
import { useSolana } from "./solana-provider";
import { ChevronDown, Wallet, LogOut } from "lucide-react";
import {
    useConnect,
    useDisconnect,
    type UiWallet,
} from "@wallet-standard/react";

function truncateAddress(address: string): string {
    return `${address.slice(0, 4)}...${address.slice(-4)}`;
}

function WalletIcon({
    wallet,
    className,
}: {
    wallet: UiWallet;
    className?: string;
}) {
    return (
        <div className={className}>
            {wallet.icon ? (
                <img
                    src={wallet.icon}
                    alt={`${wallet.name} icon`}
                    className="w-5 h-5 rounded"
                />
            ) : (
                <div className="w-5 h-5 rounded bg-gray-300 flex items-center justify-center text-xs">
                    {wallet.name.slice(0, 2).toUpperCase()}
                </div>
            )}
        </div>
    );
}

function WalletMenuItem({
    wallet,
    onConnect,
}: {
    wallet: UiWallet;
    onConnect: () => void;
}) {
    const { setWalletAndAccount } = useSolana();
    const [isConnecting, connect] = useConnect(wallet);

    const handleConnect = async () => {
        if (isConnecting) return;

        try {
            const accounts = await connect();

            if (accounts && accounts.length > 0) {
                const account = accounts[0];
                setWalletAndAccount(wallet, account);
                onConnect();
            }
        } catch (err) {
            console.error(`Failed to connect ${wallet.name}:`, err);
        }
    };

    return (
        <button
            className="flex w-full items-center justify-between px-4 py-2 text-sm outline-none hover:bg-gray-100 dark:hover:bg-gray-800 focus:bg-gray-100 dark:focus:bg-gray-800 disabled:pointer-events-none disabled:opacity-50 rounded"
            onClick={handleConnect}
            disabled={isConnecting}
        >
            <div className="flex items-center gap-3">
                <WalletIcon wallet={wallet} />
                <span>{wallet.name}</span>
            </div>
            {isConnecting && (
                <span className="text-xs text-gray-500">Connecting...</span>
            )}
        </button>
    );
}

export function WalletConnectButton() {
    const {
        wallets,
        selectedWallet,
        selectedAccount,
        isConnected,
        setWalletAndAccount,
    } = useSolana();
    const [isOpen, setIsOpen] = useState(false);
    const disconnect = useDisconnect();

    const handleDisconnect = async () => {
        if (selectedWallet) {
            try {
                await disconnect(selectedWallet);
                setWalletAndAccount(null, null);
                setIsOpen(false);
            } catch (err) {
                console.error("Failed to disconnect:", err);
            }
        }
    };

    if (isConnected && selectedAccount) {
        return (
            <div className="relative">
                <button
                    onClick={() => setIsOpen(!isOpen)}
                    className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
                >
                    <Wallet className="w-4 h-4" />
                    <span>{truncateAddress(selectedAccount.address)}</span>
                    <ChevronDown className="w-4 h-4" />
                </button>

                {isOpen && (
                    <>
                        <div
                            className="fixed inset-0 z-10"
                            onClick={() => setIsOpen(false)}
                        />
                        <div className="absolute right-0 mt-2 w-56 bg-white dark:bg-gray-800 rounded-lg shadow-lg border border-gray-200 dark:border-gray-700 z-20">
                            <div className="p-3 border-b border-gray-200 dark:border-gray-700">
                                <p className="text-xs text-gray-500 dark:text-gray-400">
                                    Connected
                                </p>
                                <p className="text-sm font-medium truncate">
                                    {selectedAccount.address}
                                </p>
                            </div>
                            <button
                                onClick={handleDisconnect}
                                className="w-full flex items-center gap-2 px-4 py-2 text-sm text-red-600 hover:bg-red-50 dark:hover:bg-red-900/20 rounded-b-lg transition-colors"
                            >
                                <LogOut className="w-4 h-4" />
                                Disconnect
                            </button>
                        </div>
                    </>
                )}
            </div>
        );
    }

    if (wallets.length === 0) {
        return (
            <div className="px-4 py-2 bg-gray-200 dark:bg-gray-700 text-gray-600 dark:text-gray-400 rounded-lg text-sm">
                No wallet found. Please install a Solana wallet extension.
            </div>
        );
    }

    return (
        <div className="relative">
            <button
                onClick={() => setIsOpen(!isOpen)}
                className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
            >
                <Wallet className="w-4 h-4" />
                <span>Connect Wallet</span>
                <ChevronDown className="w-4 h-4" />
            </button>

            {isOpen && (
                <>
                    <div
                        className="fixed inset-0 z-10"
                        onClick={() => setIsOpen(false)}
                    />
                    <div className="absolute right-0 mt-2 w-64 bg-white dark:bg-gray-800 rounded-lg shadow-lg border border-gray-200 dark:border-gray-700 z-20">
                        <div className="p-2">
                            <p className="px-2 py-1 text-xs font-semibold text-gray-500 dark:text-gray-400">
                                Select Wallet
                            </p>
                            {wallets.map((wallet) => (
                                <WalletMenuItem
                                    key={wallet.name}
                                    wallet={wallet}
                                    onConnect={() => setIsOpen(false)}
                                />
                            ))}
                        </div>
                    </div>
                </>
            )}
        </div>
    );
}
