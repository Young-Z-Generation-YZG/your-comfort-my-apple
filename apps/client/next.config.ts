import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  /* config options here */
  transpilePackages: ["@kltn-monorepo/ui-shadcn", "@kltn-monorepo/utils"],
};

export default nextConfig;
