---
trigger: always_on
globs: Services/**
---

# Service Logging Rules

This document outlines the standardized logging patterns for C# services. Adhere strictly to these patterns to ensure consistency across the distributed system.

## General Principles
- **Structured Logging**: Always use structured logging with parsed parameters (e.g., `{@Parameters}` for objects, `{ParameterName}` for primitives).
- **Service Identity**: Every log must identify the component type (e.g., `CommandHandler`, `QueryHandler`) and its specific name.
- **Context Depth**: The number of delimiters (`:`, `=`, `#`) often correlates to the depth or layer of the handler.

## Delimiter Depths by Context

### 1. CommandHandler / QueryHandler (Level 3)
Operations at the Application command/query entry point usage **3** repeating characters.

#### Standard Logging
- **Pattern**: `:::[{ComponentType}:{ComponentName}]::: ...`
- **Example**:
  ```csharp
  _logger.LogInformation(":::[CommandHandler:{CommandHandler}]::: Information message: {Message}, Parameters: {@Parameters}",
      nameof(CheckoutBasketHandler), "Start checking out basket...", request);
  ```

#### gRPC Calls
- **Pattern**: `===[{ComponentType}:{ComponentName}][gRPC:{ServiceName}]...===`
- **Example**:
  ```csharp
  _logger.LogInformation("===[CommandHandler:{CommandHandler}][gRPC:{gRPCName}][Method:{MethodName}]=== Information message: {Message}, Parameters: {@Parameters}",
      nameof(CheckoutBasketHandler), nameof(DiscountProtoService), nameof(_discountProtoServiceClient.GetEventItemByIdGrpcAsync), "Getting event item...", new { id });
  ```
- **Pattern (Failure/Catch)**: `===[{ComponentType}:{ComponentName}][gRPC:{ServiceName}]...===`
- **Example**:
  ```csharp
  _logger.LogCritical(ex, "===[CommandHandler:{CommandHandler}][gRPC:{gRPCName}][Method:{MethodName}]=== Error message: {Message}, Parameters: {@Parameters}",
      nameof(CheckoutBasketHandler), nameof(DiscountProtoService), nameof(_discountProtoServiceClient.GetEventItemByIdGrpcAsync), ex.Message, new { eventItemId });
  ```

#### Integration Event Publishing
- **Pattern**: `###[{ComponentType}:{ComponentName}][IntegrationEvent:{EventName}]### ...`
- **Example**:
  ```csharp
  _logger.LogWarning("###[CommandHandler:{CommandHandler}][IntegrationEvent:{IntegrationEvent}]### Parameters: {@Parameters}", 
      nameof(CheckoutBasketHandler), nameof(BasketCheckoutIntegrationEvent), integrationEventMessage);
  ```

### 2. DomainEventHandler (Level 4)
Operations handling Domain Events use **4** repeating characters.

#### Standard / Handling
- **Pattern**: `::::[{ComponentType}:{ComponentName}]:::: ...`
- **Example**:
  ```csharp
  _logger.LogWarning("::::[DomainEventHandler:{DomainEventHandler}]:::: Warning message: {Message}, Parameters: {@Parameters}",
      nameof(EventItemCreatedDomainEventHandler), "Processing event item created domain event", new { eventItemId });
  ```

#### Integration Event Publishing (from Domain Handler)
- **Pattern**: `####[{ComponentType}:{ComponentName}][IntegrationEvent:{EventName}]#### ...`
- **Example**:
  ```csharp
  _logger.LogWarning("####[DomainEventHandler:{DomainEventHandler}][IntegrationEvent:{IntegrationEvent}]#### Parameters: {@Parameters}",
      nameof(EventItemCreatedDomainEventHandler), nameof(EventItemCreatedIntegrationEvent), integrationEvent);
  ```

### 3. IntegrationEventHandler (Level 5)
Operations handling Integration Events (Consumers) use **5** repeating characters.

#### Standard / Handling
- **Pattern**: `:::::[{ComponentType}:{ComponentName}]::::: ...`
- **Example**:
  ```csharp
  _logger.LogWarning(":::::[IntegrationEventHandler:{IntegrationEventHandler}]::::: Warning message: {Message}, Parameters: {@Parameters}",
      nameof(EventItemCreatedIntegrationEventHandler), "Processing event item created integration event", context.Message);
  ```

## Summary Table

| Context Level | Delimiter Count | Standard Symbol | gRPC Symbol | Int. Event Publish Symbol |
| :--- | :---: | :---: | :---: | :---: |
| **CommandHandler** | 3 | `:::` | `===` | `###` |
| **QueryHandler** | 3 | `:::` | `===` | *N/A* |
| **DomainEventHandler** | 4 | `::::` | *N/A* | `####` |
| **IntegrationEventHandler** | 5 | `:::::` | *N/A* | *N/A* |

## Common Error Patterns
For errors, stick to the standard symbol for that level (e.g., `:::` or `::::`) but include `[Result:Error]` or `[Exception:{Type}]`.

- **CommandHandler Error**: `:::[CommandHandler:Name][Result:Error]::: ...`
- **DomainHandler Exception**: `::::[DomainEventHandler:Name][Exception:Type]:::: ...`