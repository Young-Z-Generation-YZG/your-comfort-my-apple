# Login Business Flow

1. Check email existed in DB

2. Verify password

3. Check email is verified

3.1. If is not verified
3.1.1. Generate OTP
3.1.1.1 Generate OTP
3.1.1.2. Store in Redis (5 mins)
3.1.2. Send email
3.1.2.1 Generate mail token
3.1.2.2 Send mail
3.1.2.3 return
3.2. If is verified
3.2.1 Generate token pair (Keycloak Service)
3.2.2 return
