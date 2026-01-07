# CPU Usage Fix for Next.js 15.1.0

## Problem
Next.js 15.1.0 is spawning multiple worker processes from `/app/.next/` directory, each consuming 80-90% CPU and 2.3GB+ RAM.

## Applied Fixes

1. **Process Monitoring Script** (`start.sh`)
   - Monitors and kills extra worker processes every 3 seconds
   - Uses `nice -n 19` to lower process priority
   - Limits to single CPU core

2. **Environment Variables**
   - `NEXT_PRIVATE_NO_WORKERS=1` - Disable Next.js workers
   - `UV_THREADPOOL_SIZE=1` - Limit libuv thread pool
   - `NODE_CPU_COUNT=1` - Limit to single CPU

3. **Docker Limits**
   - CPU: 1.0 core maximum
   - Memory: 1GB maximum, 512MB reserved

## If Problem Persists

This appears to be a bug in Next.js 15.1.0. Consider:

1. **Downgrade Next.js** (Recommended):
```bash
cd apps/admin
npm install next@15.0.4 --save-exact
```

2. **Or use Next.js 14.x** (More stable):
```bash
cd apps/admin
npm install next@14.2.15 --save-exact
npm install react@18.3.1 react-dom@18.3.1 --save-exact
```

3. **Rebuild Docker image**:
```bash
docker-compose -f docker-compose.prod.yml build --no-cache ygz.admin.web
docker-compose -f docker-compose.prod.yml up -d ygz.admin.web
```

## Monitoring

Check processes inside container:
```bash
docker exec -it <container_id> ps aux | grep node
docker exec -it <container_id> htop
```

Check resource usage:
```bash
docker stats ygz.admin.web
```
