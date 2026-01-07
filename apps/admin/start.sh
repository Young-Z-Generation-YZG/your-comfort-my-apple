#!/bin/sh
# Start script to ensure single process execution
# Prevent Next.js from spawning multiple workers

# Set environment variables to limit processes
export NODE_OPTIONS="--max-old-space-size=512 --no-warnings"
export UV_THREADPOOL_SIZE=1
export NODE_ENV=production
export NEXT_TELEMETRY_DISABLED=1
export NEXT_PRIVATE_STANDALONE=1
# Disable Next.js workers completely
export NEXT_PRIVATE_NO_WORKERS=1
# Limit to single CPU core
export NODE_CPU_COUNT=1

# Function to kill extra worker processes from .next directory
kill_workers() {
    # Kill any processes running from .next directory (worker processes)
    # These are the high CPU processes causing issues
    ps aux | grep -E '/app/\.next/[A-Za-z0-9]+' | grep -v grep | grep -v 'server.js' | awk '{print $2}' | while read pid; do
        if [ ! -z "$pid" ] && [ "$pid" != "$$" ]; then
            kill -9 "$pid" 2>/dev/null || true
        fi
    done
}

# Start server with lowest priority
nice -n 19 node --max-old-space-size=512 --no-warnings server.js &
SERVER_PID=$!

# Monitor and aggressively kill extra workers every 3 seconds
while kill -0 $SERVER_PID 2>/dev/null; do
    kill_workers
    sleep 3
done

# Wait for server process
wait $SERVER_PID
