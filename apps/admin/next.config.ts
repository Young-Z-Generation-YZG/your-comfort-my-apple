import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  /* config options here */
  output: "standalone",
  transpilePackages: ["@kltn-monorepo/ui-shadcn", "@kltn-monorepo/utils"],
};

export default nextConfig;
