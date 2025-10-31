// utils/tenant-resolver.ts

export function getTenantFromSubdomain(): string | null {
   if (typeof window === 'undefined') return null;

   const hostname = window.location.hostname;

   // localhost or IP address - no subdomain
   if (hostname === 'localhost' || /^\d+\.\d+\.\d+\.\d+$/.test(hostname)) {
      return null;
   }

   const parts = hostname.split('.');

   const subdomain = parts[0];

   // Ignore common non-tenant subdomains
   // if (['www', 'admin', 'app'].includes(subdomain)) {
   //   return null;
   // }

   return subdomain;
}

export function resolveTenantContext(
   availableTenants: {
      tenant_id: string;
      branch_id: string;
      tenant_code: string;
   }[],
): {
   tenant_id: string;
   branch_id: string;
   tenant_code: string;
} | null {
   // Strategy 1: Check subdomain
   const subdomain = getTenantFromSubdomain();
   if (subdomain) {
      const tenant = availableTenants.find(
         (t) => t.tenant_code.toLowerCase() === subdomain.toLowerCase(),
      );
      if (tenant) return tenant;
   }

   // Strategy 2: Check URL query param (for sharing links)
   // const urlParams = new URLSearchParams(window.location.search);
   // const tenantId = urlParams.get('tenant_id');
   // if (tenantId) {
   //   const tenant = availableTenants.find(t => t.tenant_id === tenantId);
   //   if (tenant) return tenant;
   // }

   // Strategy 3: Check persisted selection (from Redux persist)
   // This is handled by Redux persist automatically

   return null;
}
