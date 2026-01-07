FROM node:18-alpine AS base

# Install dependencies only when needed
FROM base AS deps
# Check https://github.com/nodejs/docker-node/tree/b4117f9333da4138b03a546ec926ef50a31506c3#nodealpine to understand why libc6-compat might be needed.
RUN apk add --no-cache libc6-compat
WORKDIR /app

# Install dependencies based on the preferred package manager
COPY package.json yarn.lock* package-lock.json* pnpm-lock.yaml* ./
RUN \
  if [ -f yarn.lock ]; then yarn --frozen-lockfile; \
  elif [ -f package-lock.json ]; then npm ci --legacy-peer-deps; \
  elif [ -f pnpm-lock.yaml ]; then corepack enable pnpm && pnpm i --frozen-lockfile; \
  else echo "Lockfile not found." && exit 1; \
  fi


# Rebuild the source code only when needed
FROM base AS builder
WORKDIR /app
COPY --from=deps /app/node_modules ./node_modules
COPY . .

# Accept build arguments for environment variables
ARG API_ENDPOINT
ARG APP_URL

# Make build args available as environment variables during build
ENV API_ENDPOINT=$API_ENDPOINT
ENV APP_URL=$APP_URL

# Next.js collects completely anonymous telemetry data about general usage.
# Learn more here: https://nextjs.org/telemetry
# Disable telemetry to reduce overhead
ENV NEXT_TELEMETRY_DISABLED=1

RUN \
  if [ -f yarn.lock ]; then yarn run build; \
  elif [ -f package-lock.json ]; then npm run build; \
  elif [ -f pnpm-lock.yaml ]; then corepack enable pnpm && pnpm run build; \
  else echo "Lockfile not found." && exit 1; \
  fi

# Production image, copy all the files and run next
FROM base AS runner
WORKDIR /app

ENV NODE_ENV=production
# Disable telemetry to reduce overhead
ENV NEXT_TELEMETRY_DISABLED=1
# Set Node.js memory limit to prevent excessive memory usage (512MB limit)
ENV NODE_OPTIONS="--max-old-space-size=512 --no-warnings"
# Set Next.js to use private standalone mode
ENV NEXT_PRIVATE_STANDALONE=1
# Disable Next.js workers completely - prevent multiple process spawning
ENV NEXT_PRIVATE_NO_WORKERS=1
# Limit Next.js workers - prevent multiple process spawning
ENV UV_THREADPOOL_SIZE=1
# Limit to single CPU core
ENV NODE_CPU_COUNT=1

RUN addgroup --system --gid 1001 nodejs
RUN adduser --system --uid 1001 nextjs

COPY --from=builder /app/public ./public

# Create cache and tmp directories with proper permissions
RUN mkdir -p .next/cache .next/trace tmp && \
    chown -R nextjs:nodejs .next tmp && \
    chmod -R 755 .next tmp

# Automatically leverage output traces to reduce image size
# https://nextjs.org/docs/advanced-features/output-file-tracing
COPY --from=builder --chown=nextjs:nodejs /app/.next/standalone ./
COPY --from=builder --chown=nextjs:nodejs /app/.next/static ./.next/static

# Copy start script from build context and make it executable
# This must be done before USER switch to set permissions
COPY start.sh /app/start.sh
RUN chmod +x /app/start.sh && \
    chown nextjs:nodejs /app/start.sh

# Ensure proper permissions for runtime (fix permission denied errors)
RUN chown -R nextjs:nodejs /app && \
    chmod -R 755 /app/.next && \
    chmod -R 755 /app/tmp || true

USER nextjs

EXPOSE 3000

ENV PORT=3000

# server.js is created by next build from the standalone output
# https://nextjs.org/docs/pages/api-reference/next-config-js/output
ENV HOSTNAME="0.0.0.0"
# Use start script to ensure single process execution
CMD ["/app/start.sh"]