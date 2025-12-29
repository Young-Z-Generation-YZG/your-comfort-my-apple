# Logging Guideline

## Seq

Example of throw exception response:

```json
{
    "type": "https://tools.ietf.org/html/rfc9110#section-15.6.1",
    "title": "An error occurred while processing your request.",
    "status": 500,
    "detail": "This is a test exception from TestLoggingThrowException endpoint to demonstrate exception logging and error handling.",
    "trace_id": "00-c1bb34902739d0f2071874f0670a7b78-e6bfc0d7b0945d73-01",
    "path": "/error"
}
```

Go to: `http://localhost:18080/`

Search: "TraceId = '00-c1bb34902739d0f2071874f0670a7b78-e6bfc0d7b0945d73-01'"

-> Trace -> Find => To get all request/response life circle
